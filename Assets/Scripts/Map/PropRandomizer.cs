using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropGenerizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        SpawnProp();
    }
 
    void SpawnProp()
    {
        foreach (GameObject sp in propSpawnPoints)
        { 
            int rand = Random.Range(0, propSpawnPoints.Count);
            GameObject prop = Instantiate(propPrefabs[rand], sp.transform.position,Quaternion.identity);
            prop.transform.parent = sp.transform;
        }
    }
}
