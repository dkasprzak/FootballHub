using FootballHub.Application.Helpers;

namespace FootballHub.Application.Tests.Helpers;

public class NameConverterTests
{
    [Theory]
    [InlineData("Premier League", "premier_league")]
    [InlineData("EFL Championship", "efl_championship")]
    [InlineData("2. Bundesliga", "2_bundesliga")]
    [InlineData("3. Liga", "3_liga")]
    [InlineData("4.Liga", "4_liga")]
    [InlineData("La Liga", "la_liga")]
    [InlineData("Serie A", "serie_a")]
    [InlineData("Ligue 1", "ligue_1")]
    [InlineData("1. Division", "1_division")]
    public void format_file_name_to_snake_case_formatted_correctly(string input, string expected)
    {
        // Act
        var result = NameConverter.FormatFileNameToSnakeCase(input);
        
        //Assert
        Assert.Equal(expected, result);
    }
}
