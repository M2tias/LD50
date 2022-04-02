using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRotation : MonoBehaviour
{
    [SerializeField]
    Terrain terrain;
    [SerializeField]
    Transform parent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 parentForward = parent.position + parent.forward * 0.1f;
        // float h = terrain.SampleHeight(parent.position);
        // float hForward = terrain.SampleHeight(parentForward);
        // transform.localRotation = Quaternion.Euler(Vector3.Angle(parentForward, new Vector3(parentForward.x, hForward*5, parentForward.z)) * 10, 0, 0);
        // Debug.Log($"{parentForward}");
        // Debug.Log($"{new Vector3(parentForward.x, hForward, parentForward.z)}");
        // Debug.Log($"{Vector3.Angle(parentForward, new Vector3(parentForward.x, hForward, parentForward.z))}");

        //Vector3 parentForward = parent.position + parent.forward * 0.1f;
        // float h = terrain.SampleHeight(parent.position);
        // float hForward = terrain.SampleHeight(parent.position + parent.forward);
        // // transform.localRotation = Quaternion.Euler(Vector3.Angle(parent.forward, new Vector3(parent.forward.x, hForward, parent.forward.z)), 0, 0);
        // transform.localRotation = Quaternion.Euler(Vector3.SignedAngle(parent.forward, new Vector3(parent.forward.x, hForward-h, parent.forward.z), parent.right), 0, 0);
        // Debug.Log($"{parent.forward}");
        // Debug.Log($"{new Vector3(parent.forward.x, hForward, parent.forward.z)}");
        // Debug.DrawLine(parent.position, parent.position + parent.forward, Color.cyan);
        // Debug.DrawLine(parent.position, parent.position + new Vector3(parent.forward.x, hForward-h, parent.forward.z), Color.magenta);

        // transform.localRotation = Quaternion.LookRotation(terrain.terrainData.GetInterpolatedNormal(transform.position.x / terrain.terrainData.size.x, transform.position.y / terrain.terrainData.size.y));


        Vector3 tp = terrain.transform.position; //GetPosition();
        var normalizedPos = new Vector2(
            Mathf.InverseLerp(0f, terrain.terrainData.size.x, transform.position.x - tp.x),
            Mathf.InverseLerp(0f, terrain.terrainData.size.z, transform.position.z - tp.z)
        );

        transform.localRotation = Quaternion.FromToRotation(Vector3.up, terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y));

        // for (float i = -25; i < 25; i += 0.5f)
        // {
        //     for (float j = -25; j < 25; j += 0.5f)
        //     {
        //         Vector3 tp = terrain.transform.position; //GetPosition();
        //         float h = terrain.SampleHeight(new Vector3(i, 0, j));
        //         var normalizedPos = new Vector2(
        //             Mathf.InverseLerp(0f, terrain.terrainData.size.x, i - tp.x),
        //             Mathf.InverseLerp(0f, terrain.terrainData.size.z, j - tp.z)
        //         );
        //         Debug.DrawRay(new Vector3(i, h, j), terrain.terrainData.GetInterpolatedNormal((i - tp.x) / terrain.terrainData.size.x, (j - tp.z) / terrain.terrainData.size.z), Color.red);
        //         Debug.DrawRay(new Vector3(i, h, j), terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y), Color.blue);
        //     }
        // }
    }
}
