using HoneyRaesAPI.Models;
using HoneyRaesAPI.Models.DTOs;
List<HoneyRaesAPI.Models.Customer> customers = new List<HoneyRaesAPI.Models.Customer> { };
List<HoneyRaesAPI.Models.Employee> employees = new List<HoneyRaesAPI.Models.Employee> { };
List<HoneyRaesAPI.Models.ServiceTicket> serviceTickets = new List<HoneyRaesAPI.Models.ServiceTicket> { };

{/*
    Current Progress: Start of below chpt
    https://github.com/nashville-software-school/server-side-dotnet-curriculum/blob/main/book-2-web-apis/chapters/honey-raes-get-emps-cust.md
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

    return new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        EmployeeId = serviceTicket.EmployeeId,
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    };
});

app.Run();

