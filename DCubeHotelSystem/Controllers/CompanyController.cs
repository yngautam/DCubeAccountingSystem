using DCubeHotelBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  [UserRoleAuthorize]
  public class CompanyController : BaseAPIController
  {
    private IDCubeRepository<Company> CompanyRepository;
        public CompanyController()
        {
            this.CompanyRepository = new DCubeRepository<Company>();
        }

        [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) CompanyBusinessLayer.GetCompany(this.CompanyRepository));

    [HttpGet]
    public HttpResponseMessage Get(int Id) => this.ToJson((object) CompanyBusinessLayer.GetCompany(this.CompanyRepository, Id));

        [HttpPost]
        public HttpResponseMessage Post(Company value)
        {
            int result = 0;
            result = CompanyBusinessLayer.PostCompany(CompanyRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Company value)
        {
            int result = 1;
            result = CompanyBusinessLayer.UpdateCompany(CompanyRepository, value, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 1;
            result = CompanyBusinessLayer.DeleteCompany(CompanyRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
