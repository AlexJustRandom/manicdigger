
public enum NewWorldPages { 
Default,
ServerSettings,
Mods
}
public class NewWorld : MainMenuScreen
{

    public NewWorld()
    {
        // initialize widgets
        wtbx_name = new LabeledTextBoxWidget();
        AddWidget(wtbx_name);
    
        wbtn_serveroptions = new ButtonWidget();
        AddWidget(wbtn_serveroptions);

        wbtn_serverModOptions = new ButtonWidget();
        AddWidget(wbtn_serverModOptions);

        wbtn_create = new ButtonWidget();
        AddWidget(wbtn_create);

        wbtn_back = new ButtonWidget();
        AddWidget(wbtn_back);

        wtxt_title = new TextWidget();
        wtxt_title.SetFont(fontTitle);
        AddWidget(wtxt_title);



        wlst_SettingList = new SettingsListWidget();
         setting=new SettingListEntry[7];
        int i = 0;
        setting[i] = new SettingListEntry();
        setting[i]._label = "Public Server";
        setting[i]._setting = "Server_SetupPublic";
        setting[i]._value = "false";
        setting[i]._type = SettingEntryType.String;
        i++;

        setting[i] = new SettingListEntry();
        setting[i]._label = "Server Name";
        setting[i]._setting = "Server_SetupName";
        setting[i]._value = "SNAME";
        setting[i]._type = SettingEntryType.String;
        i++;

        setting[i] = new SettingListEntry();
        setting[i]._label = "Public Server";
        setting[i]._setting = "Server_SetupPublic";
        setting[i]._value = "false";
        setting[i]._type = SettingEntryType.String;
        i++;

        setting[i] = new SettingListEntry();
        setting[i]._label = "MODT";
        setting[i]._value = "Server_SetupMOTD";
        setting[i]._setting = "";
        setting[i]._type = SettingEntryType.String;
        i++;

        setting[i] = new SettingListEntry();
        setting[i]._label = "Server_SetupWelcomeMessage";
        setting[i]._setting = "Server_SetupWelcomeMessage";
        setting[i]._value = "Welcome ";
        setting[i]._type = SettingEntryType.String;
        i++;

        setting[i] = new SettingListEntry();
        setting[i]._label = "Server_SetupEnableHTTP ";
        setting[i]._setting = "Server_SetupEnableHTTP";
        setting[i]._value = "false";
        setting[i]._type = SettingEntryType.Bool;
        i++;

        setting[i] = new SettingListEntry();
        setting[i]._label = "Server_SetupMaxClients ";
        setting[i]._setting = "Server_SetupMaxClients";
        setting[i]._value = "16";
        setting[i]._type = SettingEntryType.Int;

        for (int j=0;j<7;j++)
       wlst_SettingList.AddElement(setting[j]);


        wtbx_name = new LabeledTextBoxWidget();
        AddWidget(wtbx_name);


        AddWidget(wlst_SettingList);
        newWorldPage = NewWorldPages.Default;


        wmm_ModManager = new ModManagerWidget();
        AddWidget(wmm_ModManager);

        wtbx_name.visible = true;

        wlst_SettingList.visible = false;
        wmm_ModManager.visible = false;




    }
    ModManagerWidget wmm_ModManager;



    SettingListEntry[] setting;
    LabeledTextBoxWidget wtbx_name;
    ButtonWidget wbtn_back; 

    SettingsListWidget wlst_SettingList;
 

    ButtonWidget wbtn_serveroptions;
    ButtonWidget wbtn_serverModOptions;
    ButtonWidget wbtn_create;
 
    TextWidget wtxt_title;
    NewWorldPages newWorldPage;
    bool loaded;


     

    public override void LoadTranslations()
    {
        wbtn_back.SetText(menu.lang.Get("MainMenu_ButtonBack"));
        wbtn_create.SetText(menu.lang.Get("MainMenu_CreateWorld")); 
        wtxt_title.SetText(menu.lang.Get("MainMenu_Singleplayer"));
        wbtn_serveroptions.SetText(menu.lang.Get("MainMenu_ServerOptions"));  
        wbtn_serverModOptions.SetText(menu.lang.Get("MainMenu_ModOptions"));

        wmm_ModManager.LoadTranslationsandMods(gamePlatform, menu);

    }

