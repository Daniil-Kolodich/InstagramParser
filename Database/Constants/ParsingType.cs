namespace Database.Constants;

// this can be used to add search by different activity such as like on a given post, or latest activity
[Flags]
public enum ParsingType
{
    Follow = 0b1
}