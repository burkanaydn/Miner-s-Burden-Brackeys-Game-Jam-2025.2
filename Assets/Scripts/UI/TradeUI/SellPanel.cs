using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellPanel : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventoryManager playerInventoryManager;
    [SerializeField] private TradePanel tradePanel;

    [Header("Ore IDs (set to your oreID strings)")]
    [SerializeField] private string copperID = "ore_orecopper";
    [SerializeField] private string silverID = "ore_silver";
    [SerializeField] private string goldID = "ore_gold";
    [SerializeField] private string diamondID = "ore_diamond";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI copperCountText;
    [SerializeField] private TextMeshProUGUI silverCountText;
    [SerializeField] private TextMeshProUGUI goldCountText;
    [SerializeField] private TextMeshProUGUI diamondCountText;
    [SerializeField] private Button sellAllButton;

    [Header("Input Settings")]
    [SerializeField] private KeyCode sellAllKey = KeyCode.C;
    public bool IsOpen { get; set; } = false;

    private void Awake()
    {
        // Eðer inspector'dan atanmamýþsa sahnedeki manager'ý bulmaya çalýþ
        if (playerInventoryManager == null)
            playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();

        if (playerInventoryManager == null)
            Debug.LogWarning("SellPanel: PlayerInventoryManager bulunamadý. Lütfen Inspector'dan atayýn.");
    }

    private void Start()
    {
        RefreshCounts();
    }

    private void OnEnable()
    {
        // Buton olayýný baðla
        if (sellAllButton != null)
            sellAllButton.onClick.AddListener(OnSellAllClicked);

        // Envanter eventlerini dinle
        EventManager.Subscribe<InventoryEvent.ItemAdded>(OnItemAdded);
        EventManager.Subscribe<InventoryEvent.ItemRemoved>(OnItemRemoved);
        EventManager.Subscribe<InventoryEvent.AllItemsRemoved>(OnAllItemsRemoved);
    }

    private void OnDisable()
    {
        if (sellAllButton != null)
            sellAllButton.onClick.RemoveListener(OnSellAllClicked);

        EventManager.Unsubscribe<InventoryEvent.ItemAdded>(OnItemAdded);
        EventManager.Unsubscribe<InventoryEvent.ItemRemoved>(OnItemRemoved);
        EventManager.Unsubscribe<InventoryEvent.AllItemsRemoved>(OnAllItemsRemoved);
    }

    private void Update()
    {
        if (IsOpen && Input.GetKeyDown(sellAllKey))
        {
            OnSellAllClicked();
        }
    }

    private void OnItemAdded(InventoryEvent.ItemAdded evt)
    {
        // Bir item eklendiðinde sayýlarý güncelle
        RefreshCounts();
    }

    private void OnItemRemoved(InventoryEvent.ItemRemoved evt)
    {
        // Bir item kaldýrýldýðýnda sayýlarý güncelle
        RefreshCounts();
    }

    private void OnAllItemsRemoved(InventoryEvent.AllItemsRemoved evt)
    {
        // Tüm envanter temizlendiðinde güncelle
        RefreshCounts();
    }

    private void OnSellAllClicked()
    {
        if (playerInventoryManager == null)
        {
            Debug.LogWarning("SellPanel: PlayerInventoryManager atanmadý.");
            return;
        }

        playerInventoryManager.RemoveAllItems();
        tradePanel.RefreshPanels();
        SoundManager.Instance.PlayButtonClick();
        // RemoveAllItems zaten event yayýnlýyorsa RefreshCounts() otomatik tetiklenir,
        RefreshCounts();
    }

    /// <summary>
    /// Inspector'dan belirlenen oreID'lere göre sayýlarý çekip UI'yý günceller.
    /// </summary>
    public void RefreshCounts()
    {
        if (playerInventoryManager == null)
        {
            // null ise UI'ý sýfýrla
            SetTextSafe(copperCountText, "0");
            SetTextSafe(silverCountText, "0");
            SetTextSafe(goldCountText, "0");
            SetTextSafe(diamondCountText, "0");
            if (sellAllButton != null) sellAllButton.interactable = false;
            return;
        }

        int copperCount = playerInventoryManager.GetOreCount(copperID);
        int silverCount = playerInventoryManager.GetOreCount(silverID);
        int goldCount = playerInventoryManager.GetOreCount(goldID);
        int diamondCount = playerInventoryManager.GetOreCount(diamondID);

        SetTextSafe(copperCountText, copperCount.ToString());
        SetTextSafe(silverCountText, silverCount.ToString());
        SetTextSafe(goldCountText, goldCount.ToString());
        SetTextSafe(diamondCountText, diamondCount.ToString());

        // Eðer envanterde hiç item yoksa Sell All butonunu pasif yap
        bool hasAny = (copperCount + silverCount + goldCount + diamondCount) > 0;
        if (sellAllButton != null) sellAllButton.interactable = hasAny;
    }

    private void SetTextSafe(TextMeshProUGUI text, string value)
    {
        if (text != null) text.text = value;
    }
}
