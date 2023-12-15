namespace UlitMoment.Features.Auth;

public class InvalidTokenException : Exception
{
    public InvalidTokenException()
        : base("Token is not valid") { }
}
