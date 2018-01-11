using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OrganiseClientsMeetings.Models;

namespace OrganiseClientsMeetings.ViewModel
{
    public class MeetingViewModel
    {       
        public int Id { get; set; }
        
        [Display(Name="Name")]
        public string Name { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Date { get; set; }
        
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = true)]
        public string StartTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = true)]
        public string EndTime { get; set; }

        public string Payment { get; set; }
        
        public string Address { get; set; }     

        public string Comment { get; set; }

        public int PhotoId { get; set; }

        public List<string> Photos { get; set; }
    }
}