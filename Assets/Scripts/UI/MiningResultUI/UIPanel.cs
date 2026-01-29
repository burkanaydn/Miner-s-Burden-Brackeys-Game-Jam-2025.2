using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public virtual void Open(GameObject resultPanel) => resultPanel.SetActive(true);
    public virtual void Close(GameObject resultPanel) => resultPanel.SetActive(false);
}
