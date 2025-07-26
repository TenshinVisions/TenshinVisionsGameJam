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
    public int width;
    public int height;

    public GameObject floorPrefab;
    public GameObject borderPrefab;

    // Список пар: префаб моба и количество
    public List<MobPair> mobs = new List<MobPair>();
    public List<TrapPair> traps = new List<TrapPair>();
    public GameObject boss;

    public WorldData(int width, int height, GameObject floorPrefab, GameObject borderPrefab,
                     List<MobPair> mobs, List<TrapPair> traps, GameObject boss)
    {
        this.width = width;
        this.height = height;
        this.floorPrefab = floorPrefab;
        this.borderPrefab = borderPrefab;
        this.mobs = mobs;
        this.traps = traps;
        this.boss = boss;
    }
}

public class GenerateLevel : MonoBehaviour
{
    public List<WorldData> worldsData;
    public int spawnRange = 50;
    public int minGap = 3;

    private Vector2 prefabSize;
    private HashSet<Vector2Int> occupied = new();

    void Start()
    {
        StartCoroutine(GenerateWorlds());
    }

    IEnumerator GenerateWorlds()
    {
        int placed = 0;
        int attempts = 0;
        int numberOfWorlds = worldsData.Count;

        while (placed < numberOfWorlds && attempts < numberOfWorlds * 100)
        {
            attempts++;

            var world = worldsData[placed];
            int totalWidth = world.width + 2;
            int totalHeight = world.height + 2;

            SpriteRenderer sr = world.floorPrefab.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.LogError("floorPrefab должен иметь SpriteRenderer!");
                yield break;
            }
            prefabSize = sr.bounds.size;

            Vector2Int origin = new Vector2Int(
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange)
            );

            if (IsAreaFree(origin, totalWidth, totalHeight, minGap))
            {
                MarkAreaOccupied(origin, totalWidth, totalHeight);

                yield return StartCoroutine(SpawnWorld(origin, world));
                yield return StartCoroutine(SpawnMobs(origin, world.width, world.height, world.mobs));
                yield return StartCoroutine(SpawnTraps(origin, world.width, world.height, world.traps));
                yield return StartCoroutine(SpawnBoss(origin, world.width, world.height, world.boss));

                placed++;
            }
            yield return null;
        }

        Debug.Log($"Создано миров: {placed} из {numberOfWorlds}");
    }

    bool IsAreaFree(Vector2Int origin, int width, int height, int gap)
    {
        for (int y = -gap; y < height + gap; y++)
        {
            for (int x = -gap; x < width + gap; x++)
            {
                if (occupied.Contains(origin + new Vector2Int(x, y)))
                    return false;
            }
        }
        return true;
    }

    void MarkAreaOccupied(Vector2Int origin, int width, int height)
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                occupied.Add(origin + new Vector2Int(x, y));
    }

    IEnumerator SpawnWorld(Vector2Int origin, WorldData world)
    {
        for (int y = 0; y < world.height + 2; y++)
        {
            for (int x = 0; x < world.width + 2; x++)
            {
                Vector2Int grid = origin + new Vector2Int(x, y);
                Vector3 pos = new Vector3(grid.x * prefabSize.x, grid.y * prefabSize.y, 0);

                bool border = (x == 0 || x == world.width + 1 || y == 0 || y == world.height + 1);
                GameObject prefab = border ? world.borderPrefab : world.floorPrefab;

                Instantiate(prefab, pos, Quaternion.identity, transform).name = (border ? "Border" : "Floor") + $" ({grid.x},{grid.y})";
            }
            yield return null;
        }
    }

    IEnumerator SpawnMobs(Vector2Int origin, int width, int height, List<MobPair> mobs)
    {
        foreach (var mobPair in mobs)
        {
            GameObject prefab = mobPair.mobPrefab;
            int count = mobPair.count;

            int spawned = 0, tries = 0;

            while (spawned < count && tries < count * 10)
            {
                tries++;
                int x = Random.Range(1, width + 1);
                int y = Random.Range(1, height + 1);

                Vector3 pos = new Vector3((origin.x + x) * prefabSize.x, (origin.y + y) * prefabSize.y, 0);

                if (Physics2D.OverlapCircle(pos, prefabSize.x * 0.4f) == null)
                {
                    GameObject mobGO = Instantiate(prefab, pos, Quaternion.identity, transform);
                    mobGO.name = $"Mob_{prefab.name}_{spawned}";

                    MobBase baseFromPrefab = prefab.GetComponent<MobBase>();
                    MobBase spawnedMob = mobGO.GetComponent<MobBase>();

                    if (baseFromPrefab != null && spawnedMob != null)
                    {
                        spawnedMob.Initialize(
                            baseFromPrefab.damage,
                            baseFromPrefab.speed,
                            baseFromPrefab.health
                        );
                    }
                    else
                    {
                        Debug.LogWarning($"Mob prefab {prefab.name} не содержит MobBase.");
                    }

                    spawned++;
                    if (spawned % 5 == 0) yield return null;
                }
            }
        }
    }

    IEnumerator SpawnTraps(Vector2Int origin, int width, int height, List<TrapPair> traps)
    {
        foreach (var trapPair in traps)
        {
            GameObject prefab = trapPair.trapPrefab;
            int count = trapPair.count;

            int spawned = 0;
            int tries = 0;

            while (spawned < count && tries < count * 10)
            {
                tries++;

                int x = Random.Range(1, width + 1);
                int y = Random.Range(1, height + 1);

                Vector3 pos = new Vector3((origin.x + x) * prefabSize.x, (origin.y + y) * prefabSize.y, 0);

                // Проверка, чтобы не спавнить ловушки слишком близко к другим объектам
                if (Physics2D.OverlapCircle(pos, prefabSize.x * 0.4f) == null)
                {
                    GameObject trapGO = Instantiate(prefab, pos, Quaternion.identity, transform);
                    trapGO.name = $"Trap_{prefab.name}_{spawned}";

                    TrapBase baseFromPrefab = prefab.GetComponent<TrapBase>();
                    TrapBase spawnedTrap = trapGO.GetComponent<TrapBase>();

                    if (baseFromPrefab != null && spawnedTrap != null)
                    {
                        spawnedTrap.Initialize(baseFromPrefab.damage);
                    }
                    else
                    {
                        Debug.LogWarning($"Trap prefab {prefab.name} не содержит TrapBase.");
                    }

                    spawned++;

                    if (spawned % 5 == 0)
                        yield return null;
                }
            }
        }
    }

    IEnumerator SpawnBoss(Vector2Int origin, int width, int height, GameObject boss)
    {
        if (boss == null) yield return null;

        Vector3 pos = new Vector3((origin.x + width / 2 + 1) * prefabSize.x, (origin.y + height / 2 + 1) * prefabSize.y, 0);
        Instantiate(boss, pos, Quaternion.identity, transform).name = $"Boss_{boss.name}";
    }
}