namespace Core.Events;

public record ValidateAccessTokenEvent(
    string Token);