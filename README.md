# organisationsnummer 
[![Build Status](https://img.shields.io/github/workflow/status/organisationsnummer/csharp/build)](https://github.com/organisationsnummer/csharp/actions)

Validate Swedish organization numbers. Follows version 1.1 of the [specification](https://github.com/organisationsnummer/meta#package-specification-v11).

Install the module with npm:

```
dotnet add package Organisationsnummer
```

## Example

```c#
using Organisationsnummer;

Organisationsnummer.valid('202100-5489');
//=> true
```

See [Organisationsnummer.Tests/OrganisationsnummerTests.cs](OrganisationsnummerTests.cs) for more examples.

## License

MIT
