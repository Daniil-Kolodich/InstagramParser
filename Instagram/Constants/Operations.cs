namespace Instagram.Constants;

[Flags]
public enum Operations
{
    GetFollowers = 0b1,
    GetFollowings = 0b10,
    SearchFollowers = 0b100,
    SearchFollowings = 0b1000
}