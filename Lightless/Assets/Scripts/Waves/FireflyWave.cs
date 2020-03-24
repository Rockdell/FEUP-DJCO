using System.Collections;
using UnityEngine;

public class FireflyWave : IWave
{
    public FireflyWave(int nrEnemies, bool spawnObstacles, bool spawnDropLights, bool spawnPickups) : base(nrEnemies)
    {
        if (spawnObstacles)
            GameManager.Instance.StartCoroutine(SpawnObstacles());
        if (spawnDropLights)
            GameManager.Instance.StartCoroutine(SpawnDropLights(5, 10));
        if (spawnPickups)
            GameManager.Instance.StartCoroutine(SpawnPickups(true, false));
    }

    protected override IEnumerator Spawn()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var upperHeight = GameManager.Instance.screenBounds.y - 10;
        var lowerHeight = -GameManager.Instance.screenBounds.y + 10;

        while (enemies.Count != numberEnemies)
        {
            GameObject firefly = GameManager.Instance.GetObject(GameManager.ObjectType.Firefly);

            var fireflyScript = firefly.GetComponent<FireflyScript>();
            fireflyScript.Spawn(new Vector2(spawningPosition, Random.Range(lowerHeight, upperHeight)), Quaternion.identity);
            firefly.SetActive(true);

            enemies.Add(firefly);

            yield return new WaitForSeconds(1.5f);
        }
    }
}
