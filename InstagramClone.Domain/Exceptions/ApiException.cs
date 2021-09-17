using System.Net;

namespace InstagramClone.Domain.Exceptions
{
    public class ApiException : System.ApplicationException
    {
        public HttpStatusCode StatusCode { get; protected set; }

        public ApiException(string message = "Internal Server Error") : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
