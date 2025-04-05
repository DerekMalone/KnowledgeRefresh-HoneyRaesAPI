using Npgsql;
using HoneyRaesAPI.Models;
using HoneyRaesAPI.Models.DTOs;
List<HoneyRaesAPI.Models.Customer> customers = new List<HoneyRaesAPI.Models.Customer> { };
List<HoneyRaesAPI.Models.Employee> employees = new List<HoneyRaesAPI.Models.Employee> { };
List<HoneyRaesAPI.Models.ServiceTicket> serviceTickets = new List<HoneyRaesAPI.Models.ServiceTicket> { };
var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=abcd1234;Database=HoneyRaes";

{/*
    Current Progress: Start of below chpt
    https://github.com/nashville-software-school/server-side-dotnet-curriculum/blob/main/book-2-web-apis/chapters/honey-raes-delete.md
*/}

// List<Customer> customers = new List<Customer>
customers = new List<Customer>
{
    new Customer()
        { Id = 1,
            Name = "Bob",
            Address = "123 Main St",
        },
    new Customer()
        { Id = 2,
            Name = "Jim",
            Address = "124 Main St",
        },
    new Customer()
        { Id = 3,
            Name = "Thorton",
            Address = "125 Main St",
        },

};
// List<Employee> employees = new List<Employee>
employees = new List<Employee>
{
    new Employee()
    {
        Id = 1,
        Name = "Tim",
        Specialty = "Accounting",
    },
    new Employee()
    {
        Id = 2,
        Name = "Timmy",
        Specialty = "Nothing",
    }
};
// List<ServiceTicket> serviceTickets = new List<ServiceTicket>
serviceTickets = new List<ServiceTicket>
{
    new ServiceTicket()
    {
        Id = 1,
        CustomerId = 1,
        EmployeeId = 2,
        Description = "Nice Guest",
        Emergency = false,
        DateCompleted = DateTime.Now,
    },
    new ServiceTicket()
    {
        Id = 2,
        CustomerId = 1,

        Description = "Pushy Guest",
        Emergency = true,
        DateCompleted = DateTime.Now.AddDays(-3),
    },
    new ServiceTicket()
    {
        Id = 3,
        CustomerId = 2,
        EmployeeId = 2,
        Description = "Nice Guest",
        Emergency = false,
    },
    new ServiceTicket()
    {
        Id = 4,
        CustomerId = 3,
        EmployeeId = 1,
        Description = "Nice Guest",
        Emergency = false,
        DateCompleted = DateTime.Now,
    },
    new ServiceTicket()
    {
        Id = 5,
        CustomerId = 1,
        EmployeeId = 1,
        Description = "Nice Guest",
        Emergency = true,
        DateCompleted = DateTime.Now,
    },
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/servicetickets", () =>
{
    return serviceTickets.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        Description = t.Description,
        Emergency = t.Emergency,
        DateCompleted = t.DateCompleted
    });

});

// route param {id} must match the handler lambda functions param
app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    Employee employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);
    return Results.Ok(new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        EmployeeId = serviceTicket.EmployeeId,
        Employee = employee == null ? null : new EmployeeDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Specialty = employee.Specialty
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    });
});

{/* //!Original non npgsql code below.
// app.MapGet("/employees", () => 
// {
//         return employees.Select(e => new EmployeeDTO
//         {
//             Id = e.Id,
//             Name = e.Name,
//             Specialty = e.Specialty
//         });
// });
*/}

//! NpgSQL code below.
app.MapGet("/employees", () =>
{
    // create an empty list of employees to add to. 
    List<Employee> employees = new List<Employee>();
    //make a connection to the PostgreSQL database using the connection string
    using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
    //open the connection
    connection.Open();
    // create a sql command to send to the database
    using NpgsqlCommand command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM Employee";
    //send the command. 
    using NpgsqlDataReader reader = command.ExecuteReader();
    //read the results of the command row by row
    while (reader.Read()) // reader.Read() returns a boolean, to say whether there is a row or not, it also advances down to that row if it's there. 
    {
        //This code adds a new C# employee object with the data in the current row of the data reader 
        employees.Add(new Employee
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")), //find what position the Id column is in, then get the integer stored at that position
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Specialty = reader.GetString(reader.GetOrdinal("Specialty"))
        });
    }
    //once all the rows have been read, send the list of employees back to the client as JSON
    return employees;
});

