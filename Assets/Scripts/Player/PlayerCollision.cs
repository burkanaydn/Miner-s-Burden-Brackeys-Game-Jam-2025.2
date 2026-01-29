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

        if (((1 << obj.layer) & hazardLayer) != 0)
        {
            EventManager.Publish(new PlayerDeathEvent());
        }
    }
}

public class PlayerDeathEvent
{
    // ek bilgiler için.
}