    public override void Render(float dt)
    {
        // load stored values or defaults
        if (!loaded)
        {
            wtbx_name.SetLabel("World name"); //TODO LANG
            wtbx_name.SetContent(gamePlatform,"New World");
            loaded = true;

 
        }


        float scale = menu.uiRenderer.GetScale();
        float leftx = gamePlatform.GetCanvasWidth() / 2 - 128 * scale;
        float y = gamePlatform.GetCanvasHeight() / 2 ;

        float wlst_SettingListPading= 100 *scale;

        wtxt_title.SetX(gamePlatform.GetCanvasWidth() / 2 - wtxt_title.sizex/2);

        wlst_SettingList.x = wlst_SettingListPading;
        wlst_SettingList.y = wlst_SettingListPading;
        wlst_SettingList.sizex = gamePlatform.GetCanvasWidth() - wlst_SettingListPading*2;
        wlst_SettingList.sizey = gamePlatform.GetCanvasHeight() - wlst_SettingListPading*2;
 

        wtbx_name.x = leftx-128*scale;
        wtbx_name.y = y + 100 * scale;
        wtbx_name.sizex = 512 * scale;
        wtbx_name.sizey = 64 * scale;


        wbtn_serveroptions.x = 40 * scale + 256 * scale;
        wbtn_serveroptions.y = gamePlatform.GetCanvasHeight() - 104 * scale;
        wbtn_serveroptions.sizex = 256 * scale;
        wbtn_serveroptions.sizey = 64 * scale;

        wbtn_serverModOptions.x = 40 * scale + 512 * scale;
        wbtn_serverModOptions.y = gamePlatform.GetCanvasHeight() - 104 * scale;
        wbtn_serverModOptions.sizex = 256 * scale;
        wbtn_serverModOptions.sizey = 64 * scale;

        wbtn_create.x = 40 * scale + (512+256) * scale;
        wbtn_create.y = gamePlatform.GetCanvasHeight() - 104 * scale;
        wbtn_create.sizex = 256 * scale;
        wbtn_create.sizey = 64 * scale;

        wbtn_back.x = 40 * scale;
        wbtn_back.y = gamePlatform.GetCanvasHeight() - 104 * scale;
        wbtn_back.sizex = 256 * scale;
        wbtn_back.sizey = 64 * scale;



        float offsetfromborder = 50 * scale;


        wmm_ModManager.x = offsetfromborder;
        wmm_ModManager.y = 0;
        wmm_ModManager.sizex = gamePlatform.GetCanvasWidth() - offsetfromborder * 2;
        wmm_ModManager.sizey = gamePlatform.GetCanvasHeight() - offsetfromborder;


        DrawWidgets(dt);


    }

    public override void OnBackPressed()
    {
        menu.StartMainMenu();
    }
  
    public override void OnButton(AbstractMenuWidget w)
    {
       


        if (w == wbtn_back)
        {
            OnBackPressed();
        }
        if (w == wbtn_serveroptions)
        {

           
        }
        if (w == wbtn_serverModOptions || w == wbtn_serveroptions)
        {
            //this shud be a widget
            switch (newWorldPage)
            {
                case NewWorldPages.Default:
                    newWorldPage = (w == wbtn_serverModOptions) ? NewWorldPages.Mods : NewWorldPages.ServerSettings;
                    break;
                case NewWorldPages.ServerSettings:
                    newWorldPage = (w == wbtn_serverModOptions) ? NewWorldPages.Mods : NewWorldPages.Default;
                    break;
                case NewWorldPages.Mods:
                    newWorldPage = (w == wbtn_serverModOptions) ? NewWorldPages.Default : NewWorldPages.ServerSettings;
                    break;
            }

            wbtn_serveroptions.SetText(menu.lang.Get("MainMenu_ServerOptions"));
            wbtn_serverModOptions.SetText(menu.lang.Get("MainMenu_ModOptions"));

            wtbx_name.visible = false;
            wmm_ModManager.visible = false;
            wlst_SettingList.visible = false;
  

            switch (newWorldPage)
            {
                case NewWorldPages.Mods:
                    wbtn_serverModOptions.SetText(menu.lang.Get("MainMenu_WorldOptions"));
                    wmm_ModManager.visible = true;
  
                    wtxt_title.SetText(menu.lang.Get("MainMenu_ModOptions"));

                    break;
                case NewWorldPages.ServerSettings:
                    wbtn_serveroptions.SetText(menu.lang.Get("MainMenu_WorldOptions"));
                    wtxt_title.SetText(menu.lang.Get("MainMenu_ServerOptions"));

                    wlst_SettingList.visible = true; //TODO LANG
                    break;
                case NewWorldPages.Default:
                    wtxt_title.SetText(menu.lang.Get("MainMenu_WorldOptions"));

                    wtbx_name.visible = true;
                    break;
            }
        }
        if (w == wbtn_create)
        {
            string name = wtbx_name.GetContent();
            IntRef savegamesCount_=new IntRef();
            string[] savegames = menu.GetSavegames(savegamesCount_);
            //TODO ITS STUPID CODE THERE MUST BE A BETTER WAY
            bool contains;


            for (int j = 2; true; j++)
            {   contains = false;
                for (int k = 0; k < savegamesCount_.value; k++)
                {
                    if (menu.p.FileName(savegames[k]) == menu.p.StringFormat2("{0} ({1})",name,menu.p.IntToString(j)))
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains) {
                    name = menu.p.StringFormat2("{0} ({1})", name, menu.p.IntToString(j));
                    break;
                }
            }
        
            
            string wordname = menu.p.StringFormat2("{0}/{1}.mddbs", menu.p.PathSavegames(), name);

            ServerInitSettings serverInitSettings =new ServerInitSettings();
            serverInitSettings.filename = wordname;
            serverInitSettings.settingsOverride = wlst_SettingList.GetAllElements();

            IntRef activeModslLenght=new IntRef();

            serverInitSettings.mods = wmm_ModManager.GetActiveMods(activeModslLenght);
            serverInitSettings.ModCount = activeModslLenght.GetValue();

            menu.StartGame(true, serverInitSettings, null);

        }

    }


}
