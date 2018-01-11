using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OrganiseClientsMeetings.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Photos> Photos { get; set; }
         
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<OrganiseClientsMeetings.ViewModel.MeetingViewModel> MeetingViewModels { get; set; }
    }
}