using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderCollision : MonoBehaviour
{
    [SerializeField] private TradePanel tradePanel;
    private string playerTag = "Player"; // Player objesinin tag'i

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (tradePanel != null)
                tradePanel.OpenPanel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (tradePanel != null)
                tradePanel.ClosePanel();
        }
    }
}
