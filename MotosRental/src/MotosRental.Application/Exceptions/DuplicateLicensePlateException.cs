namespace MotosRental.Exceptions;

public class DuplicateLicensePlateException : Exception
{
    public DuplicateLicensePlateException(string message) : base(message) { }
}