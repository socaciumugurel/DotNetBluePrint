using System.Net;
using System.Text.Json.Serialization;

namespace BluePrint.shared.services.Responses
{
    /// <summary>
    /// Base class for an Error
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Error"/>
        /// </summary>
        /// <param name="code">the code of the error</param>
        /// <param name="message">the error message</param>
        /// <param name="property">Property from model</param>
        /// <param name="details">For more detailed/complex feedback</param>
        public Error(HttpStatusCode code, string? message, string? property = null, object? details = null)
        {
            Code = code;
            Message = message;
            Property = property;
            Details = details;
        }

        /// <summary>
        /// Code for backend, to know what HttpStatus to return
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode Code { get; }

        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// Property of Input if is based on some DTO/Model
        /// </summary>
        public string? Property { get; set; }

        /// <summary>
        /// Details JSON/Object/String for more complex feedback
        /// </summary>
        public object? Details { get; set; }

        /// <summary>
        /// Static Factory
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="property"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static Error CreateError(HttpStatusCode code, string message, string? property = null, object? details = null)
        {
            return new Error(code, message, property, details);
        }
    }
}
