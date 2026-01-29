using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [Header("Player Money")]
    [SerializeField] private int currentMoney = 0;
    public int CurrentMoney => currentMoney;

    [Header("Base Prices")]
    [SerializeField] private int doubleJumpBasePrice = 100;
    [SerializeField] private int dashBasePrice = 150;
    [SerializeField] private int bagUpgradeBasePrice = 200;
    [SerializeField] private int pickaxeUpgradeBasePrice = 250;

    [Header("Price Growth Settings")]
    [SerializeField] private float priceMultiplier = 1.25f; // satýn aldýkça fiyat artar

    private int doubleJumpLevel = 0;
    private int dashLevel = 0;
    private int bagLevel = 0;
    private int pickaxeLevel = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ---------------- MONEY ---------------- //
    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            return true;
        }
        return false;
    }

    // ---------------- PRICES ---------------- //
    public int GetDoubleJumpPrice() => Mathf.RoundToInt(doubleJumpBasePrice * Mathf.Pow(priceMultiplier, doubleJumpLevel));
    public int GetDashPrice() => Mathf.RoundToInt(dashBasePrice * Mathf.Pow(priceMultiplier, dashLevel));
    public int GetBagUpgradePrice() => Mathf.RoundToInt(bagUpgradeBasePrice * Mathf.Pow(priceMultiplier, bagLevel));
    public int GetPickaxeUpgradePrice() => Mathf.RoundToInt(pickaxeUpgradeBasePrice * Mathf.Pow(priceMultiplier, pickaxeLevel));

    // ---------------- PURCHASE METHODS ---------------- //
    public bool TryPurchaseDoubleJump()
    {
        int price = GetDoubleJumpPrice();
        if (SpendMoney(price))
        {
            doubleJumpLevel++;
            Debug.Log("Double Jump satýn alýndý! Seviye: " + doubleJumpLevel);
            return true;
        }
        return false;
    }

    public bool TryPurchaseDash()
    {
        int price = GetDashPrice();
        if (SpendMoney(price))
        {
            dashLevel++;
            Debug.Log("Dash satýn alýndý! Seviye: " + dashLevel);
            return true;
        }
        return false;
    }

    public bool TryPurchaseBagUpgrade()
    {
        int price = GetBagUpgradePrice();
        if (SpendMoney(price))
        {
            bagLevel++;
            Debug.Log("Bag Upgrade satýn alýndý! Seviye: " + bagLevel);
            return true;
        }
        return false;
    }

    public bool TryPurchasePickaxeUpgrade()
    {
        int price = GetPickaxeUpgradePrice();
        if (SpendMoney(price))
        {
            pickaxeLevel++;
            Debug.Log("Pickaxe Upgrade satýn alýndý! Seviye: " + pickaxeLevel);
            return true;
        }
        return false;
    }
}
