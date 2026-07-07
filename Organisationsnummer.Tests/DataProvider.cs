using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Organisationsnummer.Tests;

public struct OrganisationsnummerData
{
    [JsonPropertyName("input")]
    public string Input { get; set; }
    [JsonPropertyName("long_format")]
    public string LongFormat { get; set; }
    [JsonPropertyName("short_format")]
    public string ShortFormat { get; set; }
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("vat_number")]
    public string VatNumber { get; set; }
}

public class ValidDataProvider : DataProvider
{
    public override IEnumerator<object[]> GetEnumerator()
    {
        return AsObject(o => o.Valid).GetEnumerator();
    }
}

public class InvalidDataProvider : DataProvider
{
    public override IEnumerator<object[]> GetEnumerator()
    {
        return AsObject(o => !o.Valid).GetEnumerator();
    }
}

public class DataProvider : IEnumerable<object[]>
{
    private static readonly HttpClient WebClient = new();
    private static List<OrganisationsnummerData> Data { get; }

    static DataProvider()
    {
        var response = WebClient.GetStringAsync("https://raw.githubusercontent.com/organisationsnummer/meta/main/testdata/list.json").Result;
        Data = JsonSerializer.Deserialize<List<OrganisationsnummerData>>(response)!;
    }

    protected IEnumerable<object[]> AsObject(Func<OrganisationsnummerData, bool> filter) => Data.Where(filter).Select(o => new object[] { o });


    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public virtual IEnumerator<object[]> GetEnumerator()
    {
        return AsObject(o => true).GetEnumerator();
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
