using Xunit;

namespace Organisationsnummer.Tests;

public class OrganisationsnummerTests
{
    [Theory]
    [ClassData(typeof(InvalidDataProvider))]
    public void TestThrowOnInvalid(OrganisationsnummerData input)
    {
        Assert.Throws<OrganisationsnummerException>(() =>
        {
            Organisationsnummer.Parse(input.LongFormat);
        });
        Assert.Throws<OrganisationsnummerException>(() =>
        {
            Organisationsnummer.Parse(input.ShortFormat);
        });
    }

    [Theory]
    [ClassData(typeof(DataProvider))]
    public void TestValidateOrgNumbers(OrganisationsnummerData input)
    {
        Assert.Equal(input.Valid, Organisationsnummer.Valid(input.LongFormat));
        Assert.Equal(input.Valid, Organisationsnummer.Valid(input.ShortFormat));
    }

    [Theory]
    [ClassData(typeof(ValidDataProvider))]
    public void TestFormatWithOutSeparator(OrganisationsnummerData input)
    {
        Assert.Equal(input.ShortFormat, Organisationsnummer.Parse(input.LongFormat).Format(false));
        Assert.Equal(input.ShortFormat, Organisationsnummer.Parse(input.ShortFormat).Format(false));
    }

    [Theory]
    [ClassData(typeof(ValidDataProvider))]
    public void TestFormatWithSeparator(OrganisationsnummerData input)
    {
        Assert.Equal(input.LongFormat, Organisationsnummer.Parse(input.LongFormat).Format(true));
        // If short format in, we must presume it's not a + expected.
        Assert.Equal(input.LongFormat.Replace('+', '-'), Organisationsnummer.Parse(input.ShortFormat).Format(true));
    }

    [Theory]
    [ClassData(typeof(ValidDataProvider))]
    public void TestGetType(OrganisationsnummerData input)
    {
        Assert.Equal(input.Type, Organisationsnummer.Parse(input.ShortFormat).Type);
        Assert.Equal(input.Type, Organisationsnummer.Parse(input.LongFormat).Type);
    }

    [Theory]
    [ClassData(typeof(ValidDataProvider))]
    public void TestGetVat(OrganisationsnummerData input)
    {
        Assert.Equal(input.VatNumber, Organisationsnummer.Parse(input.ShortFormat).VatNumber);
        Assert.Equal(input.VatNumber, Organisationsnummer.Parse(input.LongFormat).VatNumber);
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

    [Theory]
    // Fewer than 10 or more than 13 characters.
    [InlineData("", "Input value too short")]
    [InlineData("556016068", "Input value too short")]
    [InlineData("16556016-06800", "Input value too long")]
    // Right length, but not digits with an optional separator.
    [InlineData("55601A-0680", "Input value has an invalid format")]
    [InlineData("556016 0680", "Input value has an invalid format")]
    [InlineData("1655601606800", "Input value has an invalid format")]
    // Twelve digits where the first two are not 16.
    [InlineData("185560160680", "Invalid century prefix, only 16 is allowed")]
    [InlineData("18556016-0680", "Invalid century prefix, only 16 is allowed")]
    // Digits three and four below 20.
    [InlineData("551316-0680", "Invalid organization number, digits three and four must be 20 or higher")]
    [InlineData("165513160680", "Invalid organization number, digits three and four must be 20 or higher")]
    // First two digits below 10.
    [InlineData("052016-0680", "Invalid organization number, the first two digits must be 10 or higher")]
    [InlineData("160520160680", "Invalid organization number, the first two digits must be 10 or higher")]
    // Wrong last digit.
    [InlineData("556016-0681", "Invalid check digit")]
    [InlineData("165560160681", "Invalid check digit")]
    public void TestExceptionMessage(string input, string expected)
    {
        var ex = Assert.Throws<OrganisationsnummerException>(() => Organisationsnummer.Parse(input));
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void TestExceptionDefaultMessage()
    {
        Assert.Equal("Invalid Swedish organization number", new OrganisationsnummerException().Message);
    }
}



