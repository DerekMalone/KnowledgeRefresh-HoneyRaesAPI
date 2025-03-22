using System.Net.NetworkInformation;
using Microsoft.AspNetCore.SignalR;

namespace HoneyRaesAPI.Models;

public class Customer
{
    public int Id;
    public string Name;
    public string Address;
    public List<ServiceTicket> ServiceTickets {get; set;}
}