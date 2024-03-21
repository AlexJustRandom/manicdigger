public class ModManagerWidget : AbstractMenuWidget
{

    ListWidget wlst_modList;

    ModDescriptionWidget wt_ModDesc;
    ButtonWidget wbtn_switchactive;
    ButtonWidget wbtn_configmod;

    TextWidget wtxt_title;

    TextBoxWidget wtb_newModpackName;

    ButtonWidget wbtn_accept;
    ButtonWidget wbtn_cancel;

    ButtonWidget wbtn_deleteModpack;
    ButtonWidget wbtn_notDeleteModpack;

    SelectWidget wts_modpack;

    ButtonWidget wbtn_saveChanges;
    ButtonWidget wbtn_reversChanges;
 



    ModInformation[] modinfos;
    bool[] modState;
    IntRef modinfosLenght;

    bool loaded;
    bool newModpackActive;
    bool deleteModpackActive;
    bool activeModsChanged;

    int lastSelected;
    int selected;


    public ModManagerWidget()
    {
 
        clickable = true;
        focusable = true;
        activeModsChanged = false;
        wlst_modList = new ListWidget();
 
        wt_ModDesc = new ModDescriptionWidget();
 
        wbtn_switchactive = new ButtonWidget();
 
        wbtn_configmod = new ButtonWidget();
 
        wts_modpack = new SelectWidget();
 
        wtb_newModpackName = new TextBoxWidget();
 
        wbtn_accept = new ButtonWidget();
        wbtn_cancel = new ButtonWidget();

        wbtn_deleteModpack = new ButtonWidget();
        wbtn_notDeleteModpack = new ButtonWidget();


        wbtn_saveChanges = new ButtonWidget();
        wbtn_reversChanges = new ButtonWidget();


        lastSelected=0;
        selected=0;

    }
    MainMenu menu;
    public void LoadTranslationsandMods(GamePlatform gamePlatform,MainMenu _menu) {
        menu = _menu;
        wbtn_switchactive.SetText(menu.lang.Get("Modloder_ModTurnOff"));  
        wbtn_configmod.SetText(menu.lang.Get("Modloder_Configure")); 

        wbtn_accept.SetText(menu.lang.Get("Modloder_Accept"));  
        wbtn_cancel.SetText(menu.lang.Get("Modloder_Cancel")); 

        wbtn_deleteModpack.SetText(menu.lang.Get("Modloder_DeleteModpack"));  
        wbtn_notDeleteModpack.SetText(menu.lang.Get("Modloder_Cancel"));  

        wbtn_saveChanges.SetText(menu.lang.Get("Modloder_SaveChanges"));  
        wbtn_reversChanges.SetText(menu.lang.Get("Modloder_ReverseChanges"));  

        string current = gamePlatform.GetCurrentModpack();
        wts_modpack.SetText(current);

        IntRef lenght = new IntRef();
        string[] modpacks = gamePlatform.GetModpacks(lenght);
        wts_modpack.SetOptionSize(lenght.GetValue() + 1);
        for (int i = 0; i < lenght.value; i++)
        {
            if (modpacks[i] == current)
            {
                lastSelected = i;
                selected = i;
            }
            wts_modpack.SetOption(modpacks[i], i);
        }
        wts_modpack.SetOption("New", lenght.GetValue());

        if (!loaded)
        {
            loaded = true;

            modinfosLenght = new IntRef();
            modinfos = gamePlatform.GetModlist(modinfosLenght);
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
    }
    public void OnNewNameCancel(GamePlatform p)
    {
        selected = lastSelected;
        wts_modpack.SetDisplayedValue(lastSelected);

        wtb_newModpackName.SetContent(p, "");
        newModpackActive = false;
    }
    public void OnNewNameAccept(GamePlatform p)
    {
        string content = wtb_newModpackName.GetContent();

        if (content == "") return;

        IntRef lenght = new IntRef();

        string[] modpackList = p.GetModpacks(lenght);
        bool found = false;
        for (int i = 0; i < lenght.GetValue(); i++)
        {
            if (content == modpackList[i])
            {
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

        p.SaveModpack(content, activeMods);
        p.SetCurrentModpack(content);

        wts_modpack.SetOptionSize(lenght.GetValue() + 2);

        for (int i = 0; i < lenght.GetValue(); i++)
        {
            wts_modpack.SetOption(modpackList[i], i);
        }

        wts_modpack.SetOption(content, lenght.GetValue());
        wts_modpack.SetOption("New", lenght.GetValue() + 1);

        wts_modpack.SetDisplayedValue(lenght.GetValue());
        
        newModpackActive = false;
    }

    public override void OnMouseMove(GamePlatform p, MouseEventArgs args)
    {
        wts_modpack.OnMouseMove(p, args);
        wlst_modList.OnMouseMove(p, args);

        wbtn_accept.OnMouseMove(p, args);
        wbtn_switchactive.OnMouseMove(p, args);
        wbtn_configmod.OnMouseMove(p, args);

        wbtn_cancel.OnMouseMove(p, args);
        wtb_newModpackName.OnMouseMove(p, args);

        wbtn_saveChanges.OnMouseMove(p, args);
        wbtn_reversChanges.OnMouseMove(p, args);

        wbtn_deleteModpack.OnMouseMove(p, args);
        wbtn_notDeleteModpack.OnMouseMove(p, args);
    }

    public override void OnMouseUp(GamePlatform p, MouseEventArgs args)
    {
        wts_modpack.OnMouseUp(p, args);
        wlst_modList.OnMouseUp(p, args);


        wbtn_accept.OnMouseUp(p, args);
        wbtn_switchactive.OnMouseUp(p, args);
        wbtn_configmod.OnMouseUp(p, args);

        wbtn_cancel.OnMouseUp(p, args);
        wtb_newModpackName.OnMouseUp(p, args);

        wbtn_saveChanges.OnMouseUp(p, args);
        wbtn_reversChanges.OnMouseUp(p, args);

        wbtn_deleteModpack.OnMouseUp(p, args);
        wbtn_notDeleteModpack.OnMouseUp(p, args);
    }

    public override void OnMouseDown(GamePlatform p, MouseEventArgs args)
    {
        int modListIndex = wlst_modList.GetIndexSelected();

        wts_modpack.OnMouseDown(p, args);
        wlst_modList.OnMouseDown(p, args);

        wbtn_accept.OnMouseDown(p, args);
        wbtn_switchactive.OnMouseDown(p, args);
        wbtn_configmod.OnMouseDown(p, args);

        wbtn_cancel.OnMouseDown(p, args);
        wtb_newModpackName.OnMouseDown(p, args);

        wbtn_saveChanges.OnMouseDown(p, args);
        wbtn_reversChanges.OnMouseDown(p, args);

        wbtn_deleteModpack.OnMouseDown(p, args);
        wbtn_notDeleteModpack.OnMouseDown(p, args);
 
        if (wbtn_deleteModpack.HasBeenClicked(args)) 
        {
            if (!deleteModpackActive)
            {
                deleteModpackActive = true;
                wbtn_deleteModpack.SetText(menu.lang.Get("Modloder_ModpackDeleteValidation"));  
                 return;
            }

            deleteModpackActive = false;
            wbtn_deleteModpack.SetText(menu.lang.Get("Modloder_ModpackDelete")); 

        }
 

        if (wbtn_notDeleteModpack.HasBeenClicked(args)|| wbtn_deleteModpack.HasBeenClicked(args) && deleteModpackActive) 
        {
            deleteModpackActive = false;
            wbtn_deleteModpack.SetText(menu.lang.Get("Modloder_ModpackDelete"));

        }

        if (wbtn_accept.HasBeenClicked(args))
        {
            if (newModpackActive) {
                OnNewNameAccept(p);
                return;
            }
          
            //DeleteModpack
        }
       

        if  (wbtn_cancel.HasBeenClicked(args))
        {

            if (newModpackActive)
            {
                OnNewNameCancel(p);
                return;
            }
 
        }
        if (wbtn_saveChanges.HasBeenClicked(args))
        {
            IntRef dummy=new IntRef();
            activeModsChanged = false;
            p.SaveModpack( wts_modpack.GetDisplayedValue(),GetActiveModsID(dummy));
        }
        if (wbtn_reversChanges.HasBeenClicked(args))
        {
            activeModsChanged = false;
            ReloadMods(p, wts_modpack.GetDisplayedValue());
        }



        if (wts_modpack.HasBeenClicked(args))
        {
            lastSelected = selected;
            selected = wts_modpack.GetSelected();
            p.ConsoleWriteLine(p.StringFormat("ID SELECTED: {0}", p.IntToString(selected)));
            if (selected != -1)
            {
                wts_modpack.SetDisplayedValue(selected);
                if (selected == wts_modpack.GetOptionSize() - 1)
                {
                    newModpackActive = true;
                    wtb_newModpackName.SetFocused(true);
                    wbtn_accept.SetText(menu.lang.Get("Modloder_Accept"));
                    wbtn_cancel.SetText(menu.lang.Get("Modloder_Cancel"));
                }
                else
                    ReloadMods(p,wts_modpack.GetSelectedValue());
            }
        }
        if (wlst_modList.HasBeenClicked(args))
        {
            if (modListIndex != -1)
                wt_ModDesc.SetModinfo(modinfos[modListIndex]);
        }
        if (wbtn_switchactive.HasBeenClicked(args))
        {
 
            if (modListIndex > -1 && modListIndex < modinfosLenght.value)
            {
                modState[modListIndex] = !modState[modListIndex];

                if (modState[modListIndex])
                    wlst_modList.GetElement(modListIndex).textTopRight = "&2Active";//TODO lang
                else
                    wlst_modList.GetElement(modListIndex).textTopRight = "&4Inactive";//TODO lang
                activeModsChanged=true;
               
            }

        }

    }
  

    public override void OnKeyPress(GamePlatform p, KeyPressEventArgs args)
    {
        wts_modpack.OnKeyPress(p, args);
        wlst_modList.OnKeyPress(p, args);

        wbtn_accept.OnKeyPress(p, args);
        wbtn_switchactive.OnKeyPress(p, args);
        wbtn_configmod.OnKeyPress(p, args);

        wbtn_cancel.OnKeyPress(p, args);
        wtb_newModpackName.OnKeyPress(p, args);

        wbtn_saveChanges.OnKeyPress(p, args);
        wbtn_reversChanges.OnKeyPress(p, args);

        wbtn_deleteModpack.OnKeyPress(p, args);
        wbtn_notDeleteModpack.OnKeyPress(p, args);
    }

    public override void OnKeyDown(GamePlatform p, KeyEventArgs args)
    {
        wts_modpack.OnKeyDown(p, args);
        wlst_modList.OnKeyDown(p, args);

        wtb_newModpackName.OnKeyDown(p, args);
        wbtn_accept.OnKeyDown(p, args);
        wbtn_switchactive.OnKeyDown(p, args);
        wbtn_configmod.OnKeyDown(p, args);

        wbtn_cancel.OnKeyDown(p, args);
        wtb_newModpackName.OnKeyDown(p, args);

        wbtn_saveChanges.OnKeyDown(p, args);
        wbtn_reversChanges.OnKeyDown(p, args);

        wbtn_deleteModpack.OnKeyDown(p, args);
        wbtn_notDeleteModpack.OnKeyDown(p, args);

        if (wtb_newModpackName.hasKeyboardFocus)  
        {
            if ( args.GetKeyCode() == GlKeys.Enter)
            {
                OnNewNameAccept(p);
            }
            if (args.GetKeyCode() == GlKeys.Escape)
            {
                OnNewNameCancel(p);
            }
        }
     
    }

    public override void Draw(float dt, UiRenderer renderer)
    {
        if (!visible) return;

        float scale = renderer.GetScale();
 
        float buttonheight = 64 * scale;
        float buttonwidth = 256 * scale;
        float spacebetween = 5 * scale;
  

        //Modlist / options
        wlst_modList.x = x;
        wlst_modList.y = y + buttonheight+spacebetween;
        wlst_modList.sizex = sizex * 3 / 5 - spacebetween ;
        wlst_modList.sizey = sizey - spacebetween * 4;
        wlst_modList.Draw(dt, renderer);


        wt_ModDesc.x = wlst_modList.x + wlst_modList.sizex;
        wt_ModDesc.y = y+buttonheight + spacebetween;
        wt_ModDesc.sizex = sizex * 2 / 5;
        wt_ModDesc.sizey = sizey *2/3;
        wt_ModDesc.SetPaddingX(25 * scale);
        wt_ModDesc.SetPaddingY(25 * scale);
        wt_ModDesc.Draw(dt, renderer);


        int ModDescButtonCount = 2;


        wbtn_switchactive.x = wt_ModDesc.x + spacebetween;
        wbtn_switchactive.y = wt_ModDesc.y + wt_ModDesc.sizey + spacebetween;
        wbtn_switchactive.sizex = wt_ModDesc.sizex / ModDescButtonCount - spacebetween * ModDescButtonCount;
        wbtn_switchactive.sizey = 64 * scale;
        wbtn_switchactive.Draw(dt, renderer);


        wbtn_configmod.x = wbtn_switchactive.x + wbtn_switchactive.sizex + spacebetween;
        wbtn_configmod.y = wbtn_switchactive.y;
        wbtn_configmod.sizex = wbtn_switchactive.sizex;
        wbtn_configmod.sizey = wbtn_switchactive.sizey;
        wbtn_configmod.Draw(dt, renderer);


        //Modpack widget
        wbtn_accept.SetVisible(!deleteModpackActive);
        wts_modpack.SetVisible(!newModpackActive);
        wtb_newModpackName.SetVisible(newModpackActive);
        wbtn_cancel.SetVisible(newModpackActive && !deleteModpackActive);
 
        wts_modpack.x = x;
        wts_modpack.y = y + spacebetween;
        wts_modpack.sizex = buttonwidth;
        wts_modpack.sizey = buttonheight;

        wts_modpack.Draw(dt, renderer);

        wtb_newModpackName.x = wts_modpack.x;
        wtb_newModpackName.y = wts_modpack.y;
        wtb_newModpackName.sizex = wts_modpack.sizex;
        wtb_newModpackName.sizey = wts_modpack.sizey;
        wtb_newModpackName.Draw(dt, renderer);

        wbtn_accept.x = wtb_newModpackName.x + spacebetween + wtb_newModpackName.sizex;
        wbtn_accept.y = wts_modpack.y;
        wbtn_accept.sizex = buttonwidth / 3;
        wbtn_accept.sizey = buttonheight;
        wbtn_accept.Draw(dt, renderer);


        wbtn_cancel.x = wbtn_accept.x + spacebetween + wbtn_accept.sizex;
        wbtn_cancel.y = wts_modpack.y;
        wbtn_cancel.sizex = buttonwidth / 3;
        wbtn_cancel.sizey = buttonheight;
        wbtn_cancel.Draw(dt, renderer);



        float x1 = wtb_newModpackName.x + spacebetween + wtb_newModpackName.sizex;
        float x2 = wbtn_accept.x + spacebetween + wbtn_accept.sizex;

        if(deleteModpackActive)
        {
            float tmp=x1;
            x1 = x2;
            x2 = tmp;
        }

        wbtn_notDeleteModpack.SetVisible(deleteModpackActive && !newModpackActive);
        wbtn_deleteModpack.SetVisible(!newModpackActive);

        wbtn_deleteModpack.x = x1;
        wbtn_deleteModpack.y = wts_modpack.y;
        wbtn_deleteModpack.sizex = buttonwidth / 3;
        wbtn_deleteModpack.sizey = buttonheight;
        wbtn_deleteModpack.Draw(dt, renderer);
        

        wbtn_notDeleteModpack.x = x2;
        wbtn_notDeleteModpack.y = wts_modpack.y;
        wbtn_notDeleteModpack.sizex = buttonwidth / 3;
        wbtn_notDeleteModpack.sizey = buttonheight;
        wbtn_notDeleteModpack.Draw(dt, renderer);

        if (activeModsChanged && !newModpackActive && wts_modpack.GetDisplayedValueIndex()!=0)//GetDisplayedValueIndex==0 Is Default you cant save/change default
        {
            wbtn_reversChanges.x = sizex - buttonwidth / 2;
            wbtn_reversChanges.y = y;
            wbtn_reversChanges.sizex = buttonwidth / 2;
            wbtn_reversChanges.sizey = buttonheight;
            wbtn_reversChanges.Draw(dt, renderer);

            wbtn_saveChanges.x = wbtn_reversChanges.x - buttonwidth / 2 -spacebetween;
            wbtn_saveChanges.y = wts_modpack.y;
            wbtn_saveChanges.sizex = buttonwidth / 2;
            wbtn_saveChanges.sizey = buttonheight;
            wbtn_saveChanges.Draw(dt, renderer);
        }
    }

    public ModInformation[] GetActiveMods(IntRef lenght) {
        int activeModCount = 0;
        for (int m = 0; m < modinfosLenght.value; m++)
        {
            if (modState[m] != true) continue;
            activeModCount++;
        }
        ModInformation[] activeMods = new ModInformation[activeModCount];
        int activemodIndex = 0;
        for (int m = 0; m < modinfosLenght.value; m++)
        {
            if (modState[m] != true) continue;
            activeMods[activemodIndex] = modinfos[m];
            activemodIndex++;
        }
        lenght.SetValue(activeModCount);
        return activeMods;
 
    }

    public string[] GetActiveModsID(IntRef lenght)
    {
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
        lenght.SetValue(activeModCount);
        return activeMods;

    }
    public void ReloadMods(GamePlatform  p,string name)
    {
        IntRef lenght = new IntRef();
        string[] mods = p.GetMods(name, lenght);
        modinfosLenght = new IntRef();
        modinfos = p.GetModlist(modinfosLenght);
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
                if (modinfos[m].ModID == mods[i])
                {
                    found = true;
                    break;
                }
            }
            modState[m] = found;
            if (found)
            {
                entry.textTopRight = "&2Active";
            }
            else
            {
                entry.textTopRight = "&4Inactive";
            }

            if (modinfos[m].Description != null)

                entry.textBottomLeft = modinfos[m].Description;
            if (modinfos[m].Category != null)
                entry.textBottomRight = modinfos[m].Category;

            wlst_modList.AddElement(entry);

        }


    }


}