{/* //! Original NpgSQL Code
app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    List<ServiceTicket> tickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();
    return Results.Ok(new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
});
*/}

//! NpgSQL getEmployeesById below
app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = null;
    using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
    connection.Open();
    using NpgsqlCommand command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM Employee WHERE Id = @id";
    // use command parameters to add the specific Id we are looking for to the query
    command.Parameters.AddWithValue("@id", id);
    using NpgsqlDataReader reader = command.ExecuteReader();
    if (reader.Read())
    {
        employee = new Employee
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Specialty = reader.GetString(reader.GetOrdinal("Speciality"))
        };
    }
    return employee;
});

// TODO: https://github.com/nashville-software-school/server-side-dotnet-curriculum/blob/main/book-3-sql-efcore/chapters/honey-raes-related-data.md
//! Need to pick up where I left off on the above chpt. Will be starting at "Adding ServiceTickets to the Employee object"

app.MapGet("/customers", () => 
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    });
});

app.MapGet("/customers/{id}", (int id) => 
{
    Customer customer = customers.FirstOrDefault(cust => cust.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    List<ServiceTicket> tickets = serviceTickets.Where(st => st.CustomerId == customer.Id).ToList();
    return Results.Ok(new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted,
        }).ToList()
    });
});


app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{
    // Get the customer data to check that the customerid for the service ticket is valid
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);

    // if the client did not provide a valid customer id, this is a bad request
    if (customer == null)
    {
        return Results.BadRequest();
    }

    // creates a new id (SQL will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);

    // Created returns a 201 status code with a link in the headers to where the new resource can be accessed
    return Results.Created($"/servicetickets/{serviceTicket.Id}", new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency
    });
});

app.MapDelete("/servicetickets/{id}", (int id) => 
{
    // first need to find the service ticket that has matching id
    // need to the delete service ticket from list
    // if no match return Results.NotFound()
    // need to return Results.NoContent()
    ServiceTicket ticket = serviceTickets.FirstOrDefault(s => s.Id == id);

    if (ticket == null)
    {
        return Results.NotFound();
    }

    serviceTickets.Remove(ticket);
    return Results.NoContent();
});

app.MapPut("/servicetickets/{id}", (int id, ServiceTicket serviceTicket) => 
{
    // Search for ServiceTicket, set ticketToUpdate to the found ticket
    ServiceTicket ticketToUpdate = serviceTickets.FirstOrDefault( st => st.Id == id);

    // Confirms a ticket was found to update. If not then it returns an error
    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    // Confirms that the id submitted and the ticket to update match as well.
    if (id != ticketToUpdate.Id)
    {
        return Results.BadRequest();
    }

    // Updates serviceTicket that with changed information and keeps information that doesn't need to be updated the same as well.
    ticketToUpdate.CustomerId = serviceTicket.CustomerId;
    ticketToUpdate.EmployeeId = serviceTicket.EmployeeId;
    ticketToUpdate.Description = serviceTicket.Description;
    ticketToUpdate.Emergency = serviceTicket.Emergency;
    ticketToUpdate.DateCompleted = serviceTicket.DateCompleted;

    return Results.NoContent();
});

app.MapPost("/servicetickets/{id}/complete", (int id) => 
{
    ServiceTicket ticketToComplete = serviceTickets.FirstOrDefault( st => st.Id == id);

        // Confirms a ticket was found to update. If not then it returns an error
    if (ticketToComplete == null)
    {
        return Results.NotFound();
    }
    // Confirms that the id submitted and the ticket to update match as well.
    if (id != ticketToComplete.Id)
    {
        return Results.BadRequest();
    }

    ticketToComplete.DateCompleted = DateTime.Today;
    return Results.NoContent();
});

app.Run();

