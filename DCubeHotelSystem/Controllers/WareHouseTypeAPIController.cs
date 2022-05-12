using DCubeHotelBusinessLayer.HotelInventoryBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class WareHouseTypeAPIController : BaseAPIController
    {
        private IDCubeRepository<WarehouseType> WareHouseTypeRepository;
        private IDCubeRepository<ExceptionLog> exceptionrepo;

        public WareHouseTypeAPIController()
        {
            this.WareHouseTypeRepository = (IDCubeRepository<WarehouseType>)new DCubeRepository<WarehouseType>();
            this.exceptionrepo = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
        }

        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)WareHouseBusinessLayer.GetWareHouseType(this.WareHouseTypeRepository));

        [HttpGet]
        public HttpResponseMessage Get(int id) => this.ToJson((object)WareHouseBusinessLayer.GetWareHouseType(this.WareHouseTypeRepository, id));

        [HttpPost]
        public HttpResponseMessage Post(WarehouseType value)
        {
            int result = 0;
            result = WareHouseBusinessLayer.InsertWareHouseType(this.WareHouseTypeRepository, this.exceptionrepo, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, WarehouseType value)
        {
            int result = 0;
            result = WareHouseBusinessLayer.UpdateWareHouseType(this.WareHouseTypeRepository, this.exceptionrepo, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteRoomType(int id)
        {
            int result = 0;
            result = WareHouseBusinessLayer.DeleteWareHouseType(this.WareHouseTypeRepository, this.exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public HttpResponseMessage DeleteWharehouseType(int id)
        {
            int result = 0;
            result = WareHouseBusinessLayer.DeleteWareHouseType(this.WareHouseTypeRepository, this.exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
