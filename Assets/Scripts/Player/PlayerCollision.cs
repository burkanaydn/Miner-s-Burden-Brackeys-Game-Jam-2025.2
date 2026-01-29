using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Hazard Layer")]
    [SerializeField] private LayerMask hazardLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckHazard(collision.gameObject);
    }

    private void CheckHazard(GameObject obj)
    {
        // obj LayerMask ile Hazard layer'ýna dahil mi diye kontrol et
        if (((1 << obj.layer) & hazardLayer) != 0)
        {
            // PlayerDeath event yayýnla
            EventManager.Publish(new PlayerDeathEvent());
        }
    }
}

public class PlayerDeathEvent
{
    // Ýhtiyaç varsa burada ek bilgiler tutulabilir
}
