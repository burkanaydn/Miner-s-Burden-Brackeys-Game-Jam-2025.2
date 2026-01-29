using UnityEngine;
using DG.Tweening;

public class DoTweenUIManager : MonoBehaviour
{
    public static DoTweenUIManager Instance { get; private set; }

    [Header("Animasyon Ayarlarý")]
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 20f;
    [SerializeField] private int shakeVibrato = 10;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Verilen panel GameObject’ine scale + shake animasyonu uygular.
    /// </summary>
    public void PlayPanelAnimation(GameObject panel)
    {
        if (panel == null) return;

        RemoveEffects(panel.transform);

        RectTransform rect = panel.GetComponent<RectTransform>();
        if (rect == null) return;

        // Önce scale 0’dan 1’e
        rect.localScale = Vector3.zero;
        rect.DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutBack);

        // Sonrasýnda shake
        rect.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, 90, false, true);
    }

    public void PlayAttentionLoop(GameObject panel)
    {
        if (panel == null) return;

        RemoveEffects(panel.transform);

        Transform target = panel.transform;

        // Önce scale animasyonu
        target.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.5f)
              .SetEase(Ease.InOutSine)
              .SetLoops(-1, LoopType.Incremental);

        // Ayný anda rotation animasyonu
        Sequence seq = DOTween.Sequence();

        // Sað tarafa dön
        seq.Append(target.DORotate(new Vector3(0, 0, 5), 0.5f).SetEase(Ease.InOutSine));
        // Sol tarafa dön
        seq.Append(target.DORotate(new Vector3(0, 0, -5), 0.5f).SetEase(Ease.InOutSine));
        // Sonsuz tekrar
        seq.SetLoops(-1, LoopType.Yoyo);
    }

    public void PlayAttentionLoopPop(GameObject panel)
    {
        if (panel == null) return;

        RemoveEffects(panel.transform);

        Transform target = panel.transform;

        // Önce scale animasyonu
        target.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        // Ayný anda rotation animasyonu
        Sequence seq = DOTween.Sequence();

        // Sað tarafa dön
        seq.Append(target.DORotate(new Vector3(0, 0, 5), 0.5f).SetEase(Ease.InOutSine));
        // Sol tarafa dön
        seq.Append(target.DORotate(new Vector3(0, 0, -5), 0.5f).SetEase(Ease.InOutSine));
        // Sonsuz tekrar
        seq.SetLoops(-1, LoopType.Yoyo);
    }

    public void RemoveEffects(Transform target)
    {
        // Hedefte çalýþan tüm tween'leri öldür
        target.DOKill();

        // Objeyi varsayýlan haline döndürmek istersen scale ve rotation'u sýfýrla
        target.localScale = Vector3.one;
        target.localRotation = Quaternion.identity;
    }
}
