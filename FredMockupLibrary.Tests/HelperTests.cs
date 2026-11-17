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
        "Paul", "Romain", "Thierry"
      },
      Helper.FrenchMaleFirstNames);
  }

  [Fact]
  public void FrenchFemaleFirstNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "Camille", "Lucas", "Jules", "Manon", "Ines", "Hugo", "Lea", "Adam",
        "Sarah", "Louis", "Emma", "Gabriel", "Nina", "Arthur", "Chloe", "Sacha",
        "Jeanne", "Claire", "Sophie", "Julie", "Alice", "Louise", "Margot",
        "Elodie", "Pauline", "Celine"
      },
      Helper.FrenchFemaleFirstNames);
  }

  [Fact]
  public void FrenchLastNames_ContainsExpectedNames()
  {
    Assert.Equal(
      new[]
      {
        "Martin", "Bernard", "Dubois", "Thomas", "Robert", "Richard", "Petit",
        "Durand", "Leroy", "Moreau", "Simon", "Laurent", "Michel", "Garcia",
        "Roux", "Fournier", "Girard", "Andre", "Mercier", "Dupont", "Lambert"
      },
      Helper.FrenchLastNames);
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
        "Transports Morel", "Clinique Saint-Pierre", "Comptoir des Saveurs"
      },
      Helper.FrenchCompanies);
  }

  [Fact]
  public void FrenchCities_ContainsExpectedCities()
  {
    Assert.Equal(
      new[]
      {
        "Paris", "Lyon", "Bordeaux", "Nantes", "Lille", "Toulouse",
        "Strasbourg", "Montpellier", "Rennes", "Grenoble", "Dijon", "Angers",
        "Reims", "Tours", "Clermont-Ferrand", "Saint-Etienne", "Aix-en-Provence"
      },
      Helper.FrenchCities);
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
