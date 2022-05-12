using DCubeHotelBusinessLayer.Inventory;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelUser;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class PeriodicConsumptionAPIController : BaseAPIController
    {
        private IDCubeRepository<PeriodicConsumption> PeriodicConsumptionRepository = null;
        private IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository = null;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository = null;
        private IDCubeRepository<WarehouseConsumption> WarehouseComsumptionRepository = null;

        public PeriodicConsumptionAPIController()
        {
            this.PeriodicConsumptionRepository = new DCubeRepository<PeriodicConsumption>();
            this.PeriodicConsumptionItemRepository = new DCubeRepository<PeriodicConsumptionItem>();
            this.AccountTransactionTypeRepository = new DCubeRepository<AccountTransactionType>();
            this.WarehouseComsumptionRepository = new DCubeRepository<WarehouseConsumption>();
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            List<PeriodicConsumption> ListPeriodicConsumption = new List<PeriodicConsumption>();
            ListPeriodicConsumption = PeriodicConsumptionBusines.GetPeriodicConsumptions(PeriodicConsumptionRepository, PeriodicConsumptionItemRepository);
            return ToJson(ListPeriodicConsumption.AsEnumerable());
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string Id)
        {
            PeriodicConsumption objPeriodicConsumption = new PeriodicConsumption();
            objPeriodicConsumption = PeriodicConsumptionBusines.GetPeriodicConsumption(PeriodicConsumptionRepository, PeriodicConsumptionItemRepository, int.Parse(Id));
            return ToJson(objPeriodicConsumption);
        }

        [HttpPost]
        public HttpResponseMessage Post(PeriodicConsumption value)
        {
            int result = 1;
            PeriodicConsumption objPeriodicConsumption = new PeriodicConsumption();
            result = PeriodicConsumptionBusines.Create(PeriodicConsumptionRepository, PeriodicConsumptionItemRepository, WarehouseComsumptionRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, PeriodicConsumption value)
        {
            int result = 1;
            PeriodicConsumption objPeriodicConsumption = new PeriodicConsumption();
            result = PeriodicConsumptionBusines.Edit(PeriodicConsumptionRepository, PeriodicConsumptionItemRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        //public HttpResponseMessage Delete(PeriodicConsumption value)
        //{
        //    int result = 1;
        //    PeriodicConsumption objPeriodicConsumption = new PeriodicConsumption();
        //    result = PeriodicConsumptionBusines.Delete(PeriodicConsumptionRepository, PeriodicConsumptionItemRepository, value);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
        public HttpResponseMessage Delete(int Id)
        {
            int result = 0;
            result = PeriodicConsumptionBusines.Delete(PeriodicConsumptionRepository, PeriodicConsumptionItemRepository, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result); 
        }
    }
}
