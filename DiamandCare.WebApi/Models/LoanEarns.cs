using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi.Models
{
    public class LoanEarns
    {
        public int ID { get; set; }
        public int Groups { get; set; }
        public int Persons { get; set; }
        public int EPLoans { get; set; }
        public string EPLoanTypes { get; set; }
        public int HealthBenefit { get; set; }
        public int RiskBenefit { get; set; }
        public int FeesReimbursement { get; set; }
    }
}
