using DCubeHotelSystem.DataContext;
using DCubeHotelSystem.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class BaseAPIController : ApiController
    {
        public HotelDbContext db = new HotelDbContext();
      
        protected HttpResponseMessage ToJson(dynamic obj)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
