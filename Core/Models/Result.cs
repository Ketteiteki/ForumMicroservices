using System.Text.Json.Serialization;
using Core.Models.Errors;

namespace Core.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    
    public T Value { get; set; }
    
    public Error Error { get; set; }

    [JsonConstructor]
    public Result()
    {
    }
    
    public Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }
    
    public Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }
}


