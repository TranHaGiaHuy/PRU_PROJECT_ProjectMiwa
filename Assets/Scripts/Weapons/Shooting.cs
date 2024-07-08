using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Vector3 mousePosi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPause || GameManager.instance.isGameOver || GameManager.instance.isChoosingUpgrade)
        {
            return;
        }
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosi - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }
}
