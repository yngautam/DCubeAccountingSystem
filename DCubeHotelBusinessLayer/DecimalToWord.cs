using System;

namespace DCubeHotelBusinessLayer
{
  public static class DecimalToWord
  {
    public static string ConvertToWords(string numb)
    {
      string words = "";
      string Number = numb;
      string str1 = "";
      string str2 = "";
      string str3 = "Only";
      try
      {
        int length = numb.IndexOf(".");
        if (length > 0)
        {
          Number = numb.Substring(0, length);
          string number = numb.Substring(length + 1);
          if (Convert.ToInt32(number) > 0)
          {
            str1 = "and";
            str3 = "Paisa " + str3;
            str2 = DecimalToWord.ConvertDecimals(number);
          }
        }
        words = string.Format("{0} {1}{2} {3}", (object) DecimalToWord.ConvertWholeNumber(Number).Trim(), (object) str1, (object) str2, (object) str3);
      }
      catch
      {
      }
      return words;
    }

    private static string ConvertWholeNumber(string Number)
    {
      string str1 = "";
      try
      {
        bool flag1 = false;
        bool flag2 = false;
        if (Convert.ToDouble(Number) > 0.0)
        {
          flag1 = Number.StartsWith("0");
          int length = Number.Length;
          int num = 0;
          string str2 = "";
          switch (length)
          {
            case 1:
              str1 = DecimalToWord.ones(Number);
              flag2 = true;
              break;
            case 2:
              str1 = DecimalToWord.tens(Number);
              flag2 = true;
              break;
            case 3:
              num = length % 3 + 1;
              str2 = " Hundred ";
              break;
            case 4:
            case 5:
            case 6:
              num = length % 4 + 1;
              str2 = " Thousand ";
              break;
            case 7:
            case 8:
            case 9:
              num = length % 7 + 1;
              str2 = " Lakh ";
              break;
            case 10:
            case 11:
            case 12:
              num = length % 10 + 1;
              str2 = " Crore";
              break;
            default:
              flag2 = true;
              break;
          }
          if (!flag2)
          {
            if (Number.Substring(0, num) != "0" && Number.Substring(num) != "0")
            {
              try
              {
                str1 = DecimalToWord.ConvertWholeNumber(Number.Substring(0, num)) + str2 + DecimalToWord.ConvertWholeNumber(Number.Substring(num));
              }
              catch
              {
              }
            }
            else
              str1 = DecimalToWord.ConvertWholeNumber(Number.Substring(0, num)) + DecimalToWord.ConvertWholeNumber(Number.Substring(num));
          }
          if (str1.Trim().Equals(str2.Trim()))
            str1 = "";
        }
      }
      catch
      {
      }
      return str1.Trim();
    }

    private static string tens(string Number)
    {
      int int32 = Convert.ToInt32(Number);
      string str = (string) null;
      switch (int32)
      {
        case 10:
          str = "Ten";
          break;
        case 11:
          str = "Eleven";
          break;
        case 12:
          str = "Twelve";
          break;
        case 13:
          str = "Thirteen";
          break;
        case 14:
          str = "Fourteen";
          break;
        case 15:
          str = "Fifteen";
          break;
        case 16:
          str = "Sixteen";
          break;
        case 17:
          str = "Seventeen";
          break;
        case 18:
          str = "Eighteen";
          break;
        case 19:
          str = "Nineteen";
          break;
        case 20:
          str = "Twenty";
          break;
        case 30:
          str = "Thirty";
          break;
        case 40:
          str = "Fourty";
          break;
        case 50:
          str = "Fifty";
          break;
        case 60:
          str = "Sixty";
          break;
        case 70:
          str = "Seventy";
          break;
        case 80:
          str = "Eighty";
          break;
        case 90:
          str = "Ninety";
          break;
        default:
          if (int32 > 0)
          {
            str = DecimalToWord.tens(Number.Substring(0, 1) + "0") + " " + DecimalToWord.ones(Number.Substring(1));
            break;
          }
          break;
      }
      return str;
    }

    private static string ones(string Number)
    {
      int int32 = Convert.ToInt32(Number);
      string str = "";
      switch (int32)
      {
        case 1:
          str = "One";
          break;
        case 2:
          str = "Two";
          break;
        case 3:
          str = "Three";
          break;
        case 4:
          str = "Four";
          break;
        case 5:
          str = "Five";
          break;
        case 6:
          str = "Six";
          break;
        case 7:
          str = "Seven";
          break;
        case 8:
          str = "Eight";
          break;
        case 9:
          str = "Nine";
          break;
      }
      return str;
    }

    private static string ConvertDecimals(string number)
    {
      string str1 = "";
      for (int index = 0; index < number.Length; ++index)
      {
        string Number = number[index].ToString();
        string str2 = !Number.Equals("0") ? DecimalToWord.ones(Number) : "Zero";
        str1 = str1 + " " + str2;
      }
      return str1;
    }
  }
}
