using UnityEngine;
using System.Collections;

public class LimitVelocity : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public float maxVelocity;
    public TimeLayers timeLayer;

    void Start()
    {

    }

    void FixedUpdate()
    {
        float timeDistortion = Toolbox.Instance.time.GetLayerMultiplier(timeLayer);
        rigidbody.AddForce((timeDistortion - 1) * rigidbody.velocity);

        float speed = Vector3.Magnitude(rigidbody.velocity);  // test current object speed

        if (speed > maxVelocity)

        {
            float brakeSpeed = speed - maxVelocity;  // calculate the speed decrease

            Vector3 normalisedVelocity = rigidbody.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value

            rigidbody.AddForce(-brakeVelocity);  // apply opposing brake force
        }

    }
}
