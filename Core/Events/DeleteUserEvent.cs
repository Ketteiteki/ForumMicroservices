namespace Core.Events;

public record DeleteUserEvent(
    Guid UserId);