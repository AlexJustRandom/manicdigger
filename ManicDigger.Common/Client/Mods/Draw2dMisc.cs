﻿public class ModDraw2dMisc : ClientMod
{
	public override void OnNewFrameDraw2d(Game game, float deltaTime)
	{
		if (game.guistate == GuiState.Normal)
		{
			DrawAim(game);
		}
		if (game.guistate != GuiState.MapLoading)
		{
			DrawEnemyHealthBlock(game);
 			DrawBlockInfo(game);
		}
		DrawMouseCursor(game);
		DrawDisconnected(game);
	}

	public void DrawBlockInfo(Game game)
	{
		if (!game.drawblockinfo)
		{
			return;
		}
		int x = game.SelectedBlockPosition.x;
		int y = game.SelectedBlockPosition.z;
		int z = game.SelectedBlockPosition.y;
		//string info = "None";
		if (!game.map.IsValidPos(x, y, z))
		{
			return;
		}
		int blocktype = game.map.GetBlock(x, y, z);
		if (!game.IsValid(blocktype))
		{
			return;
		}
		game.currentAttackedBlock = Vector3IntRef.Create(x, y, z);
		DrawEnemyHealthBlock(game);
	}

	internal void DrawMouseCursor(Game game)
	{
		if (!game.GetFreeMouse())
		{
			return;
		}
		if (!game.platform.MouseCursorIsVisible())
		{
			game.Draw2dBitmapFile("mousecursor.png", game.mouseCurrentX, game.mouseCurrentY, 32, 32);
		}
	}

	internal void DrawEnemyHealthBlock(Game game)
	{
		if (game.currentAttackedBlock != null)
		{
			int x = game.currentAttackedBlock.X;
			int y = game.currentAttackedBlock.Y;
			int z = game.currentAttackedBlock.Z;
			int blocktype = game.map.GetBlock(x, y, z);
            float health = 1;// game.GetCurrentBlockHealth(x, y, z);
            int now = game.platform.TimeMillisecondsFromStart();
            int end = game.whenStartedMining + game.platform.FloatToInt((game.GetMiningTime(game.d_Inventory.RightHand[game.ActiveHudIndex].BlockId, game.map.GetBlock(x, y, z)) * 1000));
            float progress = now / end;
			if (game.IsUsableBlock(blocktype))
			{
				DrawEnemyHealthUseInfo(game, game.language.Get(StringTools.StringAppend(game.platform, "Block_", game.blocktypes[blocktype].Name)), progress, true,blocktype);
			}
			DrawEnemyHealthCommon(game, game.language.Get(StringTools.StringAppend(game.platform, "Block_", game.blocktypes[blocktype].Name)), progress,blocktype);
		}
		if (game.currentlyAttackedEntity != -1)
		{
			Entity e = game.entities[game.currentlyAttackedEntity];
			if (e == null)
			{
				return;
			}
			float health;
			if (e.playerStats != null)
			{
				health = game.one * e.playerStats.CurrentHealth / e.playerStats.MaxHealth;
			}
			else
			{
				health = 1;
			}
			string name = "Unknown";
			if (e.drawName != null)
			{
				name = e.drawName.Name;
			}
			if (e.usable)
			{
				DrawEnemyHealthUseInfo(game, game.language.Get(name), health, true,0);
			}
			DrawEnemyHealthCommon(game, game.language.Get(name), health,0);
		}
	}

	internal void DrawEnemyHealthCommon(Game game, string name, float progress,int blocktype)
	{
		DrawEnemyHealthUseInfo(game, name, progress, false,blocktype);
	}

	internal void DrawEnemyHealthUseInfo(Game game, string name, float progress, bool useInfo,int blockid)
	{
		int y = useInfo ? 55 : 35;
		game.rend.Draw2dTexture(game.rend.WhiteTexture(), game.xcenter(300), 40, 300, y, null, 0, ColorCi.FromArgb(255, 0, 0, 0), false);
        game.rend.Draw2dTexture(game.rend.WhiteTexture(), game.xcenter(300), 40, 300 * progress, y, null, 0, ColorCi.FromArgb(255, 255, 0, 0), false);
        FontCi font = new FontCi();
		font.size = 14;
		IntRef w = new IntRef();
		IntRef h = new IntRef();
		game.platform.TextSize(name, font, w, h);
        game.rend.Draw2dText(name, font, game.xcenter(w.value), 40, null, false);
        if(blockid != 0) {
           bool isharvestable = (game.d_Data.IsHarvestableByTool(blockid, game.d_Inventory.RightHand[game.ActiveHudIndex].BlockId));
            int blockspeedmask = game.d_Data.ToolSpeedBonusMask(blockid);
            int toolspeedmask = game.d_Data.HarvestabilityMask(game.d_Inventory.RightHand[game.ActiveHudIndex].BlockId);
            string val = game.platform.StringFormat4("Harvestable ? {0} | gets speedbonus =  {1} | toolStrenght = {2} | mining time : {3}"
               , isharvestable ? "yes " : "no"
                , game.d_Data.GetsSpeedBonus(blockid, game.d_Inventory.RightHand[game.ActiveHudIndex].BlockId) ? "yes " : "no"
                , game.platform.FloatToString(game.d_Data.ToolStrength(game.d_Inventory.RightHand[game.ActiveHudIndex].BlockId))
                , game.platform.FloatToString(game.GetMiningTime(game.d_Inventory.RightHand[game.ActiveHudIndex].BlockId, blockid)*1000));



            // for(int i=0;i< game.d_Data.) i dont have tools mask names on client 
            // todo

            IntRef color = new IntRef();
            if (!isharvestable) //idk bugy with  ? : operator
                color.value = ColorCi.FromArgb(255, 200, 0, 0);
            else
                color.value = ColorCi.FromArgb(255, 0, 200, 0);
            game.rend.Draw2dText(val, font, game.xcenter(w.value), 70, color, false);

        }


        game.rend.Draw2dText(name, font, game.xcenter(w.value), 40, null, false);
        if (useInfo)
		{
			name = game.platform.StringFormat(game.language.PressToUse(), "E");
			FontCi font2 = new FontCi();
			font2.size = 10;
			game.platform.TextSize(name, font2, w, h);
			game.rend.Draw2dText(name, font2, game.xcenter(w.value), 70, null, false);
		}
	}

	internal void DrawAim(Game game)
	{
		if (game.cameratype == CameraType.Overhead)
		{
			return;
		}
		int aimwidth = 32;
		int aimheight = 32;
		game.platform.BindTexture2d(0);
		if (game.CurrentAimRadius() > 1)
		{
			float fov_ = game.currentfov();
			game.Circle3i(game.Width() / 2, game.Height() / 2, 2 * game.fov / fov_);
		}
		game.Draw2dBitmapFile("target.png", game.Width() / 2 - aimwidth / 2, game.Height() / 2 - aimheight / 2, aimwidth, aimheight);
	}

 

	void DrawDisconnected(Game game)
	{
		float one = 1;
		float lagSeconds = one * (game.platform.TimeMillisecondsFromStart() - game.LastReceivedMilliseconds) / 1000;
		if ((lagSeconds >= Game.DISCONNECTED_ICON_AFTER_SECONDS && lagSeconds < 60 * 60 * 24)
			&& game.invalidVersionDrawMessage == null && !(game.issingleplayer && (!game.platform.SinglePlayerServerLoaded())))
		{
			game.Draw2dBitmapFile("disconnected.png", game.Width() - 100, 50, 50, 50);
			FontCi font = new FontCi();
			font.size = 12;
			game.rend.Draw2dText(game.platform.IntToString(game.platform.FloatToInt(lagSeconds)), font, game.Width() - 100, 50 + 50 + 10, null, false);
			game.rend.Draw2dText("Press F6 to reconnect", font, game.Width() / 2 - 200 / 2, 50, null, false);
		}
	}
}