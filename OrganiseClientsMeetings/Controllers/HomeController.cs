using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
            var meetingData = _context.Meetings.Select(m => m).ToArray();
            
            List<MeetingViewModel> data = new List<MeetingViewModel>();
            foreach (var item in meetingData)
            {
                var photos = _context.Photos.Where(p => p.Id == item.PhotosId).ToList()[0];
                var photosList = new List<string> { photos.Photo1,
                    photos.Photo2, photos.Photo3,
                    photos.Photo4, photos.Photo5 };
                var viewModel = new MeetingViewModel()
                {
                    Id = item.Id,
                    Name = _context.Clients.Where(c => c.Id == item.ClientId).First().Name,
                    Date = item.Date,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Payment = item.Payment,
                    Address = item.Address,
                    Comment = item.Comment,
                    Photos = photosList
                };
                data.Add(viewModel);
                //ViewBag.Image = Image.FromStream(new MemoryStream(Convert.FromBase64String(item.Image)));
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
            if (!RequiredDateNotNull(viewModel) && !IsTimeCorrectOrder(viewModel))
                return Redirect("AddMeeting");                  
            var clientId = AddClient(viewModel.Name);
            var photosId = AddPhotos(files);

            //future functionality: rescale this image
            //var resizedImage = resizeImage(Image.FromStream(HttpPostedFileBase.InputStream, true, true), new Size(50, 50));

            var meeting = new Meeting
            {
                Date = viewModel.Date.ToString(),
                StartTime = viewModel.StartTime.ToString(),
                EndTime = viewModel.EndTime.ToString(),
                Payment = viewModel.Payment,
                ClientId = clientId,
                Comment = viewModel?.Comment,
                Address = viewModel.Address,
                PhotosId = photosId
            };         
            _context.Meetings.Add(meeting);           
            _context.SaveChanges();
            return Redirect("Index");
        }

        private bool IsTimeCorrectOrder(MeetingViewModel viewModel)
        {
            DateTime startTime;
            DateTime endTime;
            if (!DateTime.TryParse(viewModel.StartTime, out startTime)
                || !DateTime.TryParse(viewModel.EndTime, out endTime)) return false;
            var isProperOrder = DateTime.Compare(startTime, endTime);
            return isProperOrder < 0;
        }

        private bool RequiredDateNotNull(MeetingViewModel viewModel)
        {
            return viewModel.Name != null && viewModel.Date != null && viewModel.StartTime != null
                && viewModel.EndTime != null && viewModel.Payment != null && viewModel.Address != null;
        }

        private int AddPhotos(IEnumerable<HttpPostedFileBase> files)
        {
            var imageList = new List<string>();
            foreach (var image in files)
            {
                if (image == null) break;
                var imageByteArray = new byte[image.ContentLength];
                image.InputStream.Read(imageByteArray, 0, image.ContentLength);
                var base64String = Convert.ToBase64String(imageByteArray);
                imageList.Add(base64String);
            }            
            var photos = Photos.ValidateAndAssing(imageList);            
            _context.Photos.Add(photos);
            _context.SaveChanges();
            return photos.Id;
        }

        //private Image resizeImage(HttpPostedFileBase image, Size size)
        //{
        //    return (Image) new Bitmap(image, new Size(50, 50));
        //}
        
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