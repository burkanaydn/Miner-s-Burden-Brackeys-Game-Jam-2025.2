using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradePanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private UpgradePanel upgradePanel;
    [SerializeField] private SellPanel sellPanel;
    public void OpenPanel()
    {
        if (panelRoot != null) panelRoot.SetActive(true);
        upgradePanel.OpenPanel();
        DoTweenUIManager.Instance.PlayPanelAnimation(panelRoot);
        sellPanel.RefreshCounts();
        sellPanel.IsOpen = true;
    }

    public void ClosePanel()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
        upgradePanel.ClosePanel();
        sellPanel.IsOpen = false;
    }

    public void RefreshPanels()
    {
        sellPanel.RefreshCounts();
        upgradePanel.OpenPanel();
    }
}
