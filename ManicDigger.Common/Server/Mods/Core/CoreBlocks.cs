namespace ManicDigger.Mods
{
    /// <summary>
    /// This class contains all block definitions
    /// </summary>
    /// 


    public class CoreBlocks : IMod
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
			m.RequireMod("Core");

        }

        public void TemplateAddStone(string name,BlockType blocktype) {
            blocktype.AllTextures =name;
            blocktype.DrawType = DrawType.Solid;
            blocktype.WalkableType = WalkableType.Solid;
            blocktype.Sounds = solidSounds;
            blocktype.ToolSpeedBonusMask = Pickaxe;
            blocktype.HarvestabilityMask = Pickaxe;

            if (blocktype.Hardness == 1)
                blocktype.Hardness = 1.5f;
            m.SetBlockType(name, blocktype);
            m.AddToCreativeInventory(name);
        }


        public void Start(ModManager manager)
		{
            m = manager;

            #region Start inventory
            m.AddToStartInventory("Torch", 6);
			m.AddToStartInventory("Crops1", 1);
			m.AddToStartInventory("CraftingTable", 6);
			m.AddToStartInventory("GoldCoin", 2);
			m.AddToStartInventory("GoldBar", 5);
			m.AddToStartInventory("SilverCoin", 1);
			m.AddToStartInventory("Compass", 1);
			#endregion
		}

		int lastseason;
		void UpdateSeasons()
		{
			int currentSeason = m.GetSeason();
			if (currentSeason != lastseason)
			{
				// spring
				if (currentSeason == 0)
				{
					m.SetBlockType(2, "Grass", new BlockType()
					{
						TextureIdTop = "SpringGrass",
						TextureIdBack = "SpringGrassSide",
						TextureIdFront = "SpringGrassSide",
						TextureIdLeft = "SpringGrassSide",
						TextureIdRight = "SpringGrassSide",
						TextureIdForInventory = "SpringGrassSide",
						TextureIdBottom = "Dirt",
						DrawType = DrawType.Solid,
						WalkableType = WalkableType.Solid,
						Sounds = snowSounds,
						WhenPlayerPlacesGetsConvertedTo = 3,
					});
					m.SetBlockType(18, "OakLeaves", new BlockType()
					{
						AllTextures = "OakLeaves",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
					});
					m.SetBlockType(106, "Apples", new BlockType()
					{
						AllTextures = "Apples",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
						IsUsable = true,
					});
					m.SetBlockType(8, "Water", new BlockType()
					{
						AllTextures = "Water",
						DrawType = DrawType.Fluid,
						WalkableType = WalkableType.Fluid,
						Sounds = noSound,
					});
				}
				// summer
				if (currentSeason == 1)
				{
					m.SetBlockType(2, "Grass", new BlockType()
					{
						TextureIdTop = "Grass",
						TextureIdBack = "GrassSide",
						TextureIdFront = "GrassSide",
						TextureIdLeft = "GrassSide",
						TextureIdRight = "GrassSide",
						TextureIdForInventory = "GrassSide",
						TextureIdBottom = "Dirt",
						DrawType = DrawType.Solid,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
						WhenPlayerPlacesGetsConvertedTo = 3,
					});
					m.SetBlockType(18, "OakLeaves", new BlockType()
					{
						AllTextures = "OakLeaves",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
					});
					m.SetBlockType(106, "Apples", new BlockType()
					{
						AllTextures = "Apples",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
						IsUsable = true,
					});
				}
				// autumn
				if (currentSeason == 2)
				{
					m.SetBlockType(2, "Grass", new BlockType()
					{
						TextureIdTop = "AutumnGrass",
						TextureIdBack = "AutumnGrassSide",
						TextureIdFront = "AutumnGrassSide",
						TextureIdLeft = "AutumnGrassSide",
						TextureIdRight = "AutumnGrassSide",
						TextureIdForInventory = "AutumnGrassSide",
						TextureIdBottom = "Dirt",
						DrawType = DrawType.Solid,
						WalkableType = WalkableType.Solid,
						Sounds = snowSounds,
						WhenPlayerPlacesGetsConvertedTo = 3,
					});
					m.SetBlockType(18, "OakLeaves", new BlockType()
					{
						AllTextures = "AutumnLeaves",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
					});
					m.SetBlockType(106, "Apples", new BlockType()
					{
						AllTextures = "AutumnApples",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
						IsUsable = true,
					});
				}
				// winter
				if (currentSeason == 3)
				{
					m.SetBlockType(2, "Grass", new BlockType()
					{
						TextureIdTop = "WinterGrass",
						TextureIdBack = "WinterGrassSide",
						TextureIdFront = "WinterGrassSide",
						TextureIdLeft = "WinterGrassSide",
						TextureIdRight = "WinterGrassSide",
						TextureIdForInventory = "WinterGrassSide",
						TextureIdBottom = "Dirt",
						DrawType = DrawType.Solid,
						WalkableType = WalkableType.Solid,
						Sounds = snowSounds,
						WhenPlayerPlacesGetsConvertedTo = 3,
					});
					m.SetBlockType(18, "OakLeaves", new BlockType()
					{
						AllTextures = "WinterLeaves",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
					});
					m.SetBlockType(106, "Apples", new BlockType()
					{
						AllTextures = "WinterApples",
						DrawType = DrawType.Transparent,
						WalkableType = WalkableType.Solid,
						Sounds = solidSounds,
						IsUsable = true,
					});
					m.SetBlockType(8, "Water", new BlockType()
					{
						AllTextures = "Ice",
						DrawType = DrawType.Fluid,
						WalkableType = WalkableType.Solid,
						Sounds = snowSounds,
						IsSlipperyWalk = true,
					});
				}

				//Send updated BlockTypes to players
				m.UpdateBlockTypes();
				lastseason = currentSeason;

				//Readd "lost blocks" to inventory
				m.AddToCreativeInventory("OakLeaves");
				m.AddToCreativeInventory("Apples");
				m.AddToCreativeInventory("Water");
			}
		}
	}
}
