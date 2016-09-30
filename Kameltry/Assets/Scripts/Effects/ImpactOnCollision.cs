using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactOnCollision : MonoBehaviour
{
    public Rigidbody2D body;
    public FrostyPoolableObject impactEffect;
    bool isColliding;

    void Awake()
    {
        collision = new Stack<bool>();
    }

    private Stack<bool> collision;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (isColliding || collision.Count > 0 || body.velocity.magnitude < 100f || impactEffect == null) return;
        isColliding = true;
        GameObject effect = null;
        if (Toolbox.Instance.pool.TryRetrieve(impactEffect, this.transform.position, this.transform.rotation, out effect))
        {
            effect.transform.position = this.transform.position;
            collision.Push(true);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (collision.Count > 0)
        {
            collision.Pop();
        }
    }
    void Update()
    {
        isColliding = false;
    }
}
