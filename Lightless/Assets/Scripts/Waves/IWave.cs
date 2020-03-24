using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWave
{
    public bool isOver { get; protected set; }

    protected int numberEnemies;
    protected List<GameObject> enemies;

    public IWave(int nrEnemies)
    {
        isOver = false;

        numberEnemies = nrEnemies;
        enemies = new List<GameObject>();

        GameManager.Instance.StartCoroutine(Spawn());
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

    protected virtual IEnumerator SpawnObstacles(int lowerRange = 7, int upperRange = 15)
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;

        yield return new WaitForSeconds(lowerRange);

        while (!isOver)
        {
            GameObject redLight = GameManager.Instance.GetObject(GameManager.ObjectType.RedLight);
            redLight.GetComponent<RedLightScript>().Spawn(new Vector2(spawningPosition, 19.6f), Quaternion.identity);
            redLight.SetActive(true);

            yield return new WaitForSeconds(Random.Range(lowerRange, upperRange));
        }
    }

    protected virtual IEnumerator SpawnDropLights(int lowerRange = 5, int upperRange = 10)
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var upperHeight = GameManager.Instance.screenBounds.y - 10;
        var lowerHeight = -GameManager.Instance.screenBounds.y + 10;

        yield return new WaitForSeconds(lowerRange);

        while (!isOver)
        {
            GameObject dropLight = GameManager.Instance.GetObject(GameManager.ObjectType.DropLight);
            dropLight.GetComponent<DropLightScript>().Spawn(new Vector2(spawningPosition, Random.Range(lowerHeight, upperHeight)), Quaternion.identity);
            dropLight.SetActive(true);

            yield return new WaitForSeconds(Random.Range(lowerRange, upperRange));
        }
    }

    protected virtual IEnumerator SpawnPickups(bool burnEnabled, bool forceFieldEnabled)
    {
        var spawningPosition = GameManager.Instance.screenBounds.x + 20;
        var upperHeight = GameManager.Instance.screenBounds.y - 10;
        var lowerHeight = -GameManager.Instance.screenBounds.y + 10;

        yield return new WaitForSeconds(2.0f);

        float burnChance = 0.2f, forceFieldChance = 0.3f;

        while (!isOver)
        {
            if (burnEnabled && Random.value <= burnChance)
            {
                GameObject burn = GameManager.Instance.GetObject(GameManager.ObjectType.Burn);
                burn.GetComponent<PowerUpPickUpScript>().Spawn(new Vector2(spawningPosition, Random.Range(lowerHeight, upperHeight)), Quaternion.identity);
                burn.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);

            if (forceFieldEnabled && Random.value <= forceFieldChance)
            {
                GameObject forceField = GameManager.Instance.GetObject(GameManager.ObjectType.ForceField);
                forceField.GetComponent<PowerUpPickUpScript>().Spawn(new Vector2(spawningPosition, Random.Range(lowerHeight, upperHeight)), Quaternion.identity);
                forceField.SetActive(true);
            }

            yield return new WaitForSeconds(10);
        }
    }
}
