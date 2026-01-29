using UnityEngine;

[CreateAssetMenu(fileName = "NewOreData", menuName = "Ore Data", order = 0)]
public class OreData : ScriptableObject
{
    public string oreID;
    public string oreName;
    public float baseDropChance; //0-1
    public int weight;
    public int price;
    public int valueOrder; // 0 en deðersiz
    public Sprite icon;
}
