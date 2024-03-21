public class ScreenMain : MainMenuScreen
{
	public ScreenMain()
	{
		queryStringChecked = false;
		cursorLoaded = false;
		assetsLoaded = false;

		wtxt_loading = new TextWidget();
		wtxt_loading.SetFont(fontDefault);
		wtxt_loading.SetAlignment(TextAlign.Center);
		wtxt_loading.SetBaseline(TextBaseline.Middle);
		AddWidget(wtxt_loading);
		wimg_logo = new ImageWidget();
		wimg_logo.SetTextureName("logo.png");
		AddWidget(wimg_logo);

		wbtn_singleplayer = new ButtonWidget();
		AddWidget(wbtn_singleplayer);

        wbtn_Options = new ButtonWidget();
        AddWidget(wbtn_Options);

        wbtn_ModManger = new ButtonWidget();
        AddWidget(wbtn_ModManger);

        wbtn_multiplayer = new ButtonWidget();
		AddWidget(wbtn_multiplayer);

		wbtn_exit = new ButtonWidget();
		AddWidget(wbtn_exit);
	}

	TextWidget wtxt_loading;
	ImageWidget wimg_logo;
	ButtonWidget wbtn_singleplayer;
    ButtonWidget wbtn_multiplayer;
    ButtonWidget wbtn_Options;
    ButtonWidget wbtn_ModManger;
    ButtonWidget wbtn_exit;
	internal float windowX;
	internal float windowY;
	bool queryStringChecked;
	bool assetsLoaded;
	bool cursorLoaded;

	public override void LoadTranslations()
	{
		wbtn_singleplayer.SetText(menu.lang.Get("MainMenu_Singleplayer"));
		wbtn_multiplayer.SetText(menu.lang.Get("MainMenu_Multiplayer"));
        wbtn_Options.SetText(menu.lang.Get("MainMenu_Options"));
        wbtn_ModManger.SetText(menu.lang.Get("MainMenu_ModManger"));

        wbtn_exit.SetText(menu.lang.Get("MainMenu_Quit"));
	}

	public override void Render(float dt)
	{
		windowX = menu.p.GetCanvasWidth();
		windowY = menu.p.GetCanvasHeight();

		if (!assetsLoaded)
		{
             if (menu.uiRenderer.GetAssetLoadProgress().value != 1)
             {
                string s = menu.p.StringFormat(menu.lang.Get("MainMenu_AssetsLoadProgress"), menu.p.FloatToString(menu.p.FloatToInt(menu.uiRenderer.GetAssetLoadProgress().value * 100)));
				wtxt_loading.SetX(windowX / 2);
				wtxt_loading.SetY(windowY / 2);
				wtxt_loading.SetText(s);
				return;
			}
			assetsLoaded = true;
			wtxt_loading.SetVisible(false);
		}

		if (!cursorLoaded)
		{
			menu.p.SetWindowCursor(0, 0, 32, 32, menu.uiRenderer.GetFile("mousecursor.png"), menu.uiRenderer.GetFileLength("mousecursor.png"));
			cursorLoaded = true;
		}

		if (!queryStringChecked)
		{
			UseQueryStringIpAndPort(menu);
			queryStringChecked = true;
		}

		float scale = menu.uiRenderer.GetScale();
		float buttonheight = 64 * scale;
		float buttonwidth = 256 * scale;
		float spacebetween = 5 * scale;
		float offsetfromborder = 50 * scale;
        int index = 5;

		wimg_logo.sizex = 1024 * scale;
		wimg_logo.sizey = 256 * scale;
		wimg_logo.x = windowX / 2 - wimg_logo.sizex / 2;
		wimg_logo.y = 50 * scale;

		wbtn_singleplayer.x = windowX / 2 - (buttonwidth / 2);
		wbtn_singleplayer.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
		wbtn_singleplayer.sizex = buttonwidth;
		wbtn_singleplayer.sizey = buttonheight;
        index--;

        wbtn_multiplayer.x = windowX / 2 - (buttonwidth / 2);
        wbtn_multiplayer.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
        wbtn_multiplayer.sizex = buttonwidth;
        wbtn_multiplayer.sizey = buttonheight;
        index--;

        wbtn_Options.x = windowX / 2 - (buttonwidth / 2);
        wbtn_Options.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
        wbtn_Options.sizex = buttonwidth;
        wbtn_Options.sizey = buttonheight;
        index--;

        wbtn_ModManger.x = windowX / 2 - (buttonwidth / 2);
        wbtn_ModManger.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
        wbtn_ModManger.sizex = buttonwidth;
        wbtn_ModManger.sizey = buttonheight;
        index--;

        wbtn_exit.visible = menu.p.ExitAvailable();
		wbtn_exit.x = windowX / 2 - (buttonwidth / 2);
		wbtn_exit.y = windowY - (index * (buttonheight + spacebetween)) - offsetfromborder;
		wbtn_exit.sizex = buttonwidth;
		wbtn_exit.sizey = buttonheight;

		DrawWidgets(dt);
	}

	public override void OnButton(AbstractMenuWidget w)
	{
		if (w == wbtn_singleplayer)
		{
			menu.StartSingleplayer();
		}
		if (w == wbtn_multiplayer)
		{
			menu.StartMultiplayer();
		}
        if (w == wbtn_Options)
        {
            menu.StartOptions();
        }
        if (w == wbtn_ModManger)
        {
            menu.StartModManager();
        }
        if (w == wbtn_exit)
		{
			menu.Exit();
		}
	}

	public override void OnBackPressed()
	{
		menu.Exit();
	}

	public override void OnKeyDown(KeyEventArgs e)
	{
		// debug
		if (e.GetKeyCode() == GlKeys.F5)
		{
			menu.p.SinglePlayerServerDisable();
            ServerInitSettings serverInitSettings = new ServerInitSettings();
            serverInitSettings.filename = menu.p.PathCombine(menu.p.PathSavegames(), "Default.mdss");
     		menu.StartGame(true, serverInitSettings, null);
        }
		if (e.GetKeyCode() == GlKeys.F6)
        {
            ServerInitSettings serverInitSettings = new ServerInitSettings();
            serverInitSettings.filename = menu.p.PathCombine(menu.p.PathSavegames(), "Default.mddbs");
            menu.StartGame(true, serverInitSettings, null);
        }
	}

	void UseQueryStringIpAndPort(MainMenu menu)
	{
		string ip = menu.p.QueryStringValue("ip");
		string port = menu.p.QueryStringValue("port");
		int portInt = 25565;
		if (port != null && menu.p.FloatTryParse(port, new FloatRef()))
		{
			portInt = menu.p.IntParse(port);
		}
		if (ip != null)
		{
			menu.StartLogin(null, ip, portInt);
		}
	}
}
