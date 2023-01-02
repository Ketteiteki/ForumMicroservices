namespace Core.Events;

public record RegistrationEvent(
    string Nickname,
    string Password
    );
