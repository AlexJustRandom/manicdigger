
public class ModDescriptionWidget : AbstractMenuWidget
{
    ImageWidget modImage;

    LabeledTextWidget modName;
    LabeledTextWidget modDescription;

    LabeledTextWidget modAuthor;
    LabeledTextWidget modAuthorContact;

    LabeledTextWidget modCategory;
    LabeledTextWidget modModID;
    LabeledTextWidget modVersion;


    public ModDescriptionWidget()
    {
        modImage = new ImageWidget();

        modName = new LabeledTextWidget();
        modName.SetLabel("Name:");

        modDescription = new LabeledTextWidget();
        modDescription.SetLabel("Description:");

        modAuthor = new LabeledTextWidget();
        modAuthor.SetLabel("Author:");

        modAuthorContact = new LabeledTextWidget();
        modAuthorContact.SetLabel("Author Contact:");

        modCategory = new LabeledTextWidget();
        modCategory.SetLabel("Category:");

        modModID = new LabeledTextWidget();
        modModID.SetLabel("Mod ID:");

        modVersion = new LabeledTextWidget();
        modVersion.SetLabel("Version:");
         
        paddingX = 0;
        paddingY = 0;
        SetModinfo(new ModInformation());
    }

    public void SetFont(FontCi font)
    {

        modName.SetFont(font);
        modDescription.SetFont(font);

        modAuthor.SetFont(font);
        modAuthorContact.SetFont(font);

        modCategory.SetFont(font);
        modModID.SetFont(font);
        modVersion.SetFont(font);


    }
    public void SetModinfo(ModInformation modInfo)
    {

        modImage.SetTextureName(modInfo.Image);

        modName.SetText(modInfo.Name);
        modDescription.SetText(modInfo.Description);

        modAuthor.SetText(modInfo.Author);
        modAuthorContact.SetText(modInfo.AuthorContact);

        modCategory.SetText(modInfo.Category);
        modModID.SetText(modInfo.ModID);

        modVersion.SetText(modInfo.Version);

    }

    float paddingX;
    float paddingY;

    public void SetPaddingX(float newX) { paddingX = newX; }
    public float GetPaddingX() { return paddingX; }

    public void SetPaddingY(float newY) { paddingY = newY;   }
    public float GetPaddingY() { return paddingY; }


    public void UpdateX() {
        modImage.SetX(x + paddingX);

        modName.SetX(x + paddingX);
        modDescription.SetX(x + paddingX);

        modAuthor.SetX(x + paddingX);
        modAuthorContact.SetX(x + paddingX);

        modCategory.SetX(x + paddingX);
        modModID.SetX(x + paddingX);

        modVersion.SetX(x + paddingX);
    }

    public void UpdateY()
    {
        float oneLineHeight = GetSizeY() / 10 - GetSizeY() / 100;

        modImage.SetY(x + paddingY);

        int i = 3;
        modName.SetY(y + paddingY + oneLineHeight * i); i++;
        modAuthor.SetY(y + paddingY + oneLineHeight * i); i++;
        modDescription.SetY(y + paddingY + oneLineHeight * i); i++;

        modAuthor.SetY(y + paddingY + oneLineHeight * i); i++;
        modAuthorContact.SetY(y + paddingY + oneLineHeight * i); i++;

        modCategory.SetY(y + paddingY + oneLineHeight * i); i++;
        modModID.SetY(y + paddingY + oneLineHeight * i); i++;

        modVersion.SetY(y + paddingY + oneLineHeight * i); i++;
    }
    public void UpdateSizeX() 
    {
        modImage.SetSizeX(GetSizeX() - paddingX * 2);

        modName.SetSizeX(GetSizeX() - paddingX * 2);
        modDescription.SetSizeX(GetSizeX() - paddingX * 2);

        modAuthor.SetSizeX(GetSizeX() - paddingX * 2);
        modAuthorContact.SetSizeX(GetSizeX() - paddingX * 2);

        modCategory.SetSizeX(GetSizeX() - paddingX * 2);
        modModID.SetSizeX(GetSizeX() - paddingX * 2);

        modVersion.SetSizeX(GetSizeX() - paddingX * 2);
    }
    public void UpdateSizeY() {
        float oneLineHeight = GetSizeY() / 10 - GetSizeY() / 100;


        modImage.SetSizeY(oneLineHeight);

        modName.SetSizeY(oneLineHeight);
        modDescription.SetSizeY(oneLineHeight);

        modAuthor.SetSizeY(oneLineHeight);
        modAuthorContact.SetSizeY(oneLineHeight);

        modCategory.SetSizeY(oneLineHeight);
        modModID.SetSizeY(oneLineHeight);

        modVersion.SetSizeY(oneLineHeight);
    }

    public void SetX(float newX) { x = newX; }
 
    public void SetY(float newY) { y = newY; }
 
    public void SetSizeX(float newSizeX) { sizex = newSizeX; }
 
    public void SetSizeY(float newSizeY) { sizey = newSizeY;   }


    //todo optymalize and do this as setX override
    public override void Draw(float dt, UiRenderer renderer)
    {
        if (!visible) { return; }

        UpdateX();
        UpdateY();
        UpdateSizeX();
        UpdateSizeY();

        renderer.Draw2dTexture(renderer.GetTexture("serverlist_entry_background.png"), x, y, sizex, sizey, null, 0, color);
 
        modImage.Draw(dt, renderer);

        modName.Draw(dt, renderer);
        modDescription.Draw(dt, renderer);

        modAuthor.Draw(dt, renderer);
        modAuthorContact.Draw(dt, renderer);

        modCategory.Draw(dt, renderer);
        modModID.Draw(dt, renderer);

        modVersion.Draw(dt, renderer);

     }
}