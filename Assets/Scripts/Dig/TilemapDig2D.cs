using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapDig2D : MonoBehaviour, IDigAbility
{
    public event System.Action<Vector3Int> OnDigComplete;

    [SerializeField] public float digDuration = 2f;
    [SerializeField] private Tilemap diggableTilemap;
    [SerializeField] private TilemapDigVisualizer visualizer;
    [SerializeField] private Vector3 footOffset = new Vector3(0, -1f, 0);

    private IGroundChecker ground;
    private float digTimer;
    private bool digging;

    public bool IsDigging => digging;

    private HashSet<Vector3Int> dugTiles = new HashSet<Vector3Int>();
    private Vector3Int currentCell;

    void Awake()
    {
        ground = GetComponent<IGroundChecker>();
        if (diggableTilemap == null)
            Debug.LogError("Diggable Tilemap atanmadý!");
        if (visualizer == null)
            Debug.LogWarning("Visualizer atanmadý, kazý görselleþtirilmeyecek.");
    }

    public void TryStartDig(bool digHeld)
    {
        if (!digHeld || digging || ground == null || !ground.IsGrounded) return;

        currentCell = diggableTilemap.WorldToCell(transform.position + footOffset);

        if (!diggableTilemap.HasTile(currentCell))
        {
            Debug.Log("Bu tile kazýlamaz, boþ.");
            return;
        }

        if (dugTiles.Contains(currentCell))
        {
            Debug.Log("Bu tile daha önce kazýldý, tekrar kazýlamaz.");
            return;
        }

        digging = true;
        digTimer = digDuration;
        Debug.Log("Kazma baþladý!");
        SoundManager.Instance.StartPickaxeUpgradeLoop();
        PlayerAnimationController.Instance.SetDigging(true);
    }


    public void UpdateDig(float deltaTime, bool digHeld)
    {
        if (!digging) return;

        if (!digHeld)
        {
            digging = false;
            Debug.Log("Kazý iptal edildi, hiçbir þey bulunamadý.");
            SoundManager.Instance.StopPickaxeUpgradeLoop();
            PlayerAnimationController.Instance.SetDigging(false);
            return;
        }

        digTimer -= deltaTime;

        if (digTimer <= 0f)
        {
            digging = false;
            dugTiles.Add(currentCell);

            Debug.Log($"Kazý tamamlandý! Tile {currentCell} artýk tekrar kazýlamaz.");

            SoundManager.Instance.StopPickaxeUpgradeLoop();
            PlayerAnimationController.Instance.SetDigging(false);

            visualizer?.OnTileDug(currentCell);

            OnDigComplete?.Invoke(currentCell);
        }
    }

    private Vector3Int GetCurrentTileCell()
    {
        Vector3 footPosition = transform.position + footOffset;
        return diggableTilemap.WorldToCell(footPosition);
    }
}
