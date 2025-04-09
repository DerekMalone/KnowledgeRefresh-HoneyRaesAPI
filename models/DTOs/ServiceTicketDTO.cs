namespace HoneyRaesAPI.Models.DTOs;
public class ServiceTicketDTO
{
    // Id, CustomerId, EmployeeId, Description, Emergency (this is a boolean), and DateCompleted properties.
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? EmployeeId { get; set; }
    public string Description { get; set; }
    public bool Emergency { get; set; }
    public DateTime? DateCompleted { get; set; }
    public EmployeeDTO Employee {get; set;}
    public CustomerDTO Customer {get; set;}
}