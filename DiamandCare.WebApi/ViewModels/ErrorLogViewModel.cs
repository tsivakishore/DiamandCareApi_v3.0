using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi
{
    public class ErrorLogViewModel
    {
        public int LogID { get; set; }
        public int LogCount { get; set; }
        public string Application { get; set; }
        public string Location { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public DateTime LogTime { get; set; }
    }
}
