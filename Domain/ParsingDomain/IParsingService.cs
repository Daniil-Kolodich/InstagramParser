using Database.Entities;

namespace Domain.ParsingDomain;

public interface IParsingService
{
    Task<int?> GetSubscriptionForParsing();
    Task Parse(int subscriptionId);
}