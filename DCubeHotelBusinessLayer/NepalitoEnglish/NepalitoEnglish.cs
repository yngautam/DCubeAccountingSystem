using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCubeHotelBusinessLayer
{
    public static class NepalitoEnglish
    {
        public static string nepaliToEnglish(int yy, int mm, int dd)
        {
            //  this function depends on Nepali_calander class
            // formateType default=2010/1/25
            // formateType 1=2010-jan-25
            // formateType 2=2010-jan-25-saturday

            // MsgBox formateType

            string EnglishDate = "";

            NepalitoEnglishDateConveter a;
            a = new NepalitoEnglishDateConveter();
            a.initilizeClass();
            a.nep_to_eng(yy, mm, dd);
            string year = a.eng_date["year"];
            string month = a.eng_date["month"];
            string day = a.eng_date["date"];
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            EnglishDate = day + "/"+ month + "/" + year;
            return EnglishDate;
        }

        public static string englishToNepali(int yy, int mm, int dd)
        {
            //  this function depends on Nepali_calander class
            // formateType default=20671/25
            // formateType 1=2067-mangsir-25
            // formateType 2=2067-mangsir-25-saturday

            // MsgBox formateType
            string NepaliDate = "";

            NepalitoEnglishDateConveter a;
            a = new NepalitoEnglishDateConveter();
            a.initilizeClass();

            a.eng_to_nep(yy, mm, dd);
            string year = a.nep_date["year"];
            string month = a.nep_date["month"];
            string day = a.nep_date["date"];
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            NepaliDate = year + "." + month + "." + day;
            return NepaliDate;
        }
        public static string englishToNepaliMonth(int yy, int mm, int dd)
        {
            //  this function depends on Nepali_calander class
            // formateType default=20671/25
            // formateType 1=2067-mangsir-25
            // formateType 2=2067-mangsir-25-saturday

            NepalitoEnglishDateConveter a;
            a = new NepalitoEnglishDateConveter();
            a.initilizeClass();

            a.eng_to_nep(yy, mm, dd);
            string month = a.nep_date["nmonth"];

            return month;
        }
        public static string englishToNepaliYear(int yy, int mm, int dd)
        {
            //  this function depends on Nepali_calander class
            // formateType default=20671/25
            // formateType 1=2067-mangsir-25
            // formateType 2=2067-mangsir-25-saturday

            NepalitoEnglishDateConveter a;
            a = new NepalitoEnglishDateConveter();
            a.initilizeClass();

            a.eng_to_nep(yy, mm, dd);
            string year = a.nep_date["year"];

            return year;
        }
    }
}