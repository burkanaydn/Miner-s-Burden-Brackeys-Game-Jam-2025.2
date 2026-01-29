using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiningResultPanel : UIPanel
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI oreNameText;
    [SerializeField] private TextMeshProUGUI oreWeightText;
    [SerializeField] private TextMeshProUGUI orePriceText;
    [SerializeField] private Image oreIconImage;

    [Header("Buttons")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private KeyCode acceptKey = KeyCode.C;
    [SerializeField] private KeyCode rejecttKey = KeyCode.D;

    private string currentOreID; 
    private OreData currentOreData; 

    private void OnEnable()
    {
        acceptButton.onClick.AddListener(AcceptOre);
        rejectButton.onClick.AddListener(RejectOre);

        EventManager.Subscribe<MiningResultEvent>(OnMiningResult);
    }

    private void OnDisable()
    {
        acceptButton.onClick.RemoveListener(AcceptOre);
        rejectButton.onClick.RemoveListener(RejectOre);

        EventManager.Unsubscribe<MiningResultEvent>(OnMiningResult);
    }

    private void Update()
    {
        if (!resultPanel.activeSelf) return; 

        if (Input.GetKeyDown(acceptKey))
        {
            AcceptOre();
        }
        else if (Input.GetKeyDown(rejecttKey))
        {
            RejectOre();
        }
    }

    private void OnMiningResult(MiningResultEvent result)
    {
        currentOreID = result.oreID;
        currentOreData = OreDatabase.Instance.GetOreByID(currentOreID);

        if (currentOreData == null)
        {
            Debug.LogWarning($"OreID {currentOreID} bulunamadý!");
            return;
        }

        // UI güncelle
        oreNameText.text = currentOreData.oreName;
        oreWeightText.text = $"Weight: {currentOreData.weight}kg";
        orePriceText.text = $"Price: {currentOreData.price}$";
        oreIconImage.sprite = currentOreData.icon;

        Open(resultPanel);
        StartCoroutine(PlayPanelAndAttention(resultPanel));
        SoundManager.Instance.PlayItemCollect();
    }

    private IEnumerator PlayPanelAndAttention(GameObject panel)
    {
        DoTweenUIManager.Instance.PlayPanelAnimation(panel);

        yield return new WaitForSeconds(1f);

        DoTweenUIManager.Instance.PlayAttentionLoop(panel);
    }

    private void AcceptOre()
    {
        if (currentOreData != null)
        {
            EventManager.Publish(new MiningResultEvent.OreAcceptedEvent(currentOreID));
        }
        Close(resultPanel);
    }

    private void RejectOre()
    {
        if (currentOreData != null)
        {
            EventManager.Publish(new MiningResultEvent.OreRejectedEvent(currentOreID));
            SoundManager.Instance.PlayItemReject();
        }
        Close(resultPanel);
    }
}
