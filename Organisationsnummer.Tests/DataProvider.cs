using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Organisationsnummer.Tests;

public struct OrganisationsnummerData
{
    [JsonProperty("input")]
    public string Input { get; set; }
    [JsonProperty("long_format")]
    public string LongFormat { get; set; }
    [JsonProperty("short_format")]
    public string ShortFormat { get; set; }
    [JsonProperty("valid")]
    public bool Valid { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("vat_number")]
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
        Data = JsonConvert.DeserializeObject<List<OrganisationsnummerData>>(response)!;
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
