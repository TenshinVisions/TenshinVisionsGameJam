using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    [Header("Tile Content")]
    public List<MobPair> mobs;
    public List<TrapPair> traps;
    public GameObject boss;

    public virtual void Initialize(List<MobPair> mobs, List<TrapPair> traps, GameObject boss)
    {
        this.mobs = mobs;
        this.traps = traps;
        this.boss = boss;
    }

     public abstract void SpawnEntitiesOnTile();
}