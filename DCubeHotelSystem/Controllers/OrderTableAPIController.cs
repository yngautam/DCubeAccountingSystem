using DCubeHotelBusinessLayer.HotelReservationBL;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  [UserRoleAuthorize]
  public class OrderTableAPIController : BaseAPIController
  {
    private DCubeRepository<ScreenTable> ScreenTableRepository;
    private DCubeRepository<Table> TableRepository;
    private IDCubeRepository<Ticket> TicketRepository;

    public OrderTableAPIController()
    {
      this.TicketRepository = (IDCubeRepository<Ticket>) new DCubeRepository<Ticket>();
      this.ScreenTableRepository = new DCubeRepository<ScreenTable>();
      this.TableRepository = new DCubeRepository<Table>();
    }

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) TableBusinessLayer.ListTable((IDCubeRepository<Table>) this.TableRepository, this.TicketRepository));
  }
}
