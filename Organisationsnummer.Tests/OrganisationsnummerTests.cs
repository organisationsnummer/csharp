using Xunit;

namespace Organisationsnummer.Tests;

public class OrganisationsnummerTests
{
    [Theory]
    [InlineData("556016-0680", true)]
    [InlineData("556103-4249", true)]
    [InlineData("5561034249", true)]
    [InlineData("556016-0681", false)]
    [InlineData("556103-4250", false)]
    [InlineData("5561034250", false)]
    public void TestValidateOrgNumbers(string number, bool expected)
    {
        Assert.Equal(expected, Organisationsnummer.Valid(number));
    }

    [Theory]
    [InlineData("556016-0680", "5560160680")]
    [InlineData("556103-4249", "5561034249")]
    [InlineData("5561034249", "5561034249")]
    public void TestFormatWithOutSeparator(string input, string expected)
    {
        Assert.Equal(expected, Organisationsnummer.Parse(input).Format(false));
    }

    [Theory]
    [InlineData("556016-0680", "556016-0680")]
    [InlineData("556103-4249", "556103-4249")]
    [InlineData("5561034249", "556103-4249")]
    public void TestFormatWithSeparator(string input, string expected)
    {
        Assert.Equal(expected, Organisationsnummer.Parse(input).Format(true));
    }

    [Theory]
    [InlineData("556016-0680", "Aktiebolag")]
    [InlineData("556103-4249", "Aktiebolag")]
    [InlineData("5561034249", "Aktiebolag")]
    [InlineData("8510033999", "Enskild firma")]
    public void TestGetType(string input, string expected)
    {
        Assert.Equal(expected, Organisationsnummer.Parse(input).Type);
    }

    [Theory]
    [InlineData("556016-0680", "SE556016068001")]
    [InlineData("556103-4249", "SE556103424901")]
    [InlineData("5561034249", "SE556103424901")]
    [InlineData("8510033999", "SE851003399901")]
    public void TestGetVat(string input, string expected)
    {
        Assert.Equal(expected, Organisationsnummer.Parse(input).VatNumber);
    }

    [Theory]
    [InlineData("121212121212")]
    [InlineData("121212-1212")]
    public void TestWithPersonnummer(string input)
    {
        var nr = Organisationsnummer.Parse(input);
        Assert.Equal("Enskild firma", nr.Type);
        Assert.True(nr.IsPersonnummer);
    }
}



