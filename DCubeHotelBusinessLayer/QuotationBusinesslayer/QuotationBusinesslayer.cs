using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.QuotationBusinesslayer
{
    public static class QuotationBusinesslayer
    {
        public static List<Quotation> GetQuotations(
          IDCubeRepository<Quotation> Quotationrepository,
          IDCubeRepository<QuotationDetail> QuotationDetailrepository,
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear,
          int BranchId)
        {
            List<Quotation> quotationList1 = new List<Quotation>();
            List<QuotationDetail> quotationDetailList1 = new List<QuotationDetail>();
            List<Quotation> source = new List<Quotation>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    List<Quotation> quotationList2 = BranchId == 0 ? Quotationrepository.GetAllData().Where<Quotation>((Func<Quotation, bool>)(o => o.EnglishDate >= dFrom && o.EnglishDate <= dTo && o.FinancialYear == FinancialYear)).ToList<Quotation>() : Quotationrepository.GetAllData().Where<Quotation>((Func<Quotation, bool>)(o => o.EnglishDate >= dFrom && o.EnglishDate <= dTo && o.FinancialYear == FinancialYear && o.BranchId == BranchId)).ToList<Quotation>();
                    List<QuotationDetail> list1 = QuotationDetailrepository.GetAllData().Where<QuotationDetail>((Func<QuotationDetail, bool>)(o => o.FinancialYear == FinancialYear)).ToList<QuotationDetail>();
                    foreach (Quotation quotation1 in quotationList2)
                    {
                        Quotation objQuotation = quotation1;
                        Quotation quotation2 = new Quotation();
                        Quotation quotation3 = objQuotation;
                        List<QuotationDetail> quotationDetailList2 = new List<QuotationDetail>();
                        List<QuotationDetail> list2 = list1.Where<QuotationDetail>((Func<QuotationDetail, bool>)(o => o.QuotationId == objQuotation.Id)).ToList<QuotationDetail>();
                        quotation3.QuotationDetails = (IEnumerable<QuotationDetail>)list2;
                        source.Add(quotation3);
                    }
                    source = source.OrderBy<Quotation, DateTime>((Func<Quotation, DateTime>)(o => o.EnglishDate)).ToList<Quotation>();
                    return source;
                }
                catch (Exception ex)
                {
                    source = (List<Quotation>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return source;
        }

        public static Quotation GetQuotation(
          IDCubeRepository<Quotation> Quotationrepository,
          IDCubeRepository<QuotationDetail> QuotationDetailrepository,
          int Id)
        {
            Quotation objQuotation = new Quotation();
            List<QuotationDetail> quotationDetailList = new List<QuotationDetail>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    objQuotation = Quotationrepository.GetAllData().Where<Quotation>((Func<Quotation, bool>)(o => o.Id == Id)).FirstOrDefault<Quotation>();
                    if (objQuotation != null)
                    {
                        List<QuotationDetail> list = QuotationDetailrepository.GetAllData().Where<QuotationDetail>((Func<QuotationDetail, bool>)(o => o.QuotationId == objQuotation.Id)).ToList<QuotationDetail>();
                        objQuotation.QuotationDetails = (IEnumerable<QuotationDetail>)list;
                    }
                }
                catch (Exception ex)
                {
                    objQuotation = (Quotation)null;
                }
                unitOfWork.CommitTransaction();
            }
            return objQuotation;
        }

        public static int PostQuotation(
          IDCubeRepository<Quotation> Quotationrepository,
          IDCubeRepository<QuotationDetail> QuotationDetailrepository,
          Quotation value)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    Quotationrepository.Insert(new Quotation()
                    {
                        AccountId = value.AccountId,
                        BranchCode = value.BranchCode,
                        BranchId = value.BranchId,
                        CompanyCode = value.CompanyCode,
                        DepartmentId = value.DepartmentId,
                        EnglishDate = value.EnglishDate,
                        ExpiredEnglishDate = value.ExpiredEnglishDate,
                        ExpiredNepaliDate = value.ExpiredNepaliDate,
                        FinancialYear = value.FinancialYear,
                        Id = value.Id,
                        Message = value.Message,
                        MessageStatement = value.MessageStatement,
                        NepaliDate = value.NepaliDate,
                        QuotationNumber = value.QuotationNumber,
                        WareHouseId = value.WareHouseId
                    });
                    Quotationrepository.Save();
                    int id = Quotationrepository.GetAllData().OrderByDescending<Quotation, int>((Func<Quotation, int>)(x => x.Id)).FirstOrDefault<Quotation>().Id;
                    foreach (QuotationDetail quotationDetail in value.QuotationDetails)
                    {
                        QuotationDetailrepository.Insert(new QuotationDetail()
                        {
                            BranchId = quotationDetail.BranchId,
                            CompanyCode = quotationDetail.CompanyCode,
                            DepartmentId = quotationDetail.DepartmentId,
                            Discount = quotationDetail.Discount,
                            FinancialYear = quotationDetail.FinancialYear,
                            ItemId = quotationDetail.ItemId,
                            Qty = quotationDetail.Qty,
                            QuotationId = id,
                            TotalAmount = quotationDetail.TotalAmount,
                            UnitPrice = quotationDetail.UnitPrice,
                            UnitType = quotationDetail.UnitType,
                            UserName = quotationDetail.UserName,
                            WarehouseId = quotationDetail.WarehouseId
                        });
                        QuotationDetailrepository.Save();
                        num = id;
                    }
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }

        public static int UpdateQuotation(
          IDCubeRepository<Quotation> Quotationrepository,
          IDCubeRepository<QuotationDetail> QuotationDetailrepository,
          int id,
          Quotation value)
        {
            int num = 0;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        Quotationrepository.Update(new Quotation()
                        {
                            AccountId = value.AccountId,
                            BranchCode = value.BranchCode,
                            BranchId = value.BranchId,
                            CompanyCode = value.CompanyCode,
                            DepartmentId = value.DepartmentId,
                            EnglishDate = value.EnglishDate,
                            ExpiredEnglishDate = value.ExpiredEnglishDate,
                            ExpiredNepaliDate = value.ExpiredNepaliDate,
                            FinancialYear = value.FinancialYear,
                            Id = value.Id,
                            Message = value.Message,
                            MessageStatement = value.MessageStatement,
                            NepaliDate = value.NepaliDate,
                            QuotationNumber = value.QuotationNumber,
                            WareHouseId = value.WareHouseId
                        });
                        Quotationrepository.Save();
                        foreach (QuotationDetail quotationDetail in value.QuotationDetails)
                        {
                            if (quotationDetail.Id == 0)
                            {
                                QuotationDetailrepository.Insert(new QuotationDetail()
                                {
                                    BranchId = quotationDetail.BranchId,
                                    CompanyCode = quotationDetail.CompanyCode,
                                    DepartmentId = quotationDetail.DepartmentId,
                                    Discount = quotationDetail.Discount,
                                    FinancialYear = quotationDetail.FinancialYear,
                                    ItemId = quotationDetail.ItemId,
                                    Qty = quotationDetail.Qty,
                                    QuotationId = id,
                                    TotalAmount = quotationDetail.TotalAmount,
                                    UnitPrice = quotationDetail.UnitPrice,
                                    UnitType = quotationDetail.UnitType,
                                    UserName = quotationDetail.UserName,
                                    WarehouseId = quotationDetail.WarehouseId
                                });
                                QuotationDetailrepository.Save();
                                num = id;
                            }
                            else
                            {
                                QuotationDetailrepository.Update(new QuotationDetail()
                                {
                                    BranchId = quotationDetail.BranchId,
                                    CompanyCode = quotationDetail.CompanyCode,
                                    DepartmentId = quotationDetail.DepartmentId,
                                    Discount = quotationDetail.Discount,
                                    FinancialYear = quotationDetail.FinancialYear,
                                    Id = quotationDetail.Id,
                                    ItemId = quotationDetail.ItemId,
                                    Qty = quotationDetail.Qty,
                                    QuotationId = id,
                                    TotalAmount = quotationDetail.TotalAmount,
                                    UnitPrice = quotationDetail.UnitPrice,
                                    UnitType = quotationDetail.UnitType,
                                    UserName = quotationDetail.UserName,
                                    WarehouseId = quotationDetail.WarehouseId
                                });
                                QuotationDetailrepository.Save();
                                num = id;
                            }
                        }
                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollBackTransaction();
                        num = 0;
                    }
                }
            }
            else
                num = 0;
            return num;
        }

        public static int DeleteQuotation(
          IDCubeRepository<Quotation> Quotationrepository,
          IDCubeRepository<QuotationDetail> QuotationDetailrepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    Quotationrepository.Delete((object)id);
                    Quotationrepository.Save();
                    List<QuotationDetail> quotationDetailList = new List<QuotationDetail>();
                    foreach (QuotationDetail quotationDetail in QuotationDetailrepository.GetAllData().Where<QuotationDetail>((Func<QuotationDetail, bool>)(o => o.QuotationId == id)).ToList<QuotationDetail>())
                    {
                        QuotationDetailrepository.Delete((object)quotationDetail.Id);
                        QuotationDetailrepository.Save();
                    }
                    num = 1;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }

        public static int DeleteQuotationDetail(
          IDCubeRepository<QuotationDetail> QuotationDetailrepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    QuotationDetailrepository.Delete((object)id);
                    QuotationDetailrepository.Save();
                    num = 1;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }
    }
}
