public class ModManagerScreen : MainMenuScreen
{
    public ModManagerScreen()
    {
        // initialize widgets

        wbtn_modmanageroptions = new ButtonWidget();
        AddWidget(wbtn_modmanageroptions);

        wbtn_back = new ButtonWidget();
        AddWidget(wbtn_back);

        wlst_modList = new ListWidget();
        AddWidget(wlst_modList);

        wt_ModDesc = new ModDescriptionWidget();
        AddWidget(wt_ModDesc);

        wbtn_switchactive = new ButtonWidget();
        AddWidget(wbtn_switchactive);

        wbtn_configmod = new ButtonWidget();
        AddWidget(wbtn_configmod);
         
        wtxt_title = new TextWidget();
        wtxt_title.SetFont(fontTitle);
        AddWidget(wtxt_title);

        wts_modpack = new SelectWidget();
        AddWidget(wts_modpack);

        wtb_newModpackName = new TextBoxWidget();
        AddWidget(wtb_newModpackName);

        wbtn_acceptName = new ButtonWidget();
        AddWidget(wbtn_acceptName);

        wbtn_cancelName = new ButtonWidget();
        AddWidget(wbtn_cancelName);
    }

    ListWidget wlst_modList;

    ModDescriptionWidget wt_ModDesc;
    ButtonWidget wbtn_switchactive;
    ButtonWidget wbtn_configmod;

    ButtonWidget wbtn_modmanageroptions;

    ButtonWidget wbtn_back;

    TextWidget wtxt_title;

    TextBoxWidget wtb_newModpackName;
    ButtonWidget wbtn_acceptName;

    ButtonWidget wbtn_cancelName;

    SelectWidget wts_modpack;

    ModInformation[] modinfos;
    bool[] modState;
    IntRef modinfosLenght;

    bool loaded;
    bool newModpackActive;

    public override void LoadTranslations()
    {
        wtxt_title.SetText("Modmanager");
        wbtn_back.SetText(menu.lang.Get("MainMenu_ButtonBack"));
        wbtn_modmanageroptions.SetText("Mod Manager Options"); //TODO LANG
        wbtn_switchactive.SetText("Deactivate"); //TODO LANG
        wbtn_configmod.SetText("Configure"); //TODO LANG

        wbtn_acceptName.SetText("Accept"); //TODO LANG
        wbtn_cancelName.SetText("Cancel"); //TODO LANG


        string current = gamePlatform.GetCurrentModpack();
        wts_modpack.SetText(current);

        IntRef lenght = new IntRef();
        string[] modpacks = gamePlatform.GetModpacks(lenght);
        wts_modpack.SetOptionSize(lenght.GetValue() + 1);
        for (int i = 0; i < lenght.value; i++)
        {
            if (modpacks[i] == current) {
                lastSelected = i;
                 selected = i;
            }
            wts_modpack.SetOption(modpacks[i], i);
        }
        wts_modpack.SetOption("New", lenght.GetValue());


      

    }

