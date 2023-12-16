﻿using UlitMoment.Configuration;

namespace UlitMoment.Features.Auth;

public class InvalidTokenError : HttpResponseError
{
    public InvalidTokenError()
        : base(401, "Token is not valid") { }
}