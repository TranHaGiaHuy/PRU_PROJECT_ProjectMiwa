using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booping : MonoBehaviour
{
    public float speedMove;
    public float rangeMove;
    public Vector3 direction;
    Vector3 initialPosition;
    PickupItem pickUp;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        pickUp = GetComponent<PickupItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUp && !pickUp.hasBeenCollected)
        {
        transform.position = initialPosition + direction*Mathf.Sin(Time.time * speedMove) * rangeMove;
        }
    }
}
