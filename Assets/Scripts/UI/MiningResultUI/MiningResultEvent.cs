public struct MiningResultEvent
{
    public string oreID;

    public MiningResultEvent(string id)
    {
        oreID = id;
    }
    public struct OreAcceptedEvent
    {
        public string oreID;
        public OreAcceptedEvent(string id) { oreID = id; }
    }

    public struct OreRejectedEvent
    {
        public string oreID;
        public OreRejectedEvent(string id) { oreID = id; }
    }
}
