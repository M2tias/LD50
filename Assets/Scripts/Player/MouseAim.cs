using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseAim : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;
    Vector3 worldPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        worldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(worldPos.x, 1f, worldPos.z);
        Debug.Log(worldPos);
    }
}
