using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Models
{
    public class LoanEarnsModel
    {
        public int Groups { get; set; }
        public int ExpectedUsers { get; set; } //Person as ExpectedUsers
        public int EPLoans { get; set; }
        public string EPLoanTypes { get; set; }
        public int HealthBenefit { get; set; }
        public int RiskBenefit { get; set; }
        public string BranchCode { get; set; }
        public string LoanStatus { get; set; }
        public int FeesReimbursement { get; set; }
        public int JoinedUsers { get; set; }
        public int ID { get; set; }
        public int Persons { get; set; }
    }
}
