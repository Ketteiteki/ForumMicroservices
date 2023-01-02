namespace Core.Events;

public record LoginEvent(
    string Nickname,
    string Password
    );