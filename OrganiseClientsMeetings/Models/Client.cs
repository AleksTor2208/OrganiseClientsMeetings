using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }
    }
}