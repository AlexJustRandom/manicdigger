public class GameData
{
    public GameData()
    {
        mBlockIdEmpty = 0;
        mBlockIdDirt = -1;
        mBlockIdSponge = -1;
        mBlockIdTrampoline = -1;
        mBlockIdAdminium = -1;
        mBlockIdCompass = -1;
        mBlockIdLadder = -1;
        mBlockIdEmptyHand = -1;
        mBlockIdCraftingTable = -1;
        mBlockIdLava = -1;
        mBlockIdStationaryLava = -1;
        mBlockIdFillStart = -1;
        mBlockIdCuboid = -1;
        mBlockIdFillArea = -1;
        mBlockIdMinecart = -1;
        mBlockIdRailstart = -128; // 64 rail tiles
    }
    public void Start()
    {
        Initialize(GlobalVar.MAX_BLOCKTYPES);
    }
    public void Update()
    {
    }
    int BlocktypeCount;

    void Initialize(int count)
    {
        BlockTypes = new Packet_BlockType[count];
        BlocktypeCount = count;
        mDefaultHudSlotCount = 10;


        for (int i = 0; i < count; i++)
        {
            BlockTypes[i] = new Packet_BlockType();
        }

            //Tools

            tooltypes = new string[count];
    }

    public int WhenPlayerPlacesGetsConvertedTo(int blockid) { return (BlockTypes[blockid].WhenPlacedGetsConvertedTo==0) ? blockid: BlockTypes[blockid].WhenPlacedGetsConvertedTo; }
    public bool IsFlower(int blockid) { return BlockTypes[blockid].DrawType == Packet_DrawTypeEnum.Plant; }
    public int Rail(int blockid) { return BlockTypes[blockid].Rail; }
    public float WalkSpeed(int blockid) { return DeserializeFloat(BlockTypes[blockid].WalkSpeedWhenUsedFloat); }
    public bool IsSlipperyWalk(int blockid) { return BlockTypes[blockid].IsSlipperyWalk; }
    public string[] WalkSound(int blockid) { return BlockTypes[blockid].Sounds.Walk; }
    public string[] BreakSound(int blockid) { return BlockTypes[blockid].Sounds.Break1; }
    public string[] BuildSound(int blockid) { return BlockTypes[blockid].Sounds.Build; }
    public string[] CloneSound(int blockid) { return BlockTypes[blockid].Sounds.Clone; }
    public int LightRadius(int blockid) { return BlockTypes[blockid].LightRadius; }

    public int StartInventoryLenght() { return BlocktypeCount; }
    public int GetStartInventoryAmount(int blockid) { return BlockTypes[blockid].StartInventoryAmount; }
    public void SetStartInventoryAmount(int blockid,int value) {   BlockTypes[blockid].StartInventoryAmount=value; }

    public float Hardness(int blockid) { return DeserializeFloat(BlockTypes[blockid].Hardness); }
    public float ToolStrength(int blockid) { return DeserializeFloat(BlockTypes[blockid].ToolStrenghtFloat); }
    public int DamageToPlayer(int blockid) { return BlockTypes[blockid].DamageToPlayer; }
    public int WalkableType1(int blockid) { return BlockTypes[blockid].WalkableType; }
    public int HarvestabilityMask(int blockid) { return BlockTypes[blockid].HarvestabilityMask; }
    public int ToolSpeedBonusMask(int blockid) { return BlockTypes[blockid].ToolSpeedBonusMask; }
    public int ToolTypeMask(int blockid) { return BlockTypes[blockid].ToolTypeMask; }

    public int DefaultHudSlotCount() { return mDefaultHudSlotCount; }

    public Packet_BlockType GetBlockType(int id) { return BlockTypes[id]; }

    //for now not inicilized outside server TODO
    public string[] tooltypes; //STUPID TODO
    public const int MAX_TOOLTYPES = 32;
    public int tooltypesAdded;


    public bool GetsSpeedBonus(int blockId, int toolID) {
        int bonusMask = ToolSpeedBonusMask(blockId);
        int toolmask = ToolTypeMask(toolID);
         return (bonusMask & toolmask) > 0;
    }

    public bool IsHarvestableByTool(int blockId, int toolID) {
        int harvestabilitymask = HarvestabilityMask(blockId);
         int toolmask = ToolTypeMask(toolID);
        if (harvestabilitymask == 0) return true;//if no data harvestable by all
        return (harvestabilitymask & toolmask) > 0;
    }

    int mDefaultHudSlotCount;

    // TODO: hardcoded IDs
    // few code sections still expect some hardcoded IDs
    int mBlockIdEmpty;
    int mBlockIdDirt;
    int mBlockIdSponge;
    int mBlockIdTrampoline;
    int mBlockIdAdminium;
    int mBlockIdCompass;
    int mBlockIdLadder;
    int mBlockIdEmptyHand;
    int mBlockIdCraftingTable;
    int mBlockIdLava;
    int mBlockIdStationaryLava;
    int mBlockIdFillStart;
    int mBlockIdCuboid;
    int mBlockIdFillArea;
    int mBlockIdMinecart;
    int mBlockIdRailstart; // 64 rail tiles

    public int BlockIdEmpty() { return mBlockIdEmpty; }
    public int BlockIdDirt() { return mBlockIdDirt; }
    public int BlockIdSponge() { return mBlockIdSponge; }
    public int BlockIdTrampoline() { return mBlockIdTrampoline; }
    public int BlockIdAdminium() { return mBlockIdAdminium; }
    public int BlockIdCompass() { return mBlockIdCompass; }
    public int BlockIdLadder() { return mBlockIdLadder; }
    public int BlockIdEmptyHand() { return mBlockIdEmptyHand; }
    public int BlockIdCraftingTable() { return mBlockIdCraftingTable; }
    public int BlockIdLava() { return mBlockIdLava; }
    public int BlockIdStationaryLava() { return mBlockIdStationaryLava; }
    public int BlockIdFillStart() { return mBlockIdFillStart; }
    public int BlockIdCuboid() { return mBlockIdCuboid; }
    public int BlockIdFillArea() { return mBlockIdFillArea; }
    public int BlockIdMinecart() { return mBlockIdMinecart; }
    public int BlockIdRailstart() { return mBlockIdRailstart; }

    // TODO: atm it sets sepcial block id from block name - better use new block property
    public bool SetSpecialBlock(Packet_BlockType b, int id)
    {
        switch (b.Name)
        {
            case "Empty":
                this.mBlockIdEmpty = id;
                return true;
            case "Dirt":
                this.mBlockIdDirt = id;
                return true;
            case "Sponge":
                this.mBlockIdSponge = id;
                return true;
            case "Trampoline":
                this.mBlockIdTrampoline = id;
                return true;
            case "Adminium":
                this.mBlockIdAdminium = id;
                return true;
            case "Compass":
                this.mBlockIdCompass = id;
                return true;
            case "Ladder":
                this.mBlockIdLadder = id;
                return true;
            case "EmptyHand":
                this.mBlockIdEmptyHand = id;
                return true;
            case "CraftingTable":
                this.mBlockIdCraftingTable = id;
                return true;
            case "Lava":
                this.mBlockIdLava = id;
                return true;
            case "StationaryLava":
                this.mBlockIdStationaryLava = id;
                return true;
            case "FillStart":
                this.mBlockIdFillStart = id;
                return true;
            case "Cuboid":
                this.mBlockIdCuboid = id;
                return true;
            case "FillArea":
                this.mBlockIdFillArea = id;
                return true;
            case "Minecart":
                this.mBlockIdMinecart = id;
                return true;
            case "Rail0":
                this.mBlockIdRailstart = id;
                return true;
            default:
                return false;
        }
    }

    public bool IsRailTile(int id)
    {
        return id >= BlockIdRailstart() && id < BlockIdRailstart() + 64;
    }
    Packet_BlockType[] BlockTypes;

    public void UseBlockTypes(Packet_BlockType[] blocktypes, int count)
    {
        
        for (int i = 0; i < count; i++)
        {
            if (blocktypes[i] != null)
            {
                UseBlockType(i, blocktypes[i]);
            }
        }
    }

    public void UseBlockType(int id, Packet_BlockType b)
    {
        if (b.Name == null)//!b.IsValid)
        {
            return;
        }
        //public bool[] IsWater { get { return mIsWater; } }
        //            public bool[] IsTransparentForLight { get { return mIsTransparentForLight; } }
        //public bool[] IsEmptyForPhysics { get { return mIsEmptyForPhysics; } }
  
        BlockTypes[id] = b;
 
        BlockTypes[id].Sounds.Walk = new string[SoundCount];
        BlockTypes[id].Sounds.Break1 = new string[SoundCount];
        BlockTypes[id].Sounds.Build = new string[SoundCount];
        BlockTypes[id].Sounds.Clone = new string[SoundCount];


        if (b.Sounds != null)
        {
            for (int i = 0; i < b.Sounds.WalkCount; i++)
            {
                BlockTypes[id].Sounds.Walk[i] = b.Sounds.Walk[i];
            }
            for (int i = 0; i < b.Sounds.Break1Count; i++)
            {
                BlockTypes[id].Sounds.Break1[i] = b.Sounds.Break1[i];
            }
            for (int i = 0; i < b.Sounds.BuildCount; i++)
            {
                BlockTypes[id].Sounds.Build[i] = b.Sounds.Build[i];
            }
            for (int i = 0; i < b.Sounds.CloneCount; i++)
            {
                BlockTypes[id].Sounds.Clone[i] = b.Sounds.Clone[i];
            }
        }


        SetSpecialBlock(b, id);
    }

    public const int SoundCount = 8;

    float DeserializeFloat(int p)
    {
        float one = 1;
        return (one * p) / 32;
    }


    public bool IsEmptyForPhysics(Packet_BlockType block)
    {
        return (block.DrawType == Packet_DrawTypeEnum.Ladder)
            || (block.WalkableType != Packet_WalkableTypeEnum.Solid && block.WalkableType != Packet_WalkableTypeEnum.Fluid);
    }
    public bool IsRail(Packet_BlockType block)
    {
        return block.Rail > 0;  //Does not include Rail0, but this can't be placed.
    }
    public float getblockheight(int id)
    {
        float one = 1.0f;
        float RailHeight = one * 3 / 10;
 
        if (BlockTypes[id].Rail != 0)
        {
            return RailHeight;
        }
        if (BlockTypes[id].DrawType == Packet_DrawTypeEnum.HalfHeight)
        {
            return one / 2;
        }
        if (BlockTypes[id].DrawType == Packet_DrawTypeEnum.Flat)
        {
            return one / 20;
        }
        return 1;
    }
}
