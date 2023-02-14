namespace Models; 

public enum Role
{
    Manager,
    Employee
}

public class User
{
    public int ID { get;  set;}
    public string username { get;  set;}
    public string password { get;  set;}
    public Role role { get;  set;}
    
    /// <summary>
    ///This is the constructor for the User object
    ///</summary>
    /// <param name="ID">A unique int associated with the User</param>
    /// <param name="username">The username of the User</param>
    /// <param name="password">The password of the User</param>
    /// <param name="role">The role of the User, either manager or employee</param>
    public User(int ID, string username, string password, Role role)
    {
        this.ID = ID;
        this.username = username;
        this.password = password;
        this.role = role;
    }

    /// <summary>
    /// This is the constructor for the User object
    ///</summary>
    /// <param name="username">The username of the User</param>
    /// <param name="password">The password of the User</param>
    /// <param name="role">The role of the User, either manager or employee</param>
    public User(string username, string password, Role role)
    {
        this.username = username;
        this.password = password;
        this.role = role;
    }

    public User(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public User()
    {
        this.username = "";
        this.password = "";
    }

    /// <summary>
    /// This method turns a String into a Role enum.
    /// </summary>
    /// <param name="input">Only accepts "Employee" "Manager"</param>
    /// <return> Role corresponding to the input </return>
    public Role StringToRole(string input)
    {
        Dictionary<string,Role> dictRole = new Dictionary<string, Role>()
        {
            {"Employee", Role.Employee},
            {"Manager", Role.Manager}
        };

        return dictRole[input];
    }

    /// <summary>
    /// This method turns a Role enum into a String.
    /// </summary>
    /// <param name="input">Only accepts Role enums"</param>
    /// <return> String corresponding to the input </return>
    public String RoleToString(Role input)
    {
        Dictionary<Role,string> dictRole = new Dictionary<Role, string>()
        {
            {Role.Employee, "Employee"},
            {Role.Manager, "Manager"}
        };

        return dictRole[input];
    }

    public override string ToString()
    {
        return "User Object:\n" +
            "ID = " + ID + "\n" +
            "Username = " + username + "\n" +
            "Password = " + password + "\n" +
            "Role = " + role;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        else
        {
            User user = (User)obj;

            return user.ID == this.ID &&
                user.username == this.username &&
                user.password == this.password &&
                user.role ==  this.role;
        }
    }

    // public override bool Equals(object other) =>
    //     other is User usr &&
    //     (usr.ID, usr.userName, usr.passWord, usr.role).Equals((ID, userName, passWord, role));

    public override int GetHashCode()
    {
        int hashcode = 12;
        hashcode = hashcode * 2 ^ ID.GetHashCode();
        hashcode = hashcode * 2 ^ username.GetHashCode();
        hashcode = hashcode * 2 ^ password.GetHashCode();
        hashcode = hashcode * 2 ^ role.GetHashCode();
        return hashcode;
    }
    //public override int GetHashCode() => (ID, userName, passWord, role).GetHashCode();
}
