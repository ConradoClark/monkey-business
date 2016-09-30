using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableGalaxy : MonoBehaviour
{
    public bool enable;

    void Start()
    {
        if (Toolbox.Instance.galaxyCamera != null)
        {
            Toolbox.Instance.galaxyCamera.gameObject.SetActive(enable);
        }
    }
}
