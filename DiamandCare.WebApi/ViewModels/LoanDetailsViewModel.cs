using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class LoanDetailsViewModel
    {
        public string Id { get; set; }
        public int UserID { get; set; }
        public int DocumentID { get; set; }
        public int LoanID { get; set; }
        public string FirstName { get; set; }
        public string LoanType { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}