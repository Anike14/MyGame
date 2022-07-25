using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager_Land : MonoBehaviour
{
    [SerializeField]
    private Tilemap _landTilemap, _grassTilemap, 
        _forestTilemap, _rocksTilemap, _mountainTilemap, _waterTilemap, _seaTilemap;

    public static Tilemap _tilemap;

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

        landTiles = GetTilemapCellPositionsFrom(_landTilemap);
        grassTiles = GetTilemapCellPositionsFrom(_grassTilemap);
        forestTiles = GetTilemapCellPositionsFrom(_forestTilemap);
        rocksTiles = GetTilemapCellPositionsFrom(_rocksTilemap);
        mountainTiles = GetTilemapCellPositionsFrom(_mountainTilemap);
        waterTiles = GetTilemapCellPositionsFrom(_waterTilemap);
        seaTiles = GetTilemapCellPositionsFrom(_seaTilemap);
    }

    private static bool IsFirable(Tank tank, List<Vector2Int> tilePosition, LayerMask myLayerMask, Dictionary<Vector2Int, int> stepDictionary) {
        if (Physics2D.OverlapPoint(_tilemap.GetCellCenterWorld((Vector3Int)tilePosition[0]), myLayerMask) != null) return false;
        if (mountainTiles.Contains(tilePosition[0])) {
            return false;
        } else if (seaTiles.Contains(tilePosition[0])) {
            return false;
        } else if (stepDictionary[tilePosition[1]] + 1 > tank._fireRange) {
            return false;
        } else if (stepDictionary.ContainsKey(tilePosition[0])) {
            return false;
        }

        stepDictionary.Add(tilePosition[0], stepDictionary[tilePosition[1]] + 1);
        return true;
    }

    private static bool IsTerrainMovable(Tank tank, List<Vector2Int> tilePosition, LayerMask myLayerMask, LayerMask enemyLayerMask,
        float currentActionPoints, Dictionary<Vector2Int, int> stepDictionary, Dictionary<Vector2Int, float> consumptionDictionary) {
        if (Physics2D.OverlapPoint(_tilemap.GetCellCenterWorld((Vector3Int)tilePosition[0]), myLayerMask) != null) return false;
        Collider2D enemyCollider = Physics2D.OverlapPoint(_tilemap.GetCellCenterWorld((Vector3Int)tilePosition[0]), enemyLayerMask);
        if (enemyCollider != null) {
            Tank enemyTank = enemyCollider.gameObject.GetComponentInChildren<Tank>();
            if (enemyTank.IsScouted()) return false;
        }
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

    public static bool ScoutFromMyPosition(Tank tank, Vector2Int startCell, LayerMask enemyLayerMask) {
        bool res = false;
        // Another BFS
        Queue<List<Vector2Int>> tilesToBeVisited = new Queue<List<Vector2Int>>();
        Dictionary<Vector2Int, int> stepDictionary = new Dictionary<Vector2Int, int>();
        List<List<Vector2Int>> visitedTiles = new List<List<Vector2Int>>();
        List<Vector2Int> startNode = new List<Vector2Int>{startCell, startCell};

        tilesToBeVisited.Enqueue(startNode);
        stepDictionary.Add(startNode[0], 0);
        visitedTiles.Add(startNode);
        while (tilesToBeVisited.Count > 0) {
            List<Vector2Int> currentTile = tilesToBeVisited.Dequeue();
            if (!visitedTiles.Contains(currentTile)) visitedTiles.Add(currentTile);
            Collider2D enemyCollider = Physics2D.OverlapPoint(_tilemap.GetCellCenterWorld((Vector3Int)currentTile[0]), enemyLayerMask);
            if (enemyCollider != null) {
                Tank enemyTank = enemyCollider.gameObject.GetComponentInChildren<Tank>();
                if (stepDictionary[currentTile[0]] <= 3) {
                    if (!enemyTank.IsScouted()) {
                        res = true;
                        tank.PlayEngageEffect();
                    }
                    enemyTank.Scouted();
                } else {
                    float randomNum = Random.Range(1, 100);
                    float extraConcealment = 1f;
                    if (enemyTank.IsHolding()) extraConcealment = 0.5f;
                    else if (enemyTank.IsHiding()) extraConcealment = 1.1f;
                    if (randomNum > enemyTank._stealth[9 - stepDictionary[currentTile[0]]] * extraConcealment) {
                        if (!enemyTank.IsScouted()) {
                            res = true;
                            tank.PlayEngageEffect();
                        }
                        enemyTank.Scouted();
                    }
                }
            }
            int maximumViewRange = tank._viewRange + (tank.IsScouting() ? 1 : 0);
            foreach (List<Vector2Int> neighbourPosition in GetNeighboursFor(currentTile[0])) {
                if (!visitedTiles.Contains(neighbourPosition) && !tilesToBeVisited.Contains(neighbourPosition)) {
                    if (stepDictionary[neighbourPosition[1]] + 1 <= maximumViewRange) {
                        if (stepDictionary.ContainsKey(neighbourPosition[0])) {
                            if (stepDictionary[neighbourPosition[0]] > stepDictionary[neighbourPosition[1]] + 1) {
                                stepDictionary[neighbourPosition[0]] = stepDictionary[neighbourPosition[1]] + 1;
                                tilesToBeVisited.Enqueue(neighbourPosition);
                            }
                        } else {
                            stepDictionary.Add(neighbourPosition[0], stepDictionary[neighbourPosition[1]] + 1);
                            tilesToBeVisited.Enqueue(neighbourPosition);
                        }
                    }
                }
            }
        }
        return res;
    }
    
    
    public static List<List<Vector2Int>> GetFirePowerRange(Tank tank, Vector3Int currentPosition, LayerMask myLayerMask, LayerMask enemyLayerMask) {
        return MapManager_Land.BFS(tank, (Vector2Int)currentPosition, -1f, myLayerMask, enemyLayerMask);
    }

    public static List<List<Vector2Int>> GetMovementRange(Tank tank, Vector3Int currentPosition, float currentActionPoints, LayerMask myLayerMask, LayerMask enemyLayerMask) {
        return MapManager_Land.BFS(tank, (Vector2Int)currentPosition, currentActionPoints, myLayerMask, enemyLayerMask);
    }

    public static List<List<Vector2Int>> BFS(Tank tank, Vector2Int startCell, float currentActionPoints, LayerMask myLayerMask, LayerMask enemyLayerMask) {
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
            if (!visitedTiles.Contains(currentTile)) visitedTiles.Add(currentTile);
            foreach (List<Vector2Int> neighbourPosition in GetNeighboursFor(currentTile[0])) {
                if (!visitedTiles.Contains(neighbourPosition)) {
                    if (currentActionPoints == -1f) { // in firing we don't consider the action points
                        if (IsFirable(tank, neighbourPosition, myLayerMask, stepDictionary))
                            tilesToBeVisited.Enqueue(neighbourPosition);} else {
                        if (IsTerrainMovable(tank, neighbourPosition, myLayerMask, enemyLayerMask, 
                            currentActionPoints, stepDictionary, consumptionDictionary))
                            tilesToBeVisited.Enqueue(neighbourPosition);
                    }
                }
            }
        }
        visitedTiles.Remove(startNode);
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
