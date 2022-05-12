using System.Drawing;

namespace DCubeHotelSystem.Controllers
{
  public class TextColumn
  {
    public string Text { get; set; }

    public float RelativeWidth { get; set; }

    public StringAlignment Alignment { get; set; }

    public float FontSize { get; set; }

    public TextColumn(string text, float relativeWidth = 1f, StringAlignment alignment = StringAlignment.Near, float fontSize = 10f)
    {
      this.Text = text;
      this.RelativeWidth = relativeWidth;
      this.Alignment = alignment;
      this.FontSize = fontSize;
    }
  }
}
