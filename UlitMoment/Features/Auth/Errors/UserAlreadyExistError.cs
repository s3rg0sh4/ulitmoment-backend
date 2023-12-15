using UlitMoment.Configuration;

namespace UlitMoment.Features.Auth;

public class UserAlreadyExistError(string email)
    : HttpResponseError(400, $"User with email: {email} already exists") { }
