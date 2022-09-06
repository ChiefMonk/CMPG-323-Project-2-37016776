using System;
using System.Net;

namespace Project2.WebAPI.Utils.Exceptions
{
	/// <summary>
	/// MyWebApiException class
	/// </summary>
	/// <seealso cref="Exception" />
	public class MyWebApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyWebApiException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        public MyWebApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyWebApiException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="exception">The exception.</param>
        public MyWebApiException(HttpStatusCode statusCode, Exception exception) : base(exception.InnerException == null ? exception.Message : exception.InnerException.Message, exception.InnerException ?? exception)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }
    }
}
