namespace PaymentOrderAPI.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    private const string MessageTemplate = "{0} with id '{1}' was not found.";

    public NotFoundException(string entityName, object key)
        : base(string.Format(MessageTemplate, entityName, key)) { }
}
