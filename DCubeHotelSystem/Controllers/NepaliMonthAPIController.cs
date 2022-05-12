using DCubeHotelBusinessLayer;
using DCubeHotelSystem.Models;
using System;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class NepaliMonthAPIController : BaseAPIController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var getNepaliMonth = NepaliMonths.listnepalimonth();
            return ToJson(getNepaliMonth);
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string NDate)
        {
            var getEnglishDate = NepalitoEnglishDate.EnglishDate(NDate);
            return ToJson(getEnglishDate);
        }
        [HttpGet]
        public HttpResponseMessage GetNepaliDate([FromUri] string EDate)
        {
            DateTime eDate = Convert.ToDateTime(EDate);
            var getEnglishDate = NepaliDate.NepalitoEnglish.englishToNepali(eDate.Year, eDate.Month, eDate.Day);
            return ToJson(getEnglishDate);
        }
    }
}
