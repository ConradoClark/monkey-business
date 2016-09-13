using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class FrostySpriteDeformation : MonoBehaviour {
    public new FrostyCollision collider;
    public SpriteRenderer spriteRenderer;
    public float deformationStart;
    public float deformationEnd;
    public float deformationAmount;
    string debugText;
    private Vector4[] v4 = new Vector4[5];
    private float[] dampSpeed = new float[5];
    public float deformationDampening;
    public TimeLayers timeLayer;

    void Start () {
	
	}

    void OnGUI()
    {
        //GUI.contentColor = Color.green;
        //GUI.Label(new Rect(150, 350, 1000, 100), debugText);
    }

    void Update () {
        if (collider.AllHits == null) return;
        List<Collider2D> obj = new List<Collider2D>();

        int i = 0;
        foreach(var hit in collider.AllHits.Take(5))
        {
            if (obj.Contains(hit.collider)) continue;
            obj.Add(hit.collider);

            float point = (hit.point.x - this.transform.position.x + spriteRenderer.bounds.size.x / 2) / spriteRenderer.bounds.size.x;
            point = spriteRenderer.flipX ? point : 1 - point;

            point = Mathf.Lerp(deformationStart, deformationEnd, point);

            if (i == 0)
            {
                debugText = "hit: " + point.ToString() + " | pos:" + (this.transform.position.x + spriteRenderer.bounds.size.x / 2).ToString() + " | point: " + point.ToString();
            }
            
            Vector4 v = new Vector4(deformationStart, deformationEnd, point, Mathf.SmoothDamp(v4[i].w,deformationAmount,ref dampSpeed[i], deformationDampening));
            v4[i] = v;
            i++;
        }

        spriteRenderer.material.SetVectorArray("_VerticalDeform", v4.ToArray());
        for (i = 0; i < 5; i++)
        {
            v4[i].w += Toolbox.Instance.time.GetDeltaTime(timeLayer)*2;
            v4[i].w = Mathf.Clamp(v4[i].w, -1, 0);
        }
	}
}
