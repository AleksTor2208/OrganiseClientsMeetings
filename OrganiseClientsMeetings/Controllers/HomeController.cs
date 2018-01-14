using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrganiseClientsMeetings.DataValidator;
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
            var meetingData = MeetingController.GetListOfMeetings(_context);
            
            List<MeetingViewModel> data = new List<MeetingViewModel>();
            foreach (var meeting in meetingData)
            {
                var photosList = PhotoController.GetPhotosOfCurrMeeting(meeting.Id, _context);
                var viewModel = new MeetingViewModel()
                {
                    Id = meeting.Id,
                    Name = _context.Clients.Where(c => c.Id == meeting.ClientId).First().Name,
                    Date = meeting.Date,
                    StartTime = meeting.StartTime,
                    EndTime = meeting.EndTime,
                    Payment = meeting.Payment,
                    Address = meeting.Address,
                    Comment = meeting.Comment,
                    Photos = photosList
                };
                data.Add(viewModel);
            }
            return View(data);
        }

        public ActionResult AddMeeting()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult AddMeeting(MeetingViewModel viewModel, IEnumerable<HttpPostedFileBase> files)
        {
            if (new ViewModelValidator().ViewModelIsInvalid(viewModel))
                return Redirect("AddMeeting");
           
            var clientId = ClientController.AddClient(viewModel.Name, _context);
            var photosList = PhotoController.GetPhotosList(files);
            int[] photoIdArray = new int[photosList.Count];

            for (int i = 0; i < photosList.Count; i++)
            {
                photoIdArray[i] = PhotoController.AddPhotoAndGetId(photosList[i], _context);
            }
            //future functionality: rescale this image
            //var resizedImage = resizeImage(Image.FromStream(HttpPostedFileBase.InputStream, true, true), new Size(50, 50));
            var meetingId = MeetingController.SaveMeetingAndGetId(viewModel, _context, clientId);

            for (int i = 0; i < photoIdArray.Length; i++)
            {
                PhotoController.AddPhotoListInstance(_context, meetingId, photoIdArray[i]);
            }
            return Redirect("Index");
        }

        //private Image resizeImage(HttpPostedFileBase image, Size size)
        //{
        //    return (Image) new Bitmap(image, new Size(50, 50));
        //}

        public void CheckTimeAvailability(string StartTime, string EndTime, string Date)
        {
            var meetingData = _context.Meetings.Select(m => m).ToArray();
            var DateTimeList = new List<DateRange>();
            foreach (var stringDate in meetingData)
            {
                var stringStartTime = $"{stringDate.Date} {stringDate.StartTime}";
                var stringEndTime = $"{stringDate.Date} {stringDate.EndTime}";

                DateTime endTime;
                DateTime startTime;
                if (DateTime.TryParse(stringStartTime, out startTime)
                && DateTime.TryParse(stringEndTime, out endTime))         
                    DateTimeList.Add( new DateRange(startTime, endTime));                
            }     
            
            bool isAvailable = true;
            foreach (var dateTime in DateTimeList)
            {                
                DateTime checkingValue;
                DateTime.TryParse($"{Date} {StartTime}", out checkingValue);
                if (checkingValue.IsNotInRange(dateTime.StartTime, dateTime.EndTime))
                {
                    isAvailable = false;
                    break;
                }                                                 
            }
            Response.Write(isAvailable.ToString());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var meeting = _context.Meetings.First(m => m.Id == id);
            var client = _context.Clients.First(c => c.Id == meeting.ClientId);
            var photos = _context.Photos.First(p => p.Id == meeting.PhotosId);
            var meetingViewModel = new MeetingViewModel
            {
                Id = id,
                Name = client.Name,
                Date = meeting.Date,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                Address = meeting.Address,
                Payment = meeting.Payment,
                Comment = meeting.Comment,
            };
            return View(meetingViewModel);
        }

        [HttpPost]
        public ActionResult Edit(MeetingViewModel viewModel)
        {
            var meeting = _context.Meetings.Find(viewModel.Id);
            var client = _context.Clients.Find(meeting.ClientId);
            _context.Meetings.Attach(meeting);
            _context.Clients.Attach(client);    
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var meeting = _context.Meetings.First(m => m.Id == id);
            var client = _context.Clients.First(c => c.Id == meeting.ClientId);
            var clientPhotos = _context.Photos.First(p => p.Id == meeting.PhotosId);

            _context.Meetings.Attach(meeting);
            _context.Meetings.Remove(meeting);
            _context.Clients.Attach(client);
            _context.Clients.Remove(client);
            _context.Photos.Attach(clientPhotos);
            _context.Photos.Remove(clientPhotos);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}