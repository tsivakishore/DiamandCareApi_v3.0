using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiamandCare.WebApi
{
    public class EventsModel
    {
        public int EventsID { get; set; }
        public string EventName { get; set; }
        public string EtherAddress { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<TicketTypes> TicketTypes { get; set; }
    }

    public class TicketTypes
    {
        public int TicketTypesID { get; set; }
        public String TicketType { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
