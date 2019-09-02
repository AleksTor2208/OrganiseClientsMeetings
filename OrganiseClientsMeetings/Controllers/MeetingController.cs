using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrganiseClientsMeetings.Models;
using OrganiseClientsMeetings.ViewModel;

namespace OrganiseClientsMeetings.Controllers
{
    public class MeetingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public static Meeting[] GetListOfMeetings(ApplicationDbContext context)
        {
            return context.Meetings.Select(m => m).ToArray();
        }

        public static int SaveMeetingAndGetId(MeetingViewModel viewModel, ApplicationDbContext context, int clientId)
        {
            var meeting = new Meeting
            {
                Date = viewModel.Date.ToString(),
                StartTime = viewModel.StartTime.ToString(),
                EndTime = viewModel.EndTime.ToString(),
                Payment = viewModel.Payment,
                ClientId = clientId,
                Comment = viewModel?.Comment,
                Address = viewModel.Address,
            };
            context.Meetings.Add(meeting);
            context.SaveChanges();
            return meeting.Id;
        }
    }
}