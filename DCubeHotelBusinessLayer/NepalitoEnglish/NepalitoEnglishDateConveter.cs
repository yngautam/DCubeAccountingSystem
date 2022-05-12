using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCubeHotelBusinessLayer
{
    public class NepalitoEnglishDateConveter
    {
        public int[][] bs = new int[91][];
        public Dictionary<string, string> nep_date;
        public Dictionary<string, string> eng_date;
        public string debug_info = "";

        public int Year { get; set; }
        public string Month { get; set; }

        public string Day { get; set; }

        public string NepalitoEnglish = "";

        public void initilizeClass()
        {
            bs[0] = new int[] { 2000, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[1] = new int[] { 2001, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30 };
            bs[2] = new int[] { 2002, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30 };
            bs[3] = new int[] { 2003, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31 };
            bs[4] = new int[] { 2004, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31 };
            bs[5] = new int[] {2005, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[6] = new int[] {2006, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[7] = new int[] {2007, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[8] = new int[] {2008, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31};
            bs[9] = new int[] {2009, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[10] = new int[] {2010, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[11] = new int[] {2011, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[12] = new int[] {2012, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30};
            bs[13] = new int[] {2013, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[14] = new int[] {2014, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[15] = new int[] {2015, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[16] = new int[] {2016, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30};
            bs[17] = new int[] {2017, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[18] = new int[] {2018, 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[19] = new int[] {2019, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[20] = new int[] {2020, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[21] = new int[] {2021, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[22] = new int[] {2022, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30};
            bs[23] = new int[] {2023, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[24] = new int[] {2024, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[25] = new int[] {2025, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[26] = new int[] {2026, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[27] = new int[] {2027, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[28] = new int[] {2028, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[29] = new int[] {2029, 31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30};
            bs[30] = new int[] {2030, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[31] = new int[] {2031, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[32] = new int[] {2032, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[33] = new int[] {2033, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[34] = new int[] {2034, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[35] = new int[] {2035, 30, 32, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31};
            bs[36] = new int[] {2036, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[37] = new int[] {2037, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[38] = new int[] {2038, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[39] = new int[] {2039, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30};
            bs[40] = new int[] {2040, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[41] = new int[] {2041, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[42] = new int[] {2042, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[43] = new int[] {2043, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30};
            bs[44] = new int[] {2044, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[45] = new int[] {2045, 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[46] = new int[] {2046, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[47] = new int[] {2047, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[48] = new int[] {2048, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[49] = new int[] {2049, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30};
            bs[50] = new int[] {2050, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[51] = new int[] {2051, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[52] = new int[] {2052, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[53] = new int[] {2053, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30};
            bs[54] = new int[] {2054, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[55] = new int[] {2055, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[56] = new int[] {2056, 31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30};
            bs[57] = new int[] {2057, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[58] = new int[] {2058, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[59] = new int[] {2059, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[60] = new int[] {2060, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[61] = new int[] {2061, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[62] = new int[] {2062, 30, 32, 31, 32, 31, 31, 29, 30, 29, 30, 29, 31};
            bs[63] = new int[] {2063, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[64] = new int[] {2064, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[65] = new int[] {2065, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[66] = new int[] {2066, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31};
            bs[67] = new int[] {2067, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[68] = new int[] {2068, 31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[69] = new int[] {2069, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[70] = new int[] {2070, 31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30};
            bs[71] = new int[] {2071, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[72] = new int[] {2072, 31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30};
            bs[73] = new int[] {2073, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31};
            bs[74] = new int[] {2074, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[75] = new int[] {2075, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[76] = new int[] {2076, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30};
            bs[77] = new int[] {2077, 31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31};
            bs[78] = new int[] {2078, 31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[79] = new int[] {2079, 31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30};
            bs[80] = new int[] {2080, 31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30};
            bs[81] = new int[] {2081, 31, 31, 32, 32, 31, 30, 30, 30, 29, 30, 30, 30};
            bs[82] = new int[] {2082, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30};
            bs[83] = new int[] {2083, 31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30};
            bs[84] = new int[] {2084, 31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30};
            bs[85] = new int[] {2085, 31, 32, 31, 32, 30, 31, 30, 30, 29, 30, 30, 30};
            bs[86] = new int[] {2086, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30};
            bs[87] = new int[] {2087, 31, 31, 32, 31, 31, 31, 30, 30, 29, 30, 30, 30};
            bs[88] = new int[] {2088, 30, 31, 32, 32, 30, 31, 30, 30, 29, 30, 30, 30};
            bs[89] = new int[] {2089, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30};
            bs[90] = new int[] {2090, 30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30};


            nep_date = new Dictionary<string, string>();
            nep_date.Add("year", "0");
            nep_date.Add("month", "0");
            nep_date.Add("date", "0");
            nep_date.Add("day", "0");
            nep_date.Add("nmonth", "0");
            nep_date.Add("num_day", "0");

            eng_date = new Dictionary<string, string>();
            eng_date.Add("year", "0");
            eng_date.Add("month", "0");
            eng_date.Add("date", "0");
            eng_date.Add("day", "0");
            eng_date.Add("nmonth", "0");
            eng_date.Add("num_day", "0");
        }

        //        /**
        //         * Calculates wheather english year is leap year or not
        //         *
        //         * @param integer $year
        //         * @return boolean
        //         */'
        public bool is_leap_year(int year)
        {
            dynamic a = null;
            dynamic returnVal = null;
            a = year;
            if (a % 100 == 0)
            {

                if (a % 400 == 0)
                {
                    returnVal = true;
                }
                else
                {
                    returnVal = false;
                }


            }
            else
            {
                if (a % 4 == 0)
                {
                    returnVal = true;
                }
                else
                {
                    returnVal = false;
                }
            }
            return returnVal;

        }

        public string get_nepali_month(int m)
        {
            dynamic n_month = null;
            n_month = false;

            switch (m)
            {
                case 1:
                    n_month = "Baishak";
                    break;
                case 2:
                    n_month = "Jestha";
                    break;

                case 3:
                    n_month = "Ashad";
                    break;
                case 4:
                    n_month = "Shrawn";
                    break;
                case 5:
                    n_month = "Bhadra";
                    break;
                case 6:
                    n_month = "Ashwin";
                    break;
                case 7:
                    n_month = "Kartik";
                    break;
                case 8:
                    n_month = "Mangshir";
                    break;
                case 9:
                    n_month = "Poush";
                    break;
                case 10:
                    n_month = "Magh";
                    break;
                case 11:
                    n_month = "Falgun";
                    break;
                case 12:
                    n_month = "Chaitra";
                    break;
            }


            return n_month;
        }

        public string get_english_month(int m)
        {
            dynamic eMonth = null;
            eMonth = false;
            switch (m)
            {
                case 1:
                    eMonth = "January";
                    break;
                case 2:
                    eMonth = "February";
                    break;
                case 3:
                    eMonth = "March";
                    break;
                case 4:
                    eMonth = "April";
                    break;
                case 5:
                    eMonth = "May";
                    break;
                case 6:
                    eMonth = "June";
                    break;
                case 7:
                    eMonth = "July";
                    break;
                case 8:
                    eMonth = "August";
                    break;
                case 9:
                    eMonth = "September";
                    break;
                case 10:
                    eMonth = "October";
                    break;
                case 11:
                    eMonth = "November";
                    break;
                case 12:
                    eMonth = "December";
                    break;
            }

            return eMonth;

        }

        public string get_day_of_week(int d)
        {
            dynamic day = null;
            day = false;
            switch (d)
            {
                case 1:
                    day = "Sunday";
                    break;
                case 2:
                    day = "Monday";
                    break;
                case 3:
                    day = "Tuesday";
                    break;
                case 4:
                    day = "Wednesday";
                    break;
                case 5:
                    day = "Thursday";
                    break;
                case 6:
                    day = "Friday";
                    break;
                case 7:
                    day = "Saturday";
                    break;
            }
            return day;


        }

        public bool is_range_eng(int yy, int mm, int dd)
        {
            dynamic returnVal = null;
            returnVal = true;
            if ((yy < 1944 | yy > 2033))
            {
                debug_info = "Supported only between 1944-2022";
                returnVal = false;
            }
            if ((mm < 1 | mm > 12))
            {
                debug_info = "Error! value 1-12 only";
                returnVal = false;
            }
            if ((dd < 1 | dd > 31))
            {
                debug_info = "Error! value 1-31 only";
                returnVal = false;
            }
            return returnVal;
        }

        public bool is_range_nep(int yy, int mm, int dd)
        {
            dynamic returnVal = null;
            returnVal = true;

            if ((yy < 2000 | yy > 2089))
            {
                debug_info = "Supported only between 2000-2089";
                returnVal = false;
            }
            if ((mm < 1 | mm > 12))
            {
                debug_info = "Error! value 1-12 only";
                returnVal = false;
            }

            if ((dd < 1 | dd > 32))
            {
                debug_info = "Error! value 1-32 only";
                returnVal = false;
            }
            return returnVal;
        }

//                /**
//         * currently can only calculate the date between AD 1944-2033...
//         *
//         * @param unknown_type $yy
//         * @param unknown_type $mm
//         * @param unknown_type $dd
//         * @return unknown
//         */

        public void eng_to_nep(int yy, int mm, int dd)
        {
            if ((is_range_eng(yy, mm, dd) == false))
            {
                debug_info = "Out of range";
            }
            else
            {

                //                // english month data.
                int [] month;
                int [] lmonth;
                month = new int[] {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
                lmonth = new int[] {31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

                int def_eyy = 0;
                int def_nyy = 0;
                int def_nmm = 0;
                int def_ndd = 0;
                int total_eDays = 0;
                int total_nDays = 0;
                int a = 0;
                int day = 0;
                int m = 0;
                int y = 0;
                int i = 0;
                int j = 0;
                int numDay = 0;
                def_eyy = 1944;
                //                                    //spear head english date...
                def_nyy = 2000;
                def_nmm = 9;
                def_ndd = 17 - 1;
                //     //spear head nepali date...
                total_eDays = 0;
                total_nDays = 0;
                a = 0;
                day = 7 - 1;
                //    //all the initializations...
                m = 0;
                y = 0;
                i = 0;
                j = 0;
                numDay = 0;

                //// count total no. of days in-terms of year
                // //total days for month calculation...(english)
                for (i = 0; i <= yy - def_eyy - 1; i++)
                {
                    if ((is_leap_year(def_eyy + i) == true))
                    {
                        for (j = 0; j <= 12 - 1; j++)
                        {
                            total_eDays = total_eDays + lmonth[j];
                        }
                    }
                    else
                    {
                        for (j = 0; j <= 12 - 1; j++)
                        {
                            total_eDays = total_eDays + month[j];
                        }
                    }
                }

                //// count total no. of days in-terms of month
                for (i = 0; i <= (mm - 1) - 1; i++)
                {
                    if ((is_leap_year(yy) == true))
                    {
                        total_eDays = total_eDays + lmonth[i];
                    }
                    else
                    {
                        total_eDays = total_eDays + month[i];
                    }
                }

                // // count total no. of days in-terms of date
                total_eDays = total_eDays + dd;


                i = 0;
                j = def_nmm;
                total_nDays = def_ndd;
                m = def_nmm;
                y = def_nyy;

                //// count nepali date from array
                while ((total_eDays != 0))
                {
                    a = bs[i][j];
                    total_nDays = total_nDays + 1;
                    //     //count the days
                    day = day + 1;
                    //                            //count the days interms of 7 days
                    if ((total_nDays > a))
                    {
                        m = m + 1;
                        total_nDays = 1;
                        j = j + 1;
                    }
                    if ((day > 7))
                    {
                        day = 1;
                    }
                    if ((m > 12))
                    {
                        y = y + 1;
                        m = 1;
                    }
                    if ((j > 12))
                    {
                        j = 1;
                        i = i + 1;
                    }
                    total_eDays = total_eDays - 1;

                }
                numDay = day;

                /// 
                nep_date["year"] = y.ToString();
                nep_date["month"] = m.ToString();
                nep_date["date"] = total_nDays.ToString();
                nep_date["day"] = get_day_of_week(day);
                nep_date["nmonth"] = get_nepali_month(m).ToString();
                nep_date["num_day"] = numDay.ToString();
            }

        }

        public void nep_to_eng(int yy, int mm, int dd)
        {
            int def_eyy = 0;
            int def_emm = 0;
            int def_edd = 0;
            int def_nyy = 0;
            int def_nmm = 0;
            int def_ndd = 0;
            int total_eDays = 0;
            int total_nDays = 0;
            int a = 0;
            int day = 0;
            int m = 0;
            int y = 0;
            int i = 0;
            int j = 0;
            int k = 0;
            int numDay = 0;
            int[] month;
            int [] lmonth;

            def_eyy = 1943;
            def_emm = 4;
            def_edd = 14 - 1;
            //// init english date.
            def_nyy = 2000;
            def_nmm = 1;
            def_ndd = 1;
            //        // equivalent nepali date.
            total_eDays = 0;
            total_nDays = 0;
            a = 0;
            day = 4 - 1;
            //     // initializations...
            m = 0;
            y = 0;
            i = 0;
            k = 0;
            numDay = 0;

            month = new int[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            lmonth = new int[] { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            if ((is_range_nep(yy, mm, dd) == false))
            {
                debug_info = "Out of range";
            }
            else
            {

                //// count total days in-terms of year
                for (i = 0; i <= yy - def_nyy - 1; i++)
                {
                    for (j = 1; j <= 12; j++)
                    {
                        total_nDays = total_nDays + bs[k][j];
                    }
                    k = k + 1;
                }


                //// count total days in-terms of month
                for (j = 1; j <= mm - 1; j++)
                {
                    total_nDays = total_nDays + bs[k][j];
                }

                //// count total days in-terms of dat
                total_nDays = total_nDays + dd;

                ////calculation of equivalent english date...
                total_eDays = def_edd;
                m = def_emm;
                y = def_eyy;
                while ((total_nDays != 0))
                {
                    if ((is_leap_year(y)))
                    {

                        a = lmonth[m];

                    }
                    else
                    {

                        a = month[m];
                    }
                    total_eDays = total_eDays + 1;
                    day = day + 1;
                    if ((total_eDays > a))
                    {
                        m = m + 1;
                        total_eDays = 1;
                        if ((m > 12))
                        {
                            y = y + 1;
                            m = 1;
                        }
                    }
                    if ((day > 7))
                        day = 1;
                    total_nDays = total_nDays - 1;
                }
                numDay = day;

                Year = y;
                Month = get_english_month(m);
                Day = get_day_of_week(day);

                eng_date["year"] = y.ToString();
                eng_date["month"] = m.ToString();
                eng_date["date"] = total_eDays.ToString();
                eng_date["day"] = get_day_of_week(day);
                eng_date["emonth"] = get_english_month(m);
                eng_date["num_day"] = numDay.ToString();

                NepalitoEnglish = Day + "/" + Month + "/" + Year; 

            }

        }
    }
}