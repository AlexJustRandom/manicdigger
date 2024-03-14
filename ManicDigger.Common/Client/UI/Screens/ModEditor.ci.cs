public class ScreenModEditor : MainMenuScreen
{
	public ScreenModEditor()
	{
        wbtn_blocks = new ButtonWidget();
        AddWidget(wbtn_blocks);

        wbtn_items = new ButtonWidget();
        AddWidget(wbtn_items);

        wbtn_mobs = new ButtonWidget();
        AddWidget(wbtn_mobs);

        wbtn_back = new ButtonWidget();
		AddWidget(wbtn_back);
		
        wtxt_title = new TextWidget();
        wtxt_title.SetFont(fontTitle);
        wtxt_title.SetAlignment(TextAlign.Left);
		AddWidget(wtxt_title);
	}


    ButtonWidget wbtn_blocks;
    ButtonWidget wbtn_items;
    ButtonWidget wbtn_mobs;

    ButtonWidget wbtn_back;


    TextWidget wtxt_title;

    public void SetModID(string modID) {
        wtxt_title.SetText(menu.p.StringFormat("Mod editor: {0}", modID));//TODO lang
    }

    public override void LoadTranslations()
	{
		wbtn_back.SetText(menu.lang.Get("MainMenu_ButtonBack"));
		wtxt_title.SetText("Mod Editor");//TODO lang

        wbtn_blocks.SetText("Block Editor");//TODO lang
        wbtn_items.SetText("Item Editor");//TODO lang
        wbtn_mobs.SetText("Mob Editor");//TODO lang

    }

    public override void Render(float dt)
	{
		float scale = menu.uiRenderer.GetScale();

        wtxt_title.x = 40 * scale;
        wtxt_title.y = 10 * scale;

       //float windowX = menu.p.GetCanvasWidth();
        float windowY = menu.p.GetCanvasHeight();
        float buttonheight = 64 * scale;
        float buttonwidth = 256 * scale;
        float spacebetween = 5 * scale;
        float offsetfromborder = 50 * scale;
        int index = 4;
 
        wbtn_blocks.x = 40 * scale;
        wbtn_blocks.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
        wbtn_blocks.sizex = buttonwidth;
        wbtn_blocks.sizey = buttonheight;
        index--;

        wbtn_items.x = 40 * scale;
        wbtn_items.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
        wbtn_items.sizex = buttonwidth;
        wbtn_items.sizey = buttonheight;
        index--;

        wbtn_mobs.x = 40 * scale;
        wbtn_mobs.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
        wbtn_mobs.sizex = buttonwidth;
        wbtn_mobs.sizey = buttonheight;
        index--;

        wbtn_back.x = 40 * scale;
        wbtn_back.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
        wbtn_back.sizex = buttonwidth;
        wbtn_back.sizey = buttonheight;
 



        DrawWidgets(dt);
	}

	public override void OnBackPressed()
	{
		menu.StartModManager();
	}

	public override void OnButton(AbstractMenuWidget w)
	{
		if (w == wbtn_back)
		{
			OnBackPressed();
		}
	}
}
