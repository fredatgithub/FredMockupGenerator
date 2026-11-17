using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using FredMockupLibrary;

namespace FredMockupGenerator;

public partial class MainWindow : Window
{
  private static readonly string WindowSettingsPath = Path.Combine(AppContext.BaseDirectory, "window.json");
  private readonly Random _random = new();
  private DataTable? _generatedData;
  private bool _isInitializing = true;
  private bool _hasSchemaChanged;

  public ObservableCollection<FieldDefinition> Fields { get; } = [];

  public ObservableCollection<FieldTypeOption> FieldTypes { get; } = [];

  public MainWindow()
  {
    InitializeComponent();
    UpdateFieldTypes();
    CenterOnPrimaryScreen();
    RestoreWindowPlacement();
    AddVersionNumberToTitle();
    DataContext = this;

    Fields.CollectionChanged += (_, args) =>
    {
      if (args.NewItems is not null)
      {
        foreach (FieldDefinition field in args.NewItems)
        {
          field.PropertyChanged += Field_PropertyChanged;
        }
      }

      if (args.OldItems is not null)
      {
        foreach (FieldDefinition field in args.OldItems)
        {
          field.PropertyChanged -= Field_PropertyChanged;
        }
      }

      UpdateColumnCount();
      InvalidatePreview();
    };

    Fields.Add(new FieldDefinition("id", "Identifiant"));
    Fields.Add(new FieldDefinition("prenom", "Prénom"));
    Fields.Add(new FieldDefinition("nom", "Nom"));
    Fields.Add(new FieldDefinition("email", "Email"));
    Fields.Add(new FieldDefinition("ville", "Ville"));
    _isInitializing = false;
    PreviewStatusText.Text = GetText("InitialPreviewStatus");
  }

