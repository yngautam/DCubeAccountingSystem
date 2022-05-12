using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class FileUploadAPIController : BaseAPIController
    {
        private IDCubeRepository<AccountTransaction> AccountTransactionRepository;
        private IDCubeRepository<MenuItemPhoto> MenuItemPhotoRepository;
        private IDCubeRepository<Company> CompanyRepository;
        private IDCubeRepository<Table> TableRepository;

        public FileUploadAPIController()
        {
            this.AccountTransactionRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.MenuItemPhotoRepository = (IDCubeRepository<MenuItemPhoto>)new DCubeRepository<MenuItemPhoto>();
            this.CompanyRepository = (IDCubeRepository<Company>)new DCubeRepository<Company>();
            this.TableRepository = (IDCubeRepository<Table>)new DCubeRepository<Table>();
        }

        [HttpPost]
        public HttpResponseMessage Post()
        {
            int num = 0;
            HttpRequest request = HttpContext.Current.Request;
            string str1 = HttpContext.Current.Request.Params["moduleName"];
            string Id = HttpContext.Current.Request.Params["id"];
            if (request.Files.Count > 0)
            {
                foreach (string file1 in (NameObjectCollectionBase)request.Files)
                {
                    byte[] buffer = (byte[])null;
                    string str2 = string.Empty;
                    string str3 = string.Empty;
                    HttpPostedFile file2 = request.Files[file1];
                    if (file2.ContentLength > 0)
                    {
                        buffer = new byte[file2.ContentLength];
                        file2.InputStream.Read(buffer, 0, file2.ContentLength);
                        str2 = file2.FileName;
                        str3 = file2.ContentType;
                    }
                    if (buffer != null)
                    {
                        if (str1 == "JournalVoucher")
                        {
                            AccountTransaction accountTransaction1 = new AccountTransaction();
                            AccountTransaction accountTransaction2 = this.AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Id.ToString() == Id)).FirstOrDefault<AccountTransaction>();
                            accountTransaction2.IdentityFileName = str2;
                            accountTransaction2.PhoteIdentity = buffer;
                            accountTransaction2.IdentityFileType = str3;
                            num = AccountTransactionBusiness.Edit(this.AccountTransactionRepository, accountTransaction2);
                        }
                        if (str1 == "Product")
                        {
                            MenuItemPhoto menuItemPhoto1 = new MenuItemPhoto();
                            MenuItemPhoto menuItemPhoto2 = this.MenuItemPhotoRepository.GetAllData().Where<MenuItemPhoto>((Func<MenuItemPhoto, bool>)(o => o.Id.ToString() == Id)).FirstOrDefault<MenuItemPhoto>();
                            menuItemPhoto2.IdentityFileName = str2;
                            menuItemPhoto2.PhoteIdentity = buffer;
                            menuItemPhoto2.IdentityFileType = str3;
                            this.MenuItemPhotoRepository.Update(menuItemPhoto2);
                            this.MenuItemPhotoRepository.Save();
                            num = 1;
                        }
                        if (str1 == "Company")
                        {
                            Company company1 = new Company();
                            Company company2 = this.CompanyRepository.GetAllData().Where<Company>((Func<Company, bool>)(o => o.Id.ToString() == Id)).FirstOrDefault<Company>();
                            company2.IdentityFileName = str2;
                            company2.PhotoIdentity = buffer;
                            company2.IdentityFileType = str3;
                            this.CompanyRepository.Update(company2);
                            this.CompanyRepository.Save();
                            num = 1;
                        }
                        if (str1 == "Table")
                        {
                            Table table1 = new Table();
                            Table table2 = this.TableRepository.GetAllData().Where<Table>((Func<Table, bool>)(o => o.Id.ToString() == Id)).FirstOrDefault<Table>();
                            table2.IdentityFileName = str2;
                            table2.PhoteIdentity = buffer;
                            table2.IdentityFileType = str3;
                            this.TableRepository.Update(table2);
                            this.TableRepository.Save();
                            num = 1;
                        }
                    }
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string Id, [FromUri] string ApplicationModule)
        {
            if (string.IsNullOrEmpty(Id))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            if (ApplicationModule == "JournalVoucher")
            {
                AccountTransaction accountTransaction1 = new AccountTransaction();
                AccountTransaction accountTransaction2 = AccountTransactionBusiness.ScreenAccountTransaction(this.AccountTransactionRepository, Id);
                if (accountTransaction2 != null)
                {
                    byte[] photeIdentity = accountTransaction2.PhoteIdentity;
                    if (photeIdentity != null)
                    {
                        MemoryStream content = new MemoryStream(photeIdentity);
                        httpResponseMessage.Content = (HttpContent)new StreamContent((Stream)content);
                        httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        httpResponseMessage.Content.Headers.ContentDisposition.FileName = accountTransaction2.IdentityFileName;
                        httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(accountTransaction2.IdentityFileType);
                    }
                }
            }
            return httpResponseMessage;
        }
    }
}
