using System.ComponentModel;

namespace FredMockupGenerator;

public sealed class FieldDefinition : INotifyPropertyChanged
{
  private string _name;
  private string _type;

  public FieldDefinition(string name, string type)
  {
    _name = name;
    _type = type;
  }

  public string Name
  {
    get => _name;
    set => SetProperty(ref _name, value);
  }

  public string Type
  {
    get => _type;
    set
    {
      if (SetProperty(ref _type, value))
      {
        OnPropertyChanged(nameof(Example));
      }
    }
  }

  public string Example => Type switch
  {
    "Identifiant" => "7c9e6679-…",
    "Prénom" => "Camille",
    "Nom" => "Martin",
    "Email" => "camille.martin@example.com",
    "Entreprise" => "Atelier Horizon",
    "Ville" => "Lyon",
    "Téléphone" => "+33 6 12 34 56 78",
    "Date" => "2025-03-18",
    "Nombre" => "42",
    "Booléen" => "true / false",
    _ => "Texte d’exemple"
  };

  public event PropertyChangedEventHandler? PropertyChanged;

  private bool SetProperty(ref string field, string value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
  {
    if (field == value)
    {
      return false;
    }

    field = value;
    OnPropertyChanged(propertyName);
    return true;
  }

  private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }
}
