public class SelectWidget : AbstractMenuWidget
{
    TextWidget _text;
    ButtonState _state;
    string _textureNameIdle;
    string _textureNameHover;
    string _textureNamePressed;
    TextWidget[] values;
    int ValueSize;
    FontCi _fontOptions;

    public bool selectionOpen;
    public string[] selectWidgetValues;

    public SelectWidget()
    {
        _state = ButtonState.Normal;
        _textureNameIdle = "button.png";
        _textureNameHover = "button_sel.png";
        _textureNamePressed = "button_sel.png";
        _fontOptions = new FontCi();
        _fontOptions.style = 0;
        _fontOptions.size = 16;
        x = 0;
        y = 0;
        sizex = 0;
        sizey = 0;
        clickable = true;
        focusable = true;
        selected = -1;
    }

    public int GetOptionSize()
    {
        return ValueSize;
    }

    public int GetSelected() {
        return selected;
    }
    public string GetSelectedValue()
    {
        return selectWidgetValues[selected];
    }

    public void SetOptionSize(int size)
    {
        selectWidgetValues = new string[size];
        values = new TextWidget[size];

        ValueSize = size;
    }
    public void SetOption(string value, int id)
    {
        values[id] = new TextWidget();
        values[id].SetFont(_fontOptions);
        values[id].SetText(value);
        selectWidgetValues[id] = value;
    }
    void Select(bool inside)
    {

        if (!inside)
        {
            selectionOpen = false;
            selected = -1;
            return;
        }
        else {

            selectionOpen = !selectionOpen;
        }


    }

    public void SetDisplayedValue(int i) {
        _text.SetText(selectWidgetValues[i]);
    }

    public void SetIndex(GamePlatform p, MouseEventArgs args) {
        int index = p.FloatToInt((args.GetY() - y) / p.FloatToInt(sizey / 2));
        if (selectionOpen)
        {
            if (index > -1 && index < ValueSize) {
                selected = index;
            }

        }
    }
    bool ClickedOveride;
    public override bool HasBeenClicked(MouseEventArgs args)
    {
        bool value = (visible && clickable && IsCursorInside(args)) || ClickedOveride;
        ClickedOveride = false;
        return value;
    }
    public override bool IsCursorInside(MouseEventArgs args)
    {
        if (!selectionOpen)
        {
            return (args.GetX() >= x && args.GetX() <= x + sizex &&
                args.GetY() >= y && args.GetY() <= y + sizey);
        }
        return (args.GetX() >= x && args.GetX() <= x + sizex &&
              args.GetY() >= y && args.GetY() <= y + sizey * ValueSize);
    }

    int selected;
    public override void OnMouseDown(GamePlatform p, MouseEventArgs args)
    {
        if (IsCursorInside(args))
        {
            SetState(ButtonState.Pressed);
            SetIndex(p, args);
            ClickedOveride =true;
         }
        else
        {
            SetState(ButtonState.Normal);
        }
        Select(IsCursorInside(args));

    }

    public override void OnMouseUp(GamePlatform p, MouseEventArgs args)
    {
        if (IsCursorInside(args))
        {
            SetState(ButtonState.Hover);
         }
        else
        {
            SetState(ButtonState.Normal);
        }

    }

    public override void OnMouseMove(GamePlatform p, MouseEventArgs args)
    {
        // Check if mouse is inside the button rectangle
        if (IsCursorInside(args))
        {
            if (_state == ButtonState.Normal)
            {
                SetState(ButtonState.Hover);
            }
            int index =p.FloatToInt(  (args.GetY()-y) / p.FloatToInt(sizey/2));
            for (int i = 0; i < ValueSize; i++)
            {
                values[i].SetTextColor("");
            }

            if (index>-1&&index<ValueSize)
                values[index].SetTextColor("&3");
        }
        else
        {
            SetState(ButtonState.Normal);
        }
    }

    public override void Draw(float dt, UiRenderer renderer)
    {
        if (!visible) { return; }
        if (sizex <= 0 || sizey <= 0) { return; }

        if (!selectionOpen)
        {
            switch (hasKeyboardFocus ? ButtonState.Hover : _state)
            {
                // TODO: Use atlas textures
                case ButtonState.Normal:
                    renderer.Draw2dTexture(renderer.GetTexture(_textureNameIdle), x, y, sizex, sizey, null, 0, color);
                    break;
                case ButtonState.Hover:
                    renderer.Draw2dTexture(renderer.GetTexture(_textureNameHover), x, y, sizex, sizey, null, 0, color);
                    break;
                case ButtonState.Pressed:
                    renderer.Draw2dTexture(renderer.GetTexture(_textureNamePressed), x, y, sizex, sizey, null, 0, color);
                    break;
            }

            if (_text != null)
            {
                _text.SetX(x + sizex / 2);
                _text.SetY(y + sizey / 2);
                _text.Draw(dt, renderer);
            }
        }
        else {
            renderer.Draw2dTexture(renderer.GetTexture("serverlist_entry_background.png"), x, y, sizex, ValueSize* sizey/2, null, 0, color);
            for (int i=0;i<ValueSize;i++) {
                values[i].SetX(x);
                values[i].SetY(y + sizey/2 * i);
                values[i].SetSizeX(sizex);
                values[i].SetSizeY(sizey);
                values[i].Draw(dt, renderer); 
            }
        }
    }

    public ButtonState GetState()
    {
        return _state;
    }
    public void SetState(ButtonState state)
    {
        _state = state;
    }

    public void SetText(string text)
    {
        if (text == null || text == "") { return; }
        if (_text == null)
        {
            // Create new text widget if none exists
            FontCi font = new FontCi();
            font.size = 14;
            //_text = new TextWidget(x + sizex / 2, y + sizey / 2, text, font, TextAlign.Center, TextBaseline.Middle);
            _text = new TextWidget();
            _text.SetAlignment(TextAlign.Center);
            _text.SetBaseline(TextBaseline.Middle);
            _text.SetFont(font);
            _text.SetText(text);
        }
        else
        {
            // Change text of existing widget
            _text.SetText(text);
        }
    }

    public void SetTextureNames(string textureIdle, string textureHover, string texturePressed)
    {
        if (textureIdle != null && textureIdle != "")
        {
            _textureNameIdle = textureIdle;
        }
        if (textureHover != null && textureHover != "")
        {
            _textureNameHover = textureHover;
        }
        if (texturePressed != null && texturePressed != "")
        {
            _textureNamePressed = texturePressed;
        }
    }
}

