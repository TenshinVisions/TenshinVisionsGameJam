using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BossData
{
    public GameObject bossPrefab;  // Префаб босса

    public float damage;           // Урон
    public float speed;            // Скорость
    public float health;           // Здоровье

    public BossData(GameObject bossPrefab, float damage, float speed, float health)
    {
        this.bossPrefab = bossPrefab;
        this.damage = damage;
        this.speed = speed;
        this.health = health;
    }
}

[System.Serializable]
public class TrapData
{
    public GameObject trapPrefab;
    public int count;
    public float damage;

    public TrapData(GameObject trapPrefab, int count, float damage)
    {
        this.trapPrefab = trapPrefab;
        this.count = count;
        this.damage = damage;
    }
}

[System.Serializable]
public class MobData
{
    public GameObject mobPrefab;  // Префаб моба
    public int count;             // Количество мобов этого типа

    public float damage;          // Урон
    public float speed;           // Скорость
    public float health;          // Здоровье

    public MobData(GameObject mobPrefab, int count, float damage, float speed, float health)
    {
        this.mobPrefab = mobPrefab;
        this.count = count;
        this.damage = damage;
        this.speed = speed;
        this.health = health;
    }
}

[System.Serializable]
public class WorldData
{
    public int width;
    public int height;
    public List<MobData> mobs = new List<MobData>();
    public List<TrapData> traps = new List<TrapData>();
    public BossData boss;

    public WorldData(int width, int height, List<MobData> mobs, List<TrapData> traps, BossData boss)
    {
        this.width = width;
        this.height = height;
        this.mobs = mobs;
        this.traps = traps;
        this.boss = boss;
    }
}

public class GenerateLevel : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject borderPrefab;

    public List<WorldData> worldsData;

    public int spawnRange = 50;
    public int minGap = 3;

    private Vector2 prefabSize;
    private HashSet<Vector2Int> occupied = new();

    void Start()
    {
        SpriteRenderer sr = floorPrefab.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("floorPrefab должен иметь SpriteRenderer!");
            return;
        }

        prefabSize = sr.bounds.size;

        GenerateWorlds();
    }

    void GenerateWorlds()
    {
        int placed = 0;
        int attempts = 0;

        int numberOfWorlds = worldsData.Count;

        while (placed < numberOfWorlds && attempts < numberOfWorlds * 100)
        {
            attempts++;

            int worldWidth = worldsData[placed].width;
            int worldHeight = worldsData[placed].height;

            int totalWidth = worldWidth + 2;
            int totalHeight = worldHeight + 2;

            Vector2Int candidateOrigin = new Vector2Int(
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange)
            );

            if (IsAreaFree(candidateOrigin, totalWidth, totalHeight, minGap))
            {
                MarkAreaOccupied(candidateOrigin, totalWidth, totalHeight);

                SpawnWorld(candidateOrigin, worldWidth, worldHeight);

                SpawnMobs(candidateOrigin, worldWidth, worldHeight, worldsData[placed].mobs);

                SpawnTraps(candidateOrigin, worldWidth, worldHeight, worldsData[placed].traps);

                SpawnBoss(candidateOrigin, worldWidth, worldHeight, worldsData[placed].boss);

                placed++;
            }
        }

        Debug.Log($"Создано миров: {placed} из {numberOfWorlds}");
    }

    bool IsAreaFree(Vector2Int origin, int width, int height, int gap)
    {
        for (int y = -gap; y < height + gap; y++)
        {
            for (int x = -gap; x < width + gap; x++)
            {
                Vector2Int checkPos = origin + new Vector2Int(x, y);
                if (occupied.Contains(checkPos))
                    return false;
            }
        }
        return true;
    }

    void MarkAreaOccupied(Vector2Int origin, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int pos = origin + new Vector2Int(x, y);
                occupied.Add(pos);
            }
        }
    }

    void SpawnWorld(Vector2Int origin, int width, int height)
    {
        for (int y = 0; y < height + 2; y++)
        {
            for (int x = 0; x < width + 2; x++)
            {
                Vector2Int gridPos = origin + new Vector2Int(x, y);
                Vector3 spawnPos = new Vector3(gridPos.x * prefabSize.x, gridPos.y * prefabSize.y, 0);

                bool isBorder = (x == 0 || x == width + 1 || y == 0 || y == height + 1);
                GameObject prefabToSpawn = isBorder ? borderPrefab : floorPrefab;

                GameObject obj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, transform);
                obj.name = (isBorder ? "Border" : "Floor") + $" ({gridPos.x},{gridPos.y})";
            }
        }
    }

    void SpawnMobs(Vector2Int origin, int width, int height, List<MobData> mobs)
    {
        foreach (var mobData in mobs)
        {
            GameObject prefab = mobData.mobPrefab;
            if (prefab == null)
            {
                Debug.LogWarning($"Префаб моба не назначен!");
                continue;
            }

            int spawned = 0;
            int attempts = 0;
            int maxAttempts = mobData.count * 10;

            while (spawned < mobData.count && attempts < maxAttempts)
            {
                attempts++;

                int x = Random.Range(1, width + 1);
                int y = Random.Range(1, height + 1);

                Vector2Int mobGridPos = origin + new Vector2Int(x, y);

                Vector3 spawnPos = new Vector3(mobGridPos.x * prefabSize.x, mobGridPos.y * prefabSize.y, 0);

                Instantiate(prefab, spawnPos, Quaternion.identity, transform).name = $"{prefab.name}_{spawned}";

                spawned++;
            }
        }
    }

    void SpawnTraps(Vector2Int origin, int width, int height, List<TrapData> traps)
    {
        foreach (var trap in traps)
        {
            int spawned = 0;
            int attempts = 0;
            while (spawned < trap.count && attempts < trap.count * 10)
            {
                attempts++;

                int x = Random.Range(1, width + 1);
                int y = Random.Range(1, height + 1);

                Vector2Int gridPos = origin + new Vector2Int(x, y);
                Vector3 spawnPos = new Vector3(gridPos.x * prefabSize.x, gridPos.y * prefabSize.y, 0);

                Collider2D hit = Physics2D.OverlapCircle(spawnPos, prefabSize.x * 0.4f);
                if (hit == null)
                {
                    GameObject obj = Instantiate(trap.trapPrefab, spawnPos, Quaternion.identity, transform);
                    obj.name = $"Trap_{trap.trapPrefab.name}_{spawned}";

                    spawned++;
                }
            }
        }
    }

    void SpawnBoss(Vector2Int origin, int width, int height, BossData boss)
    {
        if (boss == null || boss.bossPrefab == null)
        {
            Debug.LogWarning("Босс или его префаб не назначен.");
            return;
        }

        // Центр карты
        int x = width / 2 + 1;
        int y = height / 2 + 1;

        Vector2Int gridPos = origin + new Vector2Int(x, y);
        Vector3 spawnPos = new Vector3(gridPos.x * prefabSize.x, gridPos.y * prefabSize.y, 0);

        GameObject obj = Instantiate(boss.bossPrefab, spawnPos, Quaternion.identity, transform);
        obj.name = $"Boss_{boss.bossPrefab.name}";
    }
}