using System.Net;

namespace InstagramClone.Domain.Exceptions
{
    public class AlreadyExistsApiException : ApiException
    {
        public AlreadyExistsApiException(string message = "Already exists") : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
