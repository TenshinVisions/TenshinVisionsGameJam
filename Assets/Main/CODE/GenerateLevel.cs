using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MobPair
{
    public GameObject mobPrefab;
    public int count;

    public MobPair(GameObject mobPrefab, int count)
    {
        this.mobPrefab = mobPrefab;
        this.count = count;
    }
}

[System.Serializable]
public class TrapPair
{
    public GameObject trapPrefab;
    public int count;

    public TrapPair(GameObject trapPrefab, int count)
    {
        this.trapPrefab = trapPrefab;
        this.count = count;
    }
}

[System.Serializable]
public class WorldData
{
    public GameObject tilePrefabsLava;
    public GameObject tilePrefabsJail;
    public GameObject tilePrefabsSewerage;
    public GameObject tilePrefabsForest;

    public RoomEnum roomType;

    public WorldData(
        GameObject tilePrefabsLava,
        GameObject tilePrefabsJail,
        GameObject tilePrefabsSewerage,
        GameObject tilePrefabsForest,
        RoomEnum roomType
    )
    {
        this.tilePrefabsLava = tilePrefabsLava;
        this.tilePrefabsJail = tilePrefabsJail;
        this.tilePrefabsSewerage = tilePrefabsSewerage;
        this.tilePrefabsForest = tilePrefabsForest;
        this.roomType = roomType;
    }
}

public class GenerateLevel : MonoBehaviour
{
    public List<WorldData> worldsData;
    public int spawnRange = 100;
    public int minGap = 20;

    private HashSet<Vector2Int> occupied = new();
    private GameObject tilePrefabs;

    private void Start()
    {
        StartCoroutine(GenerateWorlds());
    }

    private IEnumerator GenerateWorlds()
    {
        Vector2Int worldOffset = Vector2Int.zero;

        // Добавим переменные, чтобы сетка миров была 2D
        int worldIndex = 0;
        int worldsPerRow = 1; // Сколько миров в одной "строке" по X
        int worldSpacing = spawnRange * minGap * 2;

        foreach (var world in worldsData)
        {
            // Выбираем нужные префабы по типу комнаты
            switch (world.roomType)
            {
                case RoomEnum.sewerage:
                    tilePrefabs = world.tilePrefabsSewerage;
                    break;
                case RoomEnum.lava:
                    tilePrefabs = world.tilePrefabsLava;
                    break;
                case RoomEnum.jail:
                    tilePrefabs = world.tilePrefabsJail;
                    break;
                case RoomEnum.forest:
                    tilePrefabs = world.tilePrefabsForest;
                    break;
            }

            // Вычисляем отступ по X и Y (в 2D сетке миров)
            int row = worldIndex / worldsPerRow;
            int col = worldIndex % worldsPerRow;
            worldOffset = new Vector2Int(col * worldSpacing, row * worldSpacing);

            Vector2Int localPos = GetFreePosition();
            Vector2Int worldPos = localPos + worldOffset;

            Vector3 spawnPos = new Vector3(worldPos.x, worldPos.y, 0);

            GameObject tilePrefab = tilePrefabs;
            Instantiate(tilePrefab, spawnPos, Quaternion.identity);

            yield return null;

            worldIndex++; // переходим к следующему миру
        }

    }

    private Vector2Int GetFreePosition()
    {
        Vector2Int pos;
        int attempts = 0;

        do
        {
            int x = Random.Range(-spawnRange, spawnRange) * minGap;
            int y = Random.Range(-spawnRange, spawnRange) * minGap;
            pos = new Vector2Int(x, y);
            attempts++;
        }
        while ((!IsPositionFarEnough(pos) || occupied.Contains(pos)) && attempts < 100);

        occupied.Add(pos);
        return pos;
    }

    private bool IsPositionFarEnough(Vector2Int newPos)
    {
        foreach (var existingPos in occupied)
        {
            float distance = Vector2Int.Distance(newPos, existingPos);
            if (distance < minGap) return false;
        }
        return true;
    }
}
