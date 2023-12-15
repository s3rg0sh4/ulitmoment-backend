namespace UlitMoment.Common.Exceptions;

public class HttpResponseError(int statusCode, string message) : Exception(message)
{
	public int StatusCode { get; } = statusCode;
}
