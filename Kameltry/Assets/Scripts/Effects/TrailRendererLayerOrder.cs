using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererLayerOrder : MonoBehaviour {

    public TrailRenderer trail;
    public int sortingOrder;
	
	void Start () {
        trail.sortingOrder = sortingOrder;
    }
}
