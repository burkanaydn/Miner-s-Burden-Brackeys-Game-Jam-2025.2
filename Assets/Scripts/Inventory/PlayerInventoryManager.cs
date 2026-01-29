using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public float maxWeight = 50f;

    private List<string> itemIDs = new List<string>();
    public float CurrentWeight;

    // ID ile ekleme
    public bool AddItem(string oreID)
    {
        OreData ore = OreDatabase.Instance.GetOreByID(oreID);
        if (ore == null) return false;

        if (CurrentWeight + ore.weight > maxWeight)
            return false;

        itemIDs.Add(oreID);
        CurrentWeight += ore.weight;

        // Aðýrlýk oraný
        float weightRatio = CurrentWeight / maxWeight;

        // Event yayýnla
        EventManager.Publish(new InventoryEvent.ItemAdded(oreID, weightRatio));

        return true;
    }

    // ID ile silme
    public bool RemoveItem(string oreID)
    {
        OreData ore = OreDatabase.Instance.GetOreByID(oreID);
        if (ore == null) return false;

        if (itemIDs.Remove(oreID))
        {
            CurrentWeight -= ore.weight;
            EconomyManager.Instance.AddMoney(ore.price);

            // Aðýrlýk oraný
            float weightRatio = CurrentWeight / maxWeight;

            // Event yayýnla
            EventManager.Publish(new InventoryEvent.ItemRemoved(oreID, weightRatio));

            return true;
        }
        return false;
    }

    public void RemoveAllItems()
    {
        if (itemIDs.Count == 0)
            return;

        float totalWeightRemoved = 0f;
        int totalMoneyEarned = 0;

        foreach (var oreID in itemIDs)
        {
            OreData ore = OreDatabase.Instance.GetOreByID(oreID);
            if (ore == null) continue;

            totalWeightRemoved += ore.weight;
            totalMoneyEarned += ore.price;
        }

        // Ekonomi güncelle
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.AddMoney(totalMoneyEarned);
        }

        // Listeyi temizle
        itemIDs.Clear();
        CurrentWeight -= totalWeightRemoved;
        if (CurrentWeight < 0) CurrentWeight = 0f;

        // Event yayýnla (tüm envanter boþaltýldý)
        EventManager.Publish(new InventoryEvent.AllItemsRemoved());

        Debug.Log($"Tüm eþyalar satýldý! Kazanýlan para: {totalMoneyEarned}");
    }

    public int GetOreCount(string oreID)
    {
        int count = 0;
        foreach (string id in itemIDs)
        {
            if (id == oreID)
                count++;
        }
        return count;
    }


    public IReadOnlyList<string> GetItemIDs() => itemIDs.AsReadOnly();
}