using Services;
using DataAccess;
using Models;
using WebAPI.Controller;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(ctx => ConnectionFactory.GetInstance(builder.Configuration.GetConnectionString("elizabethDB")));

builder.Services.AddScoped<IUserDAO, UserRepository>();
builder.Services.AddScoped<ITicketDAO, TicketRepository>();

builder.Services.AddTransient<AuthServices>();
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<TicketServices>();

builder.Services.AddScoped<AuthController>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<TicketController>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/register", (User user, AuthController controller) =>controller.Register(user));
app.MapPost("/login", (User user, AuthController controller) => controller.Login(user));

app.MapGet("/user", (UserController controller) =>controller.GetAllUsers());
app.MapGet("/user/id/{id}", (int id, UserController controller) => controller.GetUserByID(id));
app.MapGet("/user/username/{username}", (string username, UserController controller) => controller.GetUserByUsername(username));

app.MapPost("/ticket/submit", (Ticket newTicket, TicketController controller) => controller.Submit(newTicket));
app.MapPost("/ticket/process", (Ticket newTicket, TicketController controller) => controller.Process(newTicket));

app.MapGet("/ticket", (TicketController controller) =>controller.GetAllTickets());
app.MapGet("/ticket/author/{authorID}", (int authorID, TicketController controller) => controller.GetTicketsByAuthor(authorID));
app.MapGet("/ticket/{ticketID}", (int ticketID, TicketController controller) => controller.GetTicketByTicketID(ticketID));
app.MapGet("/ticket/status/{state}", (string state, TicketController controller) => controller.GetTicketsByStatus(state));
app.Run();