using Services;
using CustomExceptions;
using Models;

namespace WebAPI.Controller;

public class AuthController
{
    private readonly AuthServices _authServices;
    private readonly UserServices _userServices;

    public AuthController(AuthServices authServices, UserServices userServices)
    {
        _authServices = authServices;
        _userServices = userServices;   
    }

    /// <summary>
    /// Controller for registering a new user
    /// </summary>
    /// <param name="newUser">The new user to register</param>
    /// <returns>Status code 201 for successful user registration</returns>
    public IResult Register(User newUser)
    {
        try
        {
            newUser.username = newUser.username != null ? newUser.username : "";
            _authServices.Register(newUser);
            newUser =_userServices.GetUserByUsername(newUser.username);
            return Results.Created("/register", newUser);
        }
        catch (UsernameNotAvailableException)
        {
            return Results.BadRequest("That name has been taken.");
        }
    }

    /// <summary>
    /// Controller to login an existing user
    /// </summary>
    /// <param name="newUser">The new user to register</param>
    /// <returns>Status code 200 for matching user</returns>
    public IResult Login(User newUser)
    {
        try
        {
            newUser = _authServices.Login(newUser.username, newUser.password);
            return Results.Ok(newUser);
        }
        catch(InvalidCredentialsException)
        {
            return Results.Unauthorized();
        }
        catch(ResourceNotFoundException)
        {
            return Results.Unauthorized();
        }
    }
}