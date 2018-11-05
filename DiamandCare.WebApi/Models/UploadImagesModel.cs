using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class UploadImagesModel
    {
        public int UserID { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public byte[] ImageContent { get; set; }
        public string Description { get; set; }
    }
}