﻿using UlitMoment.Database;

namespace UlitMoment.Common.HttpResponseErrors;

public class InvalidRoleError(Role role)
    : HttpResponseError(401, $"Method is not authorized for {role} user") { }
