using UnityEngine;
using System.Collections;

public class OverUI : MonoBehaviour {

    public Camera overUICamera;
    public Canvas canvas;

	void Start () {
        Toolbox.Instance.overUI = this;
	}
	
	void Update () {
	
	}
}
