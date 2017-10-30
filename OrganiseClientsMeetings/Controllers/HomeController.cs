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
                    Time = item.Time,
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
            if (viewModel == null) return Redirect("AddMeeting");
            var clientId = AddClient(viewModel.Name);
            var photosId = AddPhotos(files);         
            //rescale this image
            //var resizedImage = resizeImage(Image.FromStream(HttpPostedFileBase.InputStream, true, true), new Size(50, 50));

            var meeting = new Meeting
            {
                Date = viewModel.Date.ToString(),
                Time = viewModel.Time.ToString(),
                Payment = viewModel.Payment,
                ClientId = clientId,
                Address = viewModel.Address,
                PhotosId = photosId
            };         
            _context.Meetings.Add(meeting);           
            _context.SaveChanges();
            return Redirect("Index");
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

        public void CheckTimeAvailability(string Time, string Date)
        {
            var meetingData = _context.Meetings.Select(m => m).ToArray();
            var res = "res";
            foreach (var meeting in meetingData)
            {

            }
            Response.Write(res);
        }
    }
}