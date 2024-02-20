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
}



