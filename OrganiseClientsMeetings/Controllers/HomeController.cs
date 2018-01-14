using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var meetingData = _context.Meetings.Select(m => m).ToArray();
            
            List<MeetingViewModel> data = new List<MeetingViewModel>();
            foreach (var meeting in meetingData)
            {
                var photosList = GetPhotosofCurrMeeting(meeting.Id, _context);
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

        private List<string> GetPhotosofCurrMeeting(int meetingId, ApplicationDbContext context)
        {
            var photoIdList = context.PhotosList.Where(p => p.MeetingId == meetingId);
            var photoList = new List<string>();
            foreach (var item in photoIdList)
            {
                var PhotoInst = context.ClientPhotos.SingleOrDefault(p => p.PhotoId == item.PhotoId);
                photoList.Add(PhotoInst.Base64);
            }
            return photoList;
        }

        public ActionResult AddMeeting()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult AddMeeting(MeetingViewModel viewModel, IEnumerable<HttpPostedFileBase> files)
        {
            if (ViewModelIsInvalid(viewModel))
                return Redirect("AddMeeting");
           
            var clientId = AddClient(viewModel.Name);
            var photosList = GetPhotosList(files);
            int[] photoIdArray = new int[photosList.Count];

            for (int i = 0; i < photosList.Count; i++)
            {
                photoIdArray[i] = AddPhotoAndGetId(photosList[i]);
            }
            //future functionality: rescale this image
            //var resizedImage = resizeImage(Image.FromStream(HttpPostedFileBase.InputStream, true, true), new Size(50, 50));
            var meetingId = SaveMeetingAndGetId(viewModel, _context, clientId);

            for (int i = 0; i < photoIdArray.Length; i++)
            {
                AddPhotoListInstance(_context, meetingId, photoIdArray[i]);
            }
            return Redirect("Index");
        }

        private bool ViewModelIsInvalid(MeetingViewModel viewModel)
        {
            if (!RequiredDateNotNull(viewModel))
                return true;

            var startTime = DateTime.Parse(viewModel.StartTime);
            var endTime = DateTime.Parse(viewModel.EndTime);
            if (!IsTimePassedCorrectly(startTime, endTime))
            {
                return true;
            }
            return false;
        }

        private void AddPhotoListInstance(ApplicationDbContext context, int meetingId, int photoId)
        {
            var photoListInstance = new PhotosList
            {
                PhotoId = photoId,
                MeetingId = meetingId
            };
            context.PhotosList.Add(photoListInstance);
            context.SaveChanges();
        }

        private int SaveMeetingAndGetId(MeetingViewModel viewModel, ApplicationDbContext context, int clientId)
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
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
            return meeting.Id;
        }

        private int AddPhotoAndGetId(string imageAsBase64)
        {
            var photo = new Photo
            {
                Base64 = imageAsBase64
            };
            _context.ClientPhotos.Add(photo);
            _context.SaveChanges();
            return photo.PhotoId;
        }

        private bool IsTimePassedCorrectly(DateTime startTime, DateTime endTime)
        {
            var isProperOrder = DateTime.Compare(startTime, endTime) < 0;
            var minTimeSpan = 5;
            var timeSpan = endTime - startTime;
            return timeSpan.TotalMinutes > minTimeSpan && isProperOrder;
        }

        private bool RequiredDateNotNull(MeetingViewModel viewModel)
        {
            return viewModel.Name != null && viewModel.Date != null && viewModel.StartTime != null
                && viewModel.EndTime != null && viewModel.Payment != null && viewModel.Address != null;
        }

        private List<string> GetPhotosList(IEnumerable<HttpPostedFileBase> files)
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
            return imageList;

            //var photos = Photos.ValidateAndAssing(imageList);
            //_context.Photos.Add(photos);
            //_context.SaveChanges();
            //return photos.Id;
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