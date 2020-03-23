using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWave : IWave
{
    public BossWave() : base(1, false, false)
    {
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
