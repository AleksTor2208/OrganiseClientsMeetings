using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.Models
{
    public class PhotosList
    {
        [Key]
        public int PhotosListId { get; set; }

        public int MeetingId { get; set; }

        public int PhotoId { get; set; }
    }
}