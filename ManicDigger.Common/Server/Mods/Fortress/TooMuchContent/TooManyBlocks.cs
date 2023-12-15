namespace ManicDigger.Mods
{
    /// <summary>
    /// This class contains all block definitions
    /// </summary>
    /// 


    public class ToManyBlocks : IMod
    {
        ModManager m;
        SoundSet solidSounds;
        SoundSet snowSounds;
        SoundSet noSound;

        int Axe;
        int Shovel;
        // int Hoe = m.GetToolType("Hoe");
        int Sword;
        int Pickaxe;
        int Shears;

        public void PreStart(ModManager m)
        {

        }

         

        public void Start(ModManager manager)
        {
            m = manager;


            Axe = m.GetToolType("Axe");
            Shovel = m.GetToolType("Shovel");
            // int Hoe = m.GetToolType("Hoe");
            Sword = m.GetToolType("Sword");
            Pickaxe = m.GetToolType("Pickaxe");
            Shears = m.GetToolType("Shears");

       


            //Initialize sounds
            noSound = new SoundSet();
            solidSounds = new SoundSet()
            {
                Walk = new string[] { "walk1", "walk2", "walk3", "walk4" },
                Break = new string[] { "destruct" },
                Build = new string[] { "build" },
                Clone = new string[] { "clone" },
            };
            snowSounds = new SoundSet()
            {
                Walk = new string[] { "walksnow1", "walksnow2", "walksnow3", "walksnow4" },
                Break = new string[] { "destruct" },
                Build = new string[] { "build" },
                Clone = new string[] { "clone" },
            };


            m.SetBlockType( "Basalt", new BlockType()
            {
                TextureIdTop = "basalt_top",
                SideTextures = "basalt_side",
                TextureIdForInventory = "basalt_top",
                TextureIdBottom = "basalt_top",
                DrawType = DrawType.Solid,
                WalkableType = WalkableType.Solid,
                Sounds = solidSounds,
                 ToolSpeedBonusMask = Pickaxe,
                HarvestabilityMask = Pickaxe,
        });



            #region Start inventory
            m.AddToCreativeInventory("Basalt");

            m.AddToStartInventory("Basalt", 10);
            #endregion
        }

       
    }
}
