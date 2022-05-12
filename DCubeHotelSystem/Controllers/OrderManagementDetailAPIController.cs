using DCubeHotelBusinessLayer.OrderManagementBusiness;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class OrderManagementDetailAPIController : BaseAPIController
    {
        private IDCubeRepository<OrderManagementDetail> OrderManagementDetailRepository;

        public OrderManagementDetailAPIController()
        {
            this.OrderManagementDetailRepository = (IDCubeRepository<OrderManagementDetail>)new DCubeRepository<OrderManagementDetail>();
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = OrderManagementBusinesslayer.DeleteOrderManagementDetail(this.OrderManagementDetailRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}