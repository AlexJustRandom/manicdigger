public class ModFrost : ClientMod
{
    int FrostwalkpowerX ;
    int FrostwalkpowerY ;
    public bool enabled ;


     public override void OnNewFrameFixed(Game game, NewFrameEventArgs args)
    {
        enabled = false;
        if (enabled) return;


         FrostwalkpowerX = 3;
         FrostwalkpowerY = 3;
        int waterId = game.d_Data.GetBlockId("Water");

        int IceId = game.d_Data.GetBlockId("Ice");

        if (game.guistate == GuiState.Normal)
        {

            int x = game.platform.FloatToInt(game.player.position.x);
            int y = game.platform.FloatToInt(game.player.position.z);
            int z = game.platform.FloatToInt(game.player.position.y);
         
            for (int Xindex = x - FrostwalkpowerX; Xindex < x + FrostwalkpowerX; Xindex++)
                for (int Yindex = y - FrostwalkpowerY; Yindex < y + FrostwalkpowerY; Yindex++)
                {
 
                    if (game.GetBlockSafe(FrostwalkpowerX, FrostwalkpowerY,z-1)==0)
                    {
                         game.SendSetBlockAndUpdateSpeculative(IceId, FrostwalkpowerX, FrostwalkpowerY, z - 1, Packet_BlockSetModeEnum.Create);

                     }
                }

        }
    }
 
}
