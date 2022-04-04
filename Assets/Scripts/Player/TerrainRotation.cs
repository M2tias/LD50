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
        Vector3 tp = terrain.transform.position; //GetPosition();
        var normalizedPos = new Vector2(
            Mathf.InverseLerp(0f, terrain.terrainData.size.x, transform.position.x - tp.x),
            Mathf.InverseLerp(0f, terrain.terrainData.size.z, transform.position.z - tp.z)
        );

        transform.localRotation = Quaternion.FromToRotation(Vector3.up, terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y));
    }
}
