using UlitMoment.Common.Exceptions;

namespace UlitMoment.Features.Auth;

public class UserNotFoundError : HttpResponseError
{
    public UserNotFoundError(Guid id)
        : base(404, $"User with id {id} not found") { }

    public UserNotFoundError(string email)
        : base(404, $"User with email {email} not found") { }
}
