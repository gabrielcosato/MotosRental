namespace MotosRental.Exceptions;

public class DuplicateDataException : Exception
{
    public DuplicateDataException(string message) : base(message) { }
}