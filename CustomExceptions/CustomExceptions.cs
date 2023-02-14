namespace CustomExceptions;
public class ResourceNotFoundException : System.Exception
{
    public ResourceNotFoundException() { }
    public ResourceNotFoundException(string message) : base(message) { }
    public ResourceNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    protected ResourceNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class UsernameNotAvailableException : System.Exception
{
    public UsernameNotAvailableException() { }
    public UsernameNotAvailableException(string message) : base(message) { }
    public UsernameNotAvailableException(string message, System.Exception inner) : base(message, inner) { }
    protected UsernameNotAvailableException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
public class InvalidCredentialsException : System.Exception
{
    public InvalidCredentialsException() { }
    public InvalidCredentialsException(string message) : base(message) { }
    public InvalidCredentialsException(string message, System.Exception inner) : base(message, inner) { }
    protected InvalidCredentialsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}