public class BlockEditor : MainMenuScreen
{
    public BlockEditor()
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

        wbtn_editmod = new ButtonWidget();
        AddWidget(wbtn_editmod);

        wtxt_title = new TextWidget();
        wtxt_title.SetFont(fontTitle);
        AddWidget(wtxt_title);

    }

    ListWidget wlst_modList;

    ModDescriptionWidget wt_ModDesc;
    ButtonWidget wbtn_switchactive;
    ButtonWidget wbtn_configmod;
    ButtonWidget wbtn_editmod;

    ButtonWidget wbtn_modmanageroptions;

    ButtonWidget wbtn_back;

    TextWidget wtxt_title;



    ModInformation[] modinfos;
    bool[] modState;
    IntRef modinfosLenght;

    bool editActive;
    bool loaded;

    public override void LoadTranslations()
    {
        wtxt_title.SetText("Modmanager");
        wbtn_back.SetText(menu.lang.Get("MainMenu_ButtonBack"));
        wbtn_modmanageroptions.SetText("Mod Manager Options");
        wbtn_switchactive.SetText("Deactivate"); //TODO LANG
        wbtn_configmod.SetText("Configure"); //TODO LANG
        wbtn_editmod.SetText("Edit"); //TODO LANG
        editActive = true;
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



        //Modlist / options
        float wlst_modListPading = 50 * scale;
        wlst_modList.x = wlst_modListPading;
        wlst_modList.y = wlst_modListPading;
        wlst_modList.sizex = gamePlatform.GetCanvasWidth() * 3 / 5 - wlst_modListPading * 2;
        wlst_modList.sizey = gamePlatform.GetCanvasHeight() - wlst_modListPading * 4;


        wt_ModDesc.x = wlst_modList.x + wlst_modList.sizex;
        wt_ModDesc.y = wlst_modListPading;
        wt_ModDesc.sizex = gamePlatform.GetCanvasWidth() * 2 / 5;
        wt_ModDesc.sizey = gamePlatform.GetCanvasHeight() - wlst_modListPading * 6;
        wt_ModDesc.SetPaddingX(25 * scale);
        wt_ModDesc.SetPaddingY(25 * scale);


        int ModDescButtonCount = 2;

        if (editActive) ModDescButtonCount++;

        wbtn_switchactive.x = wt_ModDesc.x + spacebetween;
        wbtn_switchactive.y = wt_ModDesc.y + wt_ModDesc.sizey + spacebetween;
        wbtn_switchactive.sizex = wt_ModDesc.sizex / ModDescButtonCount - spacebetween * ModDescButtonCount;
        wbtn_switchactive.sizey = 64 * scale;


        wbtn_configmod.x = wbtn_switchactive.x + wbtn_switchactive.sizex + spacebetween;
        wbtn_configmod.y = wbtn_switchactive.y;
        wbtn_configmod.sizex = wbtn_switchactive.sizex;
        wbtn_configmod.sizey = wbtn_switchactive.sizey;

        if (editActive)
        {
            wbtn_editmod.x = wbtn_configmod.x + wbtn_switchactive.sizex + spacebetween;
            wbtn_editmod.y = wbtn_switchactive.y;
            wbtn_editmod.sizex = wbtn_switchactive.sizex;
            wbtn_editmod.sizey = wbtn_switchactive.sizey;
        }

        int index = 0;

        wbtn_back.x = offsetfromborder + (index * (buttonwidth + spacebetween));
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

    public override void OnButton(AbstractMenuWidget w)
    {
        int index = wlst_modList.GetIndexSelected();

        if (w == wlst_modList)
        {

            if (index != -1)
                wt_ModDesc.SetModinfo(modinfos[index]);
        }
        if (w == wbtn_editmod)
        {
            if (index == -1) index = 0;
            menu.StartModEdit(modinfos[index].ModID);
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
