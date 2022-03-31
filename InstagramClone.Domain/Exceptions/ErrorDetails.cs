using Newtonsoft.Json;
using System.Collections.Generic;

namespace InstagramClone.Domain.Exceptions
{
    public class ErrorDetails
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ValidationErrorDetails : ErrorDetails
    {
        [JsonProperty("validationSummary")]
        public IEnumerable<ValidationError> ValidationSummary { get; set; }
    }

    public class ValidationError
    {
        public ValidationErrorCode ErrorCode { get; set; }
        public string ErrorCodeDescription => ErrorCode.ToString();
        public string FieldName { get; set; }
        public string Message { get; set; }
    }

    public enum ValidationErrorCode
    {
        Required,
        AlreadyExists,
        MaxLength,
        NotFound,
        Invalid,
        Expired
    }
}