using System.Data.SqlClient;
using System.Data;
using Models;
using CustomExceptions;

namespace DataAccess;

public class TicketRepository : ITicketDAO
{
    private readonly ConnectionFactory _connectionFactory;
    
    public TicketRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<Ticket> GetAllTickets()
    {
        List<Ticket> tickets = new List<Ticket>();
        SqlConnection conn = _connectionFactory.GetConnection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM ers.tickets", conn);
        SqlDataReader reader = cmd.ExecuteReader();

        Ticket tick = new Ticket();

        while(reader.Read())
        {
            
            tickets.Add(new Ticket
            {
                ID = (int)reader["ticket_ID"],
                authorID = (int)reader["author_fk"],
                resolverID = Convert.IsDBNull(reader["resolver_fk"]) ? null : (int?) reader["resolver_fk"],
                description = (string)reader["description"],
                status = tick.StringToStatus((string)reader["status"]),
                amount = (decimal)reader["amount"]
            });
        }
        return tickets;
    }

    public List<Ticket> GetAllTicketsByAuthor(int authorID)
    {
        List<Ticket> tickets = new List<Ticket>();
        SqlConnection conn = _connectionFactory.GetConnection();
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM ers.Tickets WHERE author_fk = @a;", conn);
        cmd.Parameters.AddWithValue("@a", authorID);
        SqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            Ticket tick = new Ticket();
            
            tickets.Add(new Ticket
            {
                ID = (int)reader["ticket_ID"],
                authorID = (int)reader["author_fk"],
                resolverID = Convert.IsDBNull(reader["resolver_fk"]) ? null : (int?) reader["resolver_fk"],
                description = (string)reader["description"],
                status = tick.StringToStatus((string)reader["status"]),
                amount = (decimal)reader["amount"]
            });
        }
        return tickets;
    }
    public List<Ticket> GetAllTicketsByStatus(Status status)
    {
        List<Ticket> tickets = new List<Ticket>();
        Ticket tick = new Ticket();

        SqlConnection conn = _connectionFactory.GetConnection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM ers.Tickets WHERE status = @s;", conn);

        
        cmd.Parameters.AddWithValue("@s", tick.StatusToString(status));

        SqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {

            tickets.Add(new Ticket
            {
                ID = (int)reader["ticket_ID"],
                authorID = (int)reader["author_fk"],
                resolverID = Convert.IsDBNull(reader["resolver_fk"]) ? null : (int?) reader["resolver_fk"],
                description = (string)reader["description"],
                status = tick.StringToStatus((string)reader["status"]),
                amount = (decimal)reader["amount"]
            });
        }
        return tickets;
    }
    
    public Ticket GetTicketById(int ticketID)
    {
        SqlConnection conn = _connectionFactory.GetConnection();
        
        SqlCommand cmd = new SqlCommand("SELECT * FROM ers.Tickets WHERE ticket_ID = @ID;", conn);
        cmd.Parameters.AddWithValue("@ID", ticketID);
        
        Ticket tix = new Ticket();

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                Ticket ticket = new Ticket((int)reader[0], (int)reader[1], Convert.IsDBNull(reader[2]) ? null : (int?) reader[2], (string)reader[3], tix.StringToStatus((string)reader[4]), (decimal)reader[5]);
                tix = ticket;
            }
        }
        catch(ResourceNotFoundException)
        {

            throw new ResourceNotFoundException();
        }
        return tix;
    }
    
    public bool CreateTicket(Ticket NewTicketToAdd)
    {
        string sql= "INSERT INTO ers.Tickets (author_fk, description, status, amount) VALUES (@aid, @d, @s, @a);";
        //datatype for an active connection
        SqlConnection conn = _connectionFactory.GetConnection();    
        SqlCommand command = new SqlCommand(sql, conn);
        command.Parameters.AddWithValue("@aid", NewTicketToAdd.authorID);
        command.Parameters.AddWithValue("@d", NewTicketToAdd.description);
        command.Parameters.AddWithValue("@s", NewTicketToAdd.StatusToString(NewTicketToAdd.status));
        command.Parameters.AddWithValue("@a", NewTicketToAdd.amount);
        try
        {
            conn.Open();
            int rowsAffected = command.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
        catch (UsernameNotAvailableException)
        {
            throw new UsernameNotAvailableException();
        }
    }
    
    public bool UpdateTicket(Ticket UpdatedTicket)
    {
        SqlConnection conn = _connectionFactory.GetConnection();

        SqlCommand cmd = new SqlCommand("UPDATE ers.Tickets SET resolver_fk = @rfk, status = @s WHERE ticket_ID = @ID;", conn);
        
        cmd.Parameters.AddWithValue("@ID", UpdatedTicket.ID);
        cmd.Parameters.AddWithValue("@rfk", UpdatedTicket.resolverID);
        cmd.Parameters.AddWithValue("@s", UpdatedTicket.StatusToString(UpdatedTicket.status));

        try
        {
            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            conn.Close();

            if(rowsAffected != 0)
            {
                return true;
            }
            else
            {
                throw new ResourceNotFoundException();
            }
        }
        catch(ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
    }
}