using DCubeHotelBusinessLayer.HotelReservationBL;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class TableAPIController : BaseAPIController
    {
        private IDCubeRepository<Table> TableRepository;
        private IDCubeRepository<ExceptionLog> exceptionrepo;

        public TableAPIController()
        {
            this.TableRepository = (IDCubeRepository<Table>)new DCubeRepository<Table>();
            this.exceptionrepo = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
        }

        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)TableBusinessLayer.GetTable(this.TableRepository));

        [HttpPost]
        public HttpResponseMessage Post(Table value)
        {
            int result = 0;
            result = TableBusinessLayer.PostTable(this.TableRepository, this.exceptionrepo, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Table value)
        {
            int result = 0;
            result = TableBusinessLayer.UpdateTable(this.TableRepository, this.exceptionrepo, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = TableBusinessLayer.DeleteTable(this.TableRepository, this.exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result); ;
        }
    }
}
