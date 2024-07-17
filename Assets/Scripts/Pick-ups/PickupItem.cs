using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public bool hasBeenCollected = false;
//Magnet
    public float lifespan = 0.5f;
    protected PlayerStats target;
    protected float speed;
    Vector2 initialPosition;
    float randomOffset;


    //Hieu ung
    [System.Serializable]
    public struct BobbingAnimation
    {
        public float frequency;
        public Vector2 direction;
    }
    public BobbingAnimation bobbingAnimation = new BobbingAnimation
    {
        frequency = 2f,
        direction = new Vector2(0, 0.3f)
    };
    [Header("Bonus")]
    public int exp;
    public int healthToRestore;

    protected virtual void Start()
    {
        initialPosition = transform.position;
        randomOffset = Random.Range(0, bobbingAnimation.frequency);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;  // Disable gravity for the item
        }
    }
    protected virtual void Update()
    {
        if (target) // item magnet to player
        {
            Vector2 distance = target.transform.position - transform.position;
            if (distance.sqrMagnitude > speed * speed * Time.deltaTime)
            {
                transform.position += (Vector3)distance.normalized * speed * Time.deltaTime; // cap nhat vi tri lien tuc bam theo player
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //hieu ung bobbing tai cho
            transform.position = initialPosition + bobbingAnimation.direction * Mathf.Sin((Time.time+randomOffset) * bobbingAnimation.frequency);
        }
    }

    public virtual bool Collect(PlayerStats target, float speed, float lifespan = 3f)
    {
        if (!this.target)
        {
            this.target = target;
            this.speed = speed;
            if (lifespan > 0) this.lifespan = lifespan;
            Destroy(gameObject, Mathf.Max(0.01f, this.lifespan));
            return true;
        }
        return false;
    }

    protected virtual void OnDestroy()
    {
        if (!target)
        {
            return;
        }
        if (exp !=0)
        {
            target.IncreaseExp(exp);
        }
        if (healthToRestore != 0)
        {
            target.RestoreHealth(healthToRestore);
        }
    }
/*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }*/

}
