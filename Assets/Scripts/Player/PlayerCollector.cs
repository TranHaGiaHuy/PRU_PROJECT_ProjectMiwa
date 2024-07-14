using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D detector;
    public float pullSpeed;



    void Awake()
    {
       player = GetComponentInParent<PlayerStats>();
    }
    public void SetRadius(float r)
    {
        if (!detector)
        {
            detector = GetComponent<CircleCollider2D>();
            detector.radius = r;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PickupItem p))
        {
			
			//collect item call
			p.Collect(player,pullSpeed);
        }
    }
   
}
