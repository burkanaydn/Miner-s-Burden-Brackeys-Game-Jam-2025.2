using UnityEngine;
using static InventoryEvent;

public class BagSpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite smallBagSprite;
    [SerializeField] private Sprite mediumBagSprite;
    [SerializeField] private Sprite largeBagSprite;

    private void OnEnable()
    {
        EventManager.Subscribe<ItemAdded>(OnInventoryChanged);
        EventManager.Subscribe<ItemRemoved>(OnInventoryChanged);
        EventManager.Subscribe<AllItemsRemoved>(OnInventoryClear);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<ItemAdded>(OnInventoryChanged);
        EventManager.Unsubscribe<ItemRemoved>(OnInventoryChanged);
        EventManager.Unsubscribe<AllItemsRemoved>(OnInventoryClear);
    }

    private void OnInventoryChanged(ItemAdded evt)
    {
        UpdateBagSize(evt.weightRatio);
    }

    private void OnInventoryChanged(ItemRemoved evt)
    {
        UpdateBagSize(evt.weightRatio);
    }
    private void OnInventoryClear(AllItemsRemoved evt)
    {
        UpdateBagSize(0);
    }

    // Max aðýrlýk: PlayerInventory.MaxWeight
    public void UpdateBagSize(float currentWeightRatio)
    {
        if (currentWeightRatio <= 0.33f)
        {
            spriteRenderer.sprite = smallBagSprite;
            SoundManager.Instance.PlayBagUpgrade();
        }
        else if (currentWeightRatio <= 0.66f)
        {
            spriteRenderer.sprite = mediumBagSprite;
            SoundManager.Instance.PlayBagUpgrade();
        }
        else
        {
            spriteRenderer.sprite = largeBagSprite;
            SoundManager.Instance.PlayBagUpgrade();
        }
    }
}
