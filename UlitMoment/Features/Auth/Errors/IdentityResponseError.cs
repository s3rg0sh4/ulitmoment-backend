using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using UlitMoment.Configuration;

namespace UlitMoment.Features.Auth.Errors;

public class IdentityResponseError(IEnumerable<IdentityError> errorList)
    : HttpResponseError(400, JsonSerializer.Serialize(errorList)) { }
