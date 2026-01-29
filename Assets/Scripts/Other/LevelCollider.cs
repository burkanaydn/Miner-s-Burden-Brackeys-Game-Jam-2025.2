using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelCollider : MonoBehaviour
{
    [SerializeField] private Transform startCheckpoint;
    [SerializeField] private Transform endCheckpoint;
    [SerializeField] private int level;

    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        EventManager.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            LevelManager.Instance.CurrentLevel = level;
        }
    }

    void OnDestroy()
    {
        EventManager.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnPlayerDeath(PlayerDeathEvent e)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.size, 0f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Transform targetCheckpoint = (hit.transform.position.x < col.bounds.center.x)
                    ? startCheckpoint
                    : endCheckpoint;

                hit.transform.position = targetCheckpoint.position;
                break;
            }
        }
    }
}
