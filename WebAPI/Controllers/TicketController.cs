using Services;
using CustomExceptions;
using DataAccess;
using Models;
namespace WebAPI.Controller;
public class TicketController
{
    private readonly TicketServices _Services;

    public TicketController(TicketServices services)
    {
        _Services = services;
    }

    /// <summary>
    /// Controller get all tickets
    /// </summary>
    /// <returns>List of all tickets</returns>
    public IResult GetAllTickets()
    {
        try
        {
            List<Ticket> ListTickets = _Services.GetAllTickets();
            return Results.Accepted("/ticket", ListTickets);
        }
        catch(ResourceNotFoundException)
        {
            return Results.NotFound("There are no users");
        }
    }

    /// <summary>
    /// Controller for creating a new ticket
    /// </summary>
    /// <param name="newTicket">The ticket object for the new ticket </param>
    /// <remarks>Status code 409 ticket was not correctly submitted or created<remarks>
    /// <returns>Status code 202 for successful creation</returns>
    public IResult Submit(Ticket newTicket)
    {
        try
        {
            bool test = _Services.SubmitReimbursement(newTicket);
            List<Ticket> tix = _Services.GetReimbursementByUserID(newTicket.authorID);
            return Results.Accepted("/submit", tix);
        }
        catch(ResourceNotFoundException)
        {
            return Results.Conflict("The ticket could not be submitted");
        }
        catch(UsernameNotAvailableException)
        {
            return Results.Conflict("No user with that ID");
        }
    }

    /// <summary>
    /// Controller for getting a ticket
    /// </summary>
    /// <param name="newTicket">The ticket object for updating an existing ticket</param>
    /// <returns>Status code 202 Ticket is update</returns>
    public IResult Process(Ticket newTicket)
    {
        try
        {
            bool test = _Services.UpdateReimbursement(newTicket);
            if(test)
            {
                return Results.Accepted("/process", _Services.GetReimbursementByID(newTicket.ID));
            }
            return Results.BadRequest();
        }
        catch(ResourceNotFoundException)
        {
            return Results.Conflict("The ticket could not be updated");
        }
        catch (UsernameNotAvailableException)
            {
                return Results.Conflict("The user with that ID does not exist.");
            }
    }

    /// <summary>
    /// Controller to get a tickets with corresponding status
    /// </summary>
    /// <param name="stringStatus">The status of the ticket</param>
    /// <returns>Status code 202 and List of Tickets with the corresponding status</returns>
    public IResult GetTicketsByStatus(string stringStatus)
    {
        Status state = new Ticket().StringToStatus(stringStatus);
        try
        {
            List<Ticket> listTickets = _Services.GetReimbursementByStatus(state);
            if(listTickets.Count == 0)
            {
                throw new ResourceNotFoundException();
            }
            return Results.Accepted("/ticket/status/{state}", listTickets);
        }
        catch (ResourceNotFoundException)
        {
            return Results.BadRequest("There are no tickets that are " + stringStatus);
        }
    }

    /// <summary>
    /// Conttroller to get tickets with author ID
    /// </summary>
    /// <param name="authorID">The author ID used to search tickets</param>
    /// <returns>Status code 202 and tickets with author</returns>
    public IResult GetTicketsByAuthor(int authorID)
    {
        try
        {
            List<Ticket> listTickets = _Services.GetReimbursementByUserID(authorID);
            return Results.Accepted("/ticket/author/{authorID}", listTickets);
        }
        catch (ResourceNotFoundException)
        {
            return Results.BadRequest("The user has not authored any tickets");
        }
    }

    /// <summary>
    /// Controller to get the Ticket by its ID
    /// </summary>
    /// <param name="ticketID"></param>
    /// <returns>Status code 202 and </returns>
    public IResult GetTicketByTicketID(int ticketID)
    {
        try
        {
            Ticket ticket = _Services.GetReimbursementByID(ticketID);
            return Results.Accepted("ticket/{ticketID}", ticket);
        }
        catch (System.Exception)
        {
            return Results.BadRequest("The ticket doesn't exist");
            throw;
        }
    }
}