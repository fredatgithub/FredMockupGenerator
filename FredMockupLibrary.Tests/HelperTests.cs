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
        "Sarah", "Louis", "Emma", "Gabriel", "Nina", "Arthur", "Chloe", "Sacha"
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
        "Sarah", "Louis", "Emma", "Gabriel", "Nina", "Arthur", "Chloe", "Sacha"
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
        "Durand", "Leroy", "Moreau", "Simon", "Laurent", "Michel", "Garcia"
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
        "Groupe Atlas", "Les Jardins Modernes", "Pixel & Co"
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
        "Strasbourg", "Montpellier", "Rennes", "Grenoble"
      },
      Helper.FrenchCities);
  }
}
