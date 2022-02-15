# organisationsnummer 
[![Test](https://github.com/organisationsnummer/csharp/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/organisationsnummer/csharp/actions/workflows/test.yml)
[![CodeQL](https://github.com/organisationsnummer/csharp/actions/workflows/analysis.yml/badge.svg?branch=master)](https://github.com/organisationsnummer/csharp/actions/workflows/analysis.yml)

Validate Swedish organization numbers. Follows version 1.1 of the [specification](https://github.com/organisationsnummer/meta#package-specification-v11).

Install the package:

```
dotnet add package Organisationsnummer
```

## Example

```c#
using Organisationsnummer;

Organisationsnummer.valid('202100-5489');
//=> true
```

See [Organisationsnummer.Tests/OrganisationsnummerTests.cs](Organisationsnummer.Tests/OrganisationsnummerTests.cs) for more examples.

## License

MIT
