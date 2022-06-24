using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

// Only For Land Unit!!!
public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap _landTilemap, _grassTilemap, 
        _forestTilemap, _rocksTilemap, _mountainTilemap, _waterTilemap, _seaTilemap;

    private List<Vector2Int> landTiles, grassTiles, 
        forestTiles, rocksTiles, mountainTiles, waterTiles, seaTiles;

    public readonly static List<Vector2Int> dirs = 
        new List<Vector2Int> {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(0,1),
            new Vector2Int(-1,-1),
            new Vector2Int(1,1)
        };

    // void Awake()
    // {
    //     landTiles = GetTilemapCellPositionsFrom(_landTilemap);
    //     grassTiles = GetTilemapCellPositionsFrom(_grassTilemap);
    //     forestTiles = GetTilemapCellPositionsFrom(_forestTilemap);
    //     rocksTiles = GetTilemapCellPositionsFrom(_rocksTilemap);
    //     mountainTiles = GetTilemapCellPositionsFrom(_mountainTilemap);
    //     waterTiles = GetTilemapCellPositionsFrom(_waterTilemap);
    //     seaTiles = GetTilemapCellPositionsFrom(_seaTilemap);
    // }

    // public bool isMovable(Vector2 tilePosition) {
    //     Vector2Int tilePosition = Vector2Int.FloorToInt(tilePosition);
    //     return landTiles.Contains(tilePosition) || grassTiles.Contains(tilePosition)
    //         || forestTiles.Contains(tilePosition) || rocksTiles.Contains(tilePosition) || waterTiles.Contains(tilePosition);
    // }

    // public List<Vector2Int> GetTilemapCellPositionsFrom(tilemap tilemap) {
    //     List<Vector2Int> res = new List<Vector2Int>();
    //     foreach(Vector2Int cellPos in tilemap.cellBounds.allPositionWithin) {
    //         if (tilemap.HasTile((Vector3Int)cellPos) == null) continue;
    //         res.add((Vector2Int)GetCellPosition(cellPos));
    //     }
    //     return res;
    // }

    // public Vector3Int GetCellPosition(Vector2Int) {

    // }

    public List<Vector2Int> GetMovementRange(Vector3Int currentPosition, int maximumMovement, float stepConsumption, float currentActionPoints) {
        return MapManager.BFS((Vector2Int)currentPosition, maximumMovement, stepConsumption, currentActionPoints);
    }

    public static List<Vector2Int> BFS(Vector2Int startCell, int maximumMovement, 
        float stepConsumption, float currentActionPoints) // used for Action Points calculation 
    {
        Queue<Vector2Int> tilesToBeVisited = new Queue<Vector2Int>();
        List<Vector2Int> visitedTiles = new List<Vector2Int>();

        tilesToBeVisited.Enqueue(startCell);
        visitedTiles.Add(startCell);

        while (tilesToBeVisited.Count > 0)
        {
            Vector2Int currentTile = tilesToBeVisited.Dequeue();
            int distance = Mathf.Abs(currentTile.x - startCell.x) + Mathf.Abs(currentTile.y - startCell.y);
            if (distance > maximumMovement) continue;
            foreach (Vector2Int neighbourPosition in GetNeighboursFor(currentTile)) {
                if (!visitedTiles.Contains(neighbourPosition)) {
                    if (!visitedTiles.Contains(currentTile))
                        visitedTiles.Add(currentTile);
                    tilesToBeVisited.Enqueue(neighbourPosition);
                }
            }
        }

        return visitedTiles;
    }

    public static List<Vector2Int> GetNeighboursFor(Vector2Int tileCellPosition)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        foreach (Vector2Int dir in dirs)
        {
            Vector2Int newPosition = tileCellPosition + dir;
            if (newPosition.x >= -18 && newPosition.x <= 23 
                && newPosition.y >= -10 && newPosition.y <= 12)
                positions.Add(newPosition);
        }
        return positions;
    }
}
