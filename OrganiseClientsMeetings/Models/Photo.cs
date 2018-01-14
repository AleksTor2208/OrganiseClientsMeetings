using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }

        public string Base64 { get; set; }
    }
}