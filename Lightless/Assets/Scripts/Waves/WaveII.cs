using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveII : IWave
{
    private const int numberFireflies = 25;
    private List<GameObject> fireflies;

    public WaveII()
    {
        fireflies = new List<GameObject>();

        Spawn();
    }

    protected override void Spawn()
    {
        GameManager.Instance.StartCoroutine(SpawnOverTime());
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

    IEnumerator SpawnOverTime()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var upperHeight = GameManager.Instance.screenBounds.y - 10;
        var lowerHeight = -GameManager.Instance.screenBounds.y + 10;

        while (fireflies.Count != numberFireflies)
        {
            GameObject firefly = GameManager.Instance.GetObject(GameManager.ObjectType.Firefly);

            var fireflyScript = firefly.GetComponent<FireflyScript>();
            fireflyScript.Spawn(new Vector2(spawningPosition, Random.Range(lowerHeight, upperHeight)), Quaternion.identity);
            firefly.SetActive(true);

            fireflies.Add(firefly);

            yield return new WaitForSeconds(0.8f);
        }

        yield return null;
    }
}
