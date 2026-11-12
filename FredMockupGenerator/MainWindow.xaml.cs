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

  public ObservableCollection<FieldDefinition> Fields { get; } = [];

  public IReadOnlyList<string> FieldTypes { get; } =
  [
    "Texte", "Identifiant", "Prénom", "Nom", "Email", "Entreprise", "Ville",
    "Téléphone", "Date", "Nombre", "Booléen"
  ];

  public MainWindow()
  {
    InitializeComponent();
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
    PreviewStatusText.Text = "Configurez vos champs puis générez un aperçu.";
  }

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
      ShowWindowSettingsWarning($"Les paramètres de position de la fenêtre sont invalides et seront ignorés.\n\n{exception.Message}");
      return;
    }
    catch (IOException exception)
    {
      ShowWindowSettingsWarning($"Les paramètres de position de la fenêtre n’ont pas pu être lus.\n\n{exception.Message}");
      return;
    }
    catch (UnauthorizedAccessException exception)
    {
      ShowWindowSettingsWarning($"L’accès aux paramètres de position de la fenêtre est refusé.\n\n{exception.Message}");
      return;
    }

    if (placement is null || !IsPlacementVisible(placement))
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
      IsMaximized = WindowState == WindowState.Maximized
    };

    try
    {
      var directory = Path.GetDirectoryName(WindowSettingsPath)
        ?? throw new InvalidOperationException("Le dossier des paramètres est introuvable.");
      Directory.CreateDirectory(directory);
      File.WriteAllText(WindowSettingsPath, JsonSerializer.Serialize(placement));
    }
    catch (IOException exception)
    {
      ShowWindowSettingsWarning($"La position de la fenêtre n’a pas pu être sauvegardée.\n\n{exception.Message}");
    }
    catch (UnauthorizedAccessException exception)
    {
      ShowWindowSettingsWarning($"L’accès au dossier des paramètres est refusé.\n\n{exception.Message}");
    }
  }

  private void ShowWindowSettingsWarning(string message)
  {
    MessageBox.Show(this, message, "Paramètres de la fenêtre",
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
      MessageBox.Show(this, "Le nombre de lignes doit être compris entre 1 et 100 000.",
        "Nombre de lignes invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
      return;
    }

    if (Fields.Count == 0)
    {
      MessageBox.Show(this, "Ajoutez au moins un champ avant de générer les données.",
        "Schéma vide", MessageBoxButton.OK, MessageBoxImage.Warning);
      return;
    }

    var fieldNames = Fields.Select(field => field.Name.Trim()).ToArray();
    if (fieldNames.Any(string.IsNullOrWhiteSpace))
    {
      MessageBox.Show(this, "Chaque champ doit avoir un nom.",
        "Nom de champ manquant", MessageBoxButton.OK, MessageBoxImage.Warning);
      return;
    }

    if (fieldNames.Distinct(StringComparer.OrdinalIgnoreCase).Count() != fieldNames.Length)
    {
      MessageBox.Show(this, "Les noms des champs doivent être uniques.",
        "Noms de champs en double", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    PreviewStatusText.Text = $"{rowCount:N0} lignes générées · aperçu prêt à être exporté.";
  }

  private void Export_Click(object sender, RoutedEventArgs e)
  {
    if (_generatedData is null || _generatedData.Rows.Count == 0)
    {
      MessageBox.Show(this, "Générez d’abord les données avant de lancer l’export.",
        "Aucune donnée à exporter", MessageBoxButton.OK, MessageBoxImage.Information);
      return;
    }

    var dialog = new SaveFileDialog
    {
      Title = "Exporter les données au format CSV",
      Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichiers (*.*)|*.*",
      DefaultExt = ".csv",
      AddExtension = true,
      FileName = "donnees_generees.csv"
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

      MessageBox.Show(this, $"{_generatedData.Rows.Count:N0} lignes exportées avec succès.",
        "Export terminé", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (IOException exception)
    {
      MessageBox.Show(this, $"Le fichier n’a pas pu être écrit.\n\n{exception.Message}",
        "Erreur d’export", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    catch (UnauthorizedAccessException exception)
    {
      MessageBox.Show(this, $"L’accès au fichier est refusé.\n\n{exception.Message}",
        "Erreur d’export", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }

  private string GenerateValue(string type)
  {
    var firstName = Pick(Helper.FrenchFirstNames);
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
    if (SeparatorComboBox.SelectedItem is not ComboBoxItem item)
    {
      return ",";
    }

    return item.Content?.ToString() switch
    {
      "Point-virgule  ;" => ";",
      "Tabulation" => "\t",
      "Barre verticale  |" => "|",
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
    PreviewStatusText.Text = "Le schéma a changé. Générez à nouveau les données pour actualiser l’aperçu.";
  }

  private void UpdateColumnCount()
  {
    ColumnCountText.Text = Fields.Count == 1 ? "1 champ" : $"{Fields.Count} champs";
  }
}
