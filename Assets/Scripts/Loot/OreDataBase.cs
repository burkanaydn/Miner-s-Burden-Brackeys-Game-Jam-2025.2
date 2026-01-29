using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OreDatabase", menuName = "Ore Database")]
public class OreDatabase : ScriptableObject
{
    // Inspector'da doldurulacak ore listesi
    public List<OreData> ores = new List<OreData>();

    // Singleton instance
    private static OreDatabase _instance;

    public static OreDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                // Oyundaki tüm OreDatabase assetlerini bul
                var dbs = Resources.LoadAll<OreDatabase>(""); // Resources klasöründen yükler
                if (dbs.Length > 0)
                {
                    _instance = dbs[0];
                    _instance.Initialize();
                }
                else
                {
                    Debug.LogError("OreDatabase bulunamadý! Lütfen Resources içine ekle.");
                }
            }
            return _instance;
        }
    }

    private Dictionary<string, OreData> oreDict;

    // Runtime hýzlý eriþim için sözlük oluþtur
    public void Initialize()
    {
        if (oreDict != null) return;

        oreDict = new Dictionary<string, OreData>();
        foreach (var ore in ores)
        {
            if (!oreDict.ContainsKey(ore.oreID))
                oreDict.Add(ore.oreID, ore);
            else
                Debug.LogWarning($"Duplicate OreID: {ore.oreID} in {ore.oreName}");
        }
    }

    // ID ile ore verisi çek
    public OreData GetOreByID(string id)
    {
        if (oreDict == null) Initialize();
        return oreDict.TryGetValue(id, out var ore) ? ore : null;
    }
}
