using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintService
{
    public class Complaint
    {
        public int ComplaintId { get; set; }
        public string StudentId { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; }
        public DateTime DateLogged { get; set; }
        public DateTime ExpectedResolutionDate { get; set; }
    }
}
