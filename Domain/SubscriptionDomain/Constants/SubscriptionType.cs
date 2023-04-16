namespace Domain.SubscriptionDomain.Constants;

// this can be used to add search by different activity such as like on a given post, or latest activity
[Flags]
public enum SubscriptionType
{
    Follow = 0b1
}