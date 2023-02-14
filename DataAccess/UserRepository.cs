using System.Data.SqlClient;
using System.Data;
using Models;
using CustomExceptions;

namespace DataAccess;

public class UserRepository : IUserDAO
{
    private readonly ConnectionFactory _connectionFactory;
    
    public UserRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<User> GetAllUsers()
    {
        List<User> users = new List<User>();
        SqlConnection conn = _connectionFactory.GetConnection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM ers.Users", conn);
        SqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            User usr = new User();
            
            users.Add(new User
            {
                ID = (int)reader["user_ID"],
                username = (string)reader["username"],
                password = (string)reader["password"],
                role = usr.StringToRole((string)reader["role"])
            });
        }
        return users;
    }
    
    public User GetUserById(int userID)
    {
        SqlConnection conn = _connectionFactory.GetConnection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM ers.Users WHERE user_ID = @ID;", conn);
        cmd.Parameters.AddWithValue("@ID", userID);
        SqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            User usr = new User();

            return new User
            {
                ID = (int)reader["user_ID"],
                username = (string)reader["username"],
                password = (string)reader["password"],
                role = usr.StringToRole((string)reader["role"])
            };
        }
        throw new ResourceNotFoundException("Could not find the user associated with the ID");
    }    
    
    public User GetUserByUsername(string username)
    {
        SqlConnection conn = _connectionFactory.GetConnection();
        SqlCommand cmd = new SqlCommand("SELECT * FROM ers.Users WHERE username = @usr;", conn);
        cmd.Parameters.AddWithValue("@usr", username);
        User usr = new User();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if(!reader.HasRows)
            {
                throw new ResourceNotFoundException();
            }
            else
            {
                usr = new User((int)reader[0], (string)reader[1], (string)reader[2], usr.StringToRole((string)reader[3]));
            }
            reader.Close();
            conn.Close();
        }
        catch(UsernameNotAvailableException)
        {

            throw new UsernameNotAvailableException();
        }
        catch(ResourceNotFoundException)
        {
            throw new ResourceNotFoundException("Could not find the user associated with the username");
        }
        return usr; 
    }
    
    public User CreateUser(User newUser)
    {
        string sql = "INSERT INTO ers.users(username, password, role) VALUES (@u, @p, @r)";

        SqlConnection conn = _connectionFactory.GetConnection();

        SqlCommand command = new SqlCommand(sql, conn);

        command.Parameters.AddWithValue("@u", newUser.username);
        command.Parameters.AddWithValue("@p", newUser.password);
        command.Parameters.AddWithValue("@r", newUser.RoleToString(newUser.role));

        try
        {
            conn.Open();
            int rowsAffected = command.ExecuteNonQuery();
            conn.Close();

            if(rowsAffected != 0)
            {
                if(newUser.username != null)
                {
                    return GetUserByUsername(newUser.username);
                }
                else
                {
                    throw new UsernameNotAvailableException();
                }
            }
            else
            {
                throw new UsernameNotAvailableException();
            }
        }
        catch(UsernameNotAvailableException)
        {

            throw new UsernameNotAvailableException();
        }
    }
}