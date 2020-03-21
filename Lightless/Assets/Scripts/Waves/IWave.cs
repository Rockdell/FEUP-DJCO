using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWave
{
    public bool isOver { get; protected set; }

    protected int numberEnemies;
    protected List<GameObject> enemies;

    public IWave(int nrEnemies, bool spawnObstacles)
    {
        isOver = false;

        numberEnemies = nrEnemies;
        enemies = new List<GameObject>();

        GameManager.Instance.StartCoroutine(Spawn());

        if (spawnObstacles)
            GameManager.Instance.StartCoroutine(SpawnObstacles());

        GameManager.Instance.StartCoroutine(SpawnPickups());

        GameManager.Instance.StartCoroutine(CheckIfOver());
    }

    protected virtual IEnumerator CheckIfOver()
    {
        while (true)
        {
            if (enemies.Count != numberEnemies)
                yield return new WaitForSeconds(1.0f);

            bool active = false;

            foreach (var enemy in enemies)
            {
                if (enemy.activeInHierarchy)
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
                break;
            }
        }
    }

    protected abstract IEnumerator Spawn();

    protected virtual IEnumerator SpawnObstacles()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;

        yield return new WaitForSeconds(Random.Range(4, 10));

        while (!isOver)
        {
            GameObject redLight = GameManager.Instance.GetObject(GameManager.ObjectType.RedLight);
            redLight.GetComponent<RedLightScript>().Spawn(new Vector2(spawningPosition, 19.6f), Quaternion.identity);
            redLight.SetActive(true);

            yield return new WaitForSeconds(Random.Range(7, 15));
        }
    }

    protected virtual IEnumerator SpawnPickups()
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var upperHeight = GameManager.Instance.screenBounds.y - 10;
        var lowerHeight = -GameManager.Instance.screenBounds.y + 10;

        yield return new WaitForSeconds(5.0f);

        // TODO add cooldown for pick ups

        while (!isOver)
        {
            GameObject dropLight = GameManager.Instance.GetObject(GameManager.ObjectType.DropLight);
            dropLight.GetComponent<DropLightScript>().Spawn(new Vector2(spawningPosition, Random.Range(lowerHeight, upperHeight)), Quaternion.identity);
            dropLight.SetActive(true);

            yield return new WaitForSeconds(5.0f);
        }
    }
}
