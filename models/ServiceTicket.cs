namespace HoneyRaesAPI.Models;

public class ServiceTicket
{
    // Id, CustomerId, EmployeeId, Description, Emergency (this is a boolean), and DateCompleted properties.
    public int Id;
    public int CustomerId;
    public int EmployeeId;
    public string Description;
    public bool Emergency;
    public DateTime DateCompleted;
}