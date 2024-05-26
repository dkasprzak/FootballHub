namespace FootballHub.Application.Exceptions;

public class ErrorException : Exception
{
    public string Error { get; set; }

    public ErrorException(string error)
    {
        Error = error;
    }
}
