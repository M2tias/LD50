using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootModel : MonoBehaviour
{
    private float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        y = Mathf.Sin(Time.time * 2f) / 2f * Time.deltaTime;
        Debug.Log(y);
        transform.Rotate(0, 36f * Time.deltaTime, 0);
        transform.Translate(new Vector3(0, y, 0));
    }
}
