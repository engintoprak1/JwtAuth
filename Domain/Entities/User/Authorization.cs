namespace Domain.Entities.User;

public class Authorization
{
    public enum Roles
    {
        Administrator,
        Moderator,
        User
    }

    public const string default_username = "user_username";
    public const string default_firstname = "user_firstname";
    public const string default_lastname = "user_lastname";
    public const string default_email = "user@secureapi.com";
    public const string default_password = "Password1.";
    public const Roles default_role = Roles.User;
}
