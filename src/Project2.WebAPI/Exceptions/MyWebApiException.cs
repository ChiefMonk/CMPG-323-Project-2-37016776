using System;
using System.Net;

namespace Project2.WebAPI.Exceptions
{
	public class MyWebApiException : Exception
	{
		public MyWebApiException(HttpStatusCode statusCode, string message):base(message)
		{
			StatusCode = statusCode;
		}

		public MyWebApiException(HttpStatusCode statusCode, Exception exception) : base(exception.InnerException == null ? exception.Message: exception.InnerException.Message, exception.InnerException ?? exception)
		{
			StatusCode = statusCode;
		}

		public HttpStatusCode StatusCode { get; set; }
	}
}
