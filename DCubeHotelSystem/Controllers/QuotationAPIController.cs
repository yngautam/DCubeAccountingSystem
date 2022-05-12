using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class QuotationAPIController : BaseAPIController
    {
        private IDCubeRepository<Quotation> QuotationRepository;
        private IDCubeRepository<QuotationDetail> QuotationDetailRepository;

        public QuotationAPIController()
        {
            this.QuotationRepository = (IDCubeRepository<Quotation>)new DCubeRepository<Quotation>();
            this.QuotationDetailRepository = (IDCubeRepository<QuotationDetail>)new DCubeRepository<QuotationDetail>();
        }

        [HttpGet]
        public HttpResponseMessage Get(
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear)
        {
            int BranchId = 0;
            List<Quotation> quotationList = new List<Quotation>();
            return this.ToJson((object)DCubeHotelBusinessLayer.QuotationBusinesslayer.QuotationBusinesslayer.GetQuotations(this.QuotationRepository, this.QuotationDetailRepository, dFrom, dTo, FinancialYear, BranchId));
        }

        [HttpGet]
        public HttpResponseMessage Get(
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear,
          string BranchId)
        {
            List<Quotation> quotationList = new List<Quotation>();
            return this.ToJson((object)DCubeHotelBusinessLayer.QuotationBusinesslayer.QuotationBusinesslayer.GetQuotations(this.QuotationRepository, this.QuotationDetailRepository, dFrom, dTo, FinancialYear, int.Parse(BranchId)));
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            Quotation quotation = new Quotation();
            return this.ToJson((object)DCubeHotelBusinessLayer.QuotationBusinesslayer.QuotationBusinesslayer.GetQuotation(this.QuotationRepository, this.QuotationDetailRepository, id));
        }

        [HttpPost]
        public HttpResponseMessage Post(Quotation value)
        {
            int result = 0;
            result = DCubeHotelBusinessLayer.QuotationBusinesslayer.QuotationBusinesslayer.PostQuotation(this.QuotationRepository, this.QuotationDetailRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Quotation value)
        {
            int result = 0;
            result = DCubeHotelBusinessLayer.QuotationBusinesslayer.QuotationBusinesslayer.UpdateQuotation(this.QuotationRepository, this.QuotationDetailRepository, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = DCubeHotelBusinessLayer.QuotationBusinesslayer.QuotationBusinesslayer.DeleteQuotation(this.QuotationRepository, this.QuotationDetailRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}