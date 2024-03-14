namespace ManicDigger.Mods
{
    public class CoreTools : IMod
    {
        public void PreStart(ModManager m)
        {
 
        }
        private void AddTool(string name,BlockType type) {

            type.AllTextures =  name.ToLower().Replace(" ", "_");
            type.DrawType = DrawType.Solid;
            type.WalkableType = WalkableType.Solid;
            type.IsBuildable = false;
            m.SetBlockType(name, type);
            m.AddToStartInventory(name, 1);
            m.AddToCreativeInventory(name);
            
        }
        public void Start(ModManager manager)
        {
            m = manager;

            int Axe = m.GetToolType("Axe");
            int Shovel = m.GetToolType("Shovel");
           // int Hoe = m.GetToolType("Hoe");
            int Sword = m.GetToolType("Sword");
            int Pickaxe = m.GetToolType("Pickaxe");
            int Shears = m.GetToolType("Shears");
            
            float baseAxeStrenght=1f;
            float baseShovelStrenght=1f;
            float baseSwordStrenght=1f;
            float basePickaxeStrenght=1f;
 
            float diamondMod= 8f;
            AddTool("Diamond Axe", new BlockType()
            {
                ToolStrenght = baseAxeStrenght * diamondMod,
                ToolTypeMask = Axe
            });
            AddTool("Diamond Shovel", new BlockType()
            {
                ToolStrenght = baseShovelStrenght * diamondMod,
                ToolTypeMask = Shovel
            }); 
            AddTool("Diamond Sword", new BlockType()
            {
                ToolStrenght = baseSwordStrenght * diamondMod,
                ToolTypeMask = Sword
            });
            AddTool("Diamond Pickaxe", new BlockType()
            {
                ToolStrenght = basePickaxeStrenght * diamondMod,
                ToolTypeMask = Pickaxe
            });

            float goldMod=12f;
            AddTool("Gold Axe", new BlockType()
            {
                ToolStrenght = baseAxeStrenght * goldMod,
                ToolTypeMask = Axe
            });
            AddTool("Gold Shovel", new BlockType()
            {
                ToolStrenght = baseShovelStrenght * goldMod,
                ToolTypeMask = Shovel
            }); 
            AddTool("Gold Sword", new BlockType()
            {
                ToolStrenght = baseSwordStrenght * goldMod,
                ToolTypeMask = Sword
            });
            AddTool("Gold Pickaxe", new BlockType()
            {
                ToolStrenght = basePickaxeStrenght * goldMod,
                ToolTypeMask = Pickaxe
            });

            float steelMod=6f;
            AddTool("Steel Axe", new BlockType()
            {
                ToolStrenght = baseAxeStrenght * steelMod,
                ToolTypeMask = Axe
            });
            AddTool("Steel Shovel", new BlockType()
            {
                ToolStrenght = baseShovelStrenght * steelMod,
                ToolTypeMask = Shovel
            }); 
            AddTool("Steel Sword", new BlockType()
            {
                ToolStrenght = baseSwordStrenght * steelMod,
                ToolTypeMask = Sword
            });
            AddTool("Steel Pickaxe", new BlockType()
            {
                ToolStrenght = basePickaxeStrenght * steelMod,
                ToolTypeMask = Pickaxe
            });

            float stoneMod=4f;
            AddTool("Stone Axe", new BlockType()
            {
                ToolStrenght = baseAxeStrenght * stoneMod,
                ToolTypeMask = Axe
            });
            AddTool("Stone Shovel", new BlockType()
            {
                ToolStrenght = baseShovelStrenght * stoneMod,
                ToolTypeMask = Shovel
            }); 
            AddTool("Stone Sword", new BlockType()
            {
                ToolStrenght = baseSwordStrenght * stoneMod,
                ToolTypeMask = Sword
            });
            AddTool("Stone Pickaxe", new BlockType()
            {
                ToolStrenght = basePickaxeStrenght * stoneMod,
                ToolTypeMask = Pickaxe
            });

            float woodMod=2f;
            AddTool("Wood Axe", new BlockType()
            {
                ToolStrenght = baseAxeStrenght * woodMod,
                ToolTypeMask = Axe
            });
            AddTool("Wood Shovel", new BlockType()
            {
                ToolStrenght = baseShovelStrenght * woodMod,
                ToolTypeMask = Shovel
            }); 
            AddTool("Wood Sword", new BlockType()
            {
                ToolStrenght = baseSwordStrenght * woodMod,
                ToolTypeMask = Sword
            });
            AddTool("Wood Pickaxe", new BlockType()
            {
                ToolStrenght = basePickaxeStrenght * woodMod,
                ToolTypeMask = Pickaxe
            });


        }
        ModManager m;
    
    }
}