  private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!IsInitialized)
    {
      return;
    }

    var language = LanguageComboBox.SelectedIndex == 1 ? "en" : "fr";
    var dictionary = new ResourceDictionary
    {
      Source = new Uri($"/FredMockupGenerator;component/Languages/Strings.{language}.xaml", UriKind.Relative)
    };

    Application.Current.Resources.MergedDictionaries.Clear();
    Application.Current.Resources.MergedDictionaries.Add(dictionary);
    UpdateFieldTypes();
    UpdateColumnCount();
    SetPreviewStatus();
  }

  private void UpdateFieldTypes()
  {
    FieldTypeOption[] fieldTypes =
    [
      new("Texte", GetText("TypeText")),
      new("Identifiant", GetText("TypeIdentifier")),
      new("Prénom", GetText("TypeFirstName")),
      new("Nom", GetText("TypeLastName")),
      new("Email", GetText("TypeEmail")),
      new("Entreprise", GetText("TypeCompany")),
      new("Ville", GetText("TypeCity")),
      new("Téléphone", GetText("TypePhone")),
      new("Date", GetText("TypeDate")),
      new("Nombre", GetText("TypeNumber")),
      new("Booléen", GetText("TypeBoolean"))
    ];

    if (FieldTypes.Count == 0)
    {
      foreach (var fieldType in fieldTypes)
      {
        FieldTypes.Add(fieldType);
      }

      return;
    }

    for (var index = 0; index < fieldTypes.Length; index++)
    {
      FieldTypes[index] = fieldTypes[index];
    }
  }

  private static string GetText(string key) => (string)Application.Current.FindResource(key);

  private static string FormatText(string key, params object[] arguments) =>
    string.Format(CultureInfo.CurrentCulture, GetText(key), arguments);

  private void RestoreWindowPlacement()
  {
    if (!File.Exists(WindowSettingsPath))
    {
      return;
    }

    WindowPlacement? placement;
    try
    {
      placement = JsonSerializer.Deserialize<WindowPlacement>(File.ReadAllText(WindowSettingsPath));
    }
    catch (JsonException exception)
    {
      ShowWindowSettingsWarning(FormatText("SettingsInvalid", exception.Message));
      return;
    }
    catch (IOException exception)
    {
      ShowWindowSettingsWarning(FormatText("SettingsReadError", exception.Message));
      return;
    }
    catch (UnauthorizedAccessException exception)
    {
      ShowWindowSettingsWarning(FormatText("SettingsAccessDenied", exception.Message));
      return;
    }

    if (placement is null)
    {
      return;
    }

    RestoreSeparator(placement.Separator);

    if (!IsPlacementVisible(placement))
    {
      return;
    }

    Left = placement.Left;
    Top = placement.Top;
    Width = placement.Width;
    Height = placement.Height;

    if (placement.IsMaximized)
    {
      WindowState = WindowState.Maximized;
    }
  }

  private void CenterOnPrimaryScreen()
  {
    var workArea = SystemParameters.WorkArea;
    Left = workArea.Left + (workArea.Width - Width) / 2;
    Top = workArea.Top + (workArea.Height - Height) / 2;
  }

  private bool IsPlacementVisible(WindowPlacement placement)
  {
    if (!double.IsFinite(placement.Left)
      || !double.IsFinite(placement.Top)
      || !double.IsFinite(placement.Width)
      || !double.IsFinite(placement.Height)
      || placement.Width < MinWidth
      || placement.Height < MinHeight
      || placement.Width > 10000
      || placement.Height > 10000)
    {
      return false;
    }

    var screenLeft = SystemParameters.VirtualScreenLeft;
    var screenTop = SystemParameters.VirtualScreenTop;
    var screenRight = screenLeft + SystemParameters.VirtualScreenWidth;
    var screenBottom = screenTop + SystemParameters.VirtualScreenHeight;
    var visibleWidth = Math.Min(placement.Left + placement.Width, screenRight)
      - Math.Max(placement.Left, screenLeft);

    return visibleWidth >= Math.Min(100, placement.Width)
      && placement.Top >= screenTop
      && placement.Top + 40 <= screenBottom;
  }

  private void Window_Closing(object? sender, CancelEventArgs e)
  {
    var bounds = WindowState == WindowState.Normal
      ? new Rect(Left, Top, Width, Height)
      : RestoreBounds;
    var placement = new WindowPlacement
    {
      Left = bounds.Left,
      Top = bounds.Top,
      Width = bounds.Width,
      Height = bounds.Height,
      IsMaximized = WindowState == WindowState.Maximized,
      Separator = GetSelectedSeparator()
    };

    try
    {
      var directory = Path.GetDirectoryName(WindowSettingsPath)
        ?? throw new InvalidOperationException(GetText("SettingsDirectoryMissing"));
      Directory.CreateDirectory(directory);
      File.WriteAllText(WindowSettingsPath, JsonSerializer.Serialize(placement));
    }
    catch (IOException exception)
    {
      ShowWindowSettingsWarning(FormatText("SettingsSaveError", exception.Message));
    }
    catch (UnauthorizedAccessException exception)
    {
      ShowWindowSettingsWarning(FormatText("SettingsDirectoryAccessDenied", exception.Message));
    }
  }

  private void RestoreSeparator(string? separator)
  {
    var selectedIndex = separator switch
    {
      "," => 0,
      ";" => 1,
      "\t" => 2,
      "|" => 3,
      _ => -1
    };

    if (selectedIndex >= 0)
    {
      SeparatorComboBox.SelectedIndex = selectedIndex;
    }
  }

  private void ShowWindowSettingsWarning(string message)
  {
    MessageBox.Show(this, message, GetText("SettingsTitle"),
      MessageBoxButton.OK, MessageBoxImage.Warning);
  }

  private void AddVersionNumberToTitle()
  {
    var version = Assembly.GetExecutingAssembly().GetName().Version;
    if (version is not null)
    {
      Title += $" v{version.Major}.{version.Minor}.{version.Build}";
    }
  }

  private void AddField_Click(object sender, RoutedEventArgs e)
  {
    var fieldNumber = Fields.Count + 1;
    var name = $"champ_{fieldNumber}";
    while (Fields.Any(field => string.Equals(field.Name, name, StringComparison.OrdinalIgnoreCase)))
    {
      name = $"champ_{++fieldNumber}";
    }

    Fields.Add(new FieldDefinition(name, "Texte"));
    FieldsGrid.SelectedItem = Fields[^1];
    FieldsGrid.ScrollIntoView(Fields[^1]);
  }

  private void RemoveField_Click(object sender, RoutedEventArgs e)
  {
    if (FieldsGrid.SelectedItem is FieldDefinition selectedField)
    {
      Fields.Remove(selectedField);
    }
  }

  private void Generate_Click(object sender, RoutedEventArgs e)
  {
    FieldsGrid.CommitEdit(DataGridEditingUnit.Cell, true);
    FieldsGrid.CommitEdit(DataGridEditingUnit.Row, true);

    if (!int.TryParse(RowCountTextBox.Text, out var rowCount) || rowCount < 1 || rowCount > 100_000)
    {
      MessageBox.Show(this, GetText("RowsInvalidMessage"),
        GetText("RowsInvalidTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
      return;
    }

    if (Fields.Count == 0)
    {
      MessageBox.Show(this, GetText("EmptySchemaMessage"),
        GetText("EmptySchemaTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
      return;
    }

    var fieldNames = Fields.Select(field => field.Name.Trim()).ToArray();
    if (fieldNames.Any(string.IsNullOrWhiteSpace))
    {
      MessageBox.Show(this, GetText("FieldNameMissingMessage"),
        GetText("FieldNameMissingTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
      return;
    }

    if (fieldNames.Distinct(StringComparer.OrdinalIgnoreCase).Count() != fieldNames.Length)
    {
      MessageBox.Show(this, GetText("DuplicateFieldsMessage"),
        GetText("DuplicateFieldsTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
      return;
    }

    var table = new DataTable();
    foreach (var name in fieldNames)
    {
      table.Columns.Add(name, typeof(string));
    }

    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
    {
      var values = new object[Fields.Count];
      for (var columnIndex = 0; columnIndex < Fields.Count; columnIndex++)
      {
        values[columnIndex] = GenerateValue(Fields[columnIndex].Type);
      }

      table.Rows.Add(values);
    }

    _generatedData = table;
    PreviewGrid.ItemsSource = table.DefaultView;
    SetPreviewStatus();
  }

  private void Export_Click(object sender, RoutedEventArgs e)
  {
    if (_generatedData is null || _generatedData.Rows.Count == 0)
    {
      MessageBox.Show(this, GetText("NoDataMessage"),
        GetText("NoDataTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
      return;
    }

    var dialog = new SaveFileDialog
    {
      Title = GetText("ExportDialogTitle"),
      Filter = GetText("CsvFile"),
      DefaultExt = ".csv",
      AddExtension = true,
      FileName = $"Generated_data_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)}.csv"
    };

    if (dialog.ShowDialog(this) != true)
    {
      return;
    }

    try
    {
      var separator = GetSelectedSeparator();
      using var writer = new StreamWriter(dialog.FileName, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

      WriteCsvRow(writer, _generatedData.Columns.Cast<DataColumn>().Select(column => column.ColumnName), separator);
      foreach (DataRow row in _generatedData.Rows)
      {
        WriteCsvRow(writer, row.ItemArray.Select(value => value?.ToString() ?? string.Empty), separator);
      }

      MessageBox.Show(this, FormatText("ExportSuccessMessage", _generatedData.Rows.Count),
        GetText("ExportSuccessTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (IOException exception)
    {
      MessageBox.Show(this, FormatText("ExportErrorMessage", exception.Message),
        GetText("ExportErrorTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
    }
    catch (UnauthorizedAccessException exception)
    {
      MessageBox.Show(this, FormatText("AccessDeniedMessage", exception.Message),
        GetText("ExportErrorTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }

  private string GenerateValue(string type)
  {
    var firstName = Pick(Helper.FrenchMaleFirstNames);
    var lastName = Pick(Helper.FrenchLastNames);

    return type switch
    {
      "Identifiant" => Guid.NewGuid().ToString(),
      "Prénom" => firstName,
      "Nom" => lastName,
      "Email" => $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}{_random.Next(1, 1000)}@example.com",
      "Entreprise" => Pick(Helper.FrenchCompanies),
      "Ville" => Pick(Helper.FrenchCities),
      "Téléphone" => $"+33 6 {_random.Next(10, 100):D2} {_random.Next(10, 100):D2} {_random.Next(10, 100):D2} {_random.Next(10, 100):D2}",
      "Date" => DateTime.Today.AddDays(-_random.Next(0, 365 * 5 + 1)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
      "Nombre" => _random.Next(1, 1001).ToString(CultureInfo.InvariantCulture),
      "Booléen" => _random.Next(2) == 0 ? "false" : "true",
      _ => $"{firstName} {lastName}"
    };
  }

  private string Pick(string[] values) => values[_random.Next(values.Length)];

  private string GetSelectedSeparator()
  {
    return SeparatorComboBox.SelectedIndex switch
    {
      1 => ";",
      2 => "\t",
      3 => "|",
      _ => ","
    };
  }

  private static void WriteCsvRow(TextWriter writer, IEnumerable<string> values, string separator)
  {
    writer.WriteLine(string.Join(separator, values.Select(value =>
      $"\"{value.Replace("\"", "\"\"")}\"")));
  }

  private void Field_PropertyChanged(object? sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName is nameof(FieldDefinition.Name) or nameof(FieldDefinition.Type))
    {
      InvalidatePreview();
    }
  }

  private void InvalidatePreview()
  {
    _generatedData = null;
    PreviewGrid.ItemsSource = null;
    if (!_isInitializing)
    {
      _hasSchemaChanged = true;
    }

    SetPreviewStatus();
  }

  private void SetPreviewStatus()
  {
    PreviewStatusText.Text = _generatedData is not null
      ? FormatText("GeneratedStatus", _generatedData.Rows.Count)
      : _hasSchemaChanged
        ? GetText("SchemaChangedStatus")
        : GetText("InitialPreviewStatus");
  }

  private void UpdateColumnCount()
  {
    ColumnCountText.Text = Fields.Count == 1
      ? GetText("OneField")
      : FormatText("ManyFields", Fields.Count);
  }
}

public sealed record FieldTypeOption(string Key, string Label);
