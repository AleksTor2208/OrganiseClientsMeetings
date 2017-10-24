using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.ViewModel
{
    public class MeetingViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime Time { get; set; }

        [Required]
        public string Payment { get; set; }

        [Required]
        public string Address { get; set; }        

        [Required]
        public string Comment { get; set; }
    }
}