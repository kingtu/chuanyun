using H3.Organization;

public class UserContext
{
    public string UserId { get;  set; }
    public User User { get; internal set; }
}