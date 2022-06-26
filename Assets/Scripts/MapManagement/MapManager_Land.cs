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

    private static List<Vector2Int> landTiles, grassTiles, 
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

        landTiles = GetTilemapCellPositionsFrom(_landTilemap);
        grassTiles = GetTilemapCellPositionsFrom(_grassTilemap);
        forestTiles = GetTilemapCellPositionsFrom(_forestTilemap);
        rocksTiles = GetTilemapCellPositionsFrom(_rocksTilemap);
        mountainTiles = GetTilemapCellPositionsFrom(_mountainTilemap);
        waterTiles = GetTilemapCellPositionsFrom(_waterTilemap);
        seaTiles = GetTilemapCellPositionsFrom(_seaTilemap);
    }

    private static bool isFirable(Tank tank, List<Vector2Int> tilePosition, Dictionary<Vector2Int, int> stepDictionary) {
        // not implemented yet.
        return true;
    }

    private static bool isTerrainMovable(Tank tank, List<Vector2Int> tilePosition, float currentActionPoints, 
        Dictionary<Vector2Int, int> stepDictionary, Dictionary<Vector2Int, float> consumptionDictionary) {
        int maximumMovable = tank._maximumMovement;
        if (mountainTiles.Contains(tilePosition[0])) {
            if (stepDictionary.ContainsKey(tilePosition[0])) {
                if (stepDictionary[tilePosition[1]] + tank._mountainExtraCost + 1 <= maximumMovable
                 && stepDictionary[tilePosition[0]] > stepDictionary[tilePosition[1]] + tank._mountainExtraCost + 1
                 && consumptionDictionary[tilePosition[1]] + tank._mountainConsumption < currentActionPoints
                 && consumptionDictionary[tilePosition[0]] > consumptionDictionary[tilePosition[1]] + tank._waterStepConsumption) {
                    stepDictionary[tilePosition[0]] = stepDictionary[tilePosition[1]] + tank._mountainExtraCost + 1;
                    consumptionDictionary[tilePosition[0]] = consumptionDictionary[tilePosition[1]] + tank._waterStepConsumption;
                } else return false;
            } else {
                if (stepDictionary[tilePosition[1]] + tank._mountainExtraCost + 1 <= maximumMovable
                 && consumptionDictionary[tilePosition[1]] + tank._mountainConsumption < currentActionPoints){
                    stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + tank._mountainExtraCost + 1);
                    consumptionDictionary.Add(tilePosition[0], consumptionDictionary[tilePosition[1]] + tank._mountainConsumption);
                } else return false;
            }
        } else if (seaTiles.Contains(tilePosition[0])) {
            if (stepDictionary.ContainsKey(tilePosition[0])) {
                if (stepDictionary[tilePosition[1]] + tank._seaExtraCost + 1 <= maximumMovable
                 && stepDictionary[tilePosition[0]] > stepDictionary[tilePosition[1]] + tank._seaExtraCost + 1
                 && consumptionDictionary[tilePosition[1]] + tank._seaStepConsumption < currentActionPoints
                 && consumptionDictionary[tilePosition[0]] > consumptionDictionary[tilePosition[1]] + tank._seaStepConsumption) {
                    stepDictionary[tilePosition[0]] = stepDictionary[tilePosition[1]] + tank._seaExtraCost + 1;
                    consumptionDictionary[tilePosition[0]] = consumptionDictionary[tilePosition[1]] + tank._seaStepConsumption;
                } else return false;
            } else {
                if (stepDictionary[tilePosition[1]] + tank._seaExtraCost + 1 <= maximumMovable
                 && consumptionDictionary[tilePosition[1]] + tank._seaStepConsumption < currentActionPoints){
                    stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + tank._seaExtraCost + 1);
                    consumptionDictionary.Add(tilePosition[0], consumptionDictionary[tilePosition[1]] + tank._seaStepConsumption);
                } else return false;
            }
        } else if (rocksTiles.Contains(tilePosition[0])) {
            if (stepDictionary.ContainsKey(tilePosition[0])) {
                if (stepDictionary[tilePosition[1]] + tank._rockExtraCost + 1 <= maximumMovable
                 && stepDictionary[tilePosition[0]] > stepDictionary[tilePosition[1]] + tank._rockExtraCost + 1
                 && consumptionDictionary[tilePosition[1]] + tank._rockStepConsumption < currentActionPoints
                 && consumptionDictionary[tilePosition[0]] > consumptionDictionary[tilePosition[1]] + tank._rockStepConsumption) {
                    stepDictionary[tilePosition[0]] = stepDictionary[tilePosition[1]] + tank._rockExtraCost + 1;
                    consumptionDictionary[tilePosition[0]] = consumptionDictionary[tilePosition[1]] + tank._rockStepConsumption;
                } else return false;
            } else {
                if (stepDictionary[tilePosition[1]] + tank._rockExtraCost + 1 <= maximumMovable
                 && consumptionDictionary[tilePosition[1]] + tank._rockStepConsumption < currentActionPoints){
                    stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + tank._rockExtraCost + 1);
                    consumptionDictionary.Add(tilePosition[0], consumptionDictionary[tilePosition[1]] + tank._rockStepConsumption);
                } else return false;
            }
        } else if (forestTiles.Contains(tilePosition[0])) {
            if (stepDictionary.ContainsKey(tilePosition[0])) {
                if (stepDictionary[tilePosition[1]] + tank._forestExtraCost + 1 <= maximumMovable
                 && stepDictionary[tilePosition[0]] > stepDictionary[tilePosition[1]] + tank._forestExtraCost + 1
                 && consumptionDictionary[tilePosition[1]] + tank._forestStepConsumption < currentActionPoints
                 && consumptionDictionary[tilePosition[0]] > consumptionDictionary[tilePosition[1]] + tank._forestStepConsumption) {
                    stepDictionary[tilePosition[0]] = stepDictionary[tilePosition[1]] + tank._forestExtraCost + 1;
                    consumptionDictionary[tilePosition[0]] = consumptionDictionary[tilePosition[1]] + tank._forestStepConsumption;
                } else return false;
            } else {
                if (stepDictionary[tilePosition[1]] + tank._forestExtraCost + 1 <= maximumMovable
                 && consumptionDictionary[tilePosition[1]] + tank._forestStepConsumption < currentActionPoints){
                    stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + tank._forestExtraCost + 1);
                    consumptionDictionary.Add(tilePosition[0], consumptionDictionary[tilePosition[1]] + tank._forestStepConsumption);
                } else return false;
            }
        } else if (grassTiles.Contains(tilePosition[0])) {
            if (stepDictionary.ContainsKey(tilePosition[0])) {
                if (stepDictionary[tilePosition[1]] + tank._grassExtraCost + 1 <= maximumMovable
                 && stepDictionary[tilePosition[0]] > stepDictionary[tilePosition[1]] + tank._grassExtraCost + 1
                 && consumptionDictionary[tilePosition[1]] + tank._grassStepConsumption < currentActionPoints
                 && consumptionDictionary[tilePosition[0]] > consumptionDictionary[tilePosition[1]] + tank._grassStepConsumption) {
                    stepDictionary[tilePosition[0]] = stepDictionary[tilePosition[1]] + tank._grassExtraCost + 1;
                    consumptionDictionary[tilePosition[0]] = consumptionDictionary[tilePosition[1]] + tank._grassStepConsumption;
                } else return false;
            } else {
                if (stepDictionary[tilePosition[1]] + tank._grassExtraCost + 1 <= maximumMovable
                 && consumptionDictionary[tilePosition[1]] + tank._grassExtraCost < currentActionPoints){
                    stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + tank._grassExtraCost + 1);
                    consumptionDictionary.Add(tilePosition[0], consumptionDictionary[tilePosition[1]] + tank._grassStepConsumption);
                } else return false;
            }
        } else if (waterTiles.Contains(tilePosition[0])) {
            if (stepDictionary.ContainsKey(tilePosition[0])) {
                if (stepDictionary[tilePosition[1]] + tank._waterExtraCost + 1 <= maximumMovable
                 && stepDictionary[tilePosition[0]] > stepDictionary[tilePosition[1]] + tank._waterExtraCost + 1
                 && consumptionDictionary[tilePosition[1]] + tank._waterStepConsumption < currentActionPoints
                 && consumptionDictionary[tilePosition[0]] > consumptionDictionary[tilePosition[1]] + tank._waterStepConsumption) {
                    stepDictionary[tilePosition[0]] = stepDictionary[tilePosition[1]] + tank._waterExtraCost + 1;
                    consumptionDictionary[tilePosition[0]] = consumptionDictionary[tilePosition[1]] + tank._waterStepConsumption;
                } else return false;
            } else {
                if (stepDictionary[tilePosition[1]] + tank._waterExtraCost + 1 <= maximumMovable
                 && consumptionDictionary[tilePosition[1]] + tank._waterStepConsumption < currentActionPoints){
                    stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + tank._waterExtraCost + 1);
                    consumptionDictionary.Add(tilePosition[0], consumptionDictionary[tilePosition[1]] + tank._waterStepConsumption);
                } else return false;
            }
        } else {
            if (stepDictionary.ContainsKey(tilePosition[0])) {
                if (stepDictionary[tilePosition[1]] + tank._landExtraCost + 1 <= maximumMovable
                 && stepDictionary[tilePosition[0]] > stepDictionary[tilePosition[1]] + tank._landExtraCost + 1
                 && consumptionDictionary[tilePosition[1]] + tank._landStepConsumption < currentActionPoints
                 && consumptionDictionary[tilePosition[0]] > consumptionDictionary[tilePosition[1]] + tank._landStepConsumption) {
                    stepDictionary[tilePosition[0]] = stepDictionary[tilePosition[1]] + tank._landExtraCost + 1;
                    consumptionDictionary[tilePosition[0]] = consumptionDictionary[tilePosition[1]] + tank._landStepConsumption;
                } else return false;
            } else {
                if (stepDictionary[tilePosition[1]] + tank._landExtraCost + 1 <= maximumMovable
                 && consumptionDictionary[tilePosition[1]] + tank._landStepConsumption < currentActionPoints){
                    stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + tank._landExtraCost + 1);
                    consumptionDictionary.Add(tilePosition[0], consumptionDictionary[tilePosition[1]] + tank._landStepConsumption);
                } else return false;
            }
        }
        return true;
    }

    public List<Vector2Int> GetTilemapCellPositionsFrom(Tilemap currentMap) {
        List<Vector2Int> res = new List<Vector2Int>();
        foreach(Vector2Int cellPos in currentMap.cellBounds.allPositionsWithin) {
            if (!currentMap.HasTile((Vector3Int)cellPos)) continue;
            res.Add(cellPos);
        }
        return res;
    }

    
    public List<List<Vector2Int>> GetFirePowerRange(Tank tank, Vector3Int currentPosition) {
        return MapManager_Land.BFS(tank, (Vector2Int)currentPosition, -1f);
    }

    public List<List<Vector2Int>> GetMovementRange(Tank tank, Vector3Int currentPosition, float currentActionPoints) {
        return MapManager_Land.BFS(tank, (Vector2Int)currentPosition, currentActionPoints);
    }

    public static List<List<Vector2Int>> BFS(Tank tank, Vector2Int startCell, float currentActionPoints) {
        Queue<List<Vector2Int>> tilesToBeVisited = new Queue<List<Vector2Int>>();
        Dictionary<Vector2Int, int> stepDictionary = new Dictionary<Vector2Int, int>();
        Dictionary<Vector2Int, float> consumptionDictionary = new Dictionary<Vector2Int, float>();
        List<List<Vector2Int>> visitedTiles = new List<List<Vector2Int>>();
        List<Vector2Int> startNode = new List<Vector2Int>{startCell, startCell};

        tilesToBeVisited.Enqueue(startNode);
        stepDictionary.Add(startNode[0], 0);
        consumptionDictionary.Add(startNode[0], 0);
        visitedTiles.Add(startNode);

        while (tilesToBeVisited.Count > 0) {
            List<Vector2Int> currentTile = tilesToBeVisited.Dequeue();
            if (startNode[0] != startCell
                && Physics2D.OverlapPoint(
                    _tilemap.GetCellCenterWorld((Vector3Int)currentTile[0]), _targetLayerMask) != null) continue;
            if (!visitedTiles.Contains(currentTile)) visitedTiles.Add(currentTile);
            foreach (List<Vector2Int> neighbourPosition in GetNeighboursFor(currentTile[0])) {
                if (!visitedTiles.Contains(neighbourPosition)) {
                    if (currentActionPoints == -1f) {
                        if (isFirable(tank, neighbourPosition, stepDictionary))
                            tilesToBeVisited.Enqueue(neighbourPosition);
                    } else {
                        if (isTerrainMovable(tank, neighbourPosition, currentActionPoints, stepDictionary, consumptionDictionary))
                            tilesToBeVisited.Enqueue(neighbourPosition);
                    }
                }
            }
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
