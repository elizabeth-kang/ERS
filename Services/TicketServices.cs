using Models;
using DataAccess;
using CustomExceptions;

namespace Services;

public class TicketServices
{
    private readonly ITicketDAO _ticketDAO;

    public TicketServices(ITicketDAO ticketDAO)
    {
        _ticketDAO = ticketDAO;
    }

    /// <summary>
    /// Service to create a ticket, will be used by the employee
    /// </summary>
    /// <param name="newTicket"></param>
    /// <returns>boolean where true if ticket was created, false otherwise </returns>
    /// <exception cref="ResourceNotFoundException">Occurs if information provided was improper or if the table could not be located</exception>
    public bool SubmitReimbursement(Ticket ticket)
    {
        try
        {
            bool x = _ticketDAO.CreateTicket(ticket);
            if(x)
            {
                return true;
            }
            else {return false;}
        }
        catch(ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
    }
    
    /// <summary>
    /// Service to update a ticket, will be used by the manage changing a ticket to either approved or denied
    /// </summary>
    /// <param name="update"></param>
    /// <returns>boolean where true if ticket was successfully updated, false otherwise</returns>
    /// <exception cref="ResourceNotFoundException">Occurs if the data could not be updated, whether that ticket doesn't exist yet or not</exception>
    public bool UpdateReimbursement(Ticket update)
    {
        try
        {
            return _ticketDAO.UpdateTicket(update);
        }
        catch (ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
    }

     public List<Ticket> GetAllTickets()
    {
        try
        {
            return _ticketDAO.GetAllTickets();
        }
        catch (ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
    }

    /// <summary>
    /// Service to retrieve a specific ticket by its individual ID
    /// </summary>
    /// <param name="ticketID"></param>
    /// <returns>Ticket with specified ticketID</returns>
    /// <exception cref="ResourceNotFoundException">Occurs if that ticket hasn't been made yet</exception>
    public Ticket GetReimbursementByID(int ticketID)
    {
        Ticket tix = new Ticket();
        try
        {
            List<Ticket> ticketList = GetAllTickets();
            if (ticketList.Count < ticketID)
            {
                throw new ResourceNotFoundException();
            }
            else
            {
                tix = _ticketDAO.GetTicketById(ticketID);
            }
        }
        catch (ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
        return tix;
    }

    /// <summary>
    /// Service that will retrieve a specific group of tickets authored by the same employee
    /// </summary>
    /// <param name="userID"></param>
    /// <returns>List of tickets from a specific author</returns>
    /// <exception cref="ResourceNotFoundException">Occurs if that user has not made any tickets</exception>
    public List<Ticket> GetReimbursementByUserID(int userID)
    {
        try
        {
            return _ticketDAO.GetAllTicketsByAuthor(userID);
        }
        catch (ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
    }

    /// <summary>
    /// Service that will return a group of tickets based on the status
    /// </summary>
    /// <param name="state"></param>
    /// <returns>List of tickets with a specified status</returns>
    /// <exception cref="ResourceNotFoundException">Occurs when there are no such tickets with that status</exception>
    public List<Ticket> GetReimbursementByStatus(Status state)
    {
        try
        {
            return _ticketDAO.GetAllTicketsByStatus(state);
        }
        catch (ResourceNotFoundException)
        {
            throw new ResourceNotFoundException();
        }
    }
}