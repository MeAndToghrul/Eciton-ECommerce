using Microsoft.AspNetCore.Http;

namespace Eciton.Application.Exceptions.Commons;
public class ExistException<T> : Exception,IBaseException
{
    public int Code => StatusCodes.Status409Conflict;
    public string ErrorMessage { get; }
    const string _message = " is exist";

    public ExistException() : base(typeof(T).Name + _message)
    {
        ErrorMessage = typeof(T).Name + _message;
    }

    public ExistException(string message) : base(message)
    {
        ErrorMessage = message;
    }
}
