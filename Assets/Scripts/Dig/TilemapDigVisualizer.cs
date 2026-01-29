using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDigVisualizer : MonoBehaviour, IDigVisualizer
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase dugTile; // kazýlmýþ tile sprite

    public void OnTileDug(Vector3Int cell)
    {
        if (tilemap == null || dugTile == null) return;

        tilemap.SetTile(cell, dugTile);
    }
}
