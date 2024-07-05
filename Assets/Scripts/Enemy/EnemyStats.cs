using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
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


    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.6f;
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        movement = GetComponent<EnemyMovement>();
    }
     void Update()
    {
        if (Vector2.Distance(transform.position,player.transform.position) >=despawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {

        currentHealth -= dmg;
        StartCoroutine(DamageFlash());
        if (knockbackForce>0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce,knockbackDuration);
        }
        if (currentHealth <= 0)
        {

            Kill();
        }
    }
    //Coroutine nay lam cho enemy bi giat do khi bi danh trung
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
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
