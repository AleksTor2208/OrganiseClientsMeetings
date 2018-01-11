using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public int ClientId { get; set; } 
        
        [Required]
        public string Date { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        [Required]
        [Range(1, 10000)]
        public string Payment { get; set; }

        [Required]
        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public int PhotosId { get; set; }
    }
}