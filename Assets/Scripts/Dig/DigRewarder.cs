using UnityEngine;

[RequireComponent(typeof(TilemapDig2D))]
public class DigRewarder : MonoBehaviour
{
    private OreRandomLoot lootSystem;

    private void Awake()
    {
        lootSystem = new OreRandomLoot();

        TilemapDig2D digSystem = GetComponent<TilemapDig2D>();
        digSystem.OnDigComplete += HandleDigComplete;
    }

    private void HandleDigComplete(Vector3Int cell)
    {
        // OreRandomLoot ile rastgele ore ID al
        string oreID = lootSystem.GetRandomOreID(LevelManager.Instance.CurrentLevel);
        if (!string.IsNullOrEmpty(oreID))
        {
            // ID ile MiningResultEvent oluþtur
            MiningResultEvent result = new MiningResultEvent(oreID);

            // Event publish et
            EventManager.Publish(result);

            // Debug
            OreData ore = OreDatabase.Instance.GetOreByID(oreID);
            if (ore != null)
                Debug.Log($"Kazý sonucu: {ore.oreName} bulundu, oyuncuya gösterilecek.");
        }
    }
}
