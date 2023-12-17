namespace UlitMoment.Common.HttpResponseErrors;

public abstract class HttpResponseError(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
