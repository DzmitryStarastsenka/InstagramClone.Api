using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace InstagramClone.Domain.Exceptions
{
    public class ValidationApiException : ApiException
    {
        public IEnumerable<ValidatedField> ValidatedFields { get; protected set; }

        public ValidationApiException(IEnumerable<ValidatedField> validatedFields, string message = "Bad Request") : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ValidatedFields = validatedFields;
        }
    }

    public class ValidatedField : ValidationResult
    {
        public ValidationErrorCode ErrorCode { get; }

        public ValidatedField(string fieldName, ValidationErrorCode errorCode = ValidationErrorCode.Invalid, string message = "Invalid field")
                        : base(message, new[] { fieldName })
        {
            ErrorCode = errorCode;
        }
    }
}
