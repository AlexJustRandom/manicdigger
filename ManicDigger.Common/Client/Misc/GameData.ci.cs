public class GameData
{
	public GameData()
	{
		
		mBlockIdRailstart = -128; // 64 rail tiles
	}
	public void Start()
	{
		Initialize(GlobalVar.MAX_BLOCKTYPES);
	}
	public void Update()
    {
    }
    int blckTypesLength;
    void Initialize(int count)
	{
        blckTypesLength = count;
        blockTypes = new Packet_BlockType[count];
        tooltypes = new string[MAX_TOOLTYPES];
        mDefaultHudSlotCount = 10;
        mStartInventoryAmount = new int[count];
    }
    public int GetBlockId(string name)
    {
        for (int i = 0; i < blckTypesLength; i++)
        {
            if (blockTypes[i].Name == name)
            {
                return i;
            }
        }

        return -1;
        //throw new Exception("No Block id:" + name);
    }
    public int[] mStartInventoryAmount; //STUPID TODO

    int mDefaultHudSlotCount;

    public int DefaultHudSlotCount()
    {
            return mDefaultHudSlotCount;
    }

    //for now not inicilized outside server TODO
    public string[] tooltypes; //STUPID TODO
    public const int MAX_TOOLTYPES = 32;
    public int tooltypesAdded;

    public bool IsHarvestableByTool(int blockId, int toolID) {
        int harvestabilitymask = blockTypes[blockId].HarvestabilityMask;
        int toolmask = blockTypes[blockId].ToolTypeMask;
        if (harvestabilitymask == 0) return true;//if no data harvestable by all
        return (harvestabilitymask & toolmask) > 0;
    }

 
	int mBlockIdRailstart; // 64 rail tiles


    public int BlockIdRailstart() { return mBlockIdRailstart; }


    public bool IsRailTile(int id)
	{
		return id >= BlockIdRailstart() && id < BlockIdRailstart() + 64;
	}

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
    Packet_BlockType[] blockTypes;
    public void UseBlockType(int id, Packet_BlockType b)
	{
		if (b.Name == null)//!b.IsValid)
		{
			return;
		}
        //public bool[] IsWater { get { return mIsWater; } }
        //            public bool[] IsTransparentForLight { get { return mIsTransparentForLight; } }
        //public bool[] IsEmptyForPhysics { get { return mIsEmptyForPhysics; } }
        blockTypes[id] = b;


    }

    public const int SoundCount = 8;

	float DeserializeFloat(int p)
	{
		float one = 1;
		return (one * p) / 32;
	}
}
