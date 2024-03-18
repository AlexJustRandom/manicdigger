
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

        wlst_modList = new ListWidget();
        AddWidget(wlst_modList);

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

        wt_ModDesc = new ModDescriptionWidget();
        AddWidget(wt_ModDesc);


        wbtn_switchactive = new ButtonWidget();
        AddWidget(wbtn_switchactive);

        wbtn_configmod = new ButtonWidget();
        AddWidget(wbtn_configmod);
 
 

        wtbx_name.visible = true;

        wlst_SettingList.visible = false;
        wlst_modList.visible = false;

        wt_ModDesc.visible = false;

        wbtn_switchactive.visible = false;
        wbtn_configmod.visible = false;

        wt_ModDesc.SetFont(fontDefault);
 
    }



    SettingListEntry [] setting;
    LabeledTextBoxWidget wtbx_name;
    ButtonWidget wbtn_back; 

    SettingsListWidget wlst_SettingList;
    ListWidget wlst_modList;

    ButtonWidget wbtn_serveroptions;
    ButtonWidget wbtn_serverModOptions;
    ButtonWidget wbtn_create;
 

    ButtonWidget wbtn_switchactive;
    ButtonWidget wbtn_configmod;
    //ButtonWidget wbtn_;
    ModDescriptionWidget wt_ModDesc;



    TextWidget wtxt_title;
    NewWorldPages newWorldPage;
    bool loaded;


    ModInformation[] modinfos;
    bool[] modState;
    IntRef modinfosLenght;


    public override void LoadTranslations()
    {
        wbtn_back.SetText(menu.lang.Get("MainMenu_ButtonBack"));
        wbtn_create.SetText("Create"); //TODO LANG
        wtxt_title.SetText(menu.lang.Get("MainMenu_Singleplayer"));
        wbtn_serveroptions.SetText("Server Options"); //TODO LANG
        wbtn_serverModOptions.SetText("Mod Options"); //TODO LANG

        wbtn_switchactive.SetText("Deactivate"); //TODO LANG
        wbtn_configmod.SetText("Configure"); //TODO LANG
    }

    public override void Render(float dt)
    {
        // load stored values or defaults
        if (!loaded)
        {
            wtbx_name.SetLabel("World name"); //TODO LANG
            wtbx_name.SetContent(gamePlatform,"New World");
            loaded = true;

            modinfosLenght = new IntRef();
            modinfos = menu.GetModinfo(modinfosLenght);
            modState = new bool[modinfosLenght.value];

            for (int m = 0; m < modinfosLenght.GetValue(); m++)
            {
                ListEntry entry = new ListEntry();
                if (modinfos[m] == null) continue;
                modState[m] = true;

                if (modinfos[m].Name!=null)
                    entry.textTopLeft = modinfos[m].Name;

                entry.textTopRight = "&2Active";
                if (modinfos[m].Description != null)
                
                entry.textBottomLeft = modinfos[m].Description;
                if (modinfos[m].Category != null)
                    entry.textBottomRight = modinfos[m].Category;

                wlst_modList.AddElement(entry);
                
            }
            //wbtn_serveroptions.SetText(menu.p.StringFormat("Server Options{0}",menu.p.IntToString(lenght.GetValue())));


        }
        float scale = menu.uiRenderer.GetScale();
        float leftx = gamePlatform.GetCanvasWidth() / 2 - 128 * scale;
        float y = gamePlatform.GetCanvasHeight() / 2 + 0 * scale;

        float wlst_SettingListPading= 100 *scale;

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



        //Modlist / options
        float wlst_modListPading = 50 * scale;
        wlst_modList.x = wlst_modListPading;
        wlst_modList.y = wlst_modListPading;
        wlst_modList.sizex = gamePlatform.GetCanvasWidth() * 3 / 5 - wlst_modListPading * 2;
        wlst_modList.sizey = gamePlatform.GetCanvasHeight() - wlst_modListPading * 4;


        wt_ModDesc.x = wlst_modList.x + wlst_modList.sizex;
        wt_ModDesc.y = wlst_modListPading;
        wt_ModDesc.sizex = gamePlatform.GetCanvasWidth() * 2 / 5 ;
        wt_ModDesc.sizey = gamePlatform.GetCanvasHeight()  - wlst_modListPading * 6;
        wt_ModDesc.SetPaddingX(25*scale);
        wt_ModDesc.SetPaddingY(25*scale);



        float buttonPadding = 5 * scale;
        wbtn_switchactive.x = wt_ModDesc.x + buttonPadding;
        wbtn_switchactive.y = wt_ModDesc.y + wt_ModDesc.sizey + buttonPadding;

        wbtn_switchactive.sizex = wt_ModDesc.sizex / 2 - buttonPadding *2;
        wbtn_switchactive.sizey = 64 * scale;


        wbtn_configmod.x = wbtn_switchactive.x +wbtn_switchactive .sizex+ buttonPadding;
        wbtn_configmod.y = wbtn_switchactive.y;

        wbtn_configmod.sizex = wbtn_switchactive.sizex;
        wbtn_configmod.sizey = wbtn_switchactive.sizey;


        DrawWidgets(dt);


    }

    public override void OnBackPressed()
    {
        menu.StartMainMenu();
    }
  
    public override void OnButton(AbstractMenuWidget w)
    {
        int index = wlst_modList.GetIndexSelected();

        if (w == wlst_modList) {

            if (index != -1)
                wt_ModDesc.SetModinfo(modinfos[index]);
        }

         if(w == wbtn_switchactive) { 
            if (index>-1 && index < modinfosLenght.value) {
                modState[index] = !modState[index];

                if (modState[index])
                    wlst_modList.GetElement(index).textTopRight = "&2Active";//TODO lang
                else
                    wlst_modList.GetElement(index).textTopRight = "&4Inactive";//TODO lang
            }
 
        }
 

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

            wbtn_serverModOptions.SetText("Mod Options"); //TODO LANG
            wbtn_serveroptions.SetText("Server Options"); //TODO LANG

            wtbx_name.visible = false;

            wlst_SettingList.visible = false;
            wlst_modList.visible = false;
            wt_ModDesc.visible = false;
            wbtn_switchactive.visible = false;
            wbtn_configmod.visible = false;

            switch (newWorldPage)
            {
                case NewWorldPages.Mods:
                    wbtn_serverModOptions.SetText("World Options");
                    wlst_modList.visible = true;
                    wt_ModDesc.visible = true;
                    wbtn_switchactive.visible = true;
                    wbtn_configmod.visible = true;
                    wtxt_title.SetText("Mod settings");//TODO LANG

                    break;
                case NewWorldPages.ServerSettings:
                    wbtn_serveroptions.SetText("World Options"); //TODO LANG
                    wtxt_title.SetText("Server settings"); //TODO LANG

                    wlst_SettingList.visible = true; //TODO LANG
                    break;
                case NewWorldPages.Default:
                    wtxt_title.SetText("World settings"); //TODO LANG

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

            int activeModCount = 0;
            for(int m = 0; m < modinfosLenght.value; m++) {
                if (modState[m] != true) continue;
                activeModCount++;
            }
            ModInformation[] activeMods = new ModInformation[activeModCount];
            int activemodIndex=0;
            for (int m = 0; m < modinfosLenght.value; m++)
            {
                if (modState[m] != true) continue;
                activeMods[activemodIndex] = modinfos[m];
                activemodIndex++;
            }

            serverInitSettings.mods = activeMods;
            serverInitSettings.ModCount = activeModCount;

            menu.StartGame(true, serverInitSettings, null);

        }

    }


}
