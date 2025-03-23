namespace HoneyRaesAPI.Models;

public class ServiceTicket
{
    // Id, CustomerId, EmployeeId, Description, Emergency (this is a boolean), and DateCompleted properties.
    public int Id {get; set;}
    public int? CustomerId {get; set;}
    public int? EmployeeId {get; set;}
    public string Description {get; set;}
    public bool Emergency {get; set;}
    public DateTime DateCompleted {get; set;}
    public Employee Employee {get; set;}
    public Customer Customer {get; set;}
}