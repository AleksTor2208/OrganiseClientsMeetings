using System;
using System.Web.Mvc;
using OrganiseClientsMeetings.Models;
using OrganiseClientsMeetings.ViewModel;

namespace OrganiseClientsMeetings.Controllers
{
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddMeeting()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult AddMeeting(MeetingViewModel viewModel)
        {
            var clientId = AddClient(viewModel.ClientName);
            var meeting = new Meeting
            {
                DateTime = viewModel.DateTime.ToString(),
                Payment = viewModel.Payment,
                ClientId = clientId,
                Address = viewModel.Address
            };

            _context.Meetings.Add(meeting);
            
            _context.SaveChanges();
            return Redirect("Index");
        }

        private int AddClient(string name)
        {
            var client = new Client
            {
                Name = name
            };
            _context.Clients.Add(client);
            _context.SaveChanges();
            return client.Id;
        }
    }
}