public class ModFrost : ClientMod
{
    int FrostwalkpowerX = 2;
    int FrostwalkpowerY = 2;

 

     public override void OnNewFrameFixed(Game game, NewFrameEventArgs args)
	{
		if (game.FollowId() == null)
		{
            if (game.d_Data.GetBlockType(game.BlockUnderPlayer()).Name == "Water") { 
                    game.AddSpeculative()
            }
            int x = game.platform.FloatToInt(game.player.position.x);
            int y = game.platform.FloatToInt(game.player.position.z);
            int z = game.platform.FloatToInt(game.player.position.y);
         
            for (int Xindex = x - FrostwalkpowerX; Xindex < x + FrostwalkpowerX; Xindex++)
                for (int Yindex = y - FrostwalkpowerY; Yindex < y + FrostwalkpowerY; Yindex++)
                {
                    if (game.d_Data.GetBlockType(game.BlockUnderPlayer()).Name == "Water")
                    {
                        game.AddSpeculative(game.blockd_Data.)

                        m.SetBlock(Xindex, Yindex, z - 1, idIce);
                    }
                }

        }
	}
 
}
