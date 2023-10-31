namespace HandySquad.Exceptions;

public class Duplicate409Exception: Exception
{
    public Duplicate409Exception(string? message) : base(message)
    {
    }
}