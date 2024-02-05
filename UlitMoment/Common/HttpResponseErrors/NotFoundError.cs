namespace UlitMoment.Common.HttpResponseErrors;

public class NotFoundError
	: HttpResponseError
{
	public NotFoundError(string objectName, Guid id) : base(404, $"{objectName} with id: {id} was not found") { }
	public NotFoundError(string objectName) : base(404, $"{objectName} was not found") { }
}
