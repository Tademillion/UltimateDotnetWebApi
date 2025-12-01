using Microsoft.AspNetCore.Identity;

public class User:IdentityUser
{
//  this means the User class inherits from IdentityUser, which includes properties like UserName and Email by default  
    public string FirstName { get; set; }
    public string LastName { get; set; }
}