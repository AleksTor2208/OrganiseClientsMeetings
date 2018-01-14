using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrganiseClientsMeetings.Models;

namespace OrganiseClientsMeetings.Controllers
{
    public class PhotoController
    {
        public static List<string> GetPhotosOfCurrMeeting(int meetingId, ApplicationDbContext context)
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

        public static List<string> GetPhotosList(IEnumerable<HttpPostedFileBase> files)
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
        }

        internal static int AddPhotoAndGetId(string imageAsBase64, ApplicationDbContext context)
        {
            var photo = new Photo
            {
                Base64 = imageAsBase64
            };
            context.ClientPhotos.Add(photo);
            context.SaveChanges();
            return photo.PhotoId;
        }

        public static void AddPhotoListInstance(ApplicationDbContext context, int meetingId, int photoId)
        {
            var photoListInstance = new PhotosList
            {
                PhotoId = photoId,
                MeetingId = meetingId
            };
            context.PhotosList.Add(photoListInstance);
            context.SaveChanges();
        }
    }
}