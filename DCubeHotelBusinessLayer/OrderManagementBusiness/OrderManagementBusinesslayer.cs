using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.OrderManagementBusiness
{
    public static class OrderManagementBusinesslayer
    {
        public static List<OrderManagement> GetOrderManagements(
          IDCubeRepository<OrderManagement> OrderManagementrepository,
          IDCubeRepository<OrderManagementDetail> OrderManagementDetailrepository,
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear,
          int BranchId)
        {
            List<OrderManagement> orderManagementList1 = new List<OrderManagement>();
            List<OrderManagementDetail> managementDetailList1 = new List<OrderManagementDetail>();
            List<OrderManagement> source = new List<OrderManagement>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    List<OrderManagement> orderManagementList2 = BranchId == 0 ? OrderManagementrepository.GetAllData().Where<OrderManagement>((Func<OrderManagement, bool>)(o => o.EnglishDate >= dFrom && o.EnglishDate <= dTo && o.FinancialYear == FinancialYear)).ToList<OrderManagement>() : OrderManagementrepository.GetAllData().Where<OrderManagement>((Func<OrderManagement, bool>)(o => o.EnglishDate >= dFrom && o.EnglishDate <= dTo && o.FinancialYear == FinancialYear && o.BranchId == BranchId)).ToList<OrderManagement>();
                    List<OrderManagementDetail> list1 = OrderManagementDetailrepository.GetAllData().Where<OrderManagementDetail>((Func<OrderManagementDetail, bool>)(o => o.FinancialYear == FinancialYear)).ToList<OrderManagementDetail>();
                    foreach (OrderManagement orderManagement1 in orderManagementList2)
                    {
                        OrderManagement objOrderManagement = orderManagement1;
                        OrderManagement orderManagement2 = new OrderManagement();
                        OrderManagement orderManagement3 = objOrderManagement;
                        List<OrderManagementDetail> managementDetailList2 = new List<OrderManagementDetail>();
                        List<OrderManagementDetail> list2 = list1.Where<OrderManagementDetail>((Func<OrderManagementDetail, bool>)(o => o.OrderManagementId == objOrderManagement.Id)).ToList<OrderManagementDetail>();
                        orderManagement3.OrderDetails = (IEnumerable<OrderManagementDetail>)list2;
                        source.Add(orderManagement3);
                    }
                    source = source.OrderBy<OrderManagement, DateTime>((Func<OrderManagement, DateTime>)(o => o.EnglishDate)).ToList<OrderManagement>();
                    return source;
                }
                catch (Exception ex)
                {
                    source = (List<OrderManagement>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return source;
        }

        public static OrderManagement GetOrderManagement(
          IDCubeRepository<OrderManagement> OrderManagementrepository,
          IDCubeRepository<OrderManagementDetail> OrderManagementDetailrepository,
          int Id)
        {
            OrderManagement objOrderManagement = new OrderManagement();
            List<OrderManagementDetail> managementDetailList = new List<OrderManagementDetail>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    objOrderManagement = OrderManagementrepository.GetAllData().Where<OrderManagement>((Func<OrderManagement, bool>)(o => o.Id == Id)).FirstOrDefault<OrderManagement>();
                    if (objOrderManagement != null)
                    {
                        List<OrderManagementDetail> list = OrderManagementDetailrepository.GetAllData().Where<OrderManagementDetail>((Func<OrderManagementDetail, bool>)(o => o.OrderManagementId == objOrderManagement.Id)).ToList<OrderManagementDetail>();
                        objOrderManagement.OrderDetails = (IEnumerable<OrderManagementDetail>)list;
                    }
                }
                catch (Exception ex)
                {
                    objOrderManagement = (OrderManagement)null;
                }
                unitOfWork.CommitTransaction();
            }
            return objOrderManagement;
        }

        public static int PostOrderManagement(
          IDCubeRepository<OrderManagement> OrderManagementrepository,
          IDCubeRepository<OrderManagementDetail> OrderManagementDetailrepository,
          OrderManagement value)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    OrderManagementrepository.Insert(new OrderManagement()
                    {
                        AccountId = value.AccountId,
                        BranchCode = value.BranchCode,
                        BranchId = value.BranchId,
                        CompanyCode = value.CompanyCode,
                        DepartmentId = value.DepartmentId,
                        DueEnglishDate = value.DueEnglishDate,
                        DueNepaliDate = value.DueNepaliDate,
                        EnglishDate = value.EnglishDate,
                        FinancialYear = value.FinancialYear,
                        Id = value.Id,
                        Message = value.Message,
                        MessageStatement = value.MessageStatement,
                        NepaliDate = value.NepaliDate,
                        OrderNumber = value.OrderNumber,
                        WareHouseId = value.WareHouseId,
                        WorkDueEnglishDate = value.WorkDueEnglishDate,
                        WorkDueNepaliDate = value.WorkDueNepaliDate
                    });
                    OrderManagementrepository.Save();
                    int id = OrderManagementrepository.GetAllData().OrderByDescending<OrderManagement, int>((Func<OrderManagement, int>)(x => x.Id)).FirstOrDefault<OrderManagement>().Id;
                    foreach (OrderManagementDetail orderDetail in value.OrderDetails)
                    {
                        OrderManagementDetailrepository.Insert(new OrderManagementDetail()
                        {
                            OrderManagementId = id,
                            BranchId = orderDetail.BranchId,
                            CompanyCode = orderDetail.CompanyCode,
                            DepartmentId = orderDetail.DepartmentId,
                            Discount = orderDetail.Discount,
                            FinancialYear = orderDetail.FinancialYear,
                            ItemId = orderDetail.ItemId,
                            Qty = orderDetail.Qty,
                            TotalAmount = orderDetail.TotalAmount,
                            TaxRate = orderDetail.TaxRate,
                            UnitPrice = orderDetail.UnitPrice,
                            UnitType = orderDetail.UnitType,
                            UserName = orderDetail.UserName,
                            WarehouseId = orderDetail.WarehouseId
                        });
                        OrderManagementDetailrepository.Save();
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

        public static int UpdateOrderManagement(
          IDCubeRepository<OrderManagement> OrderManagementrepository,
          IDCubeRepository<OrderManagementDetail> OrderManagementDetailrepository,
          int id,
          OrderManagement value)
        {
            int num = 0;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        OrderManagementrepository.Update(new OrderManagement()
                        {
                            AccountId = value.AccountId,
                            BranchCode = value.BranchCode,
                            BranchId = value.BranchId,
                            CompanyCode = value.CompanyCode,
                            DepartmentId = value.DepartmentId,
                            DueEnglishDate = value.DueEnglishDate,
                            DueNepaliDate = value.DueNepaliDate,
                            EnglishDate = value.EnglishDate,
                            FinancialYear = value.FinancialYear,
                            Id = value.Id,
                            Message = value.Message,
                            MessageStatement = value.MessageStatement,
                            NepaliDate = value.NepaliDate,
                            OrderNumber = value.OrderNumber,
                            WareHouseId = value.WareHouseId,
                            WorkDueEnglishDate = value.WorkDueEnglishDate,
                            WorkDueNepaliDate = value.WorkDueNepaliDate
                        });
                        OrderManagementrepository.Save();
                        foreach (OrderManagementDetail orderDetail in value.OrderDetails)
                        {
                            if (orderDetail.Id == 0)
                            {
                                OrderManagementDetailrepository.Insert(new OrderManagementDetail()
                                {
                                    OrderManagementId = id,
                                    BranchId = orderDetail.BranchId,
                                    CompanyCode = orderDetail.CompanyCode,
                                    DepartmentId = orderDetail.DepartmentId,
                                    Discount = orderDetail.Discount,
                                    FinancialYear = orderDetail.FinancialYear,
                                    ItemId = orderDetail.ItemId,
                                    Qty = orderDetail.Qty,
                                    TotalAmount = orderDetail.TotalAmount,
                                    TaxRate = orderDetail.TaxRate,
                                    UnitPrice = orderDetail.UnitPrice,
                                    UnitType = orderDetail.UnitType,
                                    UserName = orderDetail.UserName,
                                    WarehouseId = orderDetail.WarehouseId
                                });
                                OrderManagementDetailrepository.Save();
                                num = id;
                            }
                            else
                            {
                                OrderManagementDetailrepository.Update(new OrderManagementDetail()
                                {
                                    OrderManagementId = id,
                                    Id = orderDetail.Id,
                                    BranchId = orderDetail.BranchId,
                                    CompanyCode = orderDetail.CompanyCode,
                                    DepartmentId = orderDetail.DepartmentId,
                                    Discount = orderDetail.Discount,
                                    FinancialYear = orderDetail.FinancialYear,
                                    ItemId = orderDetail.ItemId,
                                    Qty = orderDetail.Qty,
                                    TotalAmount = orderDetail.TotalAmount,
                                    TaxRate = orderDetail.TaxRate,
                                    UnitPrice = orderDetail.UnitPrice,
                                    UnitType = orderDetail.UnitType,
                                    UserName = orderDetail.UserName,
                                    WarehouseId = orderDetail.WarehouseId
                                });
                                OrderManagementDetailrepository.Save();
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

        public static int DeleteOrderManagement(
          IDCubeRepository<OrderManagement> OrderManagementrepository,
          IDCubeRepository<OrderManagementDetail> OrderManagementDetailrepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    OrderManagementrepository.Delete((object)id);
                    OrderManagementrepository.Save();
                    List<OrderManagementDetail> managementDetailList = new List<OrderManagementDetail>();
                    foreach (OrderManagementDetail managementDetail in OrderManagementDetailrepository.GetAllData().Where<OrderManagementDetail>((Func<OrderManagementDetail, bool>)(o => o.OrderManagementId == id)).ToList<OrderManagementDetail>())
                    {
                        OrderManagementDetailrepository.Delete((object)managementDetail.Id);
                        OrderManagementDetailrepository.Save();
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

        public static int DeleteOrderManagementDetail(
          IDCubeRepository<OrderManagementDetail> OrderManagementDetailrepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    OrderManagementDetailrepository.Delete((object)id);
                    OrderManagementDetailrepository.Save();
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