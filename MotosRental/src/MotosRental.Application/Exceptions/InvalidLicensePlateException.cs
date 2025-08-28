namespace MotosRental.Exceptions;

public class InvalidLicensePlateException : Exception
{
    public InvalidLicensePlateException(string message) : base(message) { }
}