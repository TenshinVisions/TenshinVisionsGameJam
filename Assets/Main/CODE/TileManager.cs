using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : BaseTile
{
    [Header("Tilemap Reference")]
    public Tilemap tilemap; // Привяжи сюда свой Tilemap в инспекторе

    private void Start()
    {
        SpawnEntitiesOnTile();
    }

    public override void SpawnEntitiesOnTile()
    {
        if (tilemap == null)
        {
            Debug.LogError("TileManager: Tilemap не привязан!");
            return;
        }

        // Спавн мобов
        foreach (var mobPair in mobs)
        {
            for (int i = 0; i < mobPair.count; i++)
            {
                Vector3 spawnPos = GetRandomWorldPositionFromTilemap();
                Instantiate(mobPair.mobPrefab, spawnPos, Quaternion.identity, transform);
            }
        }

        // Спавн ловушек с проверкой расстояния
        List<Vector3> trapPositions = new List<Vector3>();
        float minDistance = 2f;

        foreach (var trapPair in traps)
        {
            for (int i = 0; i < trapPair.count; i++)
            {
                Vector3 spawnPos;
                int tries = 0;
                const int maxTries = 50;

                do
                {
                    spawnPos = GetRandomWorldPositionFromTilemap();
                    tries++;
                    if (tries > maxTries) break;
                }
                while (IsTooClose(spawnPos, trapPositions, minDistance));

                trapPositions.Add(spawnPos);
                Instantiate(trapPair.trapPrefab, spawnPos, Quaternion.identity, transform);
            }
        }

        // Спавн босса, если есть
        if (boss != null)
        {
            Vector3 bossPos = GetRandomWorldPositionFromTilemap() + Vector3.up * 0.5f; // чуть выше тайла
            Instantiate(boss, bossPos, Quaternion.identity, transform);
        }
    }

    private bool IsTooClose(Vector3 pos, List<Vector3> positions, float minDistance)
    {
        foreach (var otherPos in positions)
        {
            if (Vector3.Distance(pos, otherPos) < minDistance)
                return true;
        }
        return false;
    }

    // Возвращает случайную мировую позицию, соответствующую заполненному тайлу Tilemap
    private Vector3 GetRandomWorldPositionFromTilemap()
    {
        BoundsInt bounds = tilemap.cellBounds;
        List<Vector3> validPositions = new List<Vector3>();

        foreach (var cellPos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(cellPos))
            {
                Vector3 worldPos = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;
                validPositions.Add(worldPos);
            }
        }

        if (validPositions.Count == 0)
        {
            Debug.LogWarning("TileManager: нет валидных тайлов для спавна!");
            return transform.position;
        }

        return validPositions[Random.Range(0, validPositions.Count)];
    }
}
