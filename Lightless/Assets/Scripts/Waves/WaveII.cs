using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveII : IWave
{
    private int numberFireflies;
    private List<GameObject> fireflies;

    public WaveII(int enemies = 25)
    {
        numberFireflies = enemies;
        fireflies = new List<GameObject>();

        GameManager.Instance.StartCoroutine(Spawn());
    }

    protected override IEnumerator Spawn()
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

        while (true)
        {
            bool active = false;

            foreach (var firefly in fireflies)
            {
                if (firefly.activeInHierarchy)
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
