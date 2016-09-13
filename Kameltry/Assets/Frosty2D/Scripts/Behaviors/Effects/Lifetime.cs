using UnityEngine;
using System.Collections;

public class Lifetime : FrostyPoolableObject
{
    public float lifeTime;
    public TimeLayers timeLayer;

    void Start()
    {
        StartCoroutine(WaitAndDie());
    }

    IEnumerator WaitAndDie()
    {
        yield return Toolbox.Instance.time.WaitForSeconds(timeLayer, lifeTime);
        Toolbox.Instance.pool.Release(this, this.gameObject);
    }

    public override void ResetState()
    {
        base.ResetState();
        StopAllCoroutines();
        StartCoroutine(WaitAndDie());
    }
}
