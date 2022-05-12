using DCubeHotelBusinessLayer.HotelReservationBL;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  public class DepartmentAPIController : BaseAPIController
  {
    private IDCubeRepository<Department> DepartmentRepository;
    private IDCubeRepository<ExceptionLog> exceptionrepo;

    public DepartmentAPIController()
    {
      this.exceptionrepo = (IDCubeRepository<ExceptionLog>) new DCubeRepository<ExceptionLog>();
      this.DepartmentRepository = (IDCubeRepository<Department>) new DCubeRepository<Department>();
    }

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) DepartmentBusinessLayer.GetDepartment(this.DepartmentRepository));

    [HttpGet]
    public HttpResponseMessage Get(int Id) => this.ToJson((object) DepartmentBusinessLayer.GetDepartment(this.DepartmentRepository, Id));

        [HttpPost]
        public HttpResponseMessage Post(Department value)
        {
            int result = 0;
            result = DepartmentBusinessLayer.PostDepartment(DepartmentRepository, exceptionrepo, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Department value)
        {
            int result = 1;
            result = DepartmentBusinessLayer.UpdateDepartment(DepartmentRepository, exceptionrepo, value, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 1;
            result = DepartmentBusinessLayer.DeleteDepartment(DepartmentRepository, exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPost]
        public HttpResponseMessage DeleteDepartment(int id)
        {
            int result = 1;
            result = DepartmentBusinessLayer.DeleteDepartment(DepartmentRepository, exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}