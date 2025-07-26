using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : BaseTile
{
    private void Start()
    {
        SpawnEntitiesOnTile();
    }

    public override void SpawnEntitiesOnTile()
    {
        float tileSize = GetTileSize();

        // Спавн мобов без ограничений по расстоянию
        foreach (var mobPair in mobs)
        {
            for (int i = 0; i < mobPair.count; i++)
            {
                Vector3 spawnPos = GetFreeRandomPositionInTile(transform.position, tileSize, 1f);
                Instantiate(mobPair.mobPrefab, spawnPos, Quaternion.identity, transform);
            }
        }

        // Для ловушек создаём список занятых позиций
        List<Vector3> trapPositions = new List<Vector3>();
        float minDistance = 2f; // минимальное расстояние между ловушками (2 блока)

        foreach (var trapPair in traps)
        {
            for (int i = 0; i < trapPair.count; i++)
            {
                Vector3 spawnPos;

                // Пытаемся найти позицию, которая не ближе minDistance к другим ловушкам
                int tries = 0;
                const int maxTries = 50;
                do
                {
                    spawnPos = GetFreeRandomPositionInTile(transform.position, tileSize, 1f);
                    tries++;
                    // Если за maxTries не нашли подходящую позицию — всё равно спавним
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
            Vector3 spawnPos = transform.position + Vector3.up * 1f; // поднимаем чуть вверх
            Instantiate(boss, spawnPos, Quaternion.identity, transform);
        }
    }

    // Проверяем, есть ли в списке позиций объекты ближе чем minDistance
    private bool IsTooClose(Vector3 pos, List<Vector3> positions, float minDistance)
    {
        foreach (var otherPos in positions)
        {
            if (Vector3.Distance(pos, otherPos) < minDistance)
                return true;
        }
        return false;
    }


    // Пример получения случайной позиции в пределах тайла (предполагается квадрат 5x5)
    private Vector3 GetFreeRandomPositionInTile(Vector3 tilePosition, float tileSize, float checkRadius = 0.5f, int maxTries = 20)
    {
        float halfSize = tileSize / 2f;

        for (int i = 0; i < maxTries; i++)
        {
            float x = Random.Range(tilePosition.x - halfSize, tilePosition.x + halfSize);
            float y = Random.Range(tilePosition.y - halfSize, tilePosition.y + halfSize);
            Vector3 pos = new Vector3(x, y, tilePosition.z);

            // Проверка на наличие коллайдера
            if (!Physics.CheckBox(pos, Vector3.one * checkRadius / 2f))
            {
                return pos;
            }
        }

        // Если не нашли свободную позицию — возвращаем центр
        return tilePosition;
    }

    private float GetTileSize()
    {
        var box = GetComponent<BoxCollider>();
        if (box != null)
        {
            // Получаем локальный размер бокса и берём минимальное значение между X и Y (по полу)
            return Mathf.Min(box.size.x * transform.localScale.x, box.size.y * transform.localScale.y);
        }

        // Если коллайдера нет — дефолт
        return 5f;
    }
}
