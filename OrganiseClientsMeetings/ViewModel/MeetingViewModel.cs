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
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Enter the issued date.")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        public string Payment { get; set; }
        public string Address { get; set; }
    }
}