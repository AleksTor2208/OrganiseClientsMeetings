using System;
using System.IO;
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
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
            }

            _context.Meetings.Add(meeting);
            
            _context.SaveChanges();
            return Redirect("Index");
        }

        
             [HttpPost]
        public void Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                file.SaveAs(path);           
            }
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