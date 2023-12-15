namespace UlitMoment.Features.Auth;

public class WrongPasswordException : Exception
{
    public WrongPasswordException()
        : base("Wrong password") { }
}
