namespace UlitMoment.Common.HttpResponseErrors;

public class NotFoundError(string objectName, Guid id)
    : HttpResponseError(404, $"{objectName} with id: {id} was not found") { }
