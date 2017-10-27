using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
                foreach (var item in meetingData)
            {
                var viewModel = new MeetingViewModel()
                {
                    Name = _context.Clients.Where(c => c.Id == item.ClientId).First().Name,
                    Date = item.Date,
                    Time = item.Time,
                    Payment = item.Payment,
                    Address = item.Address,
                    Comment = item.Comment,
                    Image = item.Image
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
        public ActionResult AddMeeting(MeetingViewModel viewModel, HttpPostedFileBase image)
        {
            
            var clientId = AddClient(viewModel.Name);
            string base64String = "";
            if (viewModel.Image != null)
            {
                //rescale this image
                //var resizedImage = resizeImage(Image.FromStream(HttpPostedFileBase.InputStream, true, true), new Size(50, 50));
                var imageByteArray = new byte[image.ContentLength];                
                image.InputStream.Read(imageByteArray, 0, image.ContentLength);
                base64String = Convert.ToBase64String(imageByteArray);
            }               
            
            var meeting = new Meeting
            {
                Date = viewModel.Date.ToString(),
                Payment = viewModel.Payment,
                ClientId = clientId,
                Address = viewModel.Address,
                Image = base64String
            };         
            _context.Meetings.Add(meeting);           
            _context.SaveChanges();
            return Redirect("Index");
        }

        //private Image resizeImage(HttpPostedFileBase image, Size size)
        //{
        //    //return (Image) new Bitmap(image, new Size(50, 50));
        //}

        [HttpPost]
        public void Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Photos/"), fileName);                
                file.SaveAs(path);                   
            }
        }

        private string PassSetter(string path)
        {
            var strBuilder = new StringBuilder(path);
            return "";
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