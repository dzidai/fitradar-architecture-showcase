namespace Fitradar.Application.Services;

public interface ICurrentUserContext
{
        string UserId { get; }

        string Email { get; }

        bool IsAuthenticated { get; }

        string Platform { get; }
}
