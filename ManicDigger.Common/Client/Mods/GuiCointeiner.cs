public class ModGuiCointainer : ClientMod
{
    public int X;
    public int Y;
    public int Z;
    public Packet_Inventory d_container;
    PacketHandlerContainerInventory handler;
    public ModGuiCointainer()
	{
		//indexed by enum WearPlace
		wearPlaceStart = new PointRef[5];
		{
			//new Point(282,85), //LeftHand,
			wearPlaceStart[0] = PointRef.Create(34, 100); //RightHand,
			wearPlaceStart[1] = PointRef.Create(74, 100); //MainArmor,
			wearPlaceStart[2] = PointRef.Create(194, 100); //Boots,
			wearPlaceStart[3] = PointRef.Create(114, 100); //Helmet,
			wearPlaceStart[4] = PointRef.Create(154, 100); //Gauntlet,
		}
     
		//indexed by enum WearPlace
		wearPlaceCells = new PointRef[5];
		{
			//new Point(2,4), //LeftHand,
			wearPlaceCells[0] = PointRef.Create(1, 1); //RightHand,
			wearPlaceCells[1] = PointRef.Create(1, 1); //MainArmor,
			wearPlaceCells[2] = PointRef.Create(1, 1); //Boots,
			wearPlaceCells[3] = PointRef.Create(1, 1); //Helmet,
			wearPlaceCells[4] = PointRef.Create(1, 1); //Gauntlet,
		}
		CellCountInPageX = 12;
		CellCountInPageY = 7;
		CellCountTotalX = 12;
		CellCountTotalY = 7 * 6;
		CellDrawSize = 40;

        X = 0;
        Y = 0;
        Z = 0;
        d_container = null;
        handler = new PacketHandlerContainerInventory();
        handler.mod = this;
    }

	internal Game game;
	internal GameDataItemsClient dataItems;
	internal InventoryUtilClient inventoryUtil;
	internal IInventoryController controller;

	internal int CellDrawSize;

	public int InventoryStartX() { return game.Width() / 2 - 560 / 2; }
	public int InventoryStartY() { return game.Height() / 2 - 600 / 2; }
    public int CellsStartCointeinerX() { return 35 + InventoryStartX(); }
    public int CellsStartCointeinerY() { return 50 + InventoryStartY(); }
    public int CellsStartInventoryX() { return 35 + InventoryStartX(); }
	public int CellsStartInventoryY() { return 260 + InventoryStartY(); }
	int MaterialSelectorStartX() { return game.platform.FloatToInt(MaterialSelectorBackgroundStartX() + 17 * game.Scale()); }
	int MaterialSelectorStartY() { return game.platform.FloatToInt(MaterialSelectorBackgroundStartY() + 17 * game.Scale()); }
	int MaterialSelectorBackgroundStartX() { return game.platform.FloatToInt(game.Width() / 2 - (512 / 2) * game.Scale()); }
	int MaterialSelectorBackgroundStartY() { return game.platform.FloatToInt(game.Height() - 90 * game.Scale()); }
	int CellCountInPageX;
	int CellCountInPageY;
	int CellCountTotalX;
	int CellCountTotalY;

	public int ActiveMaterialCellSize() { return game.platform.FloatToInt(48 * game.Scale()); }


    public override void OnKeyDown(Game game_, KeyEventArgs args)
    {
        int eKey = args.GetKeyCode();
        if (eKey == (game_.GetKey(GlKeys.E)) && game_.GuiTyping == TypingState.None)
        {
            if (!(game_.SelectedBlockPosition.x == -1 && game_.SelectedBlockPosition.y == -1 && game_.SelectedBlockPosition.z == -1))
            {
                int posx = game_.SelectedBlockPosition.x;
                int posy = game_.SelectedBlockPosition.z;
                int posz = game_.SelectedBlockPosition.y;
                if (game_.map.GetBlock(posx, posy, posz) == game_.d_Data.BlockIdChest())
                {
                    //draw crafting recipes list.
                    IntRef tableCount = new IntRef();
         
                    OpenContainer(game_, posx, posy, posz);
                    args.SetHandled(true);
                }
            }
        }
    }

    public void OpenContainer(Game game_,int posx,int posy,int posz) {
        game_.packetHandlers[Packet_ServerIdEnum.ContainerInventory] = handler;

        game_.guistate = GuiState.Container;
        game_.menustate = new MenuState();
        game_.SetFreeMouse(true);
        game.SendRequestContainer( posx,  posy,  posz);
    }


    int ScrollButtonSize() { return CellDrawSize; }

	int ScrollUpButtonX() { return CellsStartInventoryX() + CellCountInPageX * CellDrawSize; }
	int ScrollUpButtonY() { return CellsStartInventoryY(); }

	int ScrollDownButtonX() { return CellsStartInventoryX() + CellCountInPageX * CellDrawSize; }
	int ScrollDownButtonY() { return CellsStartInventoryY() + (CellCountInPageY - 1) * CellDrawSize; }

	public override void OnMouseDown(Game game_, MouseEventArgs args)
	{
		if (game.guistate != GuiState.Container)
		{
			return;
		}
		PointRef scaledMouse = PointRef.Create(args.GetX(), args.GetY());

		//material selector
		if (SelectedMaterialSelectorSlot(scaledMouse) != null)
		{
			//int oldActiveMaterial = ActiveMaterial.ActiveMaterial;
			game.ActiveHudIndex = SelectedMaterialSelectorSlot(scaledMouse).value;
			//if (oldActiveMaterial == ActiveMaterial.ActiveMaterial)
			{
				Packet_InventoryPosition p = new Packet_InventoryPosition();
				p.Type = Packet_InventoryPositionTypeEnum.MaterialSelector;
				p.MaterialId = game.ActiveHudIndex;
				controller.InventoryClick(p);
			}
			args.SetHandled(true);
			return;
		}

		if (game.guistate != GuiState.Container)
		{
			return;
		}

		//main inventory
		PointRef cellInPage = SelectedCellInventory(scaledMouse);
		//grab from inventory
		if (cellInPage != null)
		{
			if (args.GetButton() == MouseButtonEnum.Left)
			{
				Packet_InventoryPosition p = new Packet_InventoryPosition();
				p.Type = Packet_InventoryPositionTypeEnum.MainArea;
				p.AreaX = cellInPage.X;
				p.AreaY = cellInPage.Y + ScrollLineInventory;
				controller.InventoryClick(p);
				args.SetHandled(true);
				return;
			}
			else
			{
				{
					Packet_InventoryPosition p = new Packet_InventoryPosition();
					p.Type = Packet_InventoryPositionTypeEnum.MainArea;
					p.AreaX = cellInPage.X;
					p.AreaY = cellInPage.Y + ScrollLineInventory;
					controller.InventoryClick(p);
				}
				{
					Packet_InventoryPosition p = new Packet_InventoryPosition();
					p.Type = Packet_InventoryPositionTypeEnum.WearPlace;
					p.WearPlace = WearPlace_.RightHand;
					p.ActiveMaterial = game.ActiveHudIndex;
					controller.InventoryClick(p);
				}
				{
					Packet_InventoryPosition p = new Packet_InventoryPosition();
					p.Type = Packet_InventoryPositionTypeEnum.MainArea;
					p.AreaX = cellInPage.X;
					p.AreaY = cellInPage.Y + ScrollLineInventory;
					controller.InventoryClick(p);
				}
			}
			if (game.guistate == GuiState.Container)
			{
				args.SetHandled(true);
				return;
			}
		}
		// //drop items on ground
		//if (scaledMouse.X < CellsStartX() && scaledMouse.Y < MaterialSelectorStartY())
		//{
		//    int posx = game.SelectedBlockPositionX;
		//    int posy = game.SelectedBlockPositionY;
		//    int posz = game.SelectedBlockPositionZ;
		//    Packet_InventoryPosition p = new Packet_InventoryPosition();
		//    {
		//        p.Type = Packet_InventoryPositionTypeEnum.Ground;
		//        p.GroundPositionX = posx;
		//        p.GroundPositionY = posy;
		//        p.GroundPositionZ = posz;
		//    }
		//    controller.InventoryClick(p);
		//}
		if (SelectedWearPlace(scaledMouse) != null)
		{
			Packet_InventoryPosition p = new Packet_InventoryPosition();
			p.Type = Packet_InventoryPositionTypeEnum.WearPlace;
			p.WearPlace = (SelectedWearPlace(scaledMouse).value);
			p.ActiveMaterial = game.ActiveHudIndex;
			controller.InventoryClick(p);
			args.SetHandled(true);
			return;
		}
		if (scaledMouse.X >= ScrollUpButtonX() && scaledMouse.X < ScrollUpButtonX() + ScrollButtonSize()
			&& scaledMouse.Y >= ScrollUpButtonY() && scaledMouse.Y < ScrollUpButtonY() + ScrollButtonSize())
		{
			ScrollUp();
			ScrollingUpTimeMilliseconds = game.platform.TimeMillisecondsFromStart();
			args.SetHandled(true);
			return;
		}
		if (scaledMouse.X >= ScrollDownButtonX() && scaledMouse.X < ScrollDownButtonX() + ScrollButtonSize()
			&& scaledMouse.Y >= ScrollDownButtonY() && scaledMouse.Y < ScrollDownButtonY() + ScrollButtonSize())
		{
			ScrollDown();
			ScrollingDownTimeMilliseconds = game.platform.TimeMillisecondsFromStart();
			args.SetHandled(true);
			return;
		}
		game.GuiStateBackToGame();
		return;
	}

	public override void OnTouchStart(Game game_, TouchEventArgs e)
	{
		MouseEventArgs args = new MouseEventArgs();
		args.SetX(e.GetX());
		args.SetY(e.GetY());
		OnMouseDown(game_, args);
		e.SetHandled(args.GetHandled());
	}

	public bool IsMouseOverCells()
	{
		return SelectedCellOrScrollbar(game.mouseCurrentX, game.mouseCurrentY);
	}

	public void ScrollUp()
	{
		ScrollLineInventory--;
		if (ScrollLineInventory < 0) { ScrollLineInventory = 0; }
	}

	public void ScrollDown()
	{
		ScrollLineInventory++;
		int max = CellCountTotalY - CellCountInPageY;
		if (ScrollLineInventory >= max) { ScrollLineInventory = max; }
	}

	int ScrollingUpTimeMilliseconds;
	int ScrollingDownTimeMilliseconds;

	public override void OnMouseUp(Game game_, MouseEventArgs args)
	{
		if (game.guistate != GuiState.Container)
		{
			return;
		}
		ScrollingUpTimeMilliseconds = 0;
		ScrollingDownTimeMilliseconds = 0;
	}

	IntRef SelectedMaterialSelectorSlot(PointRef scaledMouse)
	{
		if (scaledMouse.X >= MaterialSelectorStartX() && scaledMouse.Y >= MaterialSelectorStartY()
			&& scaledMouse.X < MaterialSelectorStartX() + 10 * ActiveMaterialCellSize()
			&& scaledMouse.Y < MaterialSelectorStartY() + 10 * ActiveMaterialCellSize())
		{
			return IntRef.Create((scaledMouse.X - MaterialSelectorStartX()) / ActiveMaterialCellSize());
		}
		return null;
	}




	PointRef SelectedCellInventory(PointRef scaledMouse)
	{
		if (scaledMouse.X < CellsStartInventoryX() || scaledMouse.Y < CellsStartInventoryY()
			|| scaledMouse.X > CellsStartInventoryX() + CellCountInPageX * CellDrawSize
			|| scaledMouse.Y > CellsStartInventoryY() + CellCountInPageY * CellDrawSize)
		{
			return null;
		}
		PointRef cell = PointRef.Create((scaledMouse.X - CellsStartInventoryX()) / CellDrawSize,
			(scaledMouse.Y - CellsStartInventoryY()) / CellDrawSize);
		return cell;
	}

	bool SelectedCellOrScrollbar(int scaledMouseX, int scaledMouseY)
	{
		if (scaledMouseX < CellsStartInventoryX() || scaledMouseY < CellsStartInventoryY()
			|| scaledMouseX > CellsStartInventoryX() + (CellCountInPageX + 1) * CellDrawSize
			|| scaledMouseY > CellsStartInventoryY() + CellCountInPageY * CellDrawSize)
		{
			return false;
		}
		return true;
	}

    internal int ScrollLineInventory;
    internal int ScrollLineContainer;

    public override void OnNewFrameDraw2d(Game game_, float deltaTime)
	{
		game = game_;
		if (dataItems == null)
		{
			dataItems = new GameDataItemsClient();
			dataItems.game = game_;
			controller = ClientInventoryController.Create(game_);
			inventoryUtil = game.d_InventoryUtil;
		}
 
		if (game.guistate != GuiState.Container)
		{
			return;
		}
		if (ScrollingUpTimeMilliseconds != 0 && (game.platform.TimeMillisecondsFromStart() - ScrollingUpTimeMilliseconds) > 250)
		{
			ScrollingUpTimeMilliseconds = game.platform.TimeMillisecondsFromStart();
			ScrollUp();
		}
		if (ScrollingDownTimeMilliseconds != 0 && (game.platform.TimeMillisecondsFromStart() - ScrollingDownTimeMilliseconds) > 250)
		{
			ScrollingDownTimeMilliseconds = game.platform.TimeMillisecondsFromStart();
			ScrollDown();
		}

		PointRef scaledMouse = PointRef.Create(game.mouseCurrentX, game.mouseCurrentY);

		game.Draw2dBitmapFile("InventoryContainer.png", InventoryStartX(), InventoryStartY(), 1024, 1024);

        //the3d.Draw2dTexture(terrain, 50, 50, 50, 50, 0);
        //the3d.Draw2dBitmapFile("inventory_weapon_shovel.png", 100, 100, 60 * 2, 60 * 4);
        //the3d.Draw2dBitmapFile("inventory_gauntlet_gloves.png", 200, 200, 60 * 2, 60 * 2);

        //Draw Container inventory
        if(d_container!=null)
        for (int i = 0; i < d_container.ItemsCount; i++)
        {
            Packet_PositionItem k = d_container.Items[i];
            if (k == null)
            {
                continue;
            }
            int screeny = k.Y - ScrollLineInventory;
            if (screeny >= 0 && screeny < CellCountInPageY)
            {
                DrawItem(CellsStartCointeinerX() + k.X * CellDrawSize, CellsStartCointeinerY()+ screeny * CellDrawSize, k.Value_, 0, 0);
            }
        }


        //main inventory

        for (int i = 0; i < game.d_Inventory.ItemsCount; i++)
		{
			Packet_PositionItem k = game.d_Inventory.Items[i];
			if (k == null)
			{
				continue;
			}
			int screeny = k.Y - ScrollLineInventory;
			if (screeny >= 0 && screeny < CellCountInPageY)
			{
				DrawItem(CellsStartInventoryX() + k.X * CellDrawSize, CellsStartInventoryY() + screeny * CellDrawSize, k.Value_, 0, 0);
			}
		}

		//draw area selection
		if (game.d_Inventory.DragDropItem != null)
		{
			PointRef selectedInPage = SelectedCellInventory(scaledMouse);
			if (selectedInPage != null)
			{
				int x = (selectedInPage.X) * CellDrawSize + CellsStartInventoryX();
				int y = (selectedInPage.Y) * CellDrawSize + CellsStartInventoryY();
				int sizex = dataItems.ItemSizeX(game.d_Inventory.DragDropItem);
				int sizey = dataItems.ItemSizeY(game.d_Inventory.DragDropItem);
				if (selectedInPage.X + sizex <= CellCountInPageX
					&& selectedInPage.Y + sizey <= CellCountInPageY)
				{
					int c;
					IntRef itemsAtAreaCount = new IntRef();
					PointRef[] itemsAtArea = inventoryUtil.ItemsAtArea(selectedInPage.X, selectedInPage.Y + ScrollLineInventory, sizex, sizey, itemsAtAreaCount);
					if (itemsAtArea == null || itemsAtAreaCount.value > 1)
					{
						c = ColorCi.FromArgb(100, 255, 0, 0); // red
					}
					else //0 or 1
					{
						c = ColorCi.FromArgb(100, 0, 255, 0); // green
					}
					game.rend.Draw2dTexture(game.rend.WhiteTexture(), x, y,
						CellDrawSize * sizex, CellDrawSize * sizey,
						null, 0, c, false);
				}
			}
			IntRef selectedWear = SelectedWearPlace(scaledMouse);
			if (selectedWear != null)
			{
				PointRef p = PointRef.Create(wearPlaceStart[selectedWear.value].X + InventoryStartX(), wearPlaceStart[selectedWear.value].Y + InventoryStartY());
				PointRef size = wearPlaceCells[selectedWear.value];

				int c;
				Packet_Item itemsAtArea = inventoryUtil.ItemAtWearPlace(selectedWear.value, game.ActiveHudIndex);
				if (!dataItems.CanWear(selectedWear.value, game.d_Inventory.DragDropItem))
				{
					c = ColorCi.FromArgb(100, 255, 0, 0); // red
				}
				else //0 or 1
				{
					c = ColorCi.FromArgb(100, 0, 255, 0); // green
				}
				game.rend.Draw2dTexture(game.rend.WhiteTexture(), p.X, p.Y,
					CellDrawSize * size.X, CellDrawSize * size.Y,
					null, 0, c, false);
			}
		}

		//material selector
		DrawMaterialSelector();

		//wear
		//DrawItem(Offset(wearPlaceStart[(int)WearPlace.LeftHand], InventoryStart), inventory.LeftHand[ActiveMaterial.ActiveMaterial], null);
		DrawItem(wearPlaceStart[WearPlace_.RightHand].X + InventoryStartX(), wearPlaceStart[WearPlace_.RightHand].Y + InventoryStartY(), game.d_Inventory.RightHand[game.ActiveHudIndex], 0, 0);
		DrawItem(wearPlaceStart[WearPlace_.MainArmor].X + InventoryStartX(), wearPlaceStart[WearPlace_.MainArmor].Y + InventoryStartY(), game.d_Inventory.MainArmor, 0, 0);
		DrawItem(wearPlaceStart[WearPlace_.Boots].X + InventoryStartX(), wearPlaceStart[WearPlace_.Boots].Y + InventoryStartY(), game.d_Inventory.Boots, 0, 0);
		DrawItem(wearPlaceStart[WearPlace_.Helmet].X + InventoryStartX(), wearPlaceStart[WearPlace_.Helmet].Y + InventoryStartY(), game.d_Inventory.Helmet, 0, 0);
		DrawItem(wearPlaceStart[WearPlace_.Gauntlet].X + InventoryStartX(), wearPlaceStart[WearPlace_.Gauntlet].Y + InventoryStartY(), game.d_Inventory.Gauntlet, 0, 0);

		//info
		if (SelectedCellInventory(scaledMouse) != null)
		{
			PointRef selected = SelectedCellInventory(scaledMouse);
			selected.Y += ScrollLineInventory;
			PointRef itemAtCell = inventoryUtil.ItemAtCell(selected);
			if (itemAtCell != null)
			{
				Packet_Item item = GetItem(game.d_Inventory, itemAtCell.X, itemAtCell.Y);
				if (item != null)
				{
					int x = (selected.X) * CellDrawSize + CellsStartInventoryX();
					int y = (selected.Y) * CellDrawSize + CellsStartInventoryY();
					DrawItemInfo(scaledMouse.X, scaledMouse.Y, item);
				}
			}
		}
		if (SelectedWearPlace(scaledMouse) != null)
		{
			int selected = SelectedWearPlace(scaledMouse).value;
			Packet_Item itemAtWearPlace = inventoryUtil.ItemAtWearPlace(selected, game.ActiveHudIndex);
			if (itemAtWearPlace != null)
			{
				DrawItemInfo(scaledMouse.X, scaledMouse.Y, itemAtWearPlace);
			}
		}
		if (SelectedMaterialSelectorSlot(scaledMouse) != null)
		{
			int selected = SelectedMaterialSelectorSlot(scaledMouse).value;
			Packet_Item item = game.d_Inventory.RightHand[selected];
			if (item != null)
			{
				DrawItemInfo(scaledMouse.X, scaledMouse.Y, item);
			}
		}

		if (game.d_Inventory.DragDropItem != null)
		{
			DrawItem(scaledMouse.X, scaledMouse.Y, game.d_Inventory.DragDropItem, 0, 0);
		}
	}

	Packet_Item GetItem(Packet_Inventory inventory, int x, int y)
	{
		for (int i = 0; i < inventory.ItemsCount; i++)
		{
			if (inventory.Items[i].X == x && inventory.Items[i].Y == y)
			{
				return inventory.Items[i].Value_;
			}
		}
		return null;
	}

	public void DrawMaterialSelector()
	{
		game.Draw2dBitmapFile("materials.png", MaterialSelectorBackgroundStartX(), MaterialSelectorBackgroundStartY(), game.platform.FloatToInt(1024 * game.Scale()), game.platform.FloatToInt(128 * game.Scale()));
		int materialSelectorStartX_ = MaterialSelectorStartX();
		int materialSelectorStartY_ = MaterialSelectorStartY();
		for (int i = 0; i < 10; i++)
		{
			Packet_Item item = game.d_Inventory.RightHand[i];
			if (item != null)
			{
				DrawItem(materialSelectorStartX_ + i * ActiveMaterialCellSize(), materialSelectorStartY_,
					item, ActiveMaterialCellSize(), ActiveMaterialCellSize());
			}
		}
		game.Draw2dBitmapFile("activematerial.png",
			MaterialSelectorStartX() + ActiveMaterialCellSize() * game.ActiveHudIndex,
			MaterialSelectorStartY(), ActiveMaterialCellSize() * 64 / 48, ActiveMaterialCellSize() * 64 / 48);
	}

	IntRef SelectedWearPlace(PointRef scaledMouse)
	{
		for (int i = 0; i < wearPlaceStartLength; i++)
		{
			PointRef p = wearPlaceStart[i];
			p.X += InventoryStartX();
			p.Y += InventoryStartY();
			PointRef cells = wearPlaceCells[i];
			if (scaledMouse.X >= p.X && scaledMouse.Y >= p.Y
				&& scaledMouse.X < p.X + cells.X * CellDrawSize
				&& scaledMouse.Y < p.Y + cells.Y * CellDrawSize)
			{
				return IntRef.Create(i);
			}
		}
		return null;
	}

	const int wearPlaceStartLength = 5;
	//indexed by enum WearPlace
	PointRef[] wearPlaceStart;
	//indexed by enum WearPlace
	PointRef[] wearPlaceCells;

	void DrawItem(int screenposX, int screenposY, Packet_Item item, int drawsizeX, int drawsizeY)
	{
		if (item == null)
		{
			return;
		}
		int sizex = dataItems.ItemSizeX(item);
		int sizey = dataItems.ItemSizeY(item);
		if (drawsizeX == 0 || drawsizeX == -1)
		{
			drawsizeX = CellDrawSize * sizex;
			drawsizeY = CellDrawSize * sizey;
		}
		if (item.ItemClass == Packet_ItemClassEnum.Block)
		{
			if (item.BlockId == 0)
			{
				return;
			}
			game.rend.Draw2dTexture(game.terrainTexture, screenposX, screenposY,
				drawsizeX, drawsizeY, IntRef.Create(dataItems.TextureIdForInventory()[item.BlockId]), game.rend.texturesPacked(), ColorCi.FromArgb(255, 255, 255, 255), false);
			if (item.BlockCount > 1)
			{
				FontCi font = new FontCi();
				font.size = 8;
				game.rend.Draw2dText(game.platform.IntToString(item.BlockCount), font, screenposX, screenposY, null, false);
			}
		}
		else
		{
			game.Draw2dBitmapFile(dataItems.ItemGraphics(item), screenposX, screenposY,
				drawsizeX, drawsizeY);
		}
	}

	public void DrawItemInfo(int screenposX, int screenposY, Packet_Item item)
	{
		int sizex = dataItems.ItemSizeX(item);
		int sizey = dataItems.ItemSizeY(item);
		IntRef tw = new IntRef();
		IntRef th = new IntRef();
		float one = 1;
		FontCi font = new FontCi();
		font.size = 10;
		game.platform.TextSize(dataItems.ItemInfo(item), font, tw, th);
		tw.value += 6;
		th.value += 4;
		int w = game.platform.FloatToInt(tw.value + CellDrawSize * sizex);
		int h = game.platform.FloatToInt(th.value < CellDrawSize * sizey ? CellDrawSize * sizey + 4 : th.value);
		if (screenposX < w + 20) { screenposX = w + 20; }
		if (screenposY < h + 20) { screenposY = h + 20; }
		if (screenposX > game.Width() - (w + 20)) { screenposX = game.Width() - (w + 20); }
		if (screenposY > game.Height() - (h + 20)) { screenposY = game.Height() - (h + 20); }
		game.rend.Draw2dTexture(game.rend.WhiteTexture(), screenposX - w, screenposY - h, w, h, null, 0, ColorCi.FromArgb(255, 0, 0, 0), false);
		game.rend.Draw2dTexture(game.rend.WhiteTexture(), screenposX - w + 2, screenposY - h + 2, w - 4, h - 4, null, 0, ColorCi.FromArgb(255, 105, 105, 105), false);
		game.rend.Draw2dText(dataItems.ItemInfo(item), font, screenposX - tw.value + 4, screenposY - h + 2, null, false);
		Packet_Item item2 = new Packet_Item();
		item2.BlockId = item.BlockId;
		DrawItem(screenposX - w + 2, screenposY - h + 2, item2, 0, 0);
	}

	public override void OnMouseWheelChanged(Game game_, MouseWheelEventArgs args)
	{
		float delta = args.GetDeltaPrecise();
		if ((game_.guistate == GuiState.Normal || (game_.guistate == GuiState.Container && !IsMouseOverCells()))
			&& (!game_.keyboardState[game_.GetKey(GlKeys.LShift)]))
		{
			game_.ActiveHudIndex -= game_.platform.FloatToInt(delta);
			game_.ActiveHudIndex = game_.ActiveHudIndex % 10;
			while (game_.ActiveHudIndex < 0)
			{
				game_.ActiveHudIndex += 10;
			}
		}
		if (IsMouseOverCells() && game.guistate == GuiState.Container)
		{
			if (delta > 0)
			{
				ScrollUp();
			}
			if (delta < 0)
			{
				ScrollDown();
			}
		}
	}
}


public class PacketHandlerContainerInventory : ClientPacketHandler
{
    internal ModGuiCointainer mod;
    public override void Handle(Game game, Packet_Server packet)
    {
        mod.d_container = packet.Container.Inventory;
        mod.X = packet.Container.X;
        mod.Y = packet.Container.Y;
        mod.Z = packet.Container.Z;
        game.platform.ConsoleWriteLine("ADDED INVENTORY");

    }
}
