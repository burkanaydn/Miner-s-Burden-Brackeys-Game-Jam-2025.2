using UnityEngine;
using static InventoryEvent;

[RequireComponent(typeof(Rigidbody2D))]
public class BagWeightController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float weighEffectMultiply = 1f; // maksimum aðýrlýkta hýz kaça düþecek 0-1 arasýnda
    public float CurrentSpeed { get; private set; }

    private void Awake()
    {
        CurrentSpeed = baseSpeed;
    }

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
        ApplyWeight(evt.weightRatio);
    }

    private void OnInventoryChanged(ItemRemoved evt)
    {
        ApplyWeight(evt.weightRatio);
    }
    private void OnInventoryClear(AllItemsRemoved evt)
    {
        ApplyWeight(0);
    }

    // Inventory aðýrlýðýna göre hareket hýzýný günceller
    private void ApplyWeight(float weightRatio)
    {
        CurrentSpeed = Mathf.Lerp(baseSpeed, baseSpeed * weighEffectMultiply, weightRatio);
        // Ýstersen burada animasyon veya baþka sistemleri de tetikleyebilirsin
        Debug.Log($"Current Speed updated: {CurrentSpeed}");
    }
}
