using System;

namespace DCubeHotelBusinessLayer.ExtraModel
{
  public class ItemSale
  {
    public int DepartmentId { get; set; }

    public int ItemId { get; set; }

    public string UnitType { get; set; }

    public Decimal Price { get; set; }

    public int CompanyCode { get; set; }

    public int BranchId { get; set; }

    public int WarehouseId { get; set; }

    public int CustomerId { get; set; }

    public Decimal Qty { get; set; }
  }
}
