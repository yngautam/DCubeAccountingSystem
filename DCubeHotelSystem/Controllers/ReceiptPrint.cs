using DCubeHotelDomain.Domain.Models.Settings;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DCubeHotelSystem.Controllers
{
  public class ReceiptPrint : PrintBase
  {
    private string orderId;
    private List<MenuItemWithPrice> listMenuItemPortion;
    private List<Table> listTable;
    private List<Department> listDepartment;
    private List<Printer> listPrinter;
    private List<ScreenOrder> listScreenOrder;
    private string DepartmentId;
    private string DepartmentName;
    private string OrderBy = string.Empty;

    public void Print(
      string printerName,
      string orderId,
      List<MenuItemWithPrice> listMenuItemPortion,
      List<Table> listTable,
      List<Department> listDepartment,
      List<Printer> listPrinter,
      List<ScreenOrder> listScreenOrder,
      string DepartmentId,
      string DepartmentName,
      string OrderBy)
    {
      this.orderId = orderId;
      this.listMenuItemPortion = listMenuItemPortion;
      this.listTable = listTable;
      this.listDepartment = listDepartment;
      this.listPrinter = listPrinter;
      this.listScreenOrder = listScreenOrder;
      this.DepartmentId = DepartmentId;
      this.DepartmentName = DepartmentName;
      this.OrderBy = OrderBy;
      this.Print(printerName, new Action<Graphics>(this.PrintCustomerFragment));
    }

    private void PrintCustomerFragment(Graphics g)
    {
      string orderNumber = this.listScreenOrder[0].OrderNumber;
      string name1 = this.listTable.Find((Predicate<Table>) (o => o.Id == int.Parse(this.listScreenOrder[0].TableId))).Name;
      float y1 = 0.0f + 10f;
      float y2 = (float) ((double) y1 + (double) this.DrawTextColumns(g, y1, new TextColumn(this.DepartmentName, 0.8f, StringAlignment.Center, 14f)));
      float num1 = (float) ((double) y2 + (double) this.DrawTextColumns(g, y2, new TextColumn("Order Ticket ", 0.8f, StringAlignment.Center, 14f)));
      double num2 = (double) num1;
      Graphics g1 = g;
      double y3 = (double) num1;
      TextColumn[] textColumnArray = new TextColumn[1];
      string str = orderNumber;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      string shortDateString = dateTime.ToShortDateString();
      textColumnArray[0] = new TextColumn("Order No. : " + str + "   Date : " + shortDateString, 0.9f);
      double num3 = (double) this.DrawTextColumns(g1, (float) y3, textColumnArray);
      float y4 = (float) (num2 + num3);
      float y5 = (float) ((double) y4 + (double) this.DrawTextColumns(g, y4, new TextColumn("Table No.:" + name1 + "    Time : " + (object) DateTime.Now.Hour + ":" + (object) DateTime.Now.Minute, 0.9f)));
      float y6 = (float) ((double) y5 + (double) this.DrawTextColumns(g, y5, new TextColumn("Item", 0.8f, fontSize: 12f), new TextColumn("Qty ", 0.8f, fontSize: 12f)));
      foreach (ScreenOrder screenOrder in this.listScreenOrder)
      {
        foreach (ScreenOrderDetails orderItem in screenOrder.OrderItems)
        {
          ScreenOrderDetails currentitem = orderItem;
          if (currentitem.DepartmentId.ToString() == this.DepartmentId)
          {
            string name2 = this.listMenuItemPortion.Find((Predicate<MenuItemWithPrice>) (o => o.ItemId == currentitem.ItemId)).Name;
            y6 += 5f;
            y6 += (float) (double) this.DrawTextColumns(g, y6, new TextColumn(name2, 0.8f), new TextColumn(currentitem.Qty.ToString(), 0.2f, StringAlignment.Far));
            y6 += (float) (double) this.DrawTextColumns(g, y6, new TextColumn(currentitem.OrderDescription.ToString(), 0.8f));
          }
        }
      }
      float y7 = y6 + 5f;
      float num4 = (float) ((double) y7 + (double) this.DrawTextColumns(g, y7, new TextColumn("Order By" + this.OrderBy, 0.8f, StringAlignment.Center, 12f)));
    }
  }
}
