using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelReservationBL
{
    public static class TableBusinessLayer
    {
        public static List<Table> GetTable(IDCubeRepository<Table> TableRepository)
        {
            List<Table> table = new List<Table>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    table = TableRepository.GetAllData().ToList<Table>();
                    return table;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    table = (List<Table>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return table;
        }

        public static int PostTable(
          IDCubeRepository<Table> TableRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          Table value)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    TableRepository.Insert(value);
                    TableRepository.Save();
                    num = TableRepository.GetAllData().OrderByDescending<Table, int>((Func<Table, int>)(x => x.Id)).FirstOrDefault<Table>().Id;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return num;
        }

        public static int UpdateTable(
          IDCubeRepository<Table> TableRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id,
          Table value)
        {
            int num = 1;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        TableRepository.Update(value);
                        TableRepository.Save();
                        num = value.Id;
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ErrorLogging(ex);
                        num = 0;
                    }
                    unitOfWork.CommitTransaction();
                }
            }
            else
                num = 0;
            return num;
        }

        public static int DeleteTable(
          IDCubeRepository<Table> TableRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    TableRepository.Delete((object)id);
                    TableRepository.Save();
                    num = 1;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return num;
        }

        public static List<ScreenTable> ListTable(
          IDCubeRepository<Table> TableRepository,
          IDCubeRepository<Ticket> TicketRepository)
        {
            List<ScreenTable> screenTableList = new List<ScreenTable>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    IEnumerable<Table> allData = TableRepository.GetAllData();
                    List<Table> tableList = new List<Table>();
                    foreach (Table table in allData)
                    {
                        Table ObjScreenTable = table;
                        ScreenTable screenTable = new ScreenTable();
                        screenTable.Id = ObjScreenTable.Id;
                        screenTable.TableId = ObjScreenTable.Id;
                        screenTable.Name = ObjScreenTable.Name;
                        screenTable.Description = (string)null;
                        screenTable.OrderOpeningTime = DateTime.Now;
                        screenTable.TicketOpeningTime = DateTime.Now;
                        screenTable.LastOrderTime = DateTime.Now;
                        List<Ticket> ticketList = new List<Ticket>();
                        List<ScreenTicket> screenTicketList = new List<ScreenTicket>();
                        screenTable.TableStatus = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => !o.IsClosed && !o.IsLocked && o.Table_Customer_Room == ObjScreenTable.Id && !o.IS_Bill_Printed)).ToList<Ticket>().Count > 0;
                        screenTableList.Add(screenTable);
                    }
                    return screenTableList;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    screenTableList = (List<ScreenTable>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return screenTableList;
        }
    }
}
