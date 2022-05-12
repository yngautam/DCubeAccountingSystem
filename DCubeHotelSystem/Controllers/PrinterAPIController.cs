// Decompiled with JetBrains decompiler
// Type: DCubeHotelSystem.Controllers.PrinterAPIController
// Assembly: DCubeHotelSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D65FBD1C-8A72-4F10-8253-AF378855DBF4
// Assembly location: D:\DLL\DCubeHotelSystem.dll

using DCubeHotelDomain.Domain.Models.Settings;
using DCubeHotelDomain.Models;
using DCubeHotelInfrastructureData;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  [UserRoleAuthorize]
  public class PrinterAPIController : BaseAPIController
  {
    private IDCubeRepository<Printer> Printerrepo;

    public PrinterAPIController() => this.Printerrepo = (IDCubeRepository<Printer>) new DCubeRepository<Printer>();

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) this.Printerrepo.GetAllData().OrderBy<Printer, string>((Func<Printer, string>) (o => o.Name)));

    [HttpGet]
    public HttpResponseMessage Get(string Id) => this.ToJson((object) this.Printerrepo.GetAllData().Where<Printer>((Func<Printer, bool>) (o => ((ValueClass) o).Id.ToString() == Id)).FirstOrDefault<Printer>());

    [HttpPost]
    public HttpResponseMessage Post(Printer value)
    {
      int num = 0;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          this.Printerrepo.Insert(value);
          this.Printerrepo.Save();
          num = 1;
        }
        catch (Exception ex)
        {
          this.db.ExceptionLogs.Add(new ExceptionLog()
          {
            ExceptionMessage = ex.Message,
            ExceptionStackTrace = ex.StackTrace,
            ControllerName = ex.Source.ToString(),
            ErrorLogDate = DateTime.Now
          });
          ((DbContext) this.db).SaveChanges();
          num = 0;
          unitOfWork.RollBackTransaction();
          return HttpRequestMessageExtensions.CreateResponse<int>(this.Request, HttpStatusCode.OK, num);
        }
        unitOfWork.CommitTransaction();
      }
      return HttpRequestMessageExtensions.CreateResponse<int>(this.Request, HttpStatusCode.OK, num);
    }

    [HttpPut]
    public HttpResponseMessage Put(int id, Printer value)
    {
      if (id < 1)
        return this.ToJson((object) this.Printerrepo);
      int num = 0;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          this.Printerrepo.Update(value);
          this.Printerrepo.Save();
          num = 1;
        }
        catch (Exception ex)
        {
        }
        unitOfWork.CommitTransaction();
      }
      return HttpRequestMessageExtensions.CreateResponse<int>(this.Request, HttpStatusCode.OK, num);
    }

    [HttpDelete]
    public HttpResponseMessage Delete(int id)
    {
      int num = 0;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          this.Printerrepo.Delete((object) id);
          this.Printerrepo.Save();
          num = 1;
        }
        catch (Exception ex)
        {
        }
        unitOfWork.CommitTransaction();
      }
      return HttpRequestMessageExtensions.CreateResponse<int>(this.Request, HttpStatusCode.OK, num);
    }
  }
}
