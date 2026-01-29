using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayPanel : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventoryManager playerInventoryManager;

    [Header("Ore IDs (set these to match your OreDatabase oreID strings)")]
    [SerializeField] private string copperID = "ore_copper";
    [SerializeField] private string silverID = "ore_silver";
    [SerializeField] private string goldID = "ore_gold";
    [SerializeField] private string diamondID = "ore_diamond";

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI copperCountText;
    [SerializeField] private TextMeshProUGUI silverCountText;
    [SerializeField] private TextMeshProUGUI goldCountText;
    [SerializeField] private TextMeshProUGUI diamondCountText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        if (playerInventoryManager == null)
            playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();

        if (playerInventoryManager == null)
            Debug.LogWarning("GameplayPanel: PlayerInventoryManager bulunamadý. Lütfen Inspector'dan atayýn.");
    }

    private void Start()
    {
        // Baþlangýçta UI'yý güncelle
        RefreshCounts();
        UpdateMoneyText();
    }

    private void OnEnable()
    {
        // Inventory eventlerini dinle
        EventManager.Subscribe<InventoryEvent.ItemAdded>(OnItemAdded);
        EventManager.Subscribe<InventoryEvent.ItemRemoved>(OnItemRemoved);
        EventManager.Subscribe<InventoryEvent.AllItemsRemoved>(OnAllItemsRemoved);

        // Oyuncu accept ettiðinde (ve PlayerBagManager bu ore'ü envantere ekleyecek),
        // envanterin güncellenmiþ halini görebilmek için bir frame sonra yenileme yapýyoruz.
        EventManager.Subscribe<MiningResultEvent.OreAcceptedEvent>(OnOreAccepted);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartScene);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<InventoryEvent.ItemAdded>(OnItemAdded);
        EventManager.Unsubscribe<InventoryEvent.ItemRemoved>(OnItemRemoved);
        EventManager.Unsubscribe<InventoryEvent.AllItemsRemoved>(OnAllItemsRemoved);

        EventManager.Unsubscribe<MiningResultEvent.OreAcceptedEvent>(OnOreAccepted);

        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartScene);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    #region Event handlers
    private void OnItemAdded(InventoryEvent.ItemAdded evt)
    {
        // Envantere item eklendi => sayýlarý yenile (para deðiþmedi)
        RefreshCounts();
    }

    private void OnItemRemoved(InventoryEvent.ItemRemoved evt)
    {
        // Bir item satýldý/çýkarýldý => hem sayýlarý hem parayý güncelle
        RefreshCounts();
        UpdateMoneyText();
    }

    private void OnAllItemsRemoved(InventoryEvent.AllItemsRemoved evt)
    {
        RefreshCounts();
        UpdateMoneyText();
    }

    private void OnOreAccepted(MiningResultEvent.OreAcceptedEvent evt)
    {
        // Accept event'i geldiðinde PlayerBagManager muhtemelen ayný event'i iþleyip envantere ekleyecek.
        // Hangi subscriber önce çaðýrýlacaðý kesin olmadýðý için bir frame bekleyip sonra yenileme yapýyoruz.
        StartCoroutine(DelayedRefreshNextFrame());
    }
    #endregion

    private IEnumerator DelayedRefreshNextFrame()
    {
        yield return null; // bir frame bekle
        RefreshCounts();
    }

    /// <summary>
    /// copper/silver/gold count'larýný PlayerInventoryManager'dan çekip UI'yý günceller.
    /// Eðer playerInventoryManager atanmamýþsa sayýlarý 0 gösterir.
    /// </summary>
    public void RefreshCounts()
    {
        if (playerInventoryManager == null)
        {
            SetTextSafe(copperCountText, "0");
            SetTextSafe(silverCountText, "0");
            SetTextSafe(goldCountText, "0");
            SetTextSafe(diamondCountText, "0");
            return;
        }

        int copper = playerInventoryManager.GetOreCount(copperID);
        int silver = playerInventoryManager.GetOreCount(silverID);
        int gold = playerInventoryManager.GetOreCount(goldID);
        int diamond = playerInventoryManager.GetOreCount(diamondID);

        SetTextSafe(copperCountText, copper.ToString());
        SetTextSafe(silverCountText, silver.ToString());
        SetTextSafe(goldCountText, gold.ToString());
        SetTextSafe(diamondCountText, diamond.ToString());
    }

    /// <summary>
    /// EconomyManager.Instance.CurrentMoney kullanarak para metnini günceller.
    /// EconomyManager yoksa sessizce geri döner.
    /// </summary>
    public void UpdateMoneyText()
    {
        if (moneyText == null) return;
        if (EconomyManager.Instance == null)
        {
            moneyText.text = "0";
            return;
        }

        // Daha okunaklý bir gösterim istersen ToString("N0") kullanabilirsin
        moneyText.text = EconomyManager.Instance.CurrentMoney.ToString();
    }

    private void SetTextSafe(TextMeshProUGUI text, string val)
    {
        if (text != null) text.text = val;
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
