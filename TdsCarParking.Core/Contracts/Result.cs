namespace TdsCarParking.Core.Contracts;

public class Result<T>
{
    public T? Payload { get; private set; } 
    public bool IsSuccess { get; private set; } 

    private Result()
    {
    }

    public static Result<T> Failed()
    {
        return new Result<T> { IsSuccess = false, Payload = default };
    } 
    
    public static Result<T> Succeeded(T payload)
    {
        return new Result<T> { IsSuccess = true, Payload = payload };
    }
}