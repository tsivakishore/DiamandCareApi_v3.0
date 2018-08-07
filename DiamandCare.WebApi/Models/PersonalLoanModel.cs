using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi.Models
{
    public class PersonalLoanModel
    {
        public int LoanID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public double LoanAmount { get; set; }
    }
}