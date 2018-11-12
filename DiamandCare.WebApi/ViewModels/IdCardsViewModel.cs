using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiamandCare.WebApi
{
    public class IdCardsViewModel
    {
        public string FrontImageName { get; set; }
        public byte[] FrontImageContent { get; set; }
        public string RearImageName { get; set; }
        public byte[] RearImageContent { get; set; }
    }
}