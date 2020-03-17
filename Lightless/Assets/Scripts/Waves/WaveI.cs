using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveI : IWave
{
    private int numberZombies = 5;
    private List<GameObject> zombies;

    public WaveI(int enemies = 5)
    {
        numberZombies = enemies;
        zombies = new List<GameObject>();

        GameManager.Instance.StartCoroutine(Spawn());
    }

    protected override IEnumerator Spawn()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var closestPosition = -GameManager.Instance.screenBounds.x + 20;
        var spacePerZombie = (GameManager.Instance.screenBounds.x + Mathf.Abs(closestPosition)) / numberZombies;

        while (zombies.Count != numberZombies)
        {
            GameObject zombie = GameManager.Instance.GetObject(GameManager.ObjectType.Zombie);

            var zombieScript = zombie.GetComponent<ZombieScript>();
            zombieScript.targetPosition = closestPosition + zombies.Count * spacePerZombie;
            zombieScript.Spawn(new Vector2(spawningPosition + zombies.Count, -12.0f), Quaternion.identity);
            zombie.SetActive(true);

            zombies.Add(zombie);

            yield return new WaitForSeconds(0.8f);
        }

        while (true)
        {
            bool active = false;

            foreach (var zombie in zombies)
            {
                if (zombie.activeInHierarchy)
                {
                    active = true;
                    break;
                }
            }

            if (active)
            {
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                isOver = true;
                yield return null;
            }
        }
    }
}
