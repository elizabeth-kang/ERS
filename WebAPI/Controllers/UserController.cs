using Services;
using CustomExceptions;
using DataAccess;
using Models;

namespace WebAPI.Controller;

public class UserController
{
    private readonly UserServices _Services;
    public UserController(UserServices services)
    {
        _Services = services;
    }

    /// <summary>
    /// Controller for getting all users
    /// </summary>
    /// <returns>Status code 202 for success get all users</returns>
    public IResult GetAllUsers()
    {
        try
        {
            List<User> ListUsers = _Services.GetAllUsers();
            return Results.Accepted("/user", ListUsers);
        }
        catch(ResourceNotFoundException)
        {
            return Results.NotFound("There are no users");
        }
    }

    /// <summary>
    /// Controller to find user from ID
    /// </summary>
    /// <param name="ID">User ID to find</param>
    /// <returns>Status Code 202 and user exists and given</returns>
    public IResult GetUserByID(int ID)
    {
        try
        {
            User user = _Services.GetUserByUserId(ID);
            return Results.Accepted("/user/id/{id}", user);
        }
        catch(ResourceNotFoundException )
        {
            return Results.BadRequest("No user with matching ID");
        }
        catch (UsernameNotAvailableException)
        {
            return Results.BadRequest("??? idk");
        }
    }

    public IResult GetUserByUsername(string username)
    {
        try
        {
            User user = _Services.GetUserByUsername(username);
            return Results.Accepted("/user/username/{username}", user);
        }
        catch(UsernameNotAvailableException)
        {
            return Results.BadRequest("That username is not available");
        }
        catch(ResourceNotFoundException )
        {
            return Results.BadRequest("No user with matching username");
        }
    }
}