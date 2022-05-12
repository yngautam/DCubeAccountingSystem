using DCubeHotelBusinessLayer;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  public class DecimalToWordAPIController : BaseAPIController
  {
    [HttpGet]
    public HttpResponseMessage Get(string Amount) => this.ToJson((object) DecimalToWord.ConvertToWords(Amount));
  }
}
