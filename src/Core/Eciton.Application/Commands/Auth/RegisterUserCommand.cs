using MediatR;

namespace Eciton.Application.Commands.Auth;
public class RegisterUserCommand : IRequest<string>
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
