using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementRangeHighlight : MonoBehaviour
{
    [SerializeField]
    private Tilemap _movableRangeLayer;
    [SerializeField]
    private TileBase _movableRangeTile;

    public void ClearMovable() {
        _movableRangeLayer.ClearAllTiles();
    }

    public void PaintTileForMovable(IEnumerable<Vector2Int> tilePositions) {
        this.ClearMovable();
        foreach (Vector2Int tilePosition in tilePositions)
        {
            _movableRangeLayer.SetTile((Vector3Int)tilePosition, _movableRangeTile);
        }
    }
}
