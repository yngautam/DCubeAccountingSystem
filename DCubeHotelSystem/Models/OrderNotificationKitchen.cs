using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCubeHotelSystem.Models
{
    public class OrderNotificationKitchen
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Table { get; set; }
        public string Item { get; set; }
    }
}