using Microsoft.AspNetCore.Http;

namespace Eciton.Application.Exceptions.Commons;
public class NotFoundException<T> : Exception, IBaseException
{
    public int Code => StatusCodes.Status404NotFound;
    const string _message = " is not found!";
    public string ErrorMessage { get; }

    public NotFoundException() : base(typeof(T).Name + _message)
    {
        ErrorMessage = typeof(T).Name + _message;
    }

    public NotFoundException(string message) : base(message)
    {
        ErrorMessage = message;
    }
}