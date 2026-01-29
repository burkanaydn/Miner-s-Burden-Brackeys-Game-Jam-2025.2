using UnityEngine;
using UnityEngine.EventSystems;

public class ClearSelectedUI : MonoBehaviour
{
    [Header("Mouse Only UI")]
    [Tooltip("Eðer true ise, EventSystem selectedGameObject mouse ile seçilmiþ olsa bile her frame temizlenir.")]
    public bool mouseOnlyUI = true;

    void Update()
    {
        if (!mouseOnlyUI) return;

        // EventSystem mevcut mu kontrol et
        if (EventSystem.current == null) return;

        // Eðer selectedGameObject doluysa, null yap
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
