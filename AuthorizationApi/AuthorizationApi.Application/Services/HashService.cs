using System.Security.Cryptography;
using System.Text;
using AuthorizationApi.Application.Interfaces;

namespace AuthorizationApi.Application.Services;

public class HashService : IHashService
{
    public string HmacSha512CryptoHash(string value, out string salt)
    {
        var hmac = new HMACSHA512();
        var bytes = Encoding.Default.GetBytes(value);
        var computeHash = hmac.ComputeHash(bytes);
        salt = Convert.ToBase64String(hmac.Key);
		
        return Convert.ToBase64String(computeHash);
    }

    public string HmacSha512CryptoHashWithSalt(string value, string salt)
    {
        var key = Convert.FromBase64String(salt);	
        var hmac = new HMACSHA512 {Key = key};
        var bytes = Encoding.Default.GetBytes(value);
        var computeHash = hmac.ComputeHash(bytes);
		
        return Convert.ToBase64String(computeHash);
    }
}