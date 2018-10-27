using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class UploadImagesModel
    {
        public int InstituteID { get; set; }
        public string InstituteName { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }
}