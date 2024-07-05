using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DropRateManager;
using static UnityEditor.Progress;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }
    public List<Drops> drops;

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();
        foreach (Drops item in drops)
        {
            if (randomNumber <= item.dropRate)
            {
                possibleDrops.Add(item);
            }
        }
        if (possibleDrops.Count > 0)
        {
            Drops drop = possibleDrops[UnityEngine.Random.Range(0,possibleDrops.Count)];
            Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
        }
    }

}
