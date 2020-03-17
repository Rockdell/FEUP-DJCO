using System.Collections;

public abstract class IWave
{
    public bool isOver { get; protected set; }

    public IWave()
    {
        isOver = false;
    }

    protected abstract IEnumerator Spawn();
}
