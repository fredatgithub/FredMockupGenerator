namespace FredMockupLibrary
{
  public static class Helper
  {
    public static readonly string[] FrenchMaleFirstNames =
    [
      "Camille", "Lucas", "Jules", "Manon", "Ines", "Hugo", "Lea", "Adam", "Sarah", "Louis", "Emma", "Gabriel", "Nina", "Arthur", "Chloe", "Sacha",
      "Pierre", "Antoine", "Mathieu", "Nicolas", "Francois", "Emile", "Theo", "Paul", "Romain", "Thierry"
    ];

    public static readonly string[] FrenchFemaleFirstNames =
    [
      "Camille", "Lucas", "Jules", "Manon", "Ines", "Hugo", "Lea", "Adam", "Sarah", "Louis", "Emma", "Gabriel", "Nina", "Arthur", "Chloe", "Sacha",
      "Jeanne", "Claire", "Sophie", "Julie", "Alice", "Louise", "Margot", "Elodie", "Pauline", "Celine"
    ];

    public static readonly string[] FrenchLastNames =
    [
      "Martin", "Bernard", "Dubois", "Thomas", "Robert", "Richard", "Petit",
      "Durand", "Leroy", "Moreau", "Simon", "Laurent", "Michel", "Garcia",
      "Roux", "Fournier", "Girard", "Andre", "Mercier", "Dupont", "Lambert"
    ];

    public static readonly string[] FrenchCompanies =
    [
      "Atelier Horizon", "Nova Conseil", "Maison Rivage", "Studio Lumen",
      "Groupe Atlas", "Les Jardins Modernes", "Pixel & Co",
      "La Petite Fabrique", "Boulangerie du Centre", "Cafe des Arts",
      "Editions du Parc", "Transports Morel", "Clinique Saint-Pierre",
      "Comptoir des Saveurs"
    ];

    public static readonly string[] FrenchCities =
    [
      "Paris", "Lyon", "Bordeaux", "Nantes", "Lille", "Toulouse",
      "Strasbourg", "Montpellier", "Rennes", "Grenoble", "Dijon",
      "Angers", "Reims", "Tours", "Clermont-Ferrand", "Saint-Etienne",
      "Aix-en-Provence"
    ];

    public static readonly string[] EnglishMaleFirstNames =
    [
      "James", "John", "Robert", "Michael", "William", "David", "Richard",
      "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony",
      "Donald", "Mark", "Steven", "Paul", "Andrew"
    ];

    public static readonly string[] EnglishFemaleFirstNames =
    [
      "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan",
      "Dorothy", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra",
      "Ashley", "Kimberly", "Emily", "Donna"
    ];

    public static readonly string[] EnglishLastNames =
    [
      "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller",
      "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White",
      "Harris", "Martin", "Thompson", "Garcia"
    ];

    // Unique names from the 2026 Census Gazetteer for active incorporated places.
    public static readonly string[] UsCities = LoadUsCities();

    public static readonly string[] UsCounties =
    [
      "Los Angeles County", "Cook County", "Harris County", "Maricopa County",
      "San Diego County", "Orange County", "Miami-Dade County", "Kings County",
      "Queens County", "Clark County", "Tarrant County", "Bexar County",
      "Santa Clara County", "King County"
    ];

    public static readonly string[] UsStates =
    [
      "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado",
      "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho",
      "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana",
      "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota",
      "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada",
      "New Hampshire", "New Jersey", "New Mexico", "New York",
      "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon",
      "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota",
      "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington",
      "West Virginia", "Wisconsin", "Wyoming"
    ];

    private static string[] LoadUsCities()
    {
      const string resourceName = "FredMockupLibrary.Data.UsCities.txt";
      using var stream = typeof(Helper).Assembly.GetManifestResourceStream(resourceName)
        ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' was not found.");
      using var reader = new StreamReader(stream);

      var names = new List<string>();
      while (reader.ReadLine() is { } name)
      {
        names.Add(name);
      }

      if (names.Count == 0)
      {
        throw new InvalidOperationException($"Embedded resource '{resourceName}' contains no place names.");
      }

      return names.ToArray();
    }
  }
}
