public class ModManagerScreen : MainMenuScreen
{
    public ModManagerScreen()
    {
        // initialize widgets

       
        wbtn_back = new ButtonWidget();
        AddWidget(wbtn_back);

        wbtn_modmanageroptions = new ButtonWidget();
        AddWidget(wbtn_modmanageroptions);

        wtxt_title = new TextWidget();
        wtxt_title.SetFont(fontTitle);
        AddWidget(wtxt_title);

        wmm_ModManager = new ModManagerWidget();
        AddWidget(wmm_ModManager);
    }


    ButtonWidget wbtn_modmanageroptions;

    ButtonWidget wbtn_back;

    ModManagerWidget wmm_ModManager;

    TextWidget wtxt_title;
     

    public override void LoadTranslations()
    {
        wtxt_title.SetText("Modmanager");
        wbtn_back.SetText(menu.lang.Get("MainMenu_ButtonBack"));
        wbtn_modmanageroptions.SetText(menu.lang.Get("Modloader_ModManagerOptions"));  
        wmm_ModManager.LoadTranslationsandMods(gamePlatform,menu);
      }

    public override void Render(float dt)
    {

        float scale = menu.uiRenderer.GetScale();



        float buttonheight = 64 * scale;
        float buttonwidth = 256 * scale;
        float spacebetween = 5 * scale;
        float offsetfromborder = 50 * scale;



        wmm_ModManager.x = offsetfromborder;
        wmm_ModManager.y = 0;
        wmm_ModManager.sizex = gamePlatform.GetCanvasWidth()- offsetfromborder*2;
        wmm_ModManager.sizey = gamePlatform.GetCanvasHeight()- offsetfromborder;



        wtxt_title.x = gamePlatform.GetCanvasWidth() / 2;
        wtxt_title.y = 10;
        wtxt_title.SetAlignment(TextAlign.Center);

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
