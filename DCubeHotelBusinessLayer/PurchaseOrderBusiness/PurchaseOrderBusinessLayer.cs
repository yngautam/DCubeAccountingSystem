using DCubeHotelDomain.Models.Inventory;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.PurchaseOrderBusiness
{
    public static class PurchaseOrderBusinessLayer
    {
        public static List<PurchaseOrder> GetPurchaseOrders(
          IDCubeRepository<PurchaseOrder> PurchaseOrderrepository,
          IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailrepository,
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear,
          int BranchId)
        {
            List<PurchaseOrder> purchaseOrderList1 = new List<PurchaseOrder>();
            List<PurchaseOrderDetails> purchaseOrderDetailsList1 = new List<PurchaseOrderDetails>();
            List<PurchaseOrder> source = new List<PurchaseOrder>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    List<PurchaseOrder> purchaseOrderList2 = BranchId == 0 ? PurchaseOrderrepository.GetAllData().Where<PurchaseOrder>((Func<PurchaseOrder, bool>)(o => o.EnglishDate >= dFrom && o.EnglishDate <= dTo && o.FinancialYear == FinancialYear)).ToList<PurchaseOrder>() : PurchaseOrderrepository.GetAllData().Where<PurchaseOrder>((Func<PurchaseOrder, bool>)(o => o.EnglishDate >= dFrom && o.EnglishDate <= dTo && o.FinancialYear == FinancialYear && o.BranchId == BranchId)).ToList<PurchaseOrder>();
                    List<PurchaseOrderDetails> list1 = PurchaseOrderDetailrepository.GetAllData().Where<PurchaseOrderDetails>((Func<PurchaseOrderDetails, bool>)(o => o.FinancialYear == FinancialYear)).ToList<PurchaseOrderDetails>();
                    foreach (PurchaseOrder purchaseOrder1 in purchaseOrderList2)
                    {
                        PurchaseOrder objPurchaseOrder = purchaseOrder1;
                        PurchaseOrder purchaseOrder2 = new PurchaseOrder();
                        PurchaseOrder purchaseOrder3 = objPurchaseOrder;
                        List<PurchaseOrderDetails> purchaseOrderDetailsList2 = new List<PurchaseOrderDetails>();
                        List<PurchaseOrderDetails> list2 = list1.Where<PurchaseOrderDetails>((Func<PurchaseOrderDetails, bool>)(o => o.POId == objPurchaseOrder.Id && o.POId != 0)).ToList<PurchaseOrderDetails>();
                        purchaseOrder3.PurchaseOrderDetails = (IList<PurchaseOrderDetails>)list2;
                        source.Add(purchaseOrder3);
                    }
                    source = source.OrderBy<PurchaseOrder, DateTime>((Func<PurchaseOrder, DateTime>)(o => o.EnglishDate)).ToList<PurchaseOrder>();
                    return source;
                }
                catch (Exception ex)
                {
                    source = (List<PurchaseOrder>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return source;
        }

        public static PurchaseOrder GetPurchaseOrder(
          IDCubeRepository<PurchaseOrder> PurchaseOrderrepository,
          IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailrepository,
          int Id)
        {
            PurchaseOrder objPurchaseOrder = new PurchaseOrder();
            List<PurchaseOrderDetails> purchaseOrderDetailsList = new List<PurchaseOrderDetails>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    objPurchaseOrder = PurchaseOrderrepository.GetAllData().Where<PurchaseOrder>((Func<PurchaseOrder, bool>)(o => o.Id == Id)).FirstOrDefault<PurchaseOrder>();
                    if (objPurchaseOrder != null)
                    {
                        List<PurchaseOrderDetails> list = PurchaseOrderDetailrepository.GetAllData().Where<PurchaseOrderDetails>((Func<PurchaseOrderDetails, bool>)(o => o.POId == objPurchaseOrder.Id && o.POId != 0)).ToList<PurchaseOrderDetails>();
                        objPurchaseOrder.PurchaseOrderDetails = (IList<PurchaseOrderDetails>)list;
                    }
                }
                catch (Exception ex)
                {
                    objPurchaseOrder = (PurchaseOrder)null;
                }
                unitOfWork.CommitTransaction();
            }
            return objPurchaseOrder;
        }

        public static int PostPurchaseOrder(
          IDCubeRepository<PurchaseOrder> PurchaseOrderrepository,
          IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailrepository,
          PurchaseOrder value)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    PurchaseOrderrepository.Insert(new PurchaseOrder()
                    {
                        AccountId = value.AccountId,
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
                        PurchaseOrderNumber = value.PurchaseOrderNumber,
                        WareHouseId = value.WareHouseId
                    });
                    PurchaseOrderrepository.Save();
                    int id = PurchaseOrderrepository.GetAllData().OrderByDescending<PurchaseOrder, int>((Func<PurchaseOrder, int>)(x => x.Id)).FirstOrDefault<PurchaseOrder>().Id;
                    foreach (PurchaseOrderDetails purchaseOrderDetail in (IEnumerable<PurchaseOrderDetails>)value.PurchaseOrderDetails)
                    {
                        PurchaseOrderDetailrepository.Insert(new PurchaseOrderDetails()
                        {
                            BranchId = purchaseOrderDetail.BranchId,
                            CompanyCode = purchaseOrderDetail.CompanyCode,
                            DepartmentId = purchaseOrderDetail.DepartmentId,
                            FinancialYear = purchaseOrderDetail.FinancialYear,
                            ItemId = purchaseOrderDetail.ItemId,
                            Qty = purchaseOrderDetail.Qty,
                            POId = id,
                            TotalAmount = purchaseOrderDetail.TotalAmount,
                            UnitPrice = purchaseOrderDetail.UnitPrice,
                            UnitType = purchaseOrderDetail.UnitType,
                            UserName = purchaseOrderDetail.UserName,
                            WarehouseId = purchaseOrderDetail.WarehouseId
                        });
                        PurchaseOrderDetailrepository.Save();
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

        public static int UpdatePurchaseOrder(
          IDCubeRepository<PurchaseOrder> PurchaseOrderrepository,
          IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailrepository,
          int id,
          PurchaseOrder value)
        {
            int num = 0;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        PurchaseOrderrepository.Update(new PurchaseOrder()
                        {
                            AccountId = value.AccountId,
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
                            PurchaseOrderNumber = value.PurchaseOrderNumber,
                            WareHouseId = value.WareHouseId
                        });
                        PurchaseOrderrepository.Save();
                        foreach (PurchaseOrderDetails purchaseOrderDetail in (IEnumerable<PurchaseOrderDetails>)value.PurchaseOrderDetails)
                        {
                            if (purchaseOrderDetail.PurchaseOrderId == 0)
                            {
                                PurchaseOrderDetailrepository.Insert(new PurchaseOrderDetails()
                                {
                                    BranchId = purchaseOrderDetail.BranchId,
                                    CompanyCode = purchaseOrderDetail.CompanyCode,
                                    DepartmentId = purchaseOrderDetail.DepartmentId,
                                    FinancialYear = purchaseOrderDetail.FinancialYear,
                                    ItemId = purchaseOrderDetail.ItemId,
                                    Qty = purchaseOrderDetail.Qty,
                                    POId = id,
                                    TotalAmount = purchaseOrderDetail.TotalAmount,
                                    UnitPrice = purchaseOrderDetail.UnitPrice,
                                    UnitType = purchaseOrderDetail.UnitType,
                                    UserName = purchaseOrderDetail.UserName,
                                    WarehouseId = purchaseOrderDetail.WarehouseId
                                });
                                PurchaseOrderDetailrepository.Save();
                                num = id;
                            }
                            else
                            {
                                PurchaseOrderDetails purchaseOrderDetails = new PurchaseOrderDetails()
                                {
                                    BranchId = purchaseOrderDetail.BranchId,
                                    CompanyCode = purchaseOrderDetail.CompanyCode,
                                    DepartmentId = purchaseOrderDetail.DepartmentId,
                                    FinancialYear = purchaseOrderDetail.FinancialYear,
                                    PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId,
                                    ItemId = purchaseOrderDetail.ItemId,
                                    Qty = purchaseOrderDetail.Qty
                                };
                                purchaseOrderDetails.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
                                purchaseOrderDetails.POId = id;
                                purchaseOrderDetails.TotalAmount = purchaseOrderDetail.TotalAmount;
                                purchaseOrderDetails.UnitPrice = purchaseOrderDetail.UnitPrice;
                                purchaseOrderDetails.UnitType = purchaseOrderDetail.UnitType;
                                purchaseOrderDetails.UserName = purchaseOrderDetail.UserName;
                                purchaseOrderDetails.WarehouseId = purchaseOrderDetail.WarehouseId;
                                PurchaseOrderDetailrepository.Update(purchaseOrderDetails);
                                PurchaseOrderDetailrepository.Save();
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

        public static int DeletePurchaseOrder(
          IDCubeRepository<PurchaseOrder> PurchaseOrderrepository,
          IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailrepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    PurchaseOrderrepository.Delete((object)id);
                    PurchaseOrderrepository.Save();
                    List<PurchaseOrderDetails> purchaseOrderDetailsList = new List<PurchaseOrderDetails>();
                    foreach (PurchaseOrderDetails purchaseOrderDetails in PurchaseOrderDetailrepository.GetAllData().Where<PurchaseOrderDetails>((Func<PurchaseOrderDetails, bool>)(o => o.POId == id)).ToList<PurchaseOrderDetails>())
                    {
                        PurchaseOrderDetailrepository.Delete((object)purchaseOrderDetails.POId);
                        PurchaseOrderDetailrepository.Save();
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

        public static int DeletePurchaseOrderDetailDetail(
          IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailrepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    PurchaseOrderDetailrepository.Delete((object)id);
                    PurchaseOrderDetailrepository.Save();
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