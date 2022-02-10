namespace Organisationsnummer;

public class OrganisationsnummerException : Exception
{
    public OrganisationsnummerException(string message = "Invalid Swedish organization number", Exception? inner = null) : base(message: message, innerException: inner) { }
}
