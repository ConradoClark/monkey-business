using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyCamera : MonoBehaviour {

    public new Camera camera;
	void Start () {
        if (Toolbox.Instance.galaxyCamera != null)
        {
            GameObject.Destroy(this);
            return;
        }
        Toolbox.Instance.galaxyCamera = this;
        DontDestroyOnLoad(transform.gameObject);
    }
}
