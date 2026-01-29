using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameOnTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "GameScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
