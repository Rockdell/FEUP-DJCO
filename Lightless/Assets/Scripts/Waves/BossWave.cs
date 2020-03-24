using System.Collections;
using UnityEngine;

public class BossWave : IWave
{
    public BossWave(int nrEnemies, bool spawnObstacles, bool spawnDropLights, bool spawnPickups) : base(nrEnemies)
    {
        if (spawnObstacles)
            GameManager.Instance.StartCoroutine(SpawnObstacles());
        if (spawnDropLights)
            GameManager.Instance.StartCoroutine(SpawnDropLights(3, 5));
        if (spawnPickups)
            GameManager.Instance.StartCoroutine(SpawnPickups(true, true));
    }

    protected override IEnumerator Spawn()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 40;

        GameObject boss = GameManager.Instance.GetObject(GameManager.ObjectType.Boss);
        boss.GetComponent<BossScript>().Spawn(new Vector2(spawningPosition, 0.0f), Quaternion.identity);
        boss.SetActive(true);

        enemies.Add(boss);

        yield return null;
    }
}
