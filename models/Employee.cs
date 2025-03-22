namespace HoneyRaesAPI.Models;

public class Employee
{
    public int Id;
    public string Name;
    public string Specialty;
    public List<ServiceTicket> ServiceTickets {get; set;}

}