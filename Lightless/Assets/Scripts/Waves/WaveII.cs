using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveII : IWave
{
    private const int numberFireflies = 1;
    private List<GameObject> fireflies;

    public WaveII()
    {
        fireflies = new List<GameObject>();

        Spawn();
    }

    protected override void Spawn()
    {
        for (int i = 0; i < numberFireflies; i++)
        {
            GameObject firefly = GameManager.Instance.GetObject(GameManager.ObjectType.Firefly);
            //firefly.GetComponent<FireflyScript>().Spawn(new Vector2(70.0f, 0.0f), Quaternion.identity);
            firefly.SetActive(true);

            fireflies.Add(firefly);
        }


        //var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        //var closestPosition = -GameManager.Instance.screenBounds.x + 20;
        //var spacePerZombie = (GameManager.Instance.screenBounds.x + Mathf.Abs(closestPosition)) / numberZombies;

        //for (int i = 0; i < numberZombies; i++)
        //{
        //    GameObject zombie = GameManager.Instance.GetObject(GameManager.ObjectType.Zombie);

        //    var zombieScript = zombie.GetComponent<ZombieScript>();
        //    zombieScript.targetPosition = closestPosition + i * spacePerZombie;
        //    zombieScript.Spawn(new Vector2(spawningPosition + i * 10, -12.0f), Quaternion.identity);
        //    zombie.SetActive(true);

        //    fireflies.Add(zombie);
        //}
    }



    public override bool isOver()
    {
        foreach (var firefly in fireflies)
        {
            if (firefly.activeInHierarchy)
                return false;
        }

        return true;
    }
}
