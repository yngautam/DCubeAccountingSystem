using DCubeHotelBusinessLayer;
using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class OrderInvoiceController : BaseAPIController
    {
        private IDCubeRepository<Ticket> TicketRepository;

        public OrderInvoiceController()
        {
            this.TicketRepository = new DCubeRepository<Ticket>();
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string TicketNo, string InvoiceAmount)
        {
            Ticket ticket = new Ticket();
            Ticket ticketInvoicePrint = TicketBusiness.GetTicketInvoicePrint(int.Parse(TicketNo), this.TicketRepository);
            string nvDate = ticketInvoicePrint.NVDate;
            DateTime now = DateTime.Now;
            int year = now.Year;
            now = DateTime.Now;
            int month = now.Month;
            now = DateTime.Now;
            int day = now.Day;
            string nepali = NepalitoEnglish.englishToNepali(year, month, day);
            string words = DecimalToWord.ConvertToWords(InvoiceAmount);
            int num = ticketInvoicePrint.IS_Bill_Printed ? 1 : 0;
            var data = new
            {
                TDate = nvDate,
                IDate = nepali,
                AmountWord = words,
                BillStatus = num != 0
            };
            return this.ToJson((object)data);
        }
    }
}