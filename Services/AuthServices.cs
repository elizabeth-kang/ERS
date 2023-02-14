using Models;
using DataAccess;
using CustomExceptions;

namespace Services;

public class AuthServices
{
    private readonly IUserDAO _user;
    
    public AuthServices(IUserDAO userDao)
    {
        _user = userDao;
    }

    /// <summary>
    /// This methods logs in a users with the username and password and searches for the user in the database
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>User record from the database concurrent with the provided inputs</returns>
    /// <exception cref="ResourceNotFoundException">Occurs if the username does not exist in the database</exception>
    /// <exception cref="InvalidCredentialsException">Occurs if the username and password do not match</exception>
    public User Login(string username, string password)
    {
        User user;
        try
        {
            user = _user.GetUserByUsername(username);
            if(user.username == "")
            {
                throw new ResourceNotFoundException();
            }
            if(user.password == password)
            {
                return user;
            }
            else
            {
                throw new InvalidCredentialsException();
            }
        }
        catch(ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
        catch(InvalidCredentialsException)
        {
            throw new InvalidCredentialsException();
        }
    }

    /// <summary>
    /// Registers a new user and simultaneosly logs in the new user after successful registration
    /// </summary>
    /// <param name="newUser"></param>
    /// <returns>Registered new user</returns>
    /// <exception cref="UsernameNotAvailable">Occurs if the provided username already exists in the database or if the username is harmful</exception>
    public User Register(User newUser)
    {
        try
        {
            User attempt = _user.GetUserByUsername(newUser.username);
            
            if(attempt.username == newUser.username)
            {
                throw new UsernameNotAvailableException();
            }
            else if (newUser.username == "" || newUser.username.Contains(";"))
            {
                throw new UsernameNotAvailableException();
            }
            else    
            {
                User user = _user.CreateUser(newUser);
                return user;
            }
        }
        catch(UsernameNotAvailableException)
        {
            throw new UsernameNotAvailableException();                
        }
        catch(ResourceNotFoundException)
        {
            return _user.CreateUser(newUser);
        }  
    }
}
