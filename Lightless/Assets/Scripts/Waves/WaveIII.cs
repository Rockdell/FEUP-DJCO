using System.Collections.Generic;
using UnityEngine;

public class WaveIII : IWave
{
    private List<GameObject> zombies;
    private List<GameObject> fireflies;

    public WaveIII()
    {
        zombies = new List<GameObject>();
        fireflies = new List<GameObject>();
    }

    protected override void Spawn()
    {
        throw new System.NotImplementedException();
    }

    public override bool isOver()
    {
        foreach (var zombie in zombies)
        {
            if (zombie.activeInHierarchy)
                return false;
        }

        foreach (var firefly in fireflies)
        {
            if (firefly.activeInHierarchy)
                return false;
        }

        return true;
    }
}
