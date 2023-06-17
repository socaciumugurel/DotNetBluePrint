using System.Net;

namespace BluePrint.shared.services.Responses
{
    public static class ErrorCodes
    {

        private const string NOT_FOUND_MESSAGE = "Not Found";
        private const string UNSUPPORTED_MEDIA_TYPE_MESSAGE = "File type is not supported";

        public static Error NotFound =>
            Error.CreateError(HttpStatusCode.NotFound, NOT_FOUND_MESSAGE);

        public static Error BadRequest(string message, string? property = null,
            object? details = null) => Error.CreateError(HttpStatusCode.BadRequest, message, property, details);

        public static Error MethodNotAllowed(string message) =>
            Error.CreateError(HttpStatusCode.MethodNotAllowed, message);

        public static Error UnsupportedMediaType() =>
            Error.CreateError(HttpStatusCode.UnsupportedMediaType, UNSUPPORTED_MEDIA_TYPE_MESSAGE);
    }
}
