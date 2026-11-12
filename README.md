# organisationsnummer 
[![Test](https://github.com/organisationsnummer/csharp/actions/workflows/test.yml/badge.svg?branch=main)](https://github.com/organisationsnummer/csharp/actions/workflows/test.yml)
[![CodeQL](https://github.com/organisationsnummer/csharp/actions/workflows/analysis.yml/badge.svg?branch=main)](https://github.com/organisationsnummer/csharp/actions/workflows/analysis.yml)

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

See [Organisationsnummer.Tests/OrganisationsnummerTests.cs](https://github.com/organisationsnummer/csharp/blob/main/Organisationsnummer.Tests/OrganisationsnummerTests.cs) for more examples.

## In memoriam

Fredrik "Frozzare" Forsmo (1991-2026) was the initiator, co-founder and a core contributor of the organisationsnummer project. This library carries his work. He is missed.

## License

MIT
