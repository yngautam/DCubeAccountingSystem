using System;

namespace DCubeHotelBusinessLayer.ExtraModel
{
  public class ItemLedger
  {
    public int Id { get; set; }

    public int ItemId { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal Price { get; set; }

    public DateTime TDate { get; set; }

    public string NVDate { get; set; }

    public int CustomerId { get; set; }

    public string UnitType { get; set; }

    public string Name { get; set; }

    public string RefNumber { get; set; }

    public string BillNumber { get; set; }
  }
}
