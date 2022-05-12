using DCubeHotelBusinessLayer.HotelPOSPaymentBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using NepaliDate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.TicketBusinessLayer
{
    public static class TicketBusiness
    {
        public static ScreenOrderItemResponse OrderMove(
          MoveOrderRequest OrderItemMoveRequest,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          string currentuser,
          string Move)
        {
            string str = "0";
            ScreenOrderItemRequest orderItemRequest = new ScreenOrderItemRequest();
            ScreenOrderItemRequest OrderItemWithoutMoveRequest = new ScreenOrderItemRequest();
            OrderItemWithoutMoveRequest = OrderItemMoveRequest.requestObjectWithoutMovedOrderItem;
            ScreenOrderItemRequest forMovedOrderItem = OrderItemMoveRequest.requestObjectForMovedOrderItem;
            ScreenOrderItemResponse orderItemResponse = new ScreenOrderItemResponse();
            ScreenOrder screenOrder = new ScreenOrder();
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            ScreenOrderDetails screenOrderDetails = new ScreenOrderDetails();
            ScreenTicket screenTicket = new ScreenTicket();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = RepositoryAccount.GetAllData().ToList<Account>();
            using (UnitOfWork uof = new UnitOfWork())
            {
                string TicketNo = "0";
                DateTime now = DateTime.Now;
                uof.StartTransaction();
                Ticket ticket1 = new Ticket();
                Ticket objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == OrderItemWithoutMoveRequest.TicketId)).FirstOrDefault<Ticket>();
                if (objTicket != null)
                {
                    if (TicketBusiness.TicketUpdate(OrderItemWithoutMoveRequest, objTicket, objTicket.Date, TicketRepository, uof) != 0)
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        return orderItemResponse;
                    }
                    if (TicketBusiness.AccountTransactionValueUpdate(OrderItemWithoutMoveRequest, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket.TicketNumber, str) != 0)
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        return orderItemResponse;
                    }
                    string ticketNumber;
                    try
                    {
                        TicketNo = TicketBusiness.TicketLastNumber(TicketRepository);
                        if (TicketBusiness.TicketSave(forMovedOrderItem, TicketRepository, TicketNo, now, int.Parse(str)) == 0)
                        {
                            List<Ticket> list2 = TicketRepository.GetAllData().ToList<Ticket>();
                            ticketNumber = list2.Last<Ticket>().TicketNumber;
                            TicketNo = list2.Last<Ticket>().Id.ToString();
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenOrderItemResponse)null;
                            return orderItemResponse;
                        }
                    }
                    catch (Exception ex)
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        ErrorLog.ErrorLogging(ex);
                        return orderItemResponse;
                    }
                    try
                    {
                        Ticket ticket2 = new Ticket();
                        Ticket ticket3 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == TicketNo)).FirstOrDefault<Ticket>();
                        screenTicket.CustomerId = forMovedOrderItem.CustomerId;
                        screenTicket.Discount = forMovedOrderItem.Discount;
                        screenTicket.ServiceCharge = forMovedOrderItem.ServiceCharge;
                        screenTicket.Id = int.Parse(TicketNo);
                        screenTicket.IsActive = true;
                        screenTicket.isSubmitted = false;
                        screenTicket.Name = TicketNo;
                        screenTicket.Note = ticket3 == null ? "" : ticket3.Note;
                        screenTicket.TableId = int.Parse(forMovedOrderItem.TableId);
                        screenTicket.TicketId = int.Parse(TicketNo);
                        screenTicket.TicketOpeningTime = now;
                        screenTicket.TotalAmount = forMovedOrderItem.Balance;
                        List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                        screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)paymentHistoryList;
                    }
                    catch (Exception ex)
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        ErrorLog.ErrorLogging(ex);
                        return orderItemResponse;
                    }
                    int OrderNo;
                    int id;
                    try
                    {
                        OrderNo = forMovedOrderItem.OrderId != 0 ? forMovedOrderItem.OrderItem.OrderNumber : TicketBusiness.OrderNewNumber(OrderRepository);
                        if (TicketBusiness.OrderUpdate(OrderRepository, forMovedOrderItem, uof, OrderNo, int.Parse(TicketNo), Move) == 0)
                        {
                            id = forMovedOrderItem.OrderItem.Id;
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenOrderItemResponse)null;
                            return orderItemResponse;
                        }
                    }
                    catch (Exception ex)
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        ErrorLog.ErrorLogging(ex);
                        return orderItemResponse;
                    }
                    try
                    {
                        screenOrderDetails.Id = id;
                        screenOrderDetails.IsSelected = false;
                        screenOrderDetails.IsVoid = false;
                        screenOrderDetails.ItemId = forMovedOrderItem.OrderItem.ItemId;
                        screenOrderDetails.OrderId = id;
                        screenOrderDetails.OrderNumber = OrderNo;
                        screenOrderDetails.Qty = forMovedOrderItem.OrderItem.Qty;
                        screenOrderDetails.Tags = forMovedOrderItem.OrderItem.Tags;
                        screenOrderDetails.TotalAmount = Decimal.Parse(forMovedOrderItem.TicketTotal.ToString());
                        screenOrderDetails.UnitPrice = forMovedOrderItem.OrderItem.UnitPrice;
                        screenOrderDetailsList.Add(screenOrderDetails);
                    }
                    catch (Exception ex)
                    {
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        uof.RollBackTransaction();
                        ErrorLog.ErrorLogging(ex);
                        return orderItemResponse;
                    }
                    int iAccountTransactionDocument = TicketBusiness.SaveAccountTransactionDocument(TransactionDocumentRepository, RepositoryAccountType, ticketNumber, now, str, forMovedOrderItem.OrderItem.FinancialYear);
                    if (iAccountTransactionDocument == 0)
                    {
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        uof.RollBackTransaction();
                        return orderItemResponse;
                    }
                    AccountTransactionType accountTransactionType = new AccountTransactionType();
                    AccountTransactionType objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)3);
                    int iAccountTransaction = TicketBusiness.SaveAccountTransaction(objAccountTransactionType, AccountTranastionRepository, forMovedOrderItem, AccountTransactionTypeRepository, iAccountTransactionDocument, ticketNumber);
                    if (iAccountTransaction == 0)
                    {
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        uof.RollBackTransaction();
                        return orderItemResponse;
                    }
                    if (TicketBusiness.SaveAccountTransactionValue(forMovedOrderItem, objAccountTransactionType, AccountTransactionTypeRepository, list1, AccountTransactionValueRepository, iAccountTransactionDocument, iAccountTransaction, now, ticketNumber) != 0)
                    {
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        uof.RollBackTransaction();
                        return orderItemResponse;
                    }
                    Order order = new Order();
                    screenOrder.Id = id;
                    screenOrder.OrderItems = (IEnumerable<ScreenOrderDetails>)screenOrderDetailsList;
                    screenOrder.OrderNumber = OrderNo.ToString();
                    screenOrder.OrderOpeningTime = now.ToString();
                    screenOrder.OrderStatus = forMovedOrderItem.OrderItem.Tags;
                    screenOrder.TableId = forMovedOrderItem.TableId;
                    screenOrder.TicketId = TicketNo;
                    screenOrder.UserId = currentuser;
                    orderItemResponse.CustomerId = forMovedOrderItem.CustomerId;
                    orderItemResponse.Order = screenOrder;
                    orderItemResponse.OrderId = id;
                    orderItemResponse.TableId = forMovedOrderItem.TableId;
                    orderItemResponse.Ticket = screenTicket;
                    orderItemResponse.TicketId = int.Parse(TicketNo);
                    uof.CommitTransaction();
                }
                else
                {
                    uof.RollBackTransaction();
                    orderItemResponse = (ScreenOrderItemResponse)null;
                    return orderItemResponse;
                }
            }
            return orderItemResponse;
        }

        public static ScreenMultiOrderItemResponse OrderSplit(
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          MoveScreenMultiOrderItemRequest objMoveScreenMultiOrderItemRequest)
        {
            string accountdocument = "0";
            ScreenMultiOrderItemResponse orderItemResponse = new ScreenMultiOrderItemResponse();
            ScreenOrder screenOrder = new ScreenOrder();
            List<ScreenOrderDetails> source = new List<ScreenOrderDetails>();
            ScreenTicket screenTicket = new ScreenTicket();
            List<ScreenOrder> screenOrderList = new List<ScreenOrder>();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = RepositoryAccount.GetAllData().ToList<Account>();
            using (UnitOfWork uof = new UnitOfWork())
            {
                string updateType = "decreaseQuantity";
                uof.StartTransaction();
                ScreenOrderItemRequest orderItemRequest1 = new ScreenOrderItemRequest();
                ScreenMultiOrderItemRequest MainOrderItemRequest = new ScreenMultiOrderItemRequest();
                MainOrderItemRequest = objMoveScreenMultiOrderItemRequest.MainOrderItemRequest;
                ScreenOrderItemRequest OrderItemRequest1 = TicketBusiness.screenorderitemrequest(MainOrderItemRequest);
                OrderItemRequest1.CustomerId = 0;
                try
                {
                    Ticket ticket = new Ticket();
                    Ticket objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == MainOrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                    objTicket.TotalAmount = OrderItemRequest1.GrandTotal;
                    if (objTicket != null)
                    {
                        if (TicketBusiness.TicketUpdate(OrderItemRequest1, objTicket, objTicket.Date, TicketRepository, uof) != 0)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                        int OrderNo1 = TicketBusiness.OrderNewNumber(OrderRepository);
                        foreach (ScreenOrderDetails screenOrderDetails in MainOrderItemRequest.ListOrderItem)
                        {
                            ScreenOrderItemRequest OrderItemRequest2 = new ScreenOrderItemRequest();
                            OrderItemRequest2.Balance = MainOrderItemRequest.Balance;
                            OrderItemRequest2.CustomerId = MainOrderItemRequest.CustomerId;
                            OrderItemRequest2.Discount = MainOrderItemRequest.Discount;
                            OrderItemRequest2.FinancialYear = MainOrderItemRequest.FinancialYear;
                            OrderItemRequest2.GrandTotal = MainOrderItemRequest.GrandTotal;
                            OrderItemRequest2.OrderId = MainOrderItemRequest.OrderId;
                            OrderItemRequest2.TableId = MainOrderItemRequest.TableId;
                            OrderItemRequest2.TicketId = MainOrderItemRequest.TicketId;
                            OrderItemRequest2.TicketTotal = MainOrderItemRequest.TicketTotal;
                            OrderItemRequest2.UserId = MainOrderItemRequest.UserId;
                            OrderItemRequest2.VatAmount = MainOrderItemRequest.VatAmount;
                            OrderItemRequest2.OrderItem = screenOrderDetails;
                            if (screenOrderDetails.Id == 0)
                            {
                                if (TicketBusiness.OrderSave(OrderRepository, OrderItemRequest2, objTicket.Id.ToString(), OrderNo1, DateTime.Now) == 0)
                                {
                                    uof.RollBackTransaction();
                                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                    return orderItemResponse;
                                }
                            }
                            else if (TicketBusiness.OrderUpdate(OrderRepository, OrderItemRequest2, uof, OrderItemRequest2.OrderItem.OrderNumber, OrderItemRequest2.TicketId, updateType) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                        }
                        if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest1, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket.TicketNumber, accountdocument) != 0)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                        int num = 0;
                        DateTime now = DateTime.Now;
                        string str1 = "0";
                        ScreenOrderItemRequest orderItemRequest2 = new ScreenOrderItemRequest();
                        ScreenMultiOrderItemRequest orderItemRequest3 = new ScreenMultiOrderItemRequest();
                        ScreenMultiOrderItemRequest orderItemRequest4 = objMoveScreenMultiOrderItemRequest.SplitOrderItemRequest;
                        ScreenOrderItemRequest OrderItemRequest3 = TicketBusiness.screenorderitemrequest(orderItemRequest4);
                        OrderItemRequest3.CustomerId = 0;
                        string ticketNumber;
                        string str2;
                        try
                        {
                            string TicketNo = TicketBusiness.TicketLastNumber(TicketRepository);
                            if (TicketBusiness.TicketSave(OrderItemRequest3, TicketRepository, TicketNo, now, int.Parse(str1)) == 0)
                            {
                                List<Ticket> list2 = TicketRepository.GetAllData().ToList<Ticket>();
                                ticketNumber = list2.Last<Ticket>().TicketNumber;
                                str2 = list2.Last<Ticket>().Id.ToString();
                                screenTicket.CustomerId = OrderItemRequest3.CustomerId;
                                screenTicket.Discount = OrderItemRequest3.Discount;
                                screenTicket.ServiceCharge = OrderItemRequest3.ServiceCharge;
                                screenTicket.Id = int.Parse(str2);
                                screenTicket.IsActive = true;
                                screenTicket.isSubmitted = false;
                                screenTicket.Name = str2;
                                screenTicket.Note = "";
                                screenTicket.TableId = int.Parse(OrderItemRequest3.TableId);
                                screenTicket.TicketId = int.Parse(str2);
                                screenTicket.TicketOpeningTime = now;
                                screenTicket.TotalAmount = OrderItemRequest3.Balance;
                                screenTicket.FinancialYear = OrderItemRequest3.FinancialYear;
                                screenTicket.UserName = OrderItemRequest3.UserId;
                            }
                            else
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                        }
                        catch (Exception ex)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                        int OrderNo2 = TicketBusiness.OrderNewNumber(OrderRepository);
                        foreach (ScreenOrderDetails screenOrderDetails in orderItemRequest4.ListOrderItem)
                        {
                            ScreenOrderItemRequest OrderItemRequest4 = new ScreenOrderItemRequest();
                            OrderItemRequest4.Balance = orderItemRequest4.Balance;
                            OrderItemRequest4.CustomerId = orderItemRequest4.CustomerId;
                            OrderItemRequest4.Discount = orderItemRequest4.Discount;
                            OrderItemRequest4.FinancialYear = orderItemRequest4.FinancialYear;
                            OrderItemRequest4.GrandTotal = orderItemRequest4.GrandTotal;
                            OrderItemRequest4.OrderId = orderItemRequest4.OrderId;
                            OrderItemRequest4.TableId = orderItemRequest4.TableId;
                            OrderItemRequest4.TicketId = int.Parse(str2);
                            OrderItemRequest4.TicketTotal = orderItemRequest4.TicketTotal;
                            OrderItemRequest4.UserId = orderItemRequest4.UserId;
                            OrderItemRequest4.VatAmount = orderItemRequest4.VatAmount;
                            OrderItemRequest4.OrderItem = screenOrderDetails;
                            if (screenOrderDetails.OrderId == 0 && screenOrderDetails.OrderNumber == 0)
                            {
                                num = TicketBusiness.OrderSave(OrderRepository, OrderItemRequest4, str2, OrderNo2, now);
                                if (num == 0)
                                {
                                    uof.RollBackTransaction();
                                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                    return orderItemResponse;
                                }
                            }
                            else if (TicketBusiness.OrderUpdate(OrderRepository, OrderItemRequest4, uof, screenOrderDetails.OrderNumber, OrderItemRequest4.TicketId, updateType) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                            source.Add(new ScreenOrderDetails()
                            {
                                Id = num,
                                IsSelected = false,
                                IsVoid = screenOrderDetails.IsVoid,
                                ItemId = screenOrderDetails.ItemId,
                                OrderId = num,
                                OrderNumber = OrderNo2,
                                OrderDescription = screenOrderDetails.OrderDescription,
                                Qty = screenOrderDetails.Qty,
                                Tags = screenOrderDetails.Tags,
                                TotalAmount = Decimal.Parse(screenOrderDetails.TotalAmount.ToString()),
                                UnitPrice = screenOrderDetails.UnitPrice,
                                UserId = screenOrderDetails.UserId,
                                FinancialYear = screenOrderDetails.FinancialYear,
                                DepartmentId = screenOrderDetails.DepartmentId
                            });
                        }
                        screenOrder.Id = num;
                        screenOrder.OrderItems = (IEnumerable<ScreenOrderDetails>)source;
                        screenOrder.OrderNumber = OrderNo1.ToString();
                        screenOrder.OrderOpeningTime = now.ToString();
                        screenOrder.OrderStatus = source.First<ScreenOrderDetails>().Tags;
                        screenOrder.TableId = OrderItemRequest3.TableId;
                        screenOrder.TicketId = str2;
                        screenOrder.UserId = source.First<ScreenOrderDetails>().UserId;
                        screenOrderList.Add(screenOrder);
                        orderItemResponse.CustomerId = OrderItemRequest1.CustomerId;
                        orderItemResponse.ListOrder = screenOrderList;
                        orderItemResponse.OrderId = num;
                        orderItemResponse.TableId = OrderItemRequest1.TableId;
                        orderItemResponse.Ticket = screenTicket;
                        orderItemResponse.TicketId = int.Parse(str2);
                        int iAccountTransactionDocument = TicketBusiness.SaveAccountTransactionDocument(TransactionDocumentRepository, RepositoryAccountType, ticketNumber, now, str1, OrderItemRequest3.OrderItem.FinancialYear);
                        if (iAccountTransactionDocument == 0)
                        {
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse;
                        }
                        AccountTransactionType accountTransactionType = new AccountTransactionType();
                        AccountTransactionType objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)3);
                        int iAccountTransaction = TicketBusiness.SaveAccountTransaction(objAccountTransactionType, AccountTranastionRepository, OrderItemRequest3, AccountTransactionTypeRepository, iAccountTransactionDocument, ticketNumber);
                        if (iAccountTransaction == 0)
                        {
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse;
                        }
                        if (TicketBusiness.SaveAccountTransactionValue(OrderItemRequest3, objAccountTransactionType, AccountTransactionTypeRepository, list1, AccountTransactionValueRepository, iAccountTransactionDocument, iAccountTransaction, now, ticketNumber) != 0)
                        {
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse;
                        }
                        uof.CommitTransaction();
                    }
                    else
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenMultiOrderItemResponse)null;
                        return orderItemResponse;
                    }
                }
                catch (Exception ex)
                {
                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse;
                }
            }
            return orderItemResponse;
        }

        public static ScreenMultiOrderItemResponse OrderMergePartial(
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          MoveScreenMultiOrderItemRequest objMoveScreenMultiOrderItemRequest)
        {
            string accountdocument = "0";
            ScreenMultiOrderItemResponse orderItemResponse = new ScreenMultiOrderItemResponse();
            ScreenOrder screenOrder = new ScreenOrder();
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            ScreenTicket screenTicket = new ScreenTicket();
            List<ScreenOrder> screenOrderList = new List<ScreenOrder>();
            using (UnitOfWork uof = new UnitOfWork())
            {
                string updateType = "decreaseQuantity";
                uof.StartTransaction();
                ScreenOrderItemRequest orderItemRequest1 = new ScreenOrderItemRequest();
                ScreenMultiOrderItemRequest MainOrderItemRequest = new ScreenMultiOrderItemRequest();
                MainOrderItemRequest = objMoveScreenMultiOrderItemRequest.MainOrderItemRequest;
                ScreenOrderItemRequest OrderItemRequest1 = TicketBusiness.screenorderitemrequest(MainOrderItemRequest);
                OrderItemRequest1.CustomerId = 0;
                try
                {
                    Ticket ticket1 = new Ticket();
                    Ticket objTicket1 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == MainOrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                    if (objTicket1 != null)
                    {
                        if (TicketBusiness.TicketUpdate(OrderItemRequest1, objTicket1, objTicket1.Date, TicketRepository, uof) != 0)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                        int OrderNo1 = TicketBusiness.OrderNewNumber(OrderRepository);
                        foreach (ScreenOrderDetails screenOrderDetails in MainOrderItemRequest.ListOrderItem)
                        {
                            ScreenOrderItemRequest OrderItemRequest2 = new ScreenOrderItemRequest();
                            OrderItemRequest2.Balance = MainOrderItemRequest.Balance;
                            OrderItemRequest2.CustomerId = MainOrderItemRequest.CustomerId;
                            OrderItemRequest2.Discount = MainOrderItemRequest.Discount;
                            OrderItemRequest2.FinancialYear = MainOrderItemRequest.FinancialYear;
                            OrderItemRequest2.GrandTotal = MainOrderItemRequest.GrandTotal;
                            OrderItemRequest2.OrderId = MainOrderItemRequest.OrderId;
                            OrderItemRequest2.TableId = MainOrderItemRequest.TableId;
                            OrderItemRequest2.TicketId = MainOrderItemRequest.TicketId;
                            OrderItemRequest2.TicketTotal = MainOrderItemRequest.TicketTotal;
                            OrderItemRequest2.UserId = MainOrderItemRequest.UserId;
                            OrderItemRequest2.VatAmount = MainOrderItemRequest.VatAmount;
                            OrderItemRequest2.OrderItem = screenOrderDetails;
                            if (screenOrderDetails.Id == 0)
                            {
                                if (TicketBusiness.OrderSave(OrderRepository, OrderItemRequest2, objTicket1.Id.ToString(), OrderNo1, DateTime.Now) == 0)
                                {
                                    uof.RollBackTransaction();
                                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                    return orderItemResponse;
                                }
                            }
                            else if (TicketBusiness.OrderUpdate(OrderRepository, OrderItemRequest2, uof, OrderItemRequest2.OrderItem.OrderNumber, OrderItemRequest2.TicketId, updateType) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                        }
                        if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest1, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket1.TicketNumber, accountdocument) != 0)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                        try
                        {
                            ScreenOrderItemRequest orderItemRequest2 = new ScreenOrderItemRequest();
                            ScreenMultiOrderItemRequest PartialOrderItemRequest = new ScreenMultiOrderItemRequest();
                            PartialOrderItemRequest = objMoveScreenMultiOrderItemRequest.SplitOrderItemRequest;
                            ScreenOrderItemRequest OrderItemRequest3 = TicketBusiness.screenorderitemrequest(PartialOrderItemRequest);
                            OrderItemRequest3.CustomerId = 0;
                            Ticket ticket2 = new Ticket();
                            Ticket objTicket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == PartialOrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                            if (objTicket2 != null)
                            {
                                if (TicketBusiness.TicketUpdate(OrderItemRequest3, objTicket2, objTicket2.Date, TicketRepository, uof) == 0)
                                {
                                    screenTicket.CustomerId = OrderItemRequest3.CustomerId;
                                    screenTicket.Discount = OrderItemRequest3.Discount;
                                    screenTicket.ServiceCharge = OrderItemRequest3.ServiceCharge;
                                    screenTicket.Id = objTicket2.Id;
                                    screenTicket.IsActive = true;
                                    screenTicket.isSubmitted = false;
                                    screenTicket.Name = objTicket2.TicketNumber;
                                    screenTicket.Note = objTicket1 == null ? "" : objTicket2.Note;
                                    screenTicket.TableId = int.Parse(OrderItemRequest3.TableId);
                                    screenTicket.TicketId = OrderItemRequest3.TicketId;
                                    screenTicket.TicketOpeningTime = objTicket1.Date;
                                    screenTicket.TotalAmount = OrderItemRequest3.TicketTotal;
                                    List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                                    List<PaymentHistory> ticketPaymentHistory = TicketBusiness.GetTicketPaymentHistory(TicketRepository, RepositoryAccountType, TransactionDocumentRepository, AccountTransactionTypeRepository, AccountTransactionValueRepository, objTicket2.Id);
                                    screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)ticketPaymentHistory;
                                    screenTicket.FinancialYear = OrderItemRequest3.FinancialYear;
                                    screenTicket.UserName = OrderItemRequest3.UserId;
                                    int OrderNo2 = TicketBusiness.OrderNewNumber(OrderRepository);
                                    foreach (ScreenOrderDetails screenOrderDetails1 in PartialOrderItemRequest.ListOrderItem)
                                    {
                                        ScreenOrderItemRequest OrderItemRequest4 = new ScreenOrderItemRequest();
                                        OrderItemRequest4.Balance = PartialOrderItemRequest.Balance;
                                        OrderItemRequest4.CustomerId = PartialOrderItemRequest.CustomerId;
                                        OrderItemRequest4.Discount = PartialOrderItemRequest.Discount;
                                        OrderItemRequest4.FinancialYear = PartialOrderItemRequest.FinancialYear;
                                        OrderItemRequest4.GrandTotal = PartialOrderItemRequest.GrandTotal;
                                        OrderItemRequest4.OrderId = PartialOrderItemRequest.OrderId;
                                        OrderItemRequest4.TableId = PartialOrderItemRequest.TableId;
                                        OrderItemRequest4.TicketId = PartialOrderItemRequest.TicketId;
                                        OrderItemRequest4.TicketTotal = PartialOrderItemRequest.TicketTotal;
                                        OrderItemRequest4.UserId = PartialOrderItemRequest.UserId;
                                        OrderItemRequest4.VatAmount = PartialOrderItemRequest.VatAmount;
                                        OrderItemRequest4.OrderItem = screenOrderDetails1;
                                        ScreenOrderDetails screenOrderDetails2 = new ScreenOrderDetails();
                                        if (screenOrderDetails1.OrderId == 0 && screenOrderDetails1.OrderNumber == 0)
                                        {
                                            int num = TicketBusiness.OrderSave(OrderRepository, OrderItemRequest4, objTicket2.Id.ToString(), OrderNo2, DateTime.Now);
                                            screenOrderDetails2.OrderId = num;
                                            screenOrderDetails2.OrderNumber = OrderNo2;
                                            if (num == 0)
                                            {
                                                uof.RollBackTransaction();
                                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                                return orderItemResponse;
                                            }
                                        }
                                        else if (TicketBusiness.OrderUpdate(OrderRepository, OrderItemRequest4, uof, OrderItemRequest4.OrderItem.OrderNumber, OrderItemRequest4.TicketId, updateType) == 0)
                                        {
                                            screenOrderDetails2.OrderId = screenOrderDetails1.OrderId;
                                            screenOrderDetails2.OrderNumber = screenOrderDetails1.OrderNumber;
                                        }
                                        else
                                        {
                                            uof.RollBackTransaction();
                                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                            return orderItemResponse;
                                        }
                                        screenOrderDetails2.Id = screenOrderDetails1.Id;
                                        screenOrderDetails2.IsSelected = false;
                                        screenOrderDetails2.IsVoid = screenOrderDetails1.IsVoid;
                                        screenOrderDetails2.ItemId = screenOrderDetails1.ItemId;
                                        screenOrderDetails2.Qty = screenOrderDetails1.Qty;
                                        screenOrderDetails2.Tags = screenOrderDetails1.Tags;
                                        screenOrderDetails2.TotalAmount = screenOrderDetails1.TotalAmount;
                                        screenOrderDetails2.UnitPrice = screenOrderDetails1.UnitPrice;
                                        screenOrderDetailsList.Add(screenOrderDetails2);
                                    }
                                    if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest3, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket2.TicketNumber, accountdocument) != 0)
                                    {
                                        uof.RollBackTransaction();
                                        orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                        return orderItemResponse;
                                    }
                                    screenOrder.Id = OrderNo2;
                                    screenOrder.OrderItems = (IEnumerable<ScreenOrderDetails>)screenOrderDetailsList;
                                    screenOrder.OrderNumber = OrderNo2.ToString();
                                    screenOrder.OrderOpeningTime = objTicket2.Date.ToString();
                                    screenOrder.OrderStatus = PartialOrderItemRequest.ListOrderItem.First<ScreenOrderDetails>().Tags;
                                    screenOrder.TableId = objTicket2.Table_Customer_Room.ToString();
                                    screenOrder.TicketId = objTicket2.Id.ToString();
                                    screenOrder.UserId = PartialOrderItemRequest.ListOrderItem.First<ScreenOrderDetails>().UserId;
                                    screenOrderList.Add(screenOrder);
                                    orderItemResponse.CustomerId = PartialOrderItemRequest.CustomerId;
                                    orderItemResponse.ListOrder = screenOrderList;
                                    orderItemResponse.OrderId = PartialOrderItemRequest.ListOrderItem.First<ScreenOrderDetails>().OrderNumber;
                                    orderItemResponse.TableId = PartialOrderItemRequest.TableId;
                                    orderItemResponse.Ticket = screenTicket;
                                    orderItemResponse.TicketId = PartialOrderItemRequest.TicketId;
                                }
                                else
                                {
                                    uof.RollBackTransaction();
                                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                    return orderItemResponse;
                                }
                            }
                            uof.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                    }
                    else
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenMultiOrderItemResponse)null;
                        return orderItemResponse;
                    }
                }
                catch (Exception ex)
                {
                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                    uof.RollBackTransaction();
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse;
                }
            }
            return orderItemResponse;
        }

        public static ScreenMultiOrderItemResponse OrderMergeFull(
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          ScreenMultiOrderItemRequest OrderItemRequest,
          string TicketId)
        {
            string accountdocument = "0";
            ScreenMultiOrderItemResponse orderItemResponse = new ScreenMultiOrderItemResponse();
            ScreenOrder screenOrder = new ScreenOrder();
            List<ScreenOrderDetails> source = new List<ScreenOrderDetails>();
            ScreenTicket screenTicket = new ScreenTicket();
            List<ScreenOrder> screenOrderList = new List<ScreenOrder>();
            using (UnitOfWork uof = new UnitOfWork())
            {
                uof.StartTransaction();
                ScreenOrderItemRequest orderItemRequest = new ScreenOrderItemRequest();
                ScreenOrderItemRequest OrderItemRequest1 = TicketBusiness.screenorderitemrequest(OrderItemRequest);
                OrderItemRequest1.CustomerId = 0;
                try
                {
                    Ticket ticket1 = new Ticket();
                    Ticket objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == OrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                    if (objTicket != null)
                    {
                        if (TicketBusiness.TicketUpdate(OrderItemRequest1, objTicket, objTicket.Date, TicketRepository, uof) == 0)
                        {
                            screenTicket.CustomerId = OrderItemRequest1.CustomerId;
                            screenTicket.Discount = OrderItemRequest1.Discount;
                            screenTicket.ServiceCharge = OrderItemRequest1.ServiceCharge;
                            screenTicket.Id = objTicket.Id;
                            screenTicket.IsActive = true;
                            screenTicket.isSubmitted = false;
                            screenTicket.Name = objTicket.TicketNumber;
                            screenTicket.Note = objTicket == null ? "" : objTicket.Note;
                            screenTicket.TableId = int.Parse(OrderItemRequest.TableId);
                            screenTicket.TicketId = OrderItemRequest.TicketId;
                            screenTicket.TicketOpeningTime = objTicket.Date;
                            screenTicket.TotalAmount = OrderItemRequest.TicketTotal;
                            List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                            List<PaymentHistory> ticketPaymentHistory = TicketBusiness.GetTicketPaymentHistory(TicketRepository, RepositoryAccountType, TransactionDocumentRepository, AccountTransactionTypeRepository, AccountTransactionValueRepository, objTicket.Id);
                            screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)ticketPaymentHistory;
                            List<Order> orderList = new List<Order>();
                            List<Order> list = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.TicketId.ToString() == TicketId)).ToList<Order>();
                            if (list != null)
                            {
                                foreach (Order order in list)
                                {
                                    order.TicketId = OrderItemRequest.TicketId;
                                    order.CreatingUserName = OrderItemRequest.UserId;
                                    OrderRepository.Update(order);
                                    OrderRepository.Save();
                                    source.Add(new ScreenOrderDetails()
                                    {
                                        Id = order.Id,
                                        IsSelected = false,
                                        IsVoid = order.OrderStates == "Void",
                                        ItemId = order.MenuItemId,
                                        OrderId = order.Id,
                                        OrderNumber = order.OrderNumber,
                                        Qty = order.Quantity,
                                        Tags = order.Tag,
                                        TotalAmount = order.Quantity * order.Price,
                                        UnitPrice = order.Price
                                    });
                                }
                                screenOrder.Id = list.First<Order>().OrderNumber;
                                screenOrder.OrderItems = (IEnumerable<ScreenOrderDetails>)source;
                                screenOrder.OrderNumber = list.First<Order>().OrderNumber.ToString();
                                screenOrder.OrderOpeningTime = list.First<Order>().CreatedDateTime.ToString();
                                screenOrder.OrderStatus = list.First<Order>().Tag;
                                screenOrder.TableId = OrderItemRequest.TableId;
                                screenOrder.TicketId = OrderItemRequest.TicketId.ToString();
                                screenOrder.UserId = OrderItemRequest.UserId;
                                screenOrderList.Add(screenOrder);
                                orderItemResponse.CustomerId = OrderItemRequest.CustomerId;
                                orderItemResponse.ListOrder = screenOrderList;
                                orderItemResponse.OrderId = list.First<Order>().OrderNumber;
                                orderItemResponse.TableId = OrderItemRequest.TableId;
                                orderItemResponse.Ticket = screenTicket;
                                orderItemResponse.TicketId = OrderItemRequest.TicketId;
                            }
                            Ticket ticket2 = new Ticket();
                            Ticket ticket3 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id.ToString() == TicketId)).FirstOrDefault<Ticket>();
                            string ticketNumber = ticket3.TicketNumber;
                            try
                            {
                                if (ticket3 != null)
                                {
                                    ticket3.TotalAmount = 0M;
                                    ticket3.IsClosed = true;
                                    ticket3.IS_Bill_Printed = true;
                                    ticket3.IsLocked = true;
                                    ticket3.RemainingAmount = 0M;
                                    ticket3.TicketStates = "Full Merge with " + (object)OrderItemRequest.TicketId;
                                    TicketRepository.Update(ticket3);
                                    TicketRepository.Save();
                                }
                                else
                                {
                                    uof.RollBackTransaction();
                                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                    return orderItemResponse;
                                }
                            }
                            catch (Exception ex)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                            if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest1, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket.TicketNumber, accountdocument) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                            if (TicketBusiness.AccountTransactionValueUpdate(new ScreenOrderItemRequest()
                            {
                                Balance = 0M,
                                Discount = 0M,
                                GrandTotal = 0M,
                                ServiceCharge = 0M,
                                TicketTotal = 0M,
                                VatAmount = 0M,
                                OrderItem = source.FirstOrDefault<ScreenOrderDetails>()
                            }, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, ticketNumber, accountdocument) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                            uof.CommitTransaction();
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                    }
                    else
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenMultiOrderItemResponse)null;
                        return orderItemResponse;
                    }
                }
                catch (Exception ex)
                {
                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse;
                }
            }
            return orderItemResponse;
        }

        public static ScreenMultiOrderItemResponse OrderVoid(
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          ScreenMultiOrderItemRequest OrderItemRequest,
          string TicketId)
        {
            string accountdocument = "0";
            ScreenMultiOrderItemResponse orderItemResponse = new ScreenMultiOrderItemResponse();
            ScreenOrder screenOrder1 = new ScreenOrder();
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            ScreenTicket screenTicket = new ScreenTicket();
            List<ScreenOrder> screenOrderList = new List<ScreenOrder>();
            using (UnitOfWork uof = new UnitOfWork())
            {
                uof.StartTransaction();
                ScreenOrderItemRequest orderItemRequest = new ScreenOrderItemRequest();
                ScreenOrderItemRequest OrderItemRequest1 = TicketBusiness.screenorderitemrequest(OrderItemRequest);
                try
                {
                    Ticket ticket = new Ticket();
                    Ticket objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == OrderItemRequest.TicketId.ToString() && o.FinancialYear == OrderItemRequest.FinancialYear)).FirstOrDefault<Ticket>();
                    if (objTicket != null)
                    {
                        objTicket.IsClosed = true;
                        objTicket.IsLocked = true;
                        objTicket.TicketStates = "Void";
                        if (objTicket.TicketTypeId == 1)
                            OrderItemRequest1.TableId = "0";
                        if (objTicket.TicketTypeId == 2)
                            OrderItemRequest1.CustomerId = 0;
                        if (TicketBusiness.TicketUpdate(OrderItemRequest1, objTicket, objTicket.Date, TicketRepository, uof) == 0)
                        {
                            screenTicket.CustomerId = OrderItemRequest1.CustomerId;
                            screenTicket.Discount = OrderItemRequest1.Discount;
                            screenTicket.ServiceCharge = OrderItemRequest1.ServiceCharge;
                            screenTicket.Id = objTicket.Id;
                            screenTicket.IsActive = true;
                            screenTicket.isSubmitted = false;
                            screenTicket.Name = objTicket.TicketNumber;
                            screenTicket.Note = objTicket == null ? "" : objTicket.Note;
                            screenTicket.TableId = int.Parse(OrderItemRequest.TableId);
                            screenTicket.TicketId = OrderItemRequest.TicketId;
                            screenTicket.TicketOpeningTime = objTicket.Date;
                            screenTicket.TotalAmount = OrderItemRequest.TicketTotal;
                            List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                            List<PaymentHistory> ticketPaymentHistory = TicketBusiness.GetTicketPaymentHistory(TicketRepository, RepositoryAccountType, TransactionDocumentRepository, AccountTransactionTypeRepository, AccountTransactionValueRepository, objTicket.Id);
                            screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)ticketPaymentHistory;
                            if (OrderItemRequest.ListOrderItem != null)
                            {
                                foreach (ScreenOrderDetails screenOrderDetails in OrderItemRequest.ListOrderItem)
                                {
                                    ScreenOrderDetails ObjOrder = screenOrderDetails;
                                    Order order1 = new Order();
                                    Order order2 = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o =>
                                   {
                                       int id = o.Id;
                                       string str1 = id.ToString();
                                       id = ObjOrder.Id;
                                       string str2 = id.ToString();
                                       return str1 == str2;
                                   })).FirstOrDefault<Order>();
                                    if (order2 != null)
                                    {
                                        order2.OrderStates = "Void";
                                        OrderRepository.Update(order2);
                                        OrderRepository.Save();
                                    }
                                    screenOrderDetailsList.Add(new ScreenOrderDetails()
                                    {
                                        Id = ObjOrder.Id,
                                        IsSelected = false,
                                        IsVoid = ObjOrder.IsVoid,
                                        ItemId = ObjOrder.ItemId,
                                        OrderId = ObjOrder.Id,
                                        OrderNumber = ObjOrder.OrderNumber,
                                        Qty = order2.Quantity,
                                        Tags = ObjOrder.Tags,
                                        TotalAmount = ObjOrder.Qty * ObjOrder.UnitPrice,
                                        UnitPrice = ObjOrder.UnitPrice
                                    });
                                }
                                screenOrder1.Id = OrderItemRequest.ListOrderItem.First<ScreenOrderDetails>().OrderNumber;
                                screenOrder1.OrderItems = (IEnumerable<ScreenOrderDetails>)screenOrderDetailsList;
                                ScreenOrder screenOrder2 = screenOrder1;
                                int num = OrderItemRequest.ListOrderItem.First<ScreenOrderDetails>().OrderNumber;
                                string str3 = num.ToString();
                                screenOrder2.OrderNumber = str3;
                                screenOrder1.OrderStatus = OrderItemRequest.ListOrderItem.First<ScreenOrderDetails>().Tags;
                                screenOrder1.TableId = OrderItemRequest.TableId;
                                ScreenOrder screenOrder3 = screenOrder1;
                                num = OrderItemRequest.TicketId;
                                string str4 = num.ToString();
                                screenOrder3.TicketId = str4;
                                screenOrder1.UserId = OrderItemRequest.UserId;
                                screenOrderList.Add(screenOrder1);
                                orderItemResponse.CustomerId = OrderItemRequest.CustomerId;
                                orderItemResponse.ListOrder = screenOrderList;
                                orderItemResponse.OrderId = OrderItemRequest.ListOrderItem.First<ScreenOrderDetails>().OrderNumber;
                                orderItemResponse.TableId = OrderItemRequest.TableId;
                                orderItemResponse.Ticket = screenTicket;
                                orderItemResponse.TicketId = OrderItemRequest.TicketId;
                            }
                            if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest1, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket.TicketNumber, accountdocument) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                            if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest1, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket.TicketNumber, accountdocument) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenMultiOrderItemResponse)null;
                                return orderItemResponse;
                            }
                            uof.CommitTransaction();
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenMultiOrderItemResponse)null;
                            return orderItemResponse;
                        }
                    }
                    else
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenMultiOrderItemResponse)null;
                        return orderItemResponse;
                    }
                }
                catch (Exception ex)
                {
                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse;
                }
            }
            return orderItemResponse;
        }

        public static ScreenOrderItemResponse OrderCancel(
          ScreenOrderItemRequest OrderItemRequest,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          string updateType)
        {
            string empty = string.Empty;
            ScreenOrderItemResponse orderItemResponse = new ScreenOrderItemResponse();
            ScreenOrder screenOrder = new ScreenOrder();
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            ScreenOrderDetails screenOrderDetails = new ScreenOrderDetails();
            ScreenTicket screenTicket = new ScreenTicket();
            using (UnitOfWork uof = new UnitOfWork())
            {
                uof.StartTransaction();
                try
                {
                    Ticket ticket = new Ticket();
                    Ticket objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == OrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                    if (objTicket != null)
                    {
                        if (TicketBusiness.TicketUpdate(OrderItemRequest, objTicket, objTicket.Date, TicketRepository, uof) != 0)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenOrderItemResponse)null;
                            return orderItemResponse;
                        }
                        if (updateType == "Cancel")
                        {
                            if (TicketBusiness.OrderDelete(OrderRepository, OrderItemRequest, uof) != 0)
                            {
                                uof.RollBackTransaction();
                                orderItemResponse = (ScreenOrderItemResponse)null;
                                return orderItemResponse;
                            }
                        }
                        else if (TicketBusiness.OrderUpdate(OrderRepository, OrderItemRequest, uof, OrderItemRequest.OrderItem.OrderNumber, OrderItemRequest.TicketId, updateType) != 0)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenOrderItemResponse)null;
                            return orderItemResponse;
                        }
                        if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, objTicket.TicketNumber, empty) != 0)
                        {
                            uof.RollBackTransaction();
                            orderItemResponse = (ScreenOrderItemResponse)null;
                            return orderItemResponse;
                        }
                    }
                    else
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenOrderItemResponse)null;
                        return orderItemResponse;
                    }
                }
                catch (Exception ex)
                {
                    orderItemResponse = (ScreenOrderItemResponse)null;
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse;
                }
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == OrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                screenTicket.CustomerId = OrderItemRequest.CustomerId;
                screenTicket.Discount = OrderItemRequest.Discount;
                screenTicket.ServiceCharge = OrderItemRequest.ServiceCharge;
                screenTicket.Id = ticket2.Id;
                screenTicket.IsActive = true;
                screenTicket.isSubmitted = false;
                screenTicket.Name = ticket2.TicketNumber;
                screenTicket.Note = ticket2 == null ? "" : ticket2.Note;
                screenTicket.TableId = int.Parse(OrderItemRequest.TableId);
                screenTicket.TicketId = OrderItemRequest.TicketId;
                screenTicket.TicketOpeningTime = ticket2.Date;
                screenTicket.TotalAmount = OrderItemRequest.TicketTotal;
                List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)paymentHistoryList;
                screenOrderDetails.Id = OrderItemRequest.OrderItem.Id;
                screenOrderDetails.IsSelected = false;
                screenOrderDetails.IsVoid = OrderItemRequest.OrderItem.IsVoid;
                screenOrderDetails.ItemId = OrderItemRequest.OrderItem.ItemId;
                screenOrderDetails.OrderId = OrderItemRequest.OrderItem.OrderId;
                screenOrderDetails.OrderNumber = OrderItemRequest.OrderItem.OrderNumber;
                screenOrderDetails.Qty = OrderItemRequest.OrderItem.Qty;
                screenOrderDetails.Tags = OrderItemRequest.OrderItem.Tags;
                screenOrderDetails.TotalAmount = Decimal.Parse(OrderItemRequest.TicketTotal.ToString());
                screenOrderDetails.UnitPrice = OrderItemRequest.OrderItem.UnitPrice;
                screenOrderDetailsList.Add(screenOrderDetails);
                screenOrder.Id = OrderItemRequest.OrderItem.Id;
                screenOrder.OrderItems = (IEnumerable<ScreenOrderDetails>)screenOrderDetailsList;
                screenOrder.OrderNumber = OrderItemRequest.OrderItem.OrderNumber.ToString();
                screenOrder.OrderOpeningTime = ticket2.Date.ToString();
                screenOrder.OrderStatus = OrderItemRequest.OrderItem.Tags;
                screenOrder.TableId = OrderItemRequest.TableId;
                screenOrder.TicketId = OrderItemRequest.TicketId.ToString();
                screenOrder.UserId = OrderItemRequest.UserId;
                orderItemResponse.CustomerId = OrderItemRequest.CustomerId;
                orderItemResponse.Order = screenOrder;
                orderItemResponse.OrderId = OrderItemRequest.OrderItem.OrderId;
                orderItemResponse.TableId = OrderItemRequest.TableId;
                orderItemResponse.Ticket = screenTicket;
                orderItemResponse.TicketId = OrderItemRequest.TicketId;
                uof.CommitTransaction();
            }
            return orderItemResponse;
        }

        public static int TicketNote(
          IDCubeRepository<Ticket> TicketRepository,
          string TicketId,
          string note)
        {
            int num = 1;
            try
            {
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == int.Parse(TicketId))).FirstOrDefault<Ticket>();
                if (ticket2 != null)
                {
                    ticket2.Note = note;
                    TicketRepository.Update(ticket2);
                    TicketRepository.Save();
                }
            }
            catch (Exception ex)
            {
                num = 0;
            }
            return num;
        }

        public static int PrintTicket(IDCubeRepository<Ticket> TicketRepository, string TicketId)
        {
            int num = 1;
            try
            {
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == int.Parse(TicketId))).FirstOrDefault<Ticket>();
                if (ticket2 != null)
                {
                    ticket2.IsLocked = true;
                    TicketRepository.Update(ticket2);
                    TicketRepository.Save();
                }
            }
            catch (Exception ex)
            {
                num = 0;
            }
            return num;
        }

        public static List<ScreenTicket> GetUnsettleTicket(
          List<Ticket> listticket,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository)
        {
            List<ScreenTicket> unsettleTicket = new List<ScreenTicket>();
            foreach (Ticket ticket in listticket)
            {
                ScreenTicket screenTicket = new ScreenTicket();
                screenTicket.CustomerId = ticket.Table_Customer_Room;
                screenTicket.Discount = TicketBusiness.GetTicketDiscount(ticket.TicketNumber, AccountTypeRepository, TransactionDocumentRepository, AccountTransactionRepository, AccountTransactionValueRepository);
                screenTicket.Id = ticket.Id;
                screenTicket.IsActive = true;
                screenTicket.isSubmitted = true;
                screenTicket.Name = ticket.Name;
                screenTicket.Note = ticket.Note;
                List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                List<PaymentHistory> ticketPaymentHistory = TicketBusiness.GetTicketPaymentHistory(TicketRepository, AccountTypeRepository, TransactionDocumentRepository, AccountTransactionTypeRepository, AccountTransactionValueRepository, ticket.Id);
                screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)ticketPaymentHistory;
                screenTicket.TableId = ticket.Table_Customer_Room;
                screenTicket.TicketId = int.Parse(ticket.TicketNumber);
                screenTicket.TicketOpeningTime = ticket.Date;
                screenTicket.TotalAmount = ticket.TotalAmount;
                unsettleTicket.Add(screenTicket);
            }
            return unsettleTicket;
        }

        public static ScreenTicket GetScreenTicket(
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          string TicketId)
        {
            Ticket ticket1 = new Ticket();
            Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id.ToString() == TicketId)).FirstOrDefault<Ticket>();
            ScreenTicket screenTicket = new ScreenTicket();
            if (ticket2 != null)
            {
                screenTicket.CustomerId = 0;
                screenTicket.Discount = TicketBusiness.GetTicketDiscount(ticket2.TicketNumber, AccountTypeRepository, TransactionDocumentRepository, AccountTransactionRepository, AccountTransactionValueRepository);
                screenTicket.Id = ticket2.Id;
                screenTicket.IsActive = true;
                screenTicket.isSubmitted = true;
                screenTicket.Name = ticket2.Name;
                screenTicket.Note = ticket2.Note;
                List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                List<PaymentHistory> ticketPaymentHistory = TicketBusiness.GetTicketPaymentHistory(TicketRepository, AccountTypeRepository, TransactionDocumentRepository, AccountTransactionTypeRepository, AccountTransactionValueRepository, ticket2.Id);
                screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)ticketPaymentHistory;
                screenTicket.TableId = ticket2.TicketTypeId;
                screenTicket.TicketId = int.Parse(ticket2.TicketNumber);
                screenTicket.TicketOpeningTime = ticket2.Date;
                screenTicket.TotalAmount = ticket2.TotalAmount;
            }
            return screenTicket;
        }

        private static List<PaymentHistory> GetTicketPaymentHistory(
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          int id)
        {
            List<PaymentHistory> ticketPaymentHistory = new List<PaymentHistory>();
            AccountType objAccountType = new AccountType();
            objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Id == 20)).FirstOrDefault<AccountType>();
            if (objAccountType == null)
                objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>();
            Ticket objTicket = new Ticket();
            objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == id)).FirstOrDefault<Ticket>();
            AccountTransactionDocument transactionDocument1 = new AccountTransactionDocument();
            if (objAccountType != null && objTicket != null)
            {
                AccountTransactionType accountTransactionType1 = new AccountTransactionType();
                AccountTransactionType accountTransactionType2 = AccountTransactionTypeRepository.SelectDataById((object)4);
                AccountTransactionDocument transactionDocument2 = TransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name == objAccountType.Name + " [#" + objTicket.TicketNumber + "]" && o.FinancialYear == objTicket.FinancialYear)).FirstOrDefault<AccountTransactionDocument>();
                if (transactionDocument2 != null)
                    ticketPaymentHistory = POSPaymentBusinessLayer.ListPaymentHistory(transactionDocument2.Id.ToString(), accountTransactionType2.Name, AccountTransactionValueRepository, objTicket.FinancialYear);
            }
            return ticketPaymentHistory;
        }

        public static ScreenOrderItemResponse SaveTicketOrder(
          string currentuser,
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          ScreenOrderItemRequest OrderItemRequest)
        {
            ScreenOrderItemResponse orderItemResponse1 = new ScreenOrderItemResponse();
            ScreenOrder screenOrder = new ScreenOrder();
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            ScreenOrderDetails screenOrderDetails = new ScreenOrderDetails();
            ScreenTicket screenTicket = new ScreenTicket();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = RepositoryAccount.GetAllData().ToList<Account>();
            using (UnitOfWork uof = new UnitOfWork())
            {
                string TicketNo = "0";
                DateTime now = DateTime.Now;
                string str = "0";
                uof.StartTransaction();
                string ticketNumber;
                if (OrderItemRequest.TicketId == 0)
                {
                    try
                    {
                        TicketNo = TicketBusiness.TicketLastNumber(TicketRepository);
                        if (TicketBusiness.TicketSave(OrderItemRequest, TicketRepository, TicketNo, now, int.Parse(str)) == 0)
                        {
                            List<Ticket> list2 = TicketRepository.GetAllData().ToList<Ticket>();
                            ticketNumber = list2.Last<Ticket>().TicketNumber;
                            TicketNo = list2.Last<Ticket>().Id.ToString();
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            return (ScreenOrderItemResponse)null;
                        }
                    }
                    catch (Exception ex)
                    {
                        uof.RollBackTransaction();
                        ScreenOrderItemResponse orderItemResponse2 = (ScreenOrderItemResponse)null;
                        ErrorLog.ErrorLogging(ex);
                        return orderItemResponse2;
                    }
                }
                else
                {
                    try
                    {
                        Ticket ticket = new Ticket();
                        Ticket objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == OrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                        if (objTicket != null)
                        {
                            ticketNumber = objTicket.TicketNumber;
                            if (TicketBusiness.TicketUpdate(OrderItemRequest, objTicket, now, TicketRepository, uof) == 0)
                            {
                                TicketNo = objTicket.Id.ToString();
                            }
                            else
                            {
                                uof.RollBackTransaction();
                                return (ScreenOrderItemResponse)null;
                            }
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            return (ScreenOrderItemResponse)null;
                        }
                    }
                    catch (Exception ex)
                    {
                        uof.RollBackTransaction();
                        ScreenOrderItemResponse orderItemResponse3 = (ScreenOrderItemResponse)null;
                        ErrorLog.ErrorLogging(ex);
                        return orderItemResponse3;
                    }
                }
                try
                {
                    Ticket ticket1 = new Ticket();
                    Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == TicketNo)).FirstOrDefault<Ticket>();
                    screenTicket.CustomerId = OrderItemRequest.CustomerId;
                    screenTicket.Discount = OrderItemRequest.Discount;
                    screenTicket.ServiceCharge = OrderItemRequest.ServiceCharge;
                    screenTicket.Id = int.Parse(TicketNo);
                    screenTicket.IsActive = true;
                    screenTicket.isSubmitted = false;
                    screenTicket.Name = TicketNo;
                    screenTicket.Note = ticket2 == null ? "" : ticket2.Note;
                    screenTicket.TableId = int.Parse(OrderItemRequest.TableId);
                    screenTicket.TicketId = int.Parse(TicketNo);
                    screenTicket.TicketOpeningTime = now;
                    screenTicket.TotalAmount = OrderItemRequest.Balance;
                    List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                    screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)paymentHistoryList;
                }
                catch (Exception ex)
                {
                    uof.RollBackTransaction();
                    ScreenOrderItemResponse orderItemResponse4 = (ScreenOrderItemResponse)null;
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse4;
                }
                int OrderNo = OrderItemRequest.OrderId != 0 ? OrderItemRequest.OrderId : TicketBusiness.OrderNewNumber(OrderRepository);
                int num = TicketBusiness.OrderSave(OrderRepository, OrderItemRequest, TicketNo, OrderNo, now);
                if (num == 0)
                {
                    uof.RollBackTransaction();
                    return (ScreenOrderItemResponse)null;
                }
                try
                {
                    screenOrderDetails.Id = num;
                    screenOrderDetails.IsSelected = false;
                    screenOrderDetails.IsVoid = false;
                    screenOrderDetails.ItemId = OrderItemRequest.OrderItem.ItemId;
                    screenOrderDetails.OrderId = num;
                    screenOrderDetails.OrderNumber = OrderNo;
                    screenOrderDetails.Qty = OrderItemRequest.OrderItem.Qty;
                    screenOrderDetails.Tags = OrderItemRequest.OrderItem.Tags;
                    screenOrderDetails.TotalAmount = Decimal.Parse(OrderItemRequest.TicketTotal.ToString());
                    screenOrderDetails.UnitPrice = OrderItemRequest.OrderItem.UnitPrice;
                    screenOrderDetails.UserId = OrderItemRequest.OrderItem.UserId;
                    screenOrderDetailsList.Add(screenOrderDetails);
                }
                catch (Exception ex)
                {
                    ScreenOrderItemResponse orderItemResponse5 = (ScreenOrderItemResponse)null;
                    uof.RollBackTransaction();
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse5;
                }
                try
                {
                    if (OrderItemRequest.TicketId == 0)
                    {
                        int iAccountTransactionDocument = TicketBusiness.SaveAccountTransactionDocument(TransactionDocumentRepository, RepositoryAccountType, ticketNumber, now, str, OrderItemRequest.OrderItem.FinancialYear);
                        if (iAccountTransactionDocument == 0)
                        {
                            orderItemResponse1 = (ScreenOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse1;
                        }
                        AccountTransactionType accountTransactionType = new AccountTransactionType();
                        AccountTransactionType objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)3);
                        int iAccountTransaction = TicketBusiness.SaveAccountTransaction(objAccountTransactionType, AccountTranastionRepository, OrderItemRequest, AccountTransactionTypeRepository, iAccountTransactionDocument, ticketNumber);
                        if (iAccountTransaction == 0)
                        {
                            orderItemResponse1 = (ScreenOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse1;
                        }
                        if (TicketBusiness.SaveAccountTransactionValue(OrderItemRequest, objAccountTransactionType, AccountTransactionTypeRepository, list1, AccountTransactionValueRepository, iAccountTransactionDocument, iAccountTransaction, now, ticketNumber) != 0)
                        {
                            orderItemResponse1 = (ScreenOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse1;
                        }
                    }
                    else if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, ticketNumber, str) != 0)
                        ;
                    screenOrder.Id = num;
                    screenOrder.OrderItems = (IEnumerable<ScreenOrderDetails>)screenOrderDetailsList;
                    screenOrder.OrderNumber = OrderNo.ToString();
                    screenOrder.OrderOpeningTime = now.ToString();
                    screenOrder.OrderStatus = OrderItemRequest.OrderItem.Tags;
                    screenOrder.TableId = OrderItemRequest.TableId;
                    screenOrder.TicketId = TicketNo;
                    screenOrder.UserId = OrderItemRequest.OrderItem.UserId;
                    orderItemResponse1.CustomerId = OrderItemRequest.CustomerId;
                    orderItemResponse1.Order = screenOrder;
                    orderItemResponse1.OrderId = num;
                    orderItemResponse1.TableId = OrderItemRequest.TableId;
                    orderItemResponse1.Ticket = screenTicket;
                    orderItemResponse1.TicketId = int.Parse(TicketNo);
                }
                catch (Exception ex)
                {
                    uof.RollBackTransaction();
                    ErrorLog.ErrorLogging(ex);
                    return orderItemResponse1;
                }
                uof.CommitTransaction();
                return orderItemResponse1;
            }
        }

        public static ScreenMultiOrderItemResponse SaveTicketOrder(
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          ScreenMultiOrderItemRequest OrderItemRequest)
        {
            ScreenMultiOrderItemResponse orderItemResponse1 = new ScreenMultiOrderItemResponse();
            List<ScreenOrder> screenOrderList = new List<ScreenOrder>();
            List<ScreenOrderDetails> source = new List<ScreenOrderDetails>();
            ScreenTicket screenTicket = new ScreenTicket();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = RepositoryAccount.GetAllData().ToList<Account>();
            ScreenOrderItemRequest objScreenOrderItemRequest = new ScreenOrderItemRequest();
            objScreenOrderItemRequest = TicketBusiness.screenorderitemrequest(OrderItemRequest);
            using (UnitOfWork uof = new UnitOfWork())
            {
                string TicketNo = "0";
                int num = 0;
                DateTime now = DateTime.Now;
                string str = "0";
                uof.StartTransaction();
                string ticketNumber;
                if (OrderItemRequest.TicketId == 0)
                {
                    try
                    {
                        TicketNo = TicketBusiness.TicketLastNumber(TicketRepository);
                        if (TicketBusiness.TicketSave(objScreenOrderItemRequest, TicketRepository, TicketNo, now, int.Parse(str)) == 0)
                        {
                            List<Ticket> list2 = TicketRepository.GetAllData().ToList<Ticket>();
                            ticketNumber = list2.Last<Ticket>().TicketNumber;
                            TicketNo = list2.Last<Ticket>().Id.ToString();
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            return (ScreenMultiOrderItemResponse)null;
                        }
                    }
                    catch (Exception ex)
                    {
                        uof.RollBackTransaction();
                        ScreenMultiOrderItemResponse orderItemResponse2 = (ScreenMultiOrderItemResponse)null;
                        ErrorLog.ErrorLogging(ex);
                        return orderItemResponse2;
                    }
                }
                else
                {
                    try
                    {
                        Ticket ticket = new Ticket();
                        Ticket objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == objScreenOrderItemRequest.TicketId)).FirstOrDefault<Ticket>();
                        if (objTicket != null)
                        {
                            ticketNumber = objTicket.TicketNumber;
                            if (TicketBusiness.TicketUpdate(objScreenOrderItemRequest, objTicket, now, TicketRepository, uof) == 0)
                            {
                                TicketNo = objTicket.Id.ToString();
                            }
                            else
                            {
                                uof.RollBackTransaction();
                                return (ScreenMultiOrderItemResponse)null;
                            }
                        }
                        else
                        {
                            uof.RollBackTransaction();
                            return (ScreenMultiOrderItemResponse)null;
                        }
                    }
                    catch (Exception ex)
                    {
                        uof.RollBackTransaction();
                        return (ScreenMultiOrderItemResponse)null;
                    }
                }
                try
                {
                    Ticket ticket1 = new Ticket();
                    Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == TicketNo)).FirstOrDefault<Ticket>();
                    screenTicket.CustomerId = objScreenOrderItemRequest.CustomerId;
                    screenTicket.Discount = objScreenOrderItemRequest.Discount;
                    screenTicket.ServiceCharge = objScreenOrderItemRequest.ServiceCharge;
                    screenTicket.Id = int.Parse(TicketNo);
                    screenTicket.IsActive = true;
                    screenTicket.isSubmitted = false;
                    screenTicket.Name = TicketNo;
                    screenTicket.Note = ticket2 == null ? "" : ticket2.Note;
                    screenTicket.TableId = int.Parse(objScreenOrderItemRequest.TableId);
                    screenTicket.TicketId = int.Parse(TicketNo);
                    screenTicket.TicketOpeningTime = now;
                    screenTicket.TotalAmount = objScreenOrderItemRequest.Balance;
                    screenTicket.FinancialYear = objScreenOrderItemRequest.FinancialYear;
                    screenTicket.UserName = objScreenOrderItemRequest.UserId;
                    List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                    screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)paymentHistoryList;
                }
                catch (Exception ex)
                {
                    uof.RollBackTransaction();
                    return (ScreenMultiOrderItemResponse)null;
                }
                int OrderNo = objScreenOrderItemRequest.OrderId != 0 ? objScreenOrderItemRequest.OrderId : TicketBusiness.OrderNewNumber(OrderRepository);
                foreach (ScreenOrderDetails screenOrderDetails1 in OrderItemRequest.ListOrderItem)
                {
                    num = TicketBusiness.OrderSave(OrderRepository, new ScreenOrderItemRequest()
                    {
                        Balance = OrderItemRequest.Balance,
                        CustomerId = OrderItemRequest.CustomerId,
                        Discount = OrderItemRequest.Discount,
                        FinancialYear = OrderItemRequest.FinancialYear,
                        GrandTotal = OrderItemRequest.GrandTotal,
                        OrderId = OrderItemRequest.OrderId,
                        TableId = OrderItemRequest.TableId,
                        TicketId = OrderItemRequest.TicketId,
                        TicketTotal = OrderItemRequest.TicketTotal,
                        UserId = OrderItemRequest.UserId,
                        VatAmount = OrderItemRequest.VatAmount,
                        OrderItem = screenOrderDetails1
                    }, TicketNo, OrderNo, now);
                    if (num == 0)
                    {
                        uof.RollBackTransaction();
                        orderItemResponse1 = (ScreenMultiOrderItemResponse)null;
                        return orderItemResponse1;
                    }
                    ScreenOrderDetails screenOrderDetails2 = new ScreenOrderDetails();
                    try
                    {
                        screenOrderDetails2.Id = num;
                        screenOrderDetails2.IsSelected = false;
                        screenOrderDetails2.IsVoid = false;
                        screenOrderDetails2.ItemId = screenOrderDetails1.ItemId;
                        screenOrderDetails2.OrderId = num;
                        screenOrderDetails2.OrderNumber = OrderNo;
                        screenOrderDetails2.OrderDescription = screenOrderDetails1.OrderDescription;
                        screenOrderDetails2.Qty = screenOrderDetails1.Qty;
                        screenOrderDetails2.Tags = screenOrderDetails1.Tags;
                        screenOrderDetails2.TotalAmount = Decimal.Parse(screenOrderDetails1.TotalAmount.ToString());
                        screenOrderDetails2.UnitPrice = screenOrderDetails1.UnitPrice;
                        screenOrderDetails2.UserId = screenOrderDetails1.UserId;
                        screenOrderDetails2.FinancialYear = screenOrderDetails1.FinancialYear;
                        screenOrderDetails2.DepartmentId = screenOrderDetails1.DepartmentId;
                        source.Add(screenOrderDetails2);
                    }
                    catch (Exception ex)
                    {
                        orderItemResponse1 = (ScreenMultiOrderItemResponse)null;
                        uof.RollBackTransaction();
                        return orderItemResponse1;
                    }
                }
                screenOrderList.Add(new ScreenOrder()
                {
                    Id = num,
                    OrderItems = (IEnumerable<ScreenOrderDetails>)source,
                    OrderNumber = OrderNo.ToString(),
                    OrderOpeningTime = now.ToString(),
                    OrderStatus = source.First<ScreenOrderDetails>().Tags,
                    TableId = OrderItemRequest.TableId,
                    TicketId = TicketNo,
                    UserId = source.First<ScreenOrderDetails>().UserId
                });
                orderItemResponse1.CustomerId = OrderItemRequest.CustomerId;
                orderItemResponse1.ListOrder = screenOrderList;
                orderItemResponse1.OrderId = num;
                orderItemResponse1.TableId = OrderItemRequest.TableId;
                orderItemResponse1.Ticket = screenTicket;
                orderItemResponse1.TicketId = int.Parse(TicketNo);
                try
                {
                    if (objScreenOrderItemRequest.TicketId == 0)
                    {
                        int iAccountTransactionDocument = TicketBusiness.SaveAccountTransactionDocument(TransactionDocumentRepository, RepositoryAccountType, ticketNumber, now, str, objScreenOrderItemRequest.OrderItem.FinancialYear);
                        if (iAccountTransactionDocument == 0)
                        {
                            ScreenMultiOrderItemResponse orderItemResponse3 = (ScreenMultiOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse3;
                        }
                        AccountTransactionType accountTransactionType = new AccountTransactionType();
                        AccountTransactionType objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)3);
                        int iAccountTransaction = TicketBusiness.SaveAccountTransaction(objAccountTransactionType, AccountTranastionRepository, objScreenOrderItemRequest, AccountTransactionTypeRepository, iAccountTransactionDocument, ticketNumber);
                        if (iAccountTransaction == 0)
                        {
                            ScreenMultiOrderItemResponse orderItemResponse4 = (ScreenMultiOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse4;
                        }
                        if (TicketBusiness.SaveAccountTransactionValue(objScreenOrderItemRequest, objAccountTransactionType, AccountTransactionTypeRepository, list1, AccountTransactionValueRepository, iAccountTransactionDocument, iAccountTransaction, now, ticketNumber) != 0)
                        {
                            ScreenMultiOrderItemResponse orderItemResponse5 = (ScreenMultiOrderItemResponse)null;
                            uof.RollBackTransaction();
                            return orderItemResponse5;
                        }
                    }
                    else if (TicketBusiness.AccountTransactionValueUpdate(objScreenOrderItemRequest, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, ticketNumber, str) != 0)
                        ;
                    uof.CommitTransaction();
                }
                catch (Exception ex)
                {
                    ScreenMultiOrderItemResponse orderItemResponse6 = (ScreenMultiOrderItemResponse)null;
                    uof.RollBackTransaction();
                    return orderItemResponse6;
                }
                return orderItemResponse1;
            }
        }

        private static int TicketSave(
          ScreenOrderItemRequest OrderItemRequest,
          IDCubeRepository<Ticket> TicketRepository,
          string TicketNo,
          DateTime CurrentDate,
          int iAccountTransactionDocument)
        {
            int num = 0;
            try
            {
                Ticket ticket1 = new Ticket();
                ticket1.LastUpdateTime = CurrentDate;
                ticket1.TicketNumber = TicketNo;
                ticket1.Date = CurrentDate;
                ticket1.LastOrderDate = CurrentDate;
                ticket1.LastPaymentDate = CurrentDate;
                ticket1.IsClosed = false;
                ticket1.IsLocked = false;
                ticket1.RemainingAmount = OrderItemRequest.Balance;
                ticket1.TotalAmount = OrderItemRequest.GrandTotal;
                ticket1.DepartmentId = 1;
                if (OrderItemRequest.CustomerId != 0)
                {
                    ticket1.Name = "Customer Sales";
                    ticket1.TicketTypeId = 1;
                    ticket1.Table_Customer_Room = OrderItemRequest.CustomerId;
                }
                if (OrderItemRequest.TableId != "0")
                {
                    ticket1.Name = "Table Sales";
                    ticket1.TicketTypeId = 2;
                    ticket1.Table_Customer_Room = int.Parse(OrderItemRequest.TableId);
                }
                ticket1.TicketStates = "Unpaid";
                ticket1.ExchangeRate = 1M;
                ticket1.TaxIncluded = true;
                ticket1.Printed_Time = DateTime.Now;
                ticket1.FinancialYear = OrderItemRequest.OrderItem.FinancialYear;
                ticket1.ref_invoice_number = "0";
                Ticket ticket2 = ticket1;
                DateTime now = DateTime.Now;
                int year1 = now.Year;
                now = DateTime.Now;
                int month1 = now.Month;
                now = DateTime.Now;
                int day1 = now.Day;
                string nepali = NepalitoEnglish.englishToNepali(year1, month1, day1);
                ticket2.NVDate = nepali;
                Ticket ticket3 = ticket1;
                now = DateTime.Now;
                int year2 = now.Year;
                now = DateTime.Now;
                int month2 = now.Month;
                now = DateTime.Now;
                int day2 = now.Day;
                string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(year2, month2, day2);
                ticket3.NepaliMonth = nepaliMonth;
                ticket1.AccountTransactionDocumentId = iAccountTransactionDocument;
                ticket1.LastModifiedUserName = OrderItemRequest.OrderItem.UserId;
                TicketRepository.Insert(ticket1);
                TicketRepository.Save();
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 1;
            }
            return num;
        }

        public static int TicketSave(
          AccountTransaction objAccountTransaction,
          IDCubeRepository<Ticket> TicketRepository,
          string TicketNo,
          DateTime CurrentDate,
          int iAccountTransactionDocument)
        {
            int num;
            try
            {
                DateTime dateTime = DateTime.Parse(objAccountTransaction.Date);
                TicketRepository.Insert(new Ticket()
                {
                    Name = "Direct Sales",
                    LastUpdateTime = CurrentDate,
                    TicketNumber = TicketNo,
                    Date = CurrentDate,
                    LastOrderDate = CurrentDate,
                    LastPaymentDate = CurrentDate,
                    IsClosed = true,
                    IsLocked = true,
                    RemainingAmount = 0M,
                    TotalAmount = objAccountTransaction.AccountTransactionValues.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>)(o => o.Debit)),
                    DepartmentId = 1,
                    TicketTypeId = 3,
                    Table_Customer_Room = 0,
                    TicketStates = "Sales",
                    ExchangeRate = 1M,
                    TaxIncluded = true,
                    Printed_Time = DateTime.Now,
                    AccountTransactionDocumentId = iAccountTransactionDocument,
                    LastModifiedUserName = objAccountTransaction.UserName,
                    FinancialYear = objAccountTransaction.FinancialYear,
                    NepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime.Year, dateTime.Month, dateTime.Day),
                    NVDate = NepalitoEnglish.englishToNepali(dateTime.Year, dateTime.Month, dateTime.Day)
                });
                TicketRepository.Save();
                num = TicketRepository.GetAllData().ToList<Ticket>().LastOrDefault<Ticket>().Id;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 0;
            }
            return num;
        }

        private static int TicketUpdate(
          ScreenOrderItemRequest OrderItemRequest,
          Ticket objTicket,
          DateTime CurrentDate,
          IDCubeRepository<Ticket> TicketRepository,
          UnitOfWork uof)
        {
            int num = 0;
            try
            {
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == objTicket.Id)).FirstOrDefault<Ticket>();
                if (ticket2 != null)
                {
                    ticket2.Name = objTicket.Name;
                    ticket2.Date = objTicket.Date;
                    ticket2.DepartmentId = objTicket.DepartmentId;
                    ticket2.ExchangeRate = objTicket.ExchangeRate;
                    ticket2.IsClosed = objTicket.IsClosed;
                    ticket2.IsLocked = objTicket.IsLocked;
                    ticket2.LastModifiedUserName = OrderItemRequest.OrderItem.UserId;
                    ticket2.LastOrderDate = CurrentDate;
                    ticket2.LastPaymentDate = objTicket.LastPaymentDate;
                    ticket2.LastUpdateTime = CurrentDate;
                    ticket2.RemainingAmount = OrderItemRequest.Balance;
                    ticket2.TotalAmount = OrderItemRequest.GrandTotal;
                    ticket2.TicketNumber = objTicket.TicketNumber;
                    ticket2.TicketTypeId = objTicket.TicketTypeId;
                    ticket2.Table_Customer_Room = objTicket.Table_Customer_Room;
                    ticket2.Printed_Time = objTicket.Printed_Time;
                    TicketRepository.Update(ticket2);
                    TicketRepository.Save();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 1;
            }
            return num;
        }

        public static int TicketUpdate(
          AccountTransaction objAccountTransaction,
          Ticket objTicket,
          DateTime CurrentDate,
          IDCubeRepository<Ticket> TicketRepository,
          UnitOfWork uof)
        {
            int num = 0;
            try
            {
                DateTime dateTime = DateTime.Parse(objAccountTransaction.Date);
                Ticket ObjTicket = new Ticket()
                {
                    Name = objTicket.Name,
                    Date = objTicket.Date,
                    DepartmentId = objTicket.DepartmentId,
                    ExchangeRate = objTicket.ExchangeRate,
                    Id = objTicket.Id,
                    IsClosed = objTicket.IsClosed,
                    IsLocked = objTicket.IsLocked,
                    LastModifiedUserName = "1",
                    LastOrderDate = CurrentDate,
                    LastPaymentDate = objTicket.LastPaymentDate,
                    LastUpdateTime = CurrentDate,
                    RemainingAmount = 0M,
                    TotalAmount = objAccountTransaction.AccountTransactionValues.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>)(o => o.Debit)),
                    TicketNumber = objTicket.TicketNumber,
                    TicketTypeId = objTicket.TicketTypeId,
                    Table_Customer_Room = objTicket.Table_Customer_Room,
                    Printed_Time = objTicket.Printed_Time,
                    AccountTransactionDocumentId = objTicket.AccountTransactionDocumentId
                };
                ObjTicket.LastModifiedUserName = objAccountTransaction.UserName;
                ObjTicket.FinancialYear = objAccountTransaction.FinancialYear;
                ObjTicket.NepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime.Year, dateTime.Month, dateTime.Day);
                ObjTicket.NVDate = NepalitoEnglish.englishToNepali(dateTime.Year, dateTime.Month, dateTime.Day);
                uof.ITicketRepository.UpdateTicket(ObjTicket);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 1;
            }
            return num;
        }

        public static int TicketUpdateTransferTableToCustomer(
          IDCubeRepository<Ticket> TicketRepository,
          string TicketId,
          string AccountId)
        {
            int customer;
            try
            {
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id.ToString() == TicketId)).FirstOrDefault<Ticket>();
                ticket2.Name = "Customer Sales";
                ticket2.Table_Customer_Room = int.Parse(AccountId);
                ticket2.TicketTypeId = 1;
                TicketRepository.Update(ticket2);
                TicketRepository.Save();
                customer = 1;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                customer = 1;
            }
            return customer;
        }

        private static int OrderSave(
          IDCubeRepository<Order> OrderRepository,
          ScreenOrderItemRequest OrderItemRequest,
          string TicketNo,
          int OrderNo,
          DateTime CurrentDate)
        {
            int num;
            try
            {
                Order order1 = new Order();
                order1.TicketId = int.Parse(TicketNo);
                order1.WarehouseId = 1;
                order1.DepartmentId = OrderItemRequest.OrderItem.DepartmentId;
                order1.MenuItemId = OrderItemRequest.OrderItem.ItemId;
                order1.Price = OrderItemRequest.OrderItem.UnitPrice;
                order1.Quantity = OrderItemRequest.OrderItem.Qty;
                order1.OrderDescription = OrderItemRequest.OrderItem.OrderDescription;
                order1.PortionCount = 1;
                order1.Locked = true;
                order1.CalculatePrice = true;
                order1.DecreaseInventory = true;
                order1.IncreaseInventory = false;
                order1.OrderNumber = OrderNo;
                order1.CreatedDateTime = CurrentDate;
                order1.AccountTransactionTypeId = 3;
                order1.OrderStates = OrderItemRequest.OrderItem.Tags;
                order1.Printed_Time = DateTime.Now;
                order1.FinancialYear = OrderItemRequest.OrderItem.FinancialYear;
                Order order2 = order1;
                DateTime now1 = DateTime.Now;
                int year1 = now1.Year;
                now1 = DateTime.Now;
                int month1 = now1.Month;
                now1 = DateTime.Now;
                int day1 = now1.Day;
                string nepali = NepalitoEnglish.englishToNepali(year1, month1, day1);
                order2.NVDate = nepali;
                Order order3 = order1;
                DateTime now2 = DateTime.Now;
                int year2 = now2.Year;
                now2 = DateTime.Now;
                int month2 = now2.Month;
                now2 = DateTime.Now;
                int day2 = now2.Day;
                string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(year2, month2, day2);
                order3.NepaliMonth = nepaliMonth;
                order1.CreatingUserName = OrderItemRequest.OrderItem.UserId;
                OrderRepository.Insert(order1);
                OrderRepository.Save();
                num = OrderRepository.GetAllData().OrderByDescending<Order, int>((Func<Order, int>)(o => o.Id)).FirstOrDefault<Order>().Id;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 1;
            }
            return num;
        }

        private static int OrderUpdate(
          IDCubeRepository<Order> OrderRepository,
          ScreenOrderItemRequest OrderItemRequest,
          UnitOfWork uof,
          int OrderNo,
          int TicketId,
          string updateType)
        {
            int num = 0;
            try
            {
                if (OrderRepository.GetAllData().ToList<Order>().Count > 0)
                {
                    Order order1 = new Order();
                    Order order2 = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o =>
                   {
                       int id = o.Id;
                       string str1 = id.ToString();
                       id = OrderItemRequest.OrderItem.Id;
                       string str2 = id.ToString();
                       return str1 == str2;
                   })).FirstOrDefault<Order>();
                    if (order2 != null)
                    {
                        order2.TicketId = TicketId;
                        order2.Quantity = OrderItemRequest.OrderItem.Qty;
                        order2.Price = OrderItemRequest.OrderItem.UnitPrice;
                        order2.OrderNumber = OrderNo;
                        if (OrderItemRequest.OrderItem.IsVoid)
                            order2.OrderStates = "Void";
                        order2.DecreaseInventory = updateType == "increaseQuantity" || order2.DecreaseInventory;
                        order2.IncreaseInventory = updateType == "decreaseQuantity" || order2.IncreaseInventory;
                        OrderRepository.Update(order2);
                        OrderRepository.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 1;
            }
            return num;
        }

        private static int OrderDelete(
          IDCubeRepository<Order> OrderRepository,
          ScreenOrderItemRequest OrderItemRequest,
          UnitOfWork uof)
        {
            int num = 0;
            try
            {
                if (OrderRepository.GetAllData().ToList<Order>().Count > 0)
                    uof.IOrderRepository.DeleteOrder(OrderItemRequest.OrderItem.Id);
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 1;
            }
            return num;
        }

        private static int SaveAccountTransactionDocument(
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountType> RepositoryAccountType,
          string TicketKeyNo,
          DateTime CurrentDate,
          string accountdocument,
          string FinancialYear)
        {
            int num;
            try
            {
                AccountType accountType = new AccountType();
                accountdocument = (RepositoryAccountType.SelectDataById((object)20) ?? RepositoryAccountType.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>()).Name + " [#" + TicketKeyNo + "]";
                TransactionDocumentRepository.Insert(new AccountTransactionDocument()
                {
                    Date = CurrentDate,
                    DocumentTypeId = 0,
                    Name = accountdocument,
                    Printed_Time = DateTime.Now,
                    FinancialYear = FinancialYear
                });
                TransactionDocumentRepository.Save();
                num = TransactionDocumentRepository.GetAllData().ToList<AccountTransactionDocument>().Last<AccountTransactionDocument>().Id;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 0;
            }
            return num;
        }

        private static int SaveAccountTransaction(
          AccountTransactionType objAccountTransactionType,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          ScreenOrderItemRequest OrderItemRequest,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          int iAccountTransactionDocument,
          string TicketKeyNo)
        {
            int num;
            try
            {
                AccountTranastionRepository.Insert(new AccountTransaction()
                {
                    AccountTransactionDocumentId = iAccountTransactionDocument,
                    Amount = OrderItemRequest.GrandTotal,
                    CompanyCode = 0,
                    ExchangeRate = 1M,
                    AccountTransactionTypeId = 3,
                    SourceAccountTypeId = 20,
                    TargetAccountTypeId = 1,
                    IsReversed = false,
                    Reversable = true,
                    Date = DateTime.Now.ToString(),
                    Description = objAccountTransactionType.Name + " [#" + TicketKeyNo + "]",
                    Name = objAccountTransactionType.Name + " [#" + TicketKeyNo + "]",
                    Printed_Time = DateTime.Now,
                    FinancialYear = OrderItemRequest.OrderItem.FinancialYear,
                    ref_invoice_number = "0",
                    UserName = OrderItemRequest.OrderItem.UserId
                });
                AccountTranastionRepository.Save();
                num = AccountTranastionRepository.GetAllData().ToList<AccountTransaction>().Last<AccountTransaction>().Id;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 0;
            }
            return num;
        }

        private static int SaveAccountTransactionValue(
          ScreenOrderItemRequest OrderItemRequest,
          AccountTransactionType objAccountTransactionType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          List<Account> ListAccount,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          int iAccountTransactionDocument,
          int iAccountTransaction,
          DateTime CurrentDate,
          string TicketKeyNo)
        {
            int num = 0;
            AccountTransactionValue transactionValue1 = new AccountTransactionValue();
            transactionValue1.AccountTransactionId = iAccountTransaction;
            transactionValue1.AccountTransactionDocumentId = iAccountTransactionDocument;
            if (OrderItemRequest.CustomerId == 0)
            {
                transactionValue1.AccountId = 2;
                transactionValue1.AccountTypeId = ListAccount.Find((Predicate<Account>)(o => o.Id == 2)).AccountTypeId;
            }
            if (OrderItemRequest.TableId == "0")
            {
                transactionValue1.AccountId = OrderItemRequest.CustomerId;
                transactionValue1.AccountTypeId = ListAccount.Find((Predicate<Account>)(o => o.Id == OrderItemRequest.CustomerId)).AccountTypeId;
            }
            transactionValue1.Date = CurrentDate;
            transactionValue1.entityLists = "Dr";
            transactionValue1.Debit = OrderItemRequest.GrandTotal;
            transactionValue1.Credit = 0M;
            transactionValue1.Exchange = OrderItemRequest.GrandTotal;
            transactionValue1.Name = objAccountTransactionType.Name + " [#" + TicketKeyNo + "]";
            transactionValue1.Printed_Time = DateTime.Now;
            transactionValue1.FinancialYear = OrderItemRequest.OrderItem.FinancialYear;
            transactionValue1.ref_invoice_number = "0";
            transactionValue1.NepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
            transactionValue1.NVDate = NepalitoEnglish.englishToNepali(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
            transactionValue1.UserName = OrderItemRequest.OrderItem.UserId;
            AccountTransactionValueRepository.Insert(transactionValue1);
            AccountTransactionValueRepository.Save();
            AccountTransactionValueRepository.Insert(new AccountTransactionValue()
            {
                AccountTransactionId = iAccountTransaction,
                AccountTransactionDocumentId = iAccountTransactionDocument,
                AccountTypeId = 1,
                AccountId = 1,
                Date = CurrentDate,
                Debit = 0M,
                entityLists = "Cr",
                Credit = OrderItemRequest.TicketTotal,
                Exchange = OrderItemRequest.TicketTotal,
                Name = objAccountTransactionType.Name + " [#" + TicketKeyNo + "]",
                Printed_Time = DateTime.Now,
                FinancialYear = OrderItemRequest.OrderItem.FinancialYear,
                ref_invoice_number = "0",
                NepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day),
                NVDate = NepalitoEnglish.englishToNepali(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day),
                UserName = OrderItemRequest.OrderItem.UserId
            });
            AccountTransactionValueRepository.Save();
            if (OrderItemRequest.Discount != 0M)
            {
                objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)1);
                if (objAccountTransactionType == null)
                    objAccountTransactionType = AccountTransactionTypeRepository.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Name.Contains("Discount"))).FirstOrDefault<AccountTransactionType>();
                AccountTransactionValueRepository.Insert(new AccountTransactionValue()
                {
                    AccountTransactionId = iAccountTransaction,
                    AccountTransactionDocumentId = iAccountTransactionDocument,
                    AccountTypeId = 1,
                    AccountId = 3,
                    Date = CurrentDate,
                    Debit = OrderItemRequest.Discount,
                    entityLists = "Cr",
                    Credit = 0M,
                    Exchange = OrderItemRequest.Discount,
                    Name = objAccountTransactionType.Name + " [#" + TicketKeyNo + "]",
                    Printed_Time = DateTime.Now,
                    FinancialYear = OrderItemRequest.OrderItem.FinancialYear,
                    ref_invoice_number = "0",
                    NepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day),
                    NVDate = NepalitoEnglish.englishToNepali(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day),
                    UserName = OrderItemRequest.OrderItem.UserId
                });
                AccountTransactionValueRepository.Save();
            }
            if (OrderItemRequest.VatAmount != 0M)
            {
                AccountTransactionValue transactionValue2 = new AccountTransactionValue();
                objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)7);
                if (objAccountTransactionType == null)
                    objAccountTransactionType = AccountTransactionTypeRepository.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Name.Contains("Vat"))).FirstOrDefault<AccountTransactionType>();
                transactionValue2.AccountTransactionId = iAccountTransaction;
                transactionValue2.AccountTransactionDocumentId = iAccountTransactionDocument;
                transactionValue2.AccountTypeId = 20;
                transactionValue2.AccountId = 8;
                transactionValue2.Date = CurrentDate;
                transactionValue2.Debit = 0M;
                transactionValue2.entityLists = "Cr";
                transactionValue2.Credit = OrderItemRequest.VatAmount;
                transactionValue2.Exchange = OrderItemRequest.VatAmount;
                transactionValue2.Name = objAccountTransactionType.Name + " [#" + TicketKeyNo + "]";
                transactionValue2.Printed_Time = DateTime.Now;
                transactionValue2.FinancialYear = OrderItemRequest.OrderItem.FinancialYear;
                transactionValue2.ref_invoice_number = "0";
                transactionValue2.NepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                transactionValue2.NVDate = NepalitoEnglish.englishToNepali(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                transactionValue2.UserName = OrderItemRequest.OrderItem.UserId;
                AccountTransactionValueRepository.Insert(transactionValue2);
                AccountTransactionValueRepository.Save();
            }
            if (OrderItemRequest.ServiceCharge != 0M)
            {
                AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)10);
                if (objAccountTransactionType == null)
                    objAccountTransactionType = AccountTransactionTypeRepository.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Name.Contains("Service Charge"))).FirstOrDefault<AccountTransactionType>();
                transactionValue3.AccountTransactionId = iAccountTransaction;
                transactionValue3.AccountTransactionDocumentId = iAccountTransactionDocument;
                transactionValue3.AccountTypeId = 1;
                transactionValue3.AccountId = 9;
                transactionValue3.Date = CurrentDate;
                transactionValue3.Debit = 0M;
                transactionValue3.entityLists = "Cr";
                transactionValue3.Credit = OrderItemRequest.ServiceCharge;
                transactionValue3.Exchange = OrderItemRequest.ServiceCharge;
                transactionValue3.Name = objAccountTransactionType.Name + " [#" + TicketKeyNo + "]";
                transactionValue3.Printed_Time = DateTime.Now;
                transactionValue3.FinancialYear = OrderItemRequest.OrderItem.FinancialYear;
                transactionValue3.ref_invoice_number = "0";
                transactionValue3.NepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                transactionValue3.NVDate = NepalitoEnglish.englishToNepali(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                transactionValue3.UserName = OrderItemRequest.OrderItem.UserId;
                AccountTransactionValueRepository.Insert(transactionValue3);
                AccountTransactionValueRepository.Save();
            }
            return num;
        }

        private static int AccountTransactionValueUpdate(
          ScreenOrderItemRequest OrderItemRequest,
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          UnitOfWork uof,
          string TicketKeyNo,
          string accountdocument)
        {
            int num = 0;
            AccountType accountType = new AccountType();
            accountdocument = (RepositoryAccountType.SelectDataById((object)20) ?? RepositoryAccountType.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>()).Name + " [#" + TicketKeyNo + "]";
            AccountTransactionDocument objAccountTransactionDocument = new AccountTransactionDocument();
            objAccountTransactionDocument = TransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name == accountdocument)).FirstOrDefault<AccountTransactionDocument>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<AccountTransaction> list1 = AccountTranastionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransaction>();
            AccountTransaction objAccountTransaction = new AccountTransaction();
            objAccountTransaction = list1.Find((Predicate<AccountTransaction>)(o => o.AccountTransactionTypeId == 3));
            if (objAccountTransaction != null)
            {
                AccountTransaction accountTransaction1 = new AccountTransaction();
                AccountTransaction accountTransaction2 = AccountTranastionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Id == objAccountTransaction.Id)).FirstOrDefault<AccountTransaction>();
                if (accountTransaction2 != null)
                {
                    accountTransaction2.Amount = OrderItemRequest.GrandTotal;
                    accountTransaction2.UserName = OrderItemRequest.OrderItem.UserId;
                    AccountTranastionRepository.Update(accountTransaction2);
                    AccountTranastionRepository.Save();
                }
            }
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            List<AccountTransactionValue> list2 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransactionValue>();
            AccountTransactionValue objAccountTransactionValue = new AccountTransactionValue();
            if (OrderItemRequest.CustomerId == 0)
                objAccountTransactionValue = list2.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 2));
            if (OrderItemRequest.TableId == "0")
                objAccountTransactionValue = list2.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == OrderItemRequest.CustomerId));
            AccountTransactionValue transactionValue1 = new AccountTransactionValue();
            if (objAccountTransactionValue != null)
            {
                AccountTransactionValue transactionValue2 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                if (transactionValue2 != null)
                {
                    transactionValue2.Debit = OrderItemRequest.GrandTotal;
                    transactionValue2.Exchange = OrderItemRequest.GrandTotal;
                    transactionValue2.UserName = OrderItemRequest.OrderItem.UserId;
                    AccountTransactionValueRepository.Update(transactionValue2);
                    AccountTransactionValueRepository.Save();
                }
            }
            objAccountTransactionValue = new AccountTransactionValue();
            objAccountTransactionValue = list2.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 3));
            if (objAccountTransactionValue != null)
            {
                transactionValue1 = new AccountTransactionValue();
                AccountTransactionValue transactionValue3 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                if (transactionValue3.Debit == 0M)
                {
                    transactionValue3.Debit = OrderItemRequest.Discount;
                    transactionValue3.Exchange = OrderItemRequest.Discount;
                    transactionValue3.UserName = OrderItemRequest.OrderItem.UserId;
                    AccountTransactionValueRepository.Update(transactionValue3);
                    AccountTransactionValueRepository.Save();
                }
            }
            objAccountTransactionValue = new AccountTransactionValue();
            if (objAccountTransactionValue != null)
            {
                objAccountTransactionValue = list2.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 1));
                AccountTransactionValue transactionValue4 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                if (transactionValue4 != null)
                {
                    transactionValue4.Credit = OrderItemRequest.TicketTotal;
                    transactionValue4.Exchange = OrderItemRequest.TicketTotal;
                    transactionValue4.UserName = OrderItemRequest.OrderItem.UserId;
                    AccountTransactionValueRepository.Update(transactionValue4);
                    AccountTransactionValueRepository.Save();
                }
            }
            objAccountTransactionValue = new AccountTransactionValue();
            objAccountTransactionValue = list2.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 8));
            if (objAccountTransactionValue != null)
            {
                transactionValue1 = new AccountTransactionValue();
                AccountTransactionValue transactionValue5 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                if (transactionValue5 != null)
                {
                    transactionValue5.Credit = OrderItemRequest.VatAmount;
                    transactionValue5.UserName = OrderItemRequest.OrderItem.UserId;
                    AccountTransactionValueRepository.Update(transactionValue5);
                    AccountTransactionValueRepository.Save();
                }
            }
            objAccountTransactionValue = new AccountTransactionValue();
            objAccountTransactionValue = list2.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 9));
            if (objAccountTransactionValue != null)
            {
                transactionValue1 = new AccountTransactionValue();
                AccountTransactionValue transactionValue6 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                if (transactionValue6 != null)
                {
                    transactionValue6.Credit = OrderItemRequest.ServiceCharge;
                    transactionValue6.UserName = OrderItemRequest.OrderItem.UserId;
                    AccountTransactionValueRepository.Update(transactionValue6);
                    AccountTransactionValueRepository.Save();
                }
            }
            return num;
        }

        public static string TicketLastNumber(IDCubeRepository<Ticket> TicketRepository)
        {
            string s = "0";
            if (TicketRepository.GetAllData().ToList<Ticket>().Count > 0)
                s = TicketRepository.GetAllData().OrderByDescending<Ticket, int>((Func<Ticket, int>)(o => o.Id)).FirstOrDefault<Ticket>().TicketNumber;
            return (int.Parse(s) + 1).ToString();
        }

        private static int OrderNewNumber(IDCubeRepository<Order> OrderRepository)
        {
            int num = 0;
            if (OrderRepository.GetAllData().ToList<Order>().Count > 0)
                num = OrderRepository.GetAllData().OrderByDescending<Order, int>((Func<Order, int>)(o => o.Id)).FirstOrDefault<Order>().OrderNumber;
            return num + 1;
        }

        private static int OrderLastNumber(IDCubeRepository<Order> OrderRepository)
        {
            int num = 0;
            if (OrderRepository.GetAllData().ToList<Order>().Count > 0)
                num = OrderRepository.GetAllData().OrderByDescending<Order, int>((Func<Order, int>)(o => o.Id)).FirstOrDefault<Order>().OrderNumber;
            return num;
        }

        public static Decimal TotalDaySale(
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          string UserName)
        {
            return OrderRepository.GetAllData().ToList<Order>().Where<Order>((Func<Order, bool>)(o =>
           {
               DateTime dateTime = o.CreatedDateTime;
               string shortDateString1 = dateTime.ToShortDateString();
               dateTime = DateTime.Now;
               string shortDateString2 = dateTime.ToShortDateString();
               return shortDateString1 == shortDateString2 && o.CreatingUserName == UserName;
           })).ToList<Order>().Select(o => new
           {
               TotalPrice = o.Quantity * o.Price
           }).Sum(o => o.TotalPrice);
        }

        public static Decimal TotalDaySale(
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository)
        {
            return OrderRepository.GetAllData().ToList<Order>().Where<Order>((Func<Order, bool>)(o =>
           {
               DateTime dateTime = o.CreatedDateTime;
               string shortDateString1 = dateTime.ToShortDateString();
               dateTime = DateTime.Now;
               string shortDateString2 = dateTime.ToShortDateString();
               return shortDateString1 == shortDateString2;
           })).ToList<Order>().Select(o => new
           {
               TotalPrice = o.Quantity * o.Price
           }).Sum(o => o.TotalPrice);
        }

        public static Decimal TotalItemSale(
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          string UserName)
        {
            return OrderRepository.GetAllData().ToList<Order>().Where<Order>((Func<Order, bool>)(o =>
           {
               DateTime dateTime = o.CreatedDateTime;
               string shortDateString1 = dateTime.ToShortDateString();
               dateTime = DateTime.Now;
               string shortDateString2 = dateTime.ToShortDateString();
               return shortDateString1 == shortDateString2 && o.CreatingUserName == UserName;
           })).ToList<Order>().Select(o => new
           {
               TotalQuantity = o.Quantity
           }).Sum(o => o.TotalQuantity);
        }

        public static Decimal TotalItemSale(
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository)
        {
            return OrderRepository.GetAllData().ToList<Order>().Where<Order>((Func<Order, bool>)(o =>
           {
               DateTime dateTime = o.CreatedDateTime;
               string shortDateString1 = dateTime.ToShortDateString();
               dateTime = DateTime.Now;
               string shortDateString2 = dateTime.ToShortDateString();
               return shortDateString1 == shortDateString2;
           })).ToList<Order>().Select(o => new
           {
               TotalQuantity = o.Quantity
           }).Sum(o => o.TotalQuantity);
        }

        public static Decimal TotalMothSale(
          IDCubeRepository<Order> OrderRepository,
          string FinancialYear)
        {
            DateTime now = DateTime.Now;
            int year = now.Year;
            now = DateTime.Now;
            int month = now.Month;
            now = DateTime.Now;
            int day = now.Day;
            string NMonth = NepalitoEnglish.englishToNepaliMonth(year, month, day);
            return OrderRepository.GetAllData().ToList<Order>().Where<Order>((Func<Order, bool>)(o => o.NepaliMonth == NMonth && o.FinancialYear == FinancialYear)).ToList<Order>().Select(o => new
            {
                TotalPrice = o.Quantity * o.Price
            }).Sum(o => o.TotalPrice);
        }

        public static List<MonthSale> TotalListMothSale(IDCubeRepository<Order> OrderRepository, string FinancialYear)
        {
            List<MonthSale> listMonthSale = new List<MonthSale>();
            var ListOrder = OrderRepository.GetAllData().ToList().Where(o => o.FinancialYear == FinancialYear).ToList().Select(o => new { o.NepaliMonth, TotalPrice = o.Quantity * o.Price });
            var ListOrderMonth = from line in ListOrder
                                 group line by line.NepaliMonth into g
                                 select new MonthSale
                                 {
                                     Month = g.First().NepaliMonth,
                                     Amount = g.Sum(pc => pc.TotalPrice)
                                 };
            foreach (var lom in ListOrderMonth)
            {
                MonthSale objMonthSale = new MonthSale();
                objMonthSale.Amount = lom.Amount;
                objMonthSale.Month = lom.Month;
                listMonthSale.Add(objMonthSale);
            }
            return listMonthSale;
        }

        public static List<ScreenOrder> GetTicketOrder(
      int TicketId,
      IDCubeRepository<Ticket> TicketRepository,
      IDCubeRepository<Order> OrderRepository,
      IDCubeRepository<ExceptionLog> exceptionRepository)
        {
            List<ScreenOrder> ticketOrder = new List<ScreenOrder>();
            List<Order> orderList = new List<Order>();
            Ticket ticket1 = new Ticket();
            try
            {
                if (TicketId != 0)
                {
                    Ticket ticket2 = TicketRepository.SelectDataById((object)TicketId);
                    if (ticket2 != null)
                    {
                        List<Order> list1 = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.TicketId == TicketId)).ToList<Order>();
                        if (list1 != null)
                        {
                            foreach (Order order1 in list1.GroupBy<Order, int>((Func<Order, int>)(p => p.OrderNumber)).Select<IGrouping<int, Order>, Order>((Func<IGrouping<int, Order>, Order>)(g => g.First<Order>())).ToList<Order>())
                            {
                                Order objcurrentorder = order1;
                                ScreenOrder screenOrder1 = new ScreenOrder();
                                screenOrder1.Id = objcurrentorder.Id;
                                ScreenOrder screenOrder2 = screenOrder1;
                                int num = objcurrentorder.OrderNumber;
                                string str1 = num.ToString();
                                screenOrder2.OrderNumber = str1;
                                screenOrder1.OrderOpeningTime = objcurrentorder.CreatedDateTime.ToString();
                                screenOrder1.OrderStatus = objcurrentorder.OrderStates;
                                ScreenOrder screenOrder3 = screenOrder1;
                                num = ticket2.Table_Customer_Room;
                                string str2 = num.ToString();
                                screenOrder3.TableId = str2;
                                screenOrder1.UserId = objcurrentorder.CreatingUserName;
                                screenOrder1.TicketId = TicketId.ToString();
                                List<Order> list2 = list1.Where<Order>((Func<Order, bool>)(o => o.OrderNumber == objcurrentorder.OrderNumber)).ToList<Order>();
                                List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
                                foreach (Order order2 in list2)
                                    screenOrderDetailsList.Add(new ScreenOrderDetails()
                                    {
                                        Id = order2.Id,
                                        IsSelected = order2.IsSelected,
                                        ItemId = order2.MenuItemId,
                                        OrderId = order2.Id,
                                        OrderNumber = order2.OrderNumber,
                                        Qty = (Decimal)Convert.ToInt16(order2.Quantity),
                                        Tags = order2.OrderStates,
                                        TotalAmount = (Decimal)((int)Convert.ToInt16(order2.Price) * (int)Convert.ToInt16(order2.Quantity)),
                                        UnitPrice = (Decimal)Convert.ToInt16(order2.Price),
                                        UserId = order2.CreatingUserName,
                                        DepartmentId = order2.DepartmentId,
                                        OrderDescription = order2.OrderDescription,
                                        FinancialYear = order2.FinancialYear
                                    });
                                screenOrder1.OrderItems = (IEnumerable<ScreenOrderDetails>)screenOrderDetailsList;
                                ticketOrder.Add(screenOrder1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
            }
            return ticketOrder;
        }

        private static Decimal GetTicketDiscount(
          string TicketNumber,
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository)
        {
            Decimal ticketDiscount;
            try
            {
                AccountType objAccountType = new AccountType();
                objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Id == 20)).FirstOrDefault<AccountType>();
                if (objAccountType == null)
                    objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>();
                AccountTransactionDocument objAccountTransactionDocument = TransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name == objAccountType.Name + " [#" + TicketNumber + "]")).FirstOrDefault<AccountTransactionDocument>();
                AccountTransaction accountTransaction = new AccountTransaction();
                accountTransaction = AccountTranastionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).FirstOrDefault<AccountTransaction>();
                List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
                List<AccountTransactionValue> list = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransactionValue>();
                AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                transactionValue1 = new AccountTransactionValue();
                AccountTransactionValue transactionValue2 = list.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 3));
                ticketDiscount = transactionValue2 == null ? 0M : transactionValue2.Debit;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ticketDiscount = 0M;
            }
            return ticketDiscount;
        }

        public static Ticket GetTicketInvoicePrint(
          int TicketId,
          IDCubeRepository<Ticket> TicketRepository)
        {
            Ticket ticketInvoicePrint = new Ticket();
            try
            {
                if (TicketId != 0)
                {
                    ticketInvoicePrint = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == TicketId.ToString())).FirstOrDefault<Ticket>();
                    if (ticketInvoicePrint != null)
                    {
                        ticketInvoicePrint.IsClosed = true;
                        ticketInvoicePrint.IS_Bill_Active = false;
                        ticketInvoicePrint.IS_Bill_Printed = true;
                        TicketRepository.Update(ticketInvoicePrint);
                        TicketRepository.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
            }
            return ticketInvoicePrint;
        }

        public static List<TicketReference> GetCustomerTicket(
          int CustomerId,
          IDCubeRepository<Ticket> TicketRepository)
        {
            List<Ticket> ticketList = new List<Ticket>();
            List<TicketReference> customerTicket = new List<TicketReference>();
            try
            {
                if (CustomerId != 0)
                {
                    foreach (Ticket ticket in TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Table_Customer_Room == CustomerId && o.TicketTypeId == 1)).ToList<Ticket>())
                        customerTicket.Add(new TicketReference()
                        {
                            Id = int.Parse(ticket.TicketNumber),
                            Name = ticket.TicketNumber
                        });
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
            }
            return customerTicket;
        }

        public static List<TicketReference> GetCustomerTicket(
          IDCubeRepository<Ticket> TicketRepository)
        {
            List<Ticket> ticketList = new List<Ticket>();
            List<TicketReference> customerTicket = new List<TicketReference>();
            foreach (Ticket ticket in TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketTypeId == 1)).ToList<Ticket>())
                customerTicket.Add(new TicketReference()
                {
                    Id = int.Parse(ticket.TicketNumber),
                    Name = ticket.TicketNumber
                });
            return customerTicket;
        }

        private static ScreenOrderItemRequest screenorderitemrequest(
          ScreenMultiOrderItemRequest OrderItemRequest)
        {
            ScreenOrderItemRequest orderItemRequest = new ScreenOrderItemRequest();
            orderItemRequest.Balance = OrderItemRequest.Balance;
            orderItemRequest.CustomerId = OrderItemRequest.CustomerId;
            orderItemRequest.Discount = OrderItemRequest.Discount;
            orderItemRequest.FinancialYear = OrderItemRequest.FinancialYear;
            orderItemRequest.GrandTotal = OrderItemRequest.GrandTotal;
            orderItemRequest.OrderId = OrderItemRequest.OrderId;
            orderItemRequest.TableId = OrderItemRequest.TableId;
            orderItemRequest.TicketId = OrderItemRequest.TicketId;
            orderItemRequest.TicketTotal = OrderItemRequest.TicketTotal;
            orderItemRequest.UserId = OrderItemRequest.UserId;
            orderItemRequest.VatAmount = OrderItemRequest.VatAmount;
            orderItemRequest.ServiceCharge = OrderItemRequest.ServiceCharge;
            ScreenOrderDetails screenOrderDetails1 = new ScreenOrderDetails();
            ScreenOrderDetails screenOrderDetails2 = OrderItemRequest.ListOrderItem.First<ScreenOrderDetails>();
            orderItemRequest.OrderItem = screenOrderDetails2;
            return orderItemRequest;
        }

        public static ScreenMultiOrderItemResponse VoidOrder(
          IDCubeRepository<AccountType> RepositoryAccountType,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> RepositoryAccount,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<ExceptionLog> exceptionRepository,
          ScreenMultiOrderItemRequest OrderItemRequest,
          string TicketId)
        {
            string accountdocument = "0";
            ScreenTicket screenTicket = new ScreenTicket();
            ScreenOrder screenOrder = new ScreenOrder();
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            ScreenMultiOrderItemResponse orderItemResponse = new ScreenMultiOrderItemResponse();
            List<ScreenOrder> screenOrderList = new List<ScreenOrder>();
            using (UnitOfWork uof = new UnitOfWork())
            {
                ScreenOrderItemRequest orderItemRequest = new ScreenOrderItemRequest();
                ScreenOrderItemRequest OrderItemRequest1 = TicketBusiness.screenorderitemrequest(OrderItemRequest);
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id.ToString() == TicketId)).FirstOrDefault<Ticket>();
                string ticketNumber = ticket2.TicketNumber;
                try
                {
                    if (ticket2 != null)
                    {
                        ticket2.TotalAmount = 0M;
                        ticket2.IsClosed = true;
                        ticket2.IS_Bill_Printed = true;
                        ticket2.TicketNumber = "0";
                        ticket2.IsLocked = true;
                        ticket2.RemainingAmount = 0M;
                        ticket2.TicketStates = "Void Ticket " + (object)OrderItemRequest.TicketId;
                        TicketRepository.Update(ticket2);
                        TicketRepository.Save();
                    }
                    else
                    {
                        uof.RollBackTransaction();
                        orderItemResponse = (ScreenMultiOrderItemResponse)null;
                        return orderItemResponse;
                    }
                }
                catch (Exception ex)
                {
                    uof.RollBackTransaction();
                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                    return orderItemResponse;
                }
                if (TicketBusiness.AccountTransactionValueUpdate(OrderItemRequest1, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, ticket2.TicketNumber, accountdocument) != 0)
                {
                    uof.RollBackTransaction();
                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                    return orderItemResponse;
                }
                if (TicketBusiness.AccountTransactionValueUpdate(new ScreenOrderItemRequest()
                {
                    Balance = 0M,
                    Discount = 0M,
                    GrandTotal = 0M,
                    ServiceCharge = 0M,
                    TicketTotal = 0M,
                    VatAmount = 0M,
                    OrderItem = OrderItemRequest.ListOrderItem.FirstOrDefault<ScreenOrderDetails>()
                }, RepositoryAccountType, TransactionDocumentRepository, AccountTranastionRepository, AccountTransactionValueRepository, uof, ticketNumber, accountdocument) != 0)
                {
                    uof.RollBackTransaction();
                    orderItemResponse = (ScreenMultiOrderItemResponse)null;
                    return orderItemResponse;
                }
                screenTicket.CustomerId = OrderItemRequest1.CustomerId;
                screenTicket.Discount = OrderItemRequest1.Discount;
                screenTicket.ServiceCharge = OrderItemRequest1.ServiceCharge;
                screenTicket.Id = ticket2.Id;
                screenTicket.IsActive = true;
                screenTicket.isSubmitted = false;
                screenTicket.Name = ticket2.TicketNumber;
                screenTicket.Note = ticket2 == null ? "" : ticket2.Note;
                screenTicket.TableId = int.Parse(OrderItemRequest.TableId);
                screenTicket.TicketId = OrderItemRequest.TicketId;
                screenTicket.TicketOpeningTime = ticket2.Date;
                screenTicket.TotalAmount = OrderItemRequest.TicketTotal;
                List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
                List<PaymentHistory> ticketPaymentHistory = TicketBusiness.GetTicketPaymentHistory(TicketRepository, RepositoryAccountType, TransactionDocumentRepository, AccountTransactionTypeRepository, AccountTransactionValueRepository, ticket2.Id);
                screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)ticketPaymentHistory;
                List<Order> orderList = new List<Order>();
                List<Order> list = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.TicketId == OrderItemRequest.TicketId)).ToList<Order>();
                if (list != null)
                {
                    foreach (Order order in list)
                        screenOrderDetailsList.Add(new ScreenOrderDetails()
                        {
                            Id = order.Id,
                            IsSelected = false,
                            IsVoid = order.OrderStates == "Void",
                            ItemId = order.MenuItemId,
                            OrderId = order.Id,
                            OrderNumber = order.OrderNumber,
                            Qty = order.Quantity,
                            Tags = order.Tag,
                            TotalAmount = order.Quantity * order.Price,
                            UnitPrice = order.Price
                        });
                    screenOrder.Id = list.First<Order>().OrderNumber;
                    screenOrder.OrderItems = (IEnumerable<ScreenOrderDetails>)screenOrderDetailsList;
                    screenOrder.OrderNumber = list.First<Order>().OrderNumber.ToString();
                    screenOrder.OrderOpeningTime = list.First<Order>().CreatedDateTime.ToString();
                    screenOrder.OrderStatus = list.First<Order>().Tag;
                    screenOrder.TableId = OrderItemRequest.TableId;
                    screenOrder.TicketId = OrderItemRequest.TicketId.ToString();
                    screenOrder.UserId = OrderItemRequest.UserId;
                    screenOrderList.Add(screenOrder);
                    orderItemResponse.CustomerId = OrderItemRequest.CustomerId;
                    orderItemResponse.ListOrder = screenOrderList;
                    orderItemResponse.OrderId = list.First<Order>().OrderNumber;
                    orderItemResponse.TableId = OrderItemRequest.TableId;
                    orderItemResponse.Ticket = screenTicket;
                    orderItemResponse.TicketId = OrderItemRequest.TicketId;
                }
            }
            return orderItemResponse;
        }

        public static int OrderTableMove(
          string baseTable,
          string baseTicketNumber,
          string distinationTable,
          IDCubeRepository<Ticket> TicketRepository)
        {
            Ticket ticket1 = new Ticket();
            Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == baseTicketNumber && o.Table_Customer_Room.ToString() == baseTable && o.TicketTypeId == 2)).FirstOrDefault<Ticket>();
            int num;
            try
            {
                if (ticket2 == null)
                    return 0;
                ticket2.Table_Customer_Room = int.Parse(distinationTable);
                TicketRepository.Update(ticket2);
                TicketRepository.Save();
                num = 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
            return num;
        }
    }
}
