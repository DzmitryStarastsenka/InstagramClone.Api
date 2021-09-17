using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace InstagramClone.Domain.Exceptions
{
    public class ValidationApiException : ApiException
    {
        public IEnumerable<ValidationResult> ValidationResults { get; protected set; }

        public ValidationApiException(IEnumerable<ValidationResult> validationResults, string message = "Bad Request") : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ValidationResults = validationResults;
        }
    }
}
