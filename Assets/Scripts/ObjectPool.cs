using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private int poolSize;

    private List<GameObject> deadEnemies = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            obj.transform.parent = transform;
            obj.transform.position = new Vector3(300, -300, 300);
            deadEnemies.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject ActivateObject()
    {
        if (deadEnemies.Count > 0)
        {
            GameObject obj = deadEnemies[0];
            deadEnemies.RemoveAt(0);
            activeEnemies.Add(obj);
            obj.transform.parent = null;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // first should be the oldest
            return activeEnemies[0];
        }
    }

    public void DeactivateObject(GameObject obj)
    {
        if (activeEnemies.Count > 0)
        {
            obj.SetActive(false);
            obj.transform.position = new Vector3(300, -300, 300);
            IDeinitialize deinitializable = obj.GetComponent<IDeinitialize>();

            if (deinitializable != null)
            {
                deinitializable.Deinitialize();
            }

            if (activeEnemies.Contains(obj))
            {
                activeEnemies.Remove(obj);
            }
            else
            {
                Debug.LogError($"Enemy {obj.name} was not in the active enemy pool when deactivated!");
            }
            deadEnemies.Add(obj);
            obj.transform.parent = transform;
        }
        else
        {
            Debug.LogError($"Enemy {obj.name} was not in the active enemy pool when deactivated! 0 active enemies!");
        }
    }
}
