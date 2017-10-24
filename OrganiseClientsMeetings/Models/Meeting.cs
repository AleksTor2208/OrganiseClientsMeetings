﻿using System;
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
        public string Date { get; set; }
        public string Time { get; set; }
        public string Payment { get; set; }
        public string Address { get; set; }
       
        public string Comment { get; set; }

        
    }
}