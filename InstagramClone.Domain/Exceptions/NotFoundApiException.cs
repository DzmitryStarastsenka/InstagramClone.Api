using System.Net;

namespace InstagramClone.Domain.Exceptions
{
    public class NotFoundApiException : ApiException
    {
        public NotFoundApiException(string message = "Not Found") : base(message)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
