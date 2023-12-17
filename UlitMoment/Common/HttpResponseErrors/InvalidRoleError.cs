namespace UlitMoment.Common.HttpResponseErrors;

public class InvalidRoleError(string roleName)
	: HttpResponseError(403, $"Method is forbidden for {roleName} user")
{ }
