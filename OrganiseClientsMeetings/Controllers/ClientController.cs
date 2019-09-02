using System;
using OrganiseClientsMeetings.Models;

namespace OrganiseClientsMeetings.Controllers
{
    public class ClientController
    {
        public static int AddClient(string name, ApplicationDbContext context)
        {
            var client = new Client
            {
                Name = name
            };
            context.Clients.Add(client);
            context.SaveChanges();
            return client.Id;
        }
    }
}