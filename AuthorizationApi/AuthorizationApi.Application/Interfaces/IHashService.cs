namespace AuthorizationApi.Application.Interfaces;

public interface IHashService
{
    public string HmacSha512CryptoHash(string value, out string salt);

    public string HmacSha512CryptoHashWithSalt(string value, string salt);
}