    public override void Render(float dt)
    {
        if (!loaded)
        {
            loaded = true;

            modinfosLenght = new IntRef();
            modinfos = menu.GetModinfo(modinfosLenght);
            modState = new bool[modinfosLenght.value];

            for (int m = 0; m < modinfosLenght.GetValue(); m++)
            {
                ListEntry entry = new ListEntry();
                if (modinfos[m] == null) continue;
                modState[m] = true;

                if (modinfos[m].Name != null)
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

        float windowX = menu.p.GetCanvasWidth();
        float windowY = menu.p.GetCanvasHeight();

        wtxt_title.x = gamePlatform.GetCanvasWidth() / 2;
        wtxt_title.y = 10;
        wtxt_title.SetAlignment(TextAlign.Center);


        float buttonheight = 64 * scale;
        float buttonwidth = 256 * scale;
        float spacebetween = 5 * scale;
        float offsetfromborder = 50 * scale;

        //Modpack widget
            wts_modpack.SetVisible(!newModpackActive);
            wtb_newModpackName.SetVisible(newModpackActive);
            wbtn_acceptName.SetVisible(newModpackActive);
            wbtn_cancelName.SetVisible(newModpackActive);

            wts_modpack.x = offsetfromborder ;
            wts_modpack.y = spacebetween;
            wts_modpack.sizex = buttonwidth;
            wts_modpack.sizey = buttonheight;
     
            wtb_newModpackName.x = wts_modpack.x;
            wtb_newModpackName.y = wts_modpack.y;
            wtb_newModpackName.sizex = wts_modpack.sizex;
            wtb_newModpackName.sizey = wts_modpack.sizey;

            wbtn_acceptName.x = wtb_newModpackName.x + spacebetween + wtb_newModpackName.sizex;
            wbtn_acceptName.y = wts_modpack.y;
            wbtn_acceptName.sizex = buttonwidth/3;
            wbtn_acceptName.sizey = buttonheight;

            wbtn_cancelName.x = wbtn_acceptName.x + spacebetween + wbtn_acceptName.sizex;
            wbtn_cancelName.y = wts_modpack.y;
            wbtn_cancelName.sizex = buttonwidth / 3;
            wbtn_cancelName.sizey = buttonheight;
        //

        //Modlist / options
        float wlst_modListPading = 50 * scale;
        wlst_modList.x = wlst_modListPading;
        wlst_modList.y = wlst_modListPading+ spacebetween*2+buttonwidth;
        wlst_modList.sizex = gamePlatform.GetCanvasWidth() * 3 / 5 - wlst_modListPading * 2;
        wlst_modList.sizey = gamePlatform.GetCanvasHeight() - wlst_modListPading * 4;


        wt_ModDesc.x = wlst_modList.x + wlst_modList.sizex;
        wt_ModDesc.y = wlst_modListPading;
        wt_ModDesc.sizex = gamePlatform.GetCanvasWidth() * 2 / 5;
        wt_ModDesc.sizey = gamePlatform.GetCanvasHeight() - wlst_modListPading * 6;
        wt_ModDesc.SetPaddingX(25 * scale);
        wt_ModDesc.SetPaddingY(25 * scale);


        int ModDescButtonCount = 2;


        wbtn_switchactive.x = wt_ModDesc.x + spacebetween;
        wbtn_switchactive.y = wt_ModDesc.y + wt_ModDesc.sizey + spacebetween;
        wbtn_switchactive.sizex = wt_ModDesc.sizex / ModDescButtonCount - spacebetween * ModDescButtonCount;
        wbtn_switchactive.sizey = 64 * scale;


        wbtn_configmod.x = wbtn_switchactive.x + wbtn_switchactive.sizex + spacebetween;
        wbtn_configmod.y = wbtn_switchactive.y;
        wbtn_configmod.sizex = wbtn_switchactive.sizex;
        wbtn_configmod.sizey = wbtn_switchactive.sizey;
 
        int index =0;
  
        wbtn_back.x = offsetfromborder+ (index * (buttonwidth + spacebetween));
        wbtn_back.y = gamePlatform.GetCanvasHeight() - offsetfromborder - buttonheight;
        wbtn_back.sizex = buttonwidth;
        wbtn_back.sizey = buttonheight;
        index++;

        wbtn_modmanageroptions.x = offsetfromborder + (index * (buttonwidth + spacebetween));
        wbtn_modmanageroptions.y = gamePlatform.GetCanvasHeight() - offsetfromborder - buttonheight;
        wbtn_modmanageroptions.sizex = buttonwidth;
        wbtn_modmanageroptions.sizey = buttonheight;

        DrawWidgets(dt);

    }

    public override void OnBackPressed()
    {
        menu.StartMainMenu();
    }
    int lastSelected;
    int selected;
    void reloadMods(string name) {
        IntRef lenght = new IntRef();
        string[] mods = gamePlatform.GetMods(name, lenght);
         modinfosLenght = new IntRef();
        modinfos = menu.GetModinfo(modinfosLenght);
        modState = new bool[modinfosLenght.value];
        wlst_modList.Clear();
        for (int m = 0; m < modinfosLenght.GetValue(); m++)
        {
            ListEntry entry = new ListEntry();
            if (modinfos[m] == null) continue;
           

            if (modinfos[m].Name != null)
                entry.textTopLeft = modinfos[m].Name;
            bool found = false;
            for (int i = 0; i < lenght.GetValue(); i++)
            {
                 if (modinfos[m].ModID == mods[i]) {
                    found = true;
                    break;
                }
            }
            modState[m] = found;
            if (found) {
                entry.textTopRight = "&2Active";
            }
            else {
                entry.textTopRight = "&4Inactive";
            }

            if (modinfos[m].Description != null)

                entry.textBottomLeft = modinfos[m].Description;
            if (modinfos[m].Category != null)
                entry.textBottomRight = modinfos[m].Category;

            wlst_modList.AddElement(entry);

        }
 

    }
    public override void OnButton(AbstractMenuWidget w)
    {
        int modListIndex = wlst_modList.GetIndexSelected();

        if (w == wbtn_acceptName)
        {
            string content = wtb_newModpackName.GetContent();

            if (content=="") return;
 
            IntRef lenght = new IntRef();

            string[] modpackList = gamePlatform.GetModpacks(lenght);
            bool found = false;
            for(int i = 0; i < lenght.GetValue(); i++) 
            {
                if (content == modpackList[i]) {
                    found = true;
                    break;
                }
            }
            if (found) return;

            int activeModCount = 0;
            for (int m = 0; m < modinfosLenght.value; m++)
            {
                if (modState[m] != true) continue;
                activeModCount++;
            }
            string[] activeMods = new string[activeModCount];
            int activemodIndex = 0;
            for (int m = 0; m < modinfosLenght.value; m++)
            {
                if (modState[m] != true) continue;
                activeMods[activemodIndex] = modinfos[m].ModID;
                activemodIndex++;
            }

            gamePlatform.SaveModpack(content, activeMods);
            gamePlatform.SetCurrentModpack(content);
 
            wts_modpack.SetOptionSize(lenght.GetValue() + 2);

             for (int i = 0; i < lenght.GetValue(); i++)
            {
                wts_modpack.SetOption(modpackList[i], i);
            }
 
            wts_modpack.SetOption(content, lenght.GetValue());
            wts_modpack.SetOption("New", lenght.GetValue()+1);

            wts_modpack.SetDisplayedValue(lenght.GetValue());
            
            newModpackActive = false;
         }

        if (w == wbtn_cancelName) 
        {
            selected = lastSelected;
            wts_modpack.SetDisplayedValue(lastSelected);

            wtb_newModpackName.SetContent(gamePlatform,"");
            newModpackActive = false;
        }

        if  (lastSelected != selected ||w == wts_modpack )
        {
            lastSelected = selected;
            selected = wts_modpack.GetSelected();
            gamePlatform.ConsoleWriteLine(gamePlatform.StringFormat("ID SELECTED: {0}", gamePlatform.IntToString(selected)));
            if (selected != -1) {
                wts_modpack.SetDisplayedValue(selected);
                if (selected == wts_modpack.GetOptionSize()-1) 
                {
                    newModpackActive = true;
                }else
                reloadMods(wts_modpack.GetSelectedValue());
            }
        }
        if (w == wlst_modList)
        {
             if (modListIndex != -1)
                wt_ModDesc.SetModinfo(modinfos[modListIndex]);
        }

        if (w == wbtn_modmanageroptions)
        {
            OnBackPressed();
        }
        if (w == wbtn_back)
        {
            OnBackPressed();
        } 

    }




}
