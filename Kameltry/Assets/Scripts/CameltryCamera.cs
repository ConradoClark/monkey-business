using UnityEngine;
using System.Collections;

public class CameltryCamera : MonoBehaviour
{
    public Camera mainCamera;
    public float speed;
    public float gravityValue = -9.81f;
    public Transform follow;

    private Vector3 velocity;
    private Vector2 gravityVelocity;

    public TurnButton ccwButton;
    public TurnButton cwButton;

    public bool turnButtonsEnabled;
    public TimeLayers timeLayer;

    void Start()
    {
        //if (SystemInfo.supportsGyroscope)
        //{
        //    Input.gyro.enabled = true;
        //    turnButtonsEnabled = false;
        //    ccwButton.gameObject.SetActive(false);
        //    cwButton.gameObject.SetActive(false);
        //}
        //else
        //{
            turnButtonsEnabled = true;
        //}
    }

    void Update()
    {
        if (!Toolbox.Instance.levelManager.IsTimerRunning()) return;

        if (Input.GetKey(KeyCode.RightArrow) || (cwButton.IsPressed && turnButtonsEnabled))
        {
            mainCamera.transform.Rotate(0f, 0f, speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || (ccwButton.IsPressed && turnButtonsEnabled))
        {
            mainCamera.transform.Rotate(0f, 0f, -speed * Time.deltaTime);
        }

        float t = 0;
        if (turnButtonsEnabled)
        {
            t = Vector2.Angle(Physics2D.gravity, mainCamera.transform.rotation * new Vector2(0, gravityValue));

            Physics2D.gravity = Vector2.SmoothDamp(Physics2D.gravity, mainCamera.transform.rotation * new Vector2(0, gravityValue) * (1 +
            t * 0.1f)
            , ref gravityVelocity, 0.15f, float.MaxValue, Toolbox.Instance.time.GetDeltaTime(timeLayer));
        }
        else
        {            
            Quaternion gyro = Quaternion.AngleAxis(Vector2.Angle(Vector2.down, Input.gyro.gravity), new Vector3(0, 0, 1) * Mathf.Sign(Input.gyro.gravity.normalized.x));
            t = Vector2.Angle(Physics2D.gravity, gyro * new Vector2(0, gravityValue));
            Physics2D.gravity = Vector2.SmoothDamp(Physics2D.gravity, gyro * new Vector2(0, gravityValue) * (1 +
            t * 0.1f)
            , ref gravityVelocity, 0.15f, float.MaxValue, Toolbox.Instance.time.GetDeltaTime(timeLayer));
        }


    }

    void FixedUpdate()
    {
        Vector3 newPos = Vector3.SmoothDamp(mainCamera.transform.position, follow.position, ref velocity, 0.15f) * Toolbox.Instance.time.GetLayerMultiplier(timeLayer);
        mainCamera.transform.position = new Vector3(newPos.x, newPos.y, mainCamera.transform.position.z);
    }
}
