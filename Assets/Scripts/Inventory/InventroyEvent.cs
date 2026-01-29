public struct InventoryEvent
{
    public struct ItemAdded
    {
        public string oreID;
        public float weightRatio;
        public ItemAdded(string oreID, float weightRatio)
        {
            this.oreID = oreID;
            this.weightRatio = weightRatio;
        }
    }

    public struct ItemRemoved
    {
        public string oreID;
        public float weightRatio;
        public ItemRemoved(string oreID, float weightRatio)
        {
            this.oreID = oreID;
            this.weightRatio = weightRatio;
        }
    }

    public struct AllItemsRemoved
    {

    }
}