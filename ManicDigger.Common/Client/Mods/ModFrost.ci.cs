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
        game.platform.ConsoleWriteLine(game.platform.StringFormat("WATER ID  MEOW {0}", game.platform.IntToString(IceId)));

        if (game.guistate == GuiState.Normal)
        {

            int x = game.platform.FloatToInt(game.player.position.x);
            int y = game.platform.FloatToInt(game.player.position.z);
            int z = game.platform.FloatToInt(game.player.position.y);
         
            for (int Xindex = x - FrostwalkpowerX; Xindex < x + FrostwalkpowerX; Xindex++)
                for (int Yindex = y - FrostwalkpowerY; Yindex < y + FrostwalkpowerY; Yindex++)
                {
                    game.platform.ConsoleWriteLine(game.platform.StringFormat3(" Yindex {0}  Xindex {1}  z {2}", 
 game.platform.IntToString(Yindex),
 game.platform.IntToString(Xindex)
 , game.platform.IntToString(z)
                         ));

                    game.platform.ConsoleWriteLine(game.platform.StringFormat3("POSITION X {0}  y {1}  z {2}",
game.platform.IntToString(x),
game.platform.IntToString(y)
, game.platform.IntToString(z)
          ));

                    game.platform.ConsoleWriteLine(game.platform.StringFormat("BLOCK ID  MEOW {0}", game.platform.IntToString(game.GetBlockSafe(FrostwalkpowerX, FrostwalkpowerY, z - 1))));

                    if (game.GetBlockSafe(FrostwalkpowerX, FrostwalkpowerY,z-1)==0)
                    {
                        game.platform.ConsoleWriteLine("SOPECULATIVE MEOW");
                        game.SendSetBlockAndUpdateSpeculative(IceId, FrostwalkpowerX, FrostwalkpowerY, z - 1, Packet_BlockSetModeEnum.Create);

                     }
                }

        }
    }
 
}
