using System;

namespace DCubeHotelBusinessLayer
{
  public static class NepalitoEnglishDate
  {
    public static DateTime EnglishDate(string NDate)
    {
      DateTime dateTime = new DateTime(2018L);
      try
      {
        dateTime = DateTime.ParseExact(NepalitoEnglish.nepaliToEnglish(int.Parse(NDate.Split(new string[1]
        {
          "."
        }, StringSplitOptions.None)[0]), int.Parse(NDate.Split(new string[1]
        {
          "."
        }, StringSplitOptions.None)[1]), int.Parse(NDate.Split(new string[1]
        {
          "."
        }, StringSplitOptions.None)[2])), "dd/MM/yyyy", (IFormatProvider) null);
      }
      catch (Exception ex)
      {
        string message = ex.Message;
      }
      return dateTime;
    }

    public static string NepaliDate(DateTime NDate) => NepalitoEnglish.englishToNepali(NDate.Year, NDate.Month, NDate.Day);
  }
}
