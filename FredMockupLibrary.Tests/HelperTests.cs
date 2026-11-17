using FredMockupLibrary;

namespace FredMockupLibrary.Tests;

public class HelperTests
{
  [Fact]
  public void FrenchMaleFirstNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "Camille", "Lucas", "Jules", "Manon", "Ines", "Hugo", "Lea", "Adam",
        "Sarah", "Louis", "Emma", "Gabriel", "Nina", "Arthur", "Chloe", "Sacha",
        "Pierre", "Antoine", "Mathieu", "Nicolas", "Francois", "Emile", "Theo",
        "Paul", "Romain", "Thierry", "Olivier", "Etienne", "Benoit", "Sebastien",
        "Laurent", "Philippe", "Alain", "Jerome", "Marc", "Vincent", "Alexandre",
        "Alexis", "Amaury", "Andre", "Arnaud", "Auguste", "Aurelien", "Bastien",
        "Benjamin", "Bernard", "Bertrand", "Bruno", "Cedric", "Celestin", "Charles",
        "Christian", "Christophe", "Clement", "Damien", "Daniel", "David", "Denis",
        "Didier", "Dimitri", "Edouard", "Eric", "Ernest", "Fabien", "Fabrice", "Felix",
        "Ferdinand", "Fernand", "Florian", "Florent", "Frederic", "Gael", "Gaspard",
        "Gautier", "Georges", "Gerard", "Gregoire", "Guillaume", "Guy", "Hector", "Henri",
        "Herve", "Hubert", "Hugues", "Isidore", "Jacques", "Jean", "Jean-Baptiste",
        "Jean-Luc", "Jean-Pierre", "Joel", "Johan", "Jonathan", "Joseph", "Julien",
        "Kilian", "Leon", "Lionel", "Loic", "Loris", "Luc", "Ludovic", "Marcel", "Marius",
        "Martial", "Maxence", "Maxime", "Michel", "Morgan", "Norbert", "Octave", "Pascal",
        "Patrice", "Patrick", "Quentin", "Remi", "Rene", "Robin", "Rodolphe", "Roger",
        "Roland", "Rudy", "Samuel", "Serge", "Simon", "Stephane", "Sylvain", "Sylvestre",
        "Theophile", "Tristan", "Valentin", "Victor", "Xavier", "Yann", "Yves", "Zacharie"
      },
      Helper.FrenchMaleFirstNames);
    Assert.Equal(136, Helper.FrenchMaleFirstNames.Length);
    Assert.Equal(
      Helper.FrenchMaleFirstNames.Length,
      Helper.FrenchMaleFirstNames.Distinct(StringComparer.OrdinalIgnoreCase).Count());
  }

  [Fact]
  public void FrenchFemaleFirstNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "Camille", "Manon", "Ines", "Hugo", "Lea", "Adam",
        "Sarah", "Louis", "Emma", "Gabriel", "Nina", "Arthur", "Chloe", "Sacha",
        "Jeanne", "Claire", "Sophie", "Julie", "Alice", "Louise", "Margot",
        "Elodie", "Pauline", "Celine", "Adele", "Adrienne", "Agathe", "Agnes",
        "Aimee", "Alexandra", "Amandine", "Amelie", "Anais", "Andrea", "Angelique",
        "Anne", "Annette", "Apolline", "Ariane", "Astrid", "Aurore", "Audrey",
        "Beatrice", "Berenice", "Bernadette", "Brigitte", "Caroline", "Catherine",
        "Cecile", "Chantal", "Charlotte", "Clara", "Clarisse", "Colette", "Constance",
        "Coralie", "Delphine", "Denise", "Diane", "Edith", "Eleonore", "Elise", "Elsa",
        "Emilie", "Estelle", "Eugenie", "Eve", "Fabienne", "Florence", "Francine",
        "Gabrielle", "Gaelle", "Genevieve", "Helene", "Isabelle", "Jacqueline",
        "Jocelyne", "Josee", "Judith", "Juliette", "Laure", "Laurence", "Leonie",
        "Liliane", "Lucie", "Madeleine", "Maelle", "Marcelle", "Marianne", "Marie",
        "Marine", "Mathilde", "Melanie", "Michele", "Mireille", "Monique", "Muriel",
        "Nathalie", "Nicole", "Noemie", "Odette", "Patricia", "Paulette", "Perrine",
        "Rachel", "Renee", "Romane", "Rose", "Roxane", "Sandrine", "Solange", "Sylvie",
        "Valerie", "Veronique", "Victoire", "Virginie", "Yvonne", "Zoe", "Maud",
        "Myriam", "Faustine", "Capucine", "Lise", "Flavie"
      },
      Helper.FrenchFemaleFirstNames);
    Assert.Equal(124, Helper.FrenchFemaleFirstNames.Length);
    Assert.Equal(
      Helper.FrenchFemaleFirstNames.Length,
      Helper.FrenchFemaleFirstNames.Distinct(StringComparer.OrdinalIgnoreCase).Count());
  }

  [Fact]
  public void FrenchLastNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "Martin", "Bernard", "Dubois", "Thomas", "Robert", "Richard", "Petit",
        "Durand", "Leroy", "Moreau", "Simon", "Laurent", "Michel", "Garcia",
        "Roux", "Fournier", "Girard", "Andre", "Mercier", "Dupont", "Lambert",
        "Lefebvre", "Faure", "Rousseau", "Blanc", "Guerin", "Muller", "Henry", "Roussel", "Nicolas", "Gauthier",
        "Dumont", "Fontaine", "Chevalier", "Robin", "Masson", "Sanchez", "Gerard", "Nguyen", "Boyer", "Denis",
        "Lemaire", "Duval", "Joly", "Giraud", "Roger", "Brun", "Garnier", "Roche", "Roy", "Noel",
        "Meyer", "Lucas", "Meunier", "Jean", "Marchand", "Dufour", "Blanchard", "Perez", "Menard", "Boucher",
        "Legrand", "Le Gall", "Rolland", "Leclerc", "Collet", "Aubert", "Clement", "Pichon", "Caron", "Millet",
        "Guillot", "Barre", "Renaud", "Charpentier", "Perrot", "Bouvier", "Cousin", "Pons", "Lacroix", "Delorme",
        "Chevallier", "Aubry", "Vasseur", "Dumoulin", "Deschamps", "Leroux", "Lenoir", "Benoit", "Renard", "Hubert",
        "Grondin", "Marty", "Pruvost", "Brunet", "Tanguy", "Lejeune", "Leger", "Lamy", "Arnaud", "Prevost",
        "Chauvin", "Guilbert", "Kauffmann", "Peltier", "Paris", "Samson", "Guyot", "Bazin", "Delaunay", "Barreau",
        "Laporte", "Picard", "Coste", "Lagarde", "Schmitt", "Olivier", "Dumas", "Faivre", "Tessier", "Perier"
      },
      Helper.FrenchLastNames);
    Assert.Equal(121, Helper.FrenchLastNames.Length);
    Assert.Equal(
      Helper.FrenchLastNames.Length,
      Helper.FrenchLastNames.Distinct(StringComparer.OrdinalIgnoreCase).Count());
  }

  [Fact]
  public void FrenchCompanies_ContainsExpectedCompanies()
  {
    Assert.Equal(
      new[]
      {
        "Atelier Horizon", "Nova Conseil", "Maison Rivage", "Studio Lumen",
        "Groupe Atlas", "Les Jardins Modernes", "Pixel & Co", "La Petite Fabrique",
        "Boulangerie du Centre", "Cafe des Arts", "Editions du Parc",
        "Transports Morel", "Clinique Saint-Pierre", "Comptoir des Saveurs",
        "Atelier des Peupliers", "Maison du Vieux Port", "Groupe Valmont",
        "Agence Belle Saison", "Comptoir du Verger", "Les Jardins du Cedre",
        "Fabrique des Possibles", "Institut de la Colline", "La Manufacture Bleue",
        "Espace des Artisans", "Atelier du Faubourg", "Maison des Oliviers",
        "Groupe des Provinces", "Agence Cote Jardin", "Comptoir des Halles",
        "Les Serres de Provence", "Fabrique du Moulin", "Institut Nouvelle Ere",
        "La Manufacture du Lin", "Espace Beauregard", "Atelier des Saisons",
        "Maison des Vignes", "Groupe Clair Matin", "Agence des Terrasses",
        "Comptoir Saint-Martin", "Les Jardins de la Plaine", "Fabrique des Rives",
        "Institut des Metiers", "Manufacture de l'Ouest", "Espace du Cloitre",
        "Atelier du Belvedere", "Maison de la Source", "Groupe des Cimes",
        "Agence des Capucins", "Comptoir des Quais", "Les Vergers Reunis",
        "Fabrique du Littoral", "Institut des Arts", "Manufacture du Bocage",
        "Espace des Lumieres", "Atelier des Artistes", "Maison du Canal",
        "Groupe du Grand Chene", "Agence des Navigateurs", "Comptoir des Provinces",
        "Les Jardins du Soleil", "Fabrique des Saveurs", "Institut des Sciences",
        "Manufacture des Landes", "Espace des Roches", "Atelier des Trois Ponts",
        "Maison des Fleurs", "Groupe des Horizons", "Agence du Petit Parc",
        "Comptoir de la Gare", "Les Vignerons du Val", "Fabrique des Couleurs",
        "Institut de la Foret", "Manufacture de la Vallee", "Espace du Patrimoine",
        "Atelier du Grand Large", "Maison des Passages", "Groupe des Rives",
        "Agence du Bel Air", "Comptoir de l'Abbaye", "Les Jardins de l'Est",
        "Fabrique des Etoffes", "Institut des Saveurs", "Manufacture des Collines",
        "Espace des Docks", "Atelier du Marais", "Maison des Roches",
        "Groupe du Petit Bois", "Agence des Tonneliers", "Comptoir de l'Orme",
        "Les Jardins d'Argent", "Fabrique de la Loire", "Institut du Design",
        "Manufacture des Flandres", "Espace des Bateliers", "Atelier du Grand Chene",
        "Maison des Sablons", "Groupe de la Bastide", "Agence du Levant",
        "Comptoir du Coteau", "Les Jardins du Midi", "Fabrique du Canal",
        "Institut des Terroirs", "Manufacture des Vosges", "Espace des Peupliers",
        "Atelier de la Roseraie", "Maison des Bateliers", "Groupe des Trois Vallees",
        "Agence du Grand Chene", "Comptoir des Vignerons", "Les Jardins de la Source",
        "Fabrique du Faubourg", "Institut des Couleurs", "Manufacture des Marais",
        "Espace du Belvedere"
      },
      Helper.FrenchCompanies);
    Assert.Equal(114, Helper.FrenchCompanies.Length);
    Assert.Equal(
      Helper.FrenchCompanies.Length,
      Helper.FrenchCompanies.Distinct(StringComparer.OrdinalIgnoreCase).Count());
  }

  [Fact]
  public void FrenchCities_ContainsExpectedCities()
  {
    Assert.Equal(34875, Helper.FrenchCities.Length);
    Assert.Contains("Paris", Helper.FrenchCities);
    Assert.Contains("Marseille", Helper.FrenchCities);
    Assert.Contains("Cayenne", Helper.FrenchCities);
    Assert.Contains("Saint-Étienne", Helper.FrenchCities);
  }

  [Fact]
  public void EnglishMaleFirstNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "James", "John", "Robert", "Michael", "William", "David", "Richard",
        "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony",
        "Donald", "Mark", "Steven", "Paul", "Andrew"
      },
      Helper.EnglishMaleFirstNames);
  }

  [Fact]
  public void EnglishFemaleFirstNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan",
        "Dorothy", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra",
        "Ashley", "Kimberly", "Emily", "Donna"
      },
      Helper.EnglishFemaleFirstNames);
  }

  [Fact]
  public void EnglishLastNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller",
        "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White",
        "Harris", "Martin", "Thompson", "Garcia"
      },
      Helper.EnglishLastNames);
  }

  [Fact]
  public void UsCities_ContainsExpectedCities()
  {
    Assert.Equal(12662, Helper.UsCities.Length);
    Assert.Equal(
      Helper.UsCities.OrderBy(name => name, StringComparer.OrdinalIgnoreCase),
      Helper.UsCities);
    Assert.Contains("New York", Helper.UsCities);
    Assert.Contains("Los Angeles", Helper.UsCities);
    Assert.Contains("San Juan", Helper.UsCities);
  }

  [Fact]
  public void UsCounties_ContainsExpectedCounties()
  {
    Assert.Equal(
      new[]
      {
        "Los Angeles County", "Cook County", "Harris County", "Maricopa County",
        "San Diego County", "Orange County", "Miami-Dade County", "Kings County",
        "Queens County", "Clark County", "Tarrant County", "Bexar County",
        "Santa Clara County", "King County"
      },
      Helper.UsCounties);
  }

  [Fact]
  public void UsStates_ContainsExpectedStates()
  {
    Assert.Equal(
      new[]
      {
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
      },
      Helper.UsStates);
  }
}
