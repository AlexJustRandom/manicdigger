using System;
using System.IO;

namespace ManicDigger.Mods
{
 
    public class Chest : IMod
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

            Console.WriteLine("BASALT  MODDDDDDDDDDDDDDDDDDDDD");

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


            m.SetBlockType( "Chest", new BlockType()
            {
                TextureIdTop = "birch_fence",
                SideTextures = "birch_fence",
                TextureIdForInventory = "birch_fence",
                TextureIdBottom = "birch_fence",
                DrawType = DrawType.Solid,
                WalkableType = WalkableType.Solid,
                Sounds = solidSounds,
                ToolSpeedBonusMask = Axe,
                HarvestabilityMask = Axe,
            });
             m.RegisterCheckOnBlockBuild(CheckBlockBuild);
            m.AddToCreativeInventory("Chest");

            m.AddToStartInventory("Chest", 10);
         }

        public bool CheckBlockBuild(int player, int x, int y, int z) {
                
            return true;
        }

    }
}
