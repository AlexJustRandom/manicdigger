
public class ModDescriptionWidget : AbstractMenuWidget
{
    ImageWidget modImage;

    TextWidget modName;
    TextWidget modDescription;

    TextWidget modAuthor;
    TextWidget modAuthorContact;

    TextWidget modCategory;
    TextWidget modModID;
    TextWidget modVersion;


    public ModDescriptionWidget()
    {
        modImage = new ImageWidget();

        modName = new TextWidget();
        modDescription = new TextWidget();

        modAuthor = new TextWidget();
        modAuthorContact = new TextWidget();

        modCategory = new TextWidget();
        modModID = new TextWidget();
        modVersion = new TextWidget();
        paddingX = 0;
        paddingY = 0;
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

    public void SetPaddingY(float newY) { paddingY = newY; }
    public float GetPaddingY() { return paddingY; }
    //todo optymalize and do this as setX override
    public override void Draw(float dt, UiRenderer renderer)
    {
        //X
        modImage.SetX(x + paddingX);

        modName.SetX(x + paddingX);
        modDescription.SetX(x + paddingX);

        modAuthor.SetX(x + paddingX);
        modAuthorContact.SetX(x + paddingX);

        modCategory.SetX(x + paddingX);
        modModID.SetX(x + paddingX);

        modVersion.SetX(x + paddingX);
        //Y

        float oneLineHeight = GetSizeY() / 10;

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

        //Width and Height
        modImage.SetSizeX(GetSizeX() - paddingX * 2);

        modName.SetSizeX(GetSizeX() - paddingX * 2);
        modDescription.SetSizeX(GetSizeX() - paddingX * 2);

        modAuthor.SetSizeX(GetSizeX() - paddingX * 2);
        modAuthorContact.SetSizeX(GetSizeX() - paddingX * 2);

        modCategory.SetSizeX(GetSizeX() - paddingX * 2);
        modModID.SetSizeX(GetSizeX() - paddingX * 2);

        modVersion.SetSizeX(GetSizeX() - paddingX * 2);

        //Heihgt
        modImage.SetSizeY(oneLineHeight);

        modName.SetSizeY(oneLineHeight);
        modDescription.SetSizeY(oneLineHeight);

        modAuthor.SetSizeY(oneLineHeight);
        modAuthorContact.SetSizeY(oneLineHeight);

        modCategory.SetSizeY(oneLineHeight);
        modModID.SetSizeY(oneLineHeight);

        modVersion.SetSizeY(oneLineHeight);

        //render

        modImage.Draw(dt, renderer);

        modName.Draw(dt, renderer);
        modDescription.Draw(dt, renderer);

        modAuthor.Draw(dt, renderer);
        modAuthorContact.Draw(dt, renderer);

        modCategory.Draw(dt, renderer);
        modModID.Draw(dt, renderer);

        modVersion.Draw(dt, renderer);

        renderer.Draw2dTexture(renderer.GetTexture("serverlist_entry_background.png"), x, y, sizex, sizey, null, 0, color);
     }
}