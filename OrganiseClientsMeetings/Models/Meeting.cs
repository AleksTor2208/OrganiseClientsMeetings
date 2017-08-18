using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrganiseClientsMeetings.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public string Payment { get; set; }

        /*
          Change 'isRemote property into Addres class in next iteration
         */
        public string isRemote { get; set; }
        public string Comment { get; set; }
    }
}