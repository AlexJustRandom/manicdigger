public class LabeledTextWidget : AbstractMenuWidget
{
    TextWidget label;
    TextWidget text;
	public LabeledTextWidget()
	{
        label = new TextWidget();
        text = new TextWidget();
        text.SetAlignment(TextAlign.Right);
    }
	//public TextWidget(float dx, float dy, string text, FontCi font, TextAlign align, TextBaseline baseline)
	//{
	//	x = dx;
	//	y = dy;
	//	_offsetX = 0;
	//	_offsetY = 0;
	//	_font = font;
	//	_align = align;
	//	_baseline = baseline;
	//}

	public override void Draw(float dt, UiRenderer renderer)
	{
        label.Draw(dt, renderer);
        text.Draw(dt, renderer);
    }
 
	public void SetFont(FontCi font)
	{
        label.SetFont(font);
        text.SetFont(font);

    }

    public void SetX(float dx)
	{
        label.SetX(dx);
        text.SetX(dx+sizex);
    }

	public void SetY(float dy)
	{
        label.SetY(dy);
        text.SetY(dy);
    }

 
	public void SetText(string _text)
	{
        text.SetText(_text);
    }
    public void SetLabel(string _text)
    {
        label.SetText(_text);
    }

}
