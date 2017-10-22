using System;
using System.Linq;
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
            var myData = _context.Meetings.Select(m => m).ToArray();
            return View(myData);
        }

        public ActionResult AddMeeting()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult AddMeeting(MeetingViewModel viewModel)
        {
            var clientId = AddClient(viewModel.Name);
            var meeting = new Meeting
            {
                DateTime = viewModel.Date.ToString(),
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