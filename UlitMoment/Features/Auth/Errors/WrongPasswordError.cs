using UlitMoment.Configuration;

namespace UlitMoment.Features.Auth;

public class WrongPasswordError : HttpResponseError
{
    public WrongPasswordError()
        : base(400, "Wrong password") { }
}
