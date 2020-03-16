using System.Collections.Generic;
using UnityEngine;

public class WaveI : IWave
{
    private const int numberZombies = 5;
    private List<GameObject> zombies;

    public WaveI()
    {
        zombies = new List<GameObject>();

        Spawn();
    }

    protected override void Spawn()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var closestPosition = -GameManager.Instance.screenBounds.x + 20;
        var spacePerZombie = (GameManager.Instance.screenBounds.x + Mathf.Abs(closestPosition)) / numberZombies;

        for (int i = 0; i < numberZombies; i++)
        {
            GameObject zombie = GameManager.Instance.GetObject(GameManager.ObjectType.Zombie);

            var zombieScript = zombie.GetComponent<ZombieScript>();
            zombieScript.targetPosition = closestPosition + i * spacePerZombie;
            zombieScript.Spawn(new Vector2(spawningPosition + i * 10, -12.0f), Quaternion.identity);
            zombie.SetActive(true);

            zombies.Add(zombie);
        }
    }

    public override bool isOver()
    {
        foreach (var zombie in zombies)
        {
            if (zombie.activeInHierarchy)
                return false;
        }

        return true;
    }
}
