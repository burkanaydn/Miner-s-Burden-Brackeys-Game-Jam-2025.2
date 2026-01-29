using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanel : MonoBehaviour
{ 
    [Header("Double Jump")]
    [SerializeField] private TextMeshProUGUI doubleJumpPriceText;
    [SerializeField] private Button doubleJumpButton;

    [Header("Dash")]
    [SerializeField] private TextMeshProUGUI dashPriceText;
    [SerializeField] private Button dashButton;

    [Header("Bag Upgrade")]
    [SerializeField] private TextMeshProUGUI bagUpgradePriceText;
    [SerializeField] private Button bagUpgradeButton;

    [Header("Pickaxe Upgrade")]
    [SerializeField] private TextMeshProUGUI pickaxePriceText;
    [SerializeField] private Button pickaxeButton;

    [Header("Referances")]
    [SerializeField] private SimpleJump2D SimpleJump2D;
    [SerializeField] private SimpleDash2D SimpleDash2D;
    [SerializeField] private TilemapDig2D TilemapDig2D;
    [SerializeField] private PlayerInventoryManager PlayerInventoryManager;
    [SerializeField] private GameplayPanel gameplayPanel;

    [Header("Bools")]
    private bool onDoubleJump;
    private bool onDash;
    private bool onPickaxeMax;
    private bool onBagMax;

    [Header("Upgrade Levels")]
    private int pickaxeLevel;
    [SerializeField] private int pickaxeMaxLevel = 2;
    private int bagLevel;
    [SerializeField] private int bagMaxLevel = 2;

    private void Start()
    {
        RefreshPanel();
    }

    /*private void OnEnable()
    {
        BindButtons();
        RefreshPanel(); // panel açýldýðýnda fiyatlarý & buton durumlarýný güncelle
    }*/

    /*private void OnDisable()
    {
        UnbindButtons();
    }*/

    private void BindButtons()
    {
        if (doubleJumpButton != null) doubleJumpButton.onClick.AddListener(OnBuyDoubleJump);
        if (dashButton != null) dashButton.onClick.AddListener(OnBuyDash);
        if (bagUpgradeButton != null) bagUpgradeButton.onClick.AddListener(OnBuyBagUpgrade);
        if (pickaxeButton != null) pickaxeButton.onClick.AddListener(OnBuyPickaxeUpgrade);
    }

    private void UnbindButtons()
    {
        if (doubleJumpButton != null) doubleJumpButton.onClick.RemoveListener(OnBuyDoubleJump);
        if (dashButton != null) dashButton.onClick.RemoveListener(OnBuyDash);
        if (bagUpgradeButton != null) bagUpgradeButton.onClick.RemoveListener(OnBuyBagUpgrade);
        if (pickaxeButton != null) pickaxeButton.onClick.RemoveListener(OnBuyPickaxeUpgrade);
    }

    /// <summary>
    /// Dýþarýdan panel açýldýðýnda veya manuel yenileme istediðinde çaðýr.
    /// </summary>
    public void RefreshPanel()
    {
        if (EconomyManager.Instance == null)
        {
            return;
        }
        UpdatePricesAndButtons();
    }

    

    private void UpdatePricesAndButtons()
    {
        // Double Jump
        if (doubleJumpPriceText != null && !onDoubleJump)
            doubleJumpPriceText.text = EconomyManager.Instance.GetDoubleJumpPrice().ToString();

        if (doubleJumpPriceText != null && onDoubleJump)
            doubleJumpPriceText.text = "Unlocked";

        if (doubleJumpButton != null && !onDoubleJump)
            doubleJumpButton.interactable = EconomyManager.Instance.CurrentMoney >= EconomyManager.Instance.GetDoubleJumpPrice();

        if (doubleJumpButton != null && onDoubleJump)
            doubleJumpButton.interactable = false;

        // Dash
        if (dashPriceText != null && !onDash)
            dashPriceText.text = EconomyManager.Instance.GetDashPrice().ToString();

        if (dashPriceText != null && onDash)
            dashPriceText.text = "Unlocked";

        if (dashButton != null && !onDash)
            dashButton.interactable = EconomyManager.Instance.CurrentMoney >= EconomyManager.Instance.GetDashPrice();

        if (dashButton != null && onDash)
            dashButton.interactable = false;

        // Bag Upgrade
        if (bagUpgradePriceText != null && !onBagMax)
            bagUpgradePriceText.text = EconomyManager.Instance.GetBagUpgradePrice().ToString();

        if (bagUpgradePriceText != null && onBagMax)
            bagUpgradePriceText.text = "MAX";

        if (bagUpgradeButton != null && !onBagMax)
            bagUpgradeButton.interactable = EconomyManager.Instance.CurrentMoney >= EconomyManager.Instance.GetBagUpgradePrice();

        if (bagUpgradeButton != null && onBagMax)
            bagUpgradeButton.interactable = false;

        // Pickaxe Upgrade
        if (pickaxePriceText != null && !onPickaxeMax)
            pickaxePriceText.text = EconomyManager.Instance.GetPickaxeUpgradePrice().ToString();

        if (pickaxePriceText != null && onPickaxeMax)
            pickaxePriceText.text = "MAX";

        if (pickaxeButton != null && !onPickaxeMax)
            pickaxeButton.interactable = EconomyManager.Instance.CurrentMoney >= EconomyManager.Instance.GetPickaxeUpgradePrice();

        if (pickaxeButton != null && onPickaxeMax)
            pickaxeButton.interactable = false;
    }

    #region Button Handlers
    private void OnBuyDoubleJump()
    {
        if (EconomyManager.Instance == null) return;

        bool ok = EconomyManager.Instance.TryPurchaseDoubleJump();
        if (ok)
        {
            SimpleJump2D.maxAirJumps = 1;
            onDoubleJump = true;
            OnPurchaseSuccess();
            gameplayPanel.UpdateMoneyText();
            SoundManager.Instance.PlayButtonClick();

        }
        else
        {
            OnPurchaseFailed();
        }
    }

    private void OnBuyDash()
    {
        if (EconomyManager.Instance == null) return;

        bool ok = EconomyManager.Instance.TryPurchaseDash();
        if (ok)
        {
            SimpleDash2D.enableDash = true;
            onDash = true;
            OnPurchaseSuccess();
            gameplayPanel.UpdateMoneyText();
            SoundManager.Instance.PlayButtonClick();
        }
        else OnPurchaseFailed();
    }

    private void OnBuyBagUpgrade()
    {
        if (EconomyManager.Instance == null) return;

        if (bagLevel >= bagMaxLevel)
        {
            // Zaten maksimum seviyeye ulaþmýþ
            onBagMax = true;
            return;
        }

        bool ok = EconomyManager.Instance.TryPurchaseBagUpgrade();

        if (ok)
        {
            bagLevel++;
            PlayerInventoryManager.maxWeight += 10; // Her seviye için +10 aðýrlýk

            if (bagLevel >= bagMaxLevel)
                onBagMax = true;

            OnPurchaseSuccess();
            gameplayPanel.UpdateMoneyText();
            SoundManager.Instance.PlayBagUpgrade();
        }
        else
        {
            // Parasý yetmiyor
            OnPurchaseFailed();
        }
    }

    private void OnBuyPickaxeUpgrade()
    {
        if (EconomyManager.Instance == null) return;

        if (pickaxeLevel >= pickaxeMaxLevel)
        {
            // Zaten maksimum seviyeye ulaþmýþ
            onPickaxeMax = true;
            return;
        }

        bool ok = EconomyManager.Instance.TryPurchasePickaxeUpgrade();

        if (ok)
        {
            pickaxeLevel++;
            TilemapDig2D.digDuration -= 0.5f;

            if (pickaxeLevel >= pickaxeMaxLevel)
                onPickaxeMax = true;

            OnPurchaseSuccess(); // UI güncellemesi için kesinlikle çaðrýlýyor
            gameplayPanel.UpdateMoneyText();
            SoundManager.Instance.PlayPickaxeUpgrade();
        }
        else
        {
            // Parasý yetmiyor
            OnPurchaseFailed();
        }
    }

    #endregion

    private void OnPurchaseSuccess()
    {
        // Baþarýlý satýn alým sonrasý yapýlacaklar:
        RefreshPanel();
        Debug.Log("Purchase successful.");
    }

    private void OnPurchaseFailed()
    {
        Debug.Log("Purchase failed: not enough money.");
    }

    public void OpenPanel()
    {
        BindButtons();
        RefreshPanel();
    }

    public void ClosePanel()
    {
        UnbindButtons();
    }
}
