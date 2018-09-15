using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class ReportsTypesModel
    {
        public int ReportTypeID { set; get; }
        public string ReportDescription { set; get; }
        public int LoginType { set; get; }
    }

    public class ReportsModel
    {
        public string FromDate { set; get; }
        public string ToDate { set; get; }
        public string ReportType { set; get; }
        public int UserID { set; get; }
    }
}