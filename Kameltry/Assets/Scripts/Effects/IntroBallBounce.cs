using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBallBounce : MonoBehaviour {

    public Rigidbody2D ballBody;

    void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 circ = Random.insideUnitCircle;
        circ = new Vector2(circ.x * 50000f, circ.y * 10000f);
        ballBody.AddForce(circ - coll.relativeVelocity*600f, ForceMode2D.Impulse);
    }

}
