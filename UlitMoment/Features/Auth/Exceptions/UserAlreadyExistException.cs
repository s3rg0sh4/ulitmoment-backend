namespace UlitMoment.Features.Auth;

public class UserAlreadyExistException(string email)
    : Exception($"User with email: {email} already exists") { }
