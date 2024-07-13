using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;
	List<Rigidbody2D> attractedRigidbodies = new List<Rigidbody2D>();

	void Awake()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = FindObjectOfType<CircleCollider2D>();
    }
    private void Update()
    {
        playerCollector.radius = player.CurrentCollectRange;
		List<int> toRemove = new List<int>();

		for (int i = 0; i < attractedRigidbodies.Count; i++)
		{
			if (attractedRigidbodies[i] == null)
			{
				//toRemove.Add(i);
				continue;
			}

			//Vector2 pointing from the item to the owner
			Vector2 forceDirection = (transform.position - attractedRigidbodies[i].transform.position).normalized;
			//Applies force to the item in the forceDirection with pullSpeed
			attractedRigidbodies[i].velocity = forceDirection * pullSpeed;
		}

		// Remove all null objects in our attracted rigidbodies.
		//foreach(int i in toRemove) attractedRigidbodies.RemoveAt(i);
		attractedRigidbodies.Clear();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
        {
			//Pull item animation
			attractedRigidbodies.Add(collision.gameObject.GetComponent<Rigidbody2D>());
			//collect item call
			collectible.Collect();
        }
    }
   
}
