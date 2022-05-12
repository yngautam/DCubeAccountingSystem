using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class FinancialYearAPIController : BaseAPIController
    {
        private IDCubeRepository<FinancialYear> FinancialYearRepository;

        public FinancialYearAPIController()
        {
            this.FinancialYearRepository = new DCubeRepository<FinancialYear>();
        }

        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)FinancialYearBusinessLayer.GetFinancialYear(this.FinancialYearRepository));

        [HttpGet]
        public HttpResponseMessage Get(int id) => this.ToJson((object)FinancialYearBusinessLayer.GetFinancialYear(this.FinancialYearRepository, id));

        [HttpPost]
        public HttpResponseMessage Post(FinancialYear value)
        {
            int result = 0;
            result = FinancialYearBusinessLayer.PostFinancialYear(FinancialYearRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, FinancialYear value)
        {
            int result = 1;
            result = FinancialYearBusinessLayer.UpdateFinancialYear(FinancialYearRepository, value, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public HttpResponseMessage DeleteFinancialYear(int id)
        {
            int result = 1;
            result = FinancialYearBusinessLayer.DeleteFinancialYear(FinancialYearRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}