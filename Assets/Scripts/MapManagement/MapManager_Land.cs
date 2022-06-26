using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager_Land : MonoBehaviour
{
    [SerializeField]
    private Tilemap _landTilemap, _grassTilemap, 
        _forestTilemap, _rocksTilemap, _mountainTilemap, _waterTilemap, _seaTilemap;
    [SerializeField]
	private LayerMask _layerMask;

    public static Tilemap _tilemap;
	public static LayerMask _targetLayerMask;

    private List<Vector2Int> landTiles, grassTiles, 
        forestTiles, rocksTiles, mountainTiles, waterTiles, seaTiles;

    public readonly static List<Vector2Int> oddYDirs = 
        new List<Vector2Int> {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(0,1),
            new Vector2Int(1,1),
            new Vector2Int(1,-1)
        };
    public readonly static List<Vector2Int> evenYDirs = 
        new List<Vector2Int> {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(0,1),
            new Vector2Int(-1,1),
            new Vector2Int(-1,-1)
        };

    void Awake()
    {
        _tilemap = this._landTilemap;
        _targetLayerMask = this._layerMask;

        // landTiles = GetTilemapCellPositionsFrom(_landTilemap);
        // grassTiles = GetTilemapCellPositionsFrom(_grassTilemap);
        // forestTiles = GetTilemapCellPositionsFrom(_forestTilemap);
        // rocksTiles = GetTilemapCellPositionsFrom(_rocksTilemap);
        // mountainTiles = GetTilemapCellPositionsFrom(_mountainTilemap);
        // waterTiles = GetTilemapCellPositionsFrom(_waterTilemap);
        // seaTiles = GetTilemapCellPositionsFrom(_seaTilemap);
    }

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

    public List<List<Vector2Int>> GetMovementRange(Vector3Int currentPosition, int maximumMovement, float stepConsumption, float currentActionPoints) {
        return MapManager_Land.BFS((Vector2Int)currentPosition, maximumMovement, stepConsumption, currentActionPoints);
    }

    public static List<List<Vector2Int>> BFS(Vector2Int startCell, int maximumMovement, 
        float stepConsumption, float currentActionPoints) // used for Action Points calculation 
    {
        Queue<List<Vector2Int>> tilesToBeVisited = new Queue<List<Vector2Int>>();
        List<List<Vector2Int>> visitedTiles = new List<List<Vector2Int>>();
        List<Vector2Int> startNode = new List<Vector2Int>{startCell, startCell};

        tilesToBeVisited.Enqueue(startNode);
        visitedTiles.Add(startNode);

        int steps = 0;
        while (tilesToBeVisited.Count > 0) {
            Queue<List<Vector2Int>> tilesToBeVisited_Next_Round = new Queue<List<Vector2Int>>();
            while (tilesToBeVisited.Count > 0) {
                List<Vector2Int> currentTile = tilesToBeVisited.Dequeue();
                if (startNode[0] != startCell
                    && Physics2D.OverlapPoint(
                        _tilemap.GetCellCenterWorld((Vector3Int)currentTile[0]), _targetLayerMask) != null) continue;
                if (!visitedTiles.Contains(currentTile)) visitedTiles.Add(currentTile);
                foreach (List<Vector2Int> neighbourPosition in GetNeighboursFor(currentTile[0])) {
                    if (!visitedTiles.Contains(neighbourPosition)
                        && !tilesToBeVisited.Contains(neighbourPosition)
                        && !tilesToBeVisited_Next_Round.Contains(neighbourPosition)) {
                        tilesToBeVisited_Next_Round.Enqueue(neighbourPosition);
                    }
                }
            }
            if (steps++ == maximumMovement) break;
            if (tilesToBeVisited_Next_Round.Count > 0) tilesToBeVisited = tilesToBeVisited_Next_Round;
        }
        return visitedTiles;
    }

    public static List<List<Vector2Int>> GetNeighboursFor(Vector2Int tileCellPosition)
    {
        List<List<Vector2Int>> neighbours = new List<List<Vector2Int>>();
        List<Vector2Int> dirs = tileCellPosition.y % 2 == 0 ? evenYDirs : oddYDirs;
        foreach (Vector2Int dir in dirs) {
            Vector2Int newPosition = tileCellPosition + dir;
            List<Vector2Int> thisTile = new List<Vector2Int>(){newPosition, tileCellPosition};
            if (newPosition.x >= -18 && newPosition.x <= 23 
                && newPosition.y >= -10 && newPosition.y <= 12)
                neighbours.Add(thisTile);
        }
        return neighbours;
    }
}
