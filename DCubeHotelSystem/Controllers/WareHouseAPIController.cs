using DCubeHotelBusinessLayer.HotelInventoryBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class WareHouseAPIController : BaseAPIController
    {
        private IDCubeRepository<Warehouse> WarehouseRepository;
        private IDCubeRepository<WarehouseType> WarehouseTypeRepository;
        private IDCubeRepository<ExceptionLog> exceptionrepo;

        public WareHouseAPIController()
        {
            this.WarehouseRepository = (IDCubeRepository<Warehouse>)new DCubeRepository<Warehouse>();
            this.WarehouseTypeRepository = (IDCubeRepository<WarehouseType>)new DCubeRepository<WarehouseType>();
            this.exceptionrepo = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
        }

        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)WareHouseBusinessLayer.GetWareHouse(this.WarehouseRepository));

        [HttpGet]
        public HttpResponseMessage Get(int id) => this.ToJson((object)WareHouseBusinessLayer.GetWareHouse(this.WarehouseRepository, id));

        [HttpPost]
        public HttpResponseMessage Post(Warehouse value)
        {
            int result = 0;
            result = WareHouseBusinessLayer.InsertWareHouse(this.WarehouseRepository, this.exceptionrepo, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Warehouse value)
        {
            int result = 0;
            result = WareHouseBusinessLayer.UpdateWareHouse(this.WarehouseRepository, this.exceptionrepo, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public HttpResponseMessage DeleteWareHous(int id)
        {
            int result = 0;
            result = WareHouseBusinessLayer.DeleteWareHouse(this.WarehouseRepository, this.exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}