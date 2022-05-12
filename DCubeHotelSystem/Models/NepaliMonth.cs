using DCubeHotelDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCubeHotelSystem.Models
{
    public class NepaliMonths
    {
        public static List<NepaliMonth> listnepalimonth()
        {
            List<NepaliMonth> nepalimonths = new List<NepaliMonth>();
            nepalimonths.Add(new NepaliMonth("Baishakh"));
            nepalimonths.Add(new NepaliMonth("Jestha"));
            nepalimonths.Add(new NepaliMonth("Ashadh"));
            nepalimonths.Add(new NepaliMonth("Shrawan"));
            nepalimonths.Add(new NepaliMonth("Bhadra"));
            nepalimonths.Add(new NepaliMonth("Ashwin"));
            nepalimonths.Add(new NepaliMonth("Kartik"));
            nepalimonths.Add(new NepaliMonth("Mangshir"));
            nepalimonths.Add(new NepaliMonth("Poush"));
            nepalimonths.Add(new NepaliMonth("Magh"));
            nepalimonths.Add(new NepaliMonth("Falgun"));
            nepalimonths.Add(new NepaliMonth("Chaitra"));
            return nepalimonths;
        }
    }
}