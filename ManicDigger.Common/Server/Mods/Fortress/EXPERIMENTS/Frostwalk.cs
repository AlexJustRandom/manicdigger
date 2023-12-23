using System.Collections.Generic;

namespace ManicDigger.Mods
{
	//for debugging
	public class Frostwalk : IMod
	{
		public void PreStart(ModManager m) {
            m.RequireMod("CoreBlocks");
            m.RequireMod("ToManyBlocks");
        }

        public void Start(ModManager manager)
		{
			m = manager;
            enabled = true;
			if (enabled)
			{
				m.RegisterOnPlayerMove(onMove);
                idWater = m.GetBlockId("Water");
                idIce = m.GetBlockId("Ice");
            }

		}
        int idWater;
        int idIce;

        bool enabled;
		ModManager m;
		int ghost;
        int FrostwalkpowerX = 2;
        int FrostwalkpowerY = 2;

        void onMove(int id,int x,int z,int y)
		{
            for(int Xindex=x-FrostwalkpowerX;Xindex<x+FrostwalkpowerX;Xindex++)
                for (int Yindex = y - FrostwalkpowerY; Yindex < y + FrostwalkpowerY; Yindex++) {
                    if (m.GetBlock(Xindex, Yindex, z - 1) == idWater)
                    {
                        m.SetBlock(Xindex, Yindex, z - 1, idIce);
                    }
                }






        
        }
	}
}
