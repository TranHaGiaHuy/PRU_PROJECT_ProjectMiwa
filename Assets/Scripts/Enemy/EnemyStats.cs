using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    PlayerStats player;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
    }
     void Update()
    {
        if (Vector2.Distance(transform.position,player.transform.position) >=despawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {

            Kill();
        }
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
     void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }
    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }
        EnemySpawnner enemySpawnner = FindObjectOfType<EnemySpawnner>();
        enemySpawnner.OnEnemyKilled();
    }

    public void ReturnEnemy()
    {
        EnemySpawnner es = FindObjectOfType<EnemySpawnner>();
        transform.position = player.transform.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }


}
