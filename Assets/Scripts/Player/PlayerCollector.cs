using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    void Awake()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = FindObjectOfType<CircleCollider2D>();
    }
    private void Update()
    {
        playerCollector.radius = player.CurrentCollectRange;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
        {
            //Pull item animation
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(forceDirection*pullSpeed);

            //collect item call
            collectible.Collect();
        }
    }
   
}
