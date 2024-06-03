using FootballHub.Application.Exceptions;

namespace FootballHub.Api.Application.Response;

public class ValidationResponse
{
    public ValidationResponse()
    {
    }

    public ValidationResponse(ValidationException validationException)
    {
        if (validationException != null)
        {
            if (validationException.Errors != null)
            {
                Errors = validationException.Errors.Select(e => new FieldValidationError
                {
                    Error = e.Error,
                    FieldName = e.FieldName
                }).ToList();
            }
        }
    }
    
    public List<FieldValidationError> Errors { get; set; } = new();
    
    public class FieldValidationError
    {
        public required string FieldName { get; set; }
        public required string Error { get; set; }
    }
}
