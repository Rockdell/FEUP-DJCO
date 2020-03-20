using System.Collections;
using UnityEngine;

public class ZombieWave : IWave
{
    public ZombieWave(int nrEnemies, bool spawnObstacles = false) : base(nrEnemies, spawnObstacles)
    {
    }

    protected override IEnumerator Spawn()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var closestPosition = -GameManager.Instance.screenBounds.x + 20;
        var spacePerZombie = (GameManager.Instance.screenBounds.x + Mathf.Abs(closestPosition)) / numberEnemies;

        while (enemies.Count != numberEnemies)
        {
            GameObject zombie = GameManager.Instance.GetObject(GameManager.ObjectType.Zombie);

            var zombieScript = zombie.GetComponent<ZombieScript>();
            zombieScript.targetPosition = closestPosition + enemies.Count * spacePerZombie;
            zombieScript.Spawn(new Vector2(spawningPosition, -12.0f), Quaternion.identity);
            zombie.SetActive(true);

            enemies.Add(zombie);

            yield return new WaitForSeconds(0.8f);
        }
    }
}
