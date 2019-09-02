using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.Models
{
    public class Photos
    {
        public Photos()
        { }

        public Photos(string photo1, string photo2, string photo3, string photo4, string photo5)
        {
            Photo1 = photo1;
            Photo2 = photo2;
            Photo3 = photo3;
            Photo4 = photo4;
            Photo5 = photo5;
        }

        [Key]
        public int Id { get; set; }
        public int MeetingsId { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }       

        public static Photos ValidateAndAssing(List<string> imageList)
        {
            var photo1 = imageList.Count >= 1 ? imageList[0] : null;
            var photo2 = imageList.Count >= 2 ? imageList[1] : null;
            var photo3 = imageList.Count >= 3 ? imageList[2] : null;
            var photo4 = imageList.Count >= 4 ? imageList[3] : null;
            var photo5 = imageList.Count == 5 ? imageList[4] : null;
            return new Photos(photo1, photo2, photo3, photo4, photo5);
        }
    }
}