using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class FirableRangeHighlight : MonoBehaviour
{
    [SerializeField]
    private Tilemap _firableRangeLayer;
    [SerializeField]
    private TileBase _firableRangeTile;

    public void ClearFirable() {
        _firableRangeLayer.ClearAllTiles();
    }

    public void PaintTileForFirable(IEnumerable<List<Vector2Int>> tilePositions) {
        this.ClearFirable();
        foreach (List<Vector2Int> tilePosition in tilePositions)
        {
            _firableRangeLayer.SetTile((Vector3Int)tilePosition[0], _firableRangeTile);
        }
    }
}
