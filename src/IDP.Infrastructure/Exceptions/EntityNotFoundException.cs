namespace IDP.Infrastructure.Exceptions;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException() : base("Entity not found")
    {
    }
}
