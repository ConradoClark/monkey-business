using UnityEngine;
using System.Collections;

public class TurnButton : MonoBehaviour
{
    public Camera OverUICamera;
    public Collider2D region;
    public SpriteRenderer spriteRenderer;
    public Sprite normalState;
    public Sprite pressedState;
    public bool runOutsideOfTimer;

    private bool pressing;

    void Start()
    {

    }

    void Update()
    {
        if ((!Toolbox.Instance.levelManager.IsTimerRunning() && !runOutsideOfTimer) || (pressing && !Input.GetMouseButton(0)))
        {
            pressing = false;
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = normalState;
            }
            return;
        }

        var mousePos = OverUICamera.ScreenToWorldPoint(Input.mousePosition);
        if (!pressing && Input.GetMouseButton(0) && region.OverlapPoint(mousePos))
        {
            pressing = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = pressedState;
            }
        }
    }

    public bool IsPressed { get { return pressing; } }
}
