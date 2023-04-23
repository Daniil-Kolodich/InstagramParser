using Instagram.Constants;

namespace Instagram.Services;

internal interface IInstagramService
{
    Operations SupportedOperations { get; }
    int MinimumDelayInSeconds { get; }
}