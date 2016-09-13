using UnityEngine;
using System.Collections;

public class MaintainRotation : MonoBehaviour
{
    public Transform parent;

    void Update()
    {
        this.transform.localRotation = Quaternion.Inverse(parent.transform.rotation);
    }
}
