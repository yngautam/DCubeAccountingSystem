using System;
using System.Drawing;
using System.Drawing.Printing;

namespace DCubeHotelSystem.Controllers
{
  public abstract class PrintBase
  {
    protected Action<Graphics> print;
    protected float width;

    protected void Print(string printerName, Action<Graphics> print)
    {
      this.print = print;
      PrintDocument printDocument = new PrintDocument();
      printDocument.PrinterSettings.PrinterName = printerName;
      printDocument.PrintPage += new PrintPageEventHandler(this.PrintPage);
      this.width = (float) ((double) printDocument.DefaultPageSettings.PrintableArea.Width * 2.53999304771423 / 10.0);
      try
      {
        if (!printDocument.PrinterSettings.IsValid)
          return;
        printDocument.Print();
      }
      catch (Exception ex)
      {
        string message = ex.Message;
      }
    }

    private void PrintPage(object sender, PrintPageEventArgs e)
    {
      Graphics graphics = e.Graphics;
      graphics.PageUnit = GraphicsUnit.Millimeter;
      this.print(graphics);
    }

    protected float DrawTextColumns(Graphics g, float y, params TextColumn[] textColumns)
    {
      float num = 0.0f;
      float x = 0.0f;
      foreach (TextColumn textColumn in textColumns)
      {
        using (Font font = new Font(FontFamily.GenericSansSerif, textColumn.FontSize, FontStyle.Regular))
        {
          SizeF sizeF = g.MeasureString(textColumn.Text, font, new SizeF(this.width * textColumn.RelativeWidth, 0.0f));
          if ((double) num < (double) sizeF.Height)
            num = sizeF.Height;
          g.DrawString(textColumn.Text, font, Brushes.Black, new RectangleF(x, y, this.width * textColumn.RelativeWidth, sizeF.Height), new StringFormat()
          {
            Alignment = textColumn.Alignment
          });
          x += this.width * textColumn.RelativeWidth;
        }
      }
      return num;
    }
  }
}
