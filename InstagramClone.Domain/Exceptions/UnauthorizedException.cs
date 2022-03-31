using System.Net;

namespace InstagramClone.Domain.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message)
        {
            StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}
