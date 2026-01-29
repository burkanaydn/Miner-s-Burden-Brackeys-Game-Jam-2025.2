using System.Collections;
using TMPro;
using UnityEngine;

public class WarningPanel : MonoBehaviour
{
    public static WarningPanel Instance;

    [SerializeField] private RectTransform panel;        // UI panel
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float moveDuration = 1f;    // panelin yukarý kayma süresi
    [SerializeField] private float moveDistance = 150f;  // yukarý hareket mesafesi

    private Vector2 startPos;
    private Vector2 endPos;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (panel != null)
            startPos = panel.anchoredPosition;
    }

    /// <summary>
    /// Uyarý panelini göster
    /// </summary>
    public void ShowWarning(string message)
    {
        if (panel == null || warningText == null) return;

        StopAllCoroutines();
        warningText.text = message;

        startPos = panel.anchoredPosition;
        endPos = startPos + Vector2.up * moveDistance;
        panel.gameObject.SetActive(true);

        StartCoroutine(MoveAndHide());
    }

    private IEnumerator MoveAndHide()
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / moveDuration;
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        panel.gameObject.SetActive(false);
        panel.anchoredPosition = startPos;
    }
}
