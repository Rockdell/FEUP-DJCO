using System.Collections;
using UnityEngine;

public class WaveIII : IWave
{
    private WaveI zombiesWave;
    private WaveII firefliesWave;

    public WaveIII()
    {
        GameManager.Instance.StartCoroutine(Spawn());
    }

    protected override IEnumerator Spawn()
    {
        for (int wave = 0; wave < 3; wave++)
        {
            zombiesWave = new WaveI(3 + wave);
            firefliesWave = new WaveII(5 + 2 * wave);

            while (!zombiesWave.isOver || !firefliesWave.isOver)
            {
                yield return new WaitForSeconds(1.0f);
            }
        }

        isOver = true;
        yield return null;
    }
}
