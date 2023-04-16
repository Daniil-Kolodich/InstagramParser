namespace Database.Constants;

/*
point is that one list of people should be subscribed to another list of people
the only difference is from we are taking these lists

if i do this in web job way this type can differentiate where to take peoples
 */
public enum ParsingSource
{
    // followers of a given accounts
    AccountsFollowers,
    // followings of a given accounts
    AccountsFollowings,
    // prepared list of people
    AccountsList
}