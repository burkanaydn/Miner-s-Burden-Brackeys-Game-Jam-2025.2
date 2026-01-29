using System.Collections.Generic;
using UnityEngine;

public class OreRandomLoot
{
    public int maxLevel = 6; // Oyundaki toplam seviye sayýsý

    // OreDatabase null-safe eriþim
    private List<OreData> ores => OreDatabase.Instance?.ores;

    // Rastgele ore seç ve ID'sini döndür
    public string GetRandomOreID(int playerLevel)
    {
        var localOres = ores;
        if (localOres == null || localOres.Count == 0)
        {
            Debug.LogWarning("OreRandomLoot.GetRandomOreID: OreDatabase boþ veya bulunamadý.");
            return null;
        }

        // Eðer tek bir ore varsa direkt döndür
        if (localOres.Count == 1) return localOres[0].oreID;

        // playerLevel güvenli aralýða al
        playerLevel = Mathf.Clamp(playerLevel, 1, Mathf.Max(1, maxLevel));

        // maxOrder sýfýr olmamasýný saðla
        int maxOrder = Mathf.Max(1, localOres.Count - 1);

        // Aðýrlýklarý bir kez hesapla (performans ve tutarlýlýk)
        float totalWeight = 0f;
        float[] weights = new float[localOres.Count];
        for (int i = 0; i < localOres.Count; i++)
        {
            float w = GetAdjustedWeight(localOres[i], playerLevel, maxOrder);
            weights[i] = w;
            totalWeight += w;
        }

        if (totalWeight <= 0f)
        {
            // olaðandýþý durumda rastgele bir tane döndür (hiçbir aðýrlýk >0 olmadýysa)
            int idx = Random.Range(0, localOres.Count);
            return localOres[idx].oreID;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < localOres.Count; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
                return localOres[i].oreID;
        }

        // Güvenlik: döngüden gelmezse sonuncuyu döndür
        return localOres[localOres.Count - 1].oreID;
    }

    // maxOrder parametresi dýþarýdan verilerek division-by-zero önlenir
    private float GetAdjustedWeight(OreData ore, int playerLevel, int maxOrder)
    {
        // Güvenlik: ore null kontrolü
        if (ore == null) return 0f;

        // levelT hesaplama (maxLevel>1 varsayýmý için güvenlik)
        float levelT = (playerLevel - 1f) / Mathf.Max(1f, (maxLevel - 1f));
        float orderT = ore.valueOrder / (float)maxOrder; // maxOrder >= 1

        float rarityBias = 1f - Mathf.Abs(orderT - levelT);    // 0..1

        const float underMatchMult = 0.03f; // uyumsuzsa %3’e kadar düþür
        const float overMatchMult = 4.0f;  // çok uyumluysa 4x yükselt
        const float floor = 0.01f; // asgari aðýrlýk (hiç yok olmasýn)

        float mult = Mathf.Lerp(underMatchMult, overMatchMult, rarityBias);
        float adjusted = ore.baseDropChance * mult;

        return Mathf.Max(floor, adjusted);
    }
}
