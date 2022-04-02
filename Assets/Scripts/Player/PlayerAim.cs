using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;

    [SerializeField]
    GameObject aimIndicator;
    [SerializeField]
    private float rotationSpeed;

    Vector3 worldPos;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        LayerMask aimTarget = LayerMask.GetMask("AimTarget");
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 100f, aimTarget))
        {
            aimIndicator.transform.position = hit.point;
            // Debug.Log(hit.point);
        }
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y + mousePos.x * rotationSpeed * Time.deltaTime, 0);

    }

    void OnMouse(InputValue value)
    {
        mousePos = value.Get<Vector2>();
    }
}
