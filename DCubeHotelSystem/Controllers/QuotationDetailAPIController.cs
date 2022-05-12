using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class QuotationDetailAPIController : BaseAPIController
    {
        private IDCubeRepository<QuotationDetail> QuotationDetailRepository;

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = DCubeHotelBusinessLayer.QuotationBusinesslayer.QuotationBusinesslayer.DeleteQuotationDetail(this.QuotationDetailRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
