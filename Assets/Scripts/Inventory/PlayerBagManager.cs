using UnityEngine;
using static MiningResultEvent;

[RequireComponent(typeof(PlayerInventoryManager))]
[RequireComponent(typeof(BagWeightController))]
public class PlayerBagManager : MonoBehaviour
{
    private PlayerInventoryManager inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventoryManager>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe<OreAcceptedEvent>(OnOreAccepted);
        EventManager.Subscribe<OreRejectedEvent>(OnOreRejected);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OreAcceptedEvent>(OnOreAccepted);
        EventManager.Unsubscribe<OreRejectedEvent>(OnOreRejected);
    }

    private void OnOreAccepted(OreAcceptedEvent evt)
    {
        if (evt.oreID == null) return;

        // Sadece oreID ile ekleme
        if (inventory.AddItem(evt.oreID))
        {
            Debug.Log($"Oyuncu {evt.oreID} aldý!");
            SoundManager.Instance.PlayItemAccept();
        }
        else
        {
            WarningPanel.Instance.ShowWarning("Bag is Full!");
            Debug.Log("Envanter dolu!");
            SoundManager.Instance.PlayItemReject();
        }
    }

    private void OnOreRejected(OreRejectedEvent evt)
    {
        if (evt.oreID == null) return;

        Debug.Log($"{evt.oreID} reddedildi!");
    }
}
