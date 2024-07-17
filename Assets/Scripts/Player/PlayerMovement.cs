using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const float DEFAULT_MOVESPEED = 5F;

    Rigidbody2D rb;

    // Movement
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector] // check diem cuoi di chuyen quay huong nao de flip + auto ban vu khi
    public float lastHorizontalVector;
    [HideInInspector] // check diem cuoi di chuyen quay huong nao de flip + auto ban vu khi
    public float lastVerticalVector;
    public Vector2 lastMovedVector;

    //References
    PlayerStats player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<PlayerStats>();
        lastMovedVector = new Vector2(1, 0);
    }
    // Update is called once per frame
    private void Update()
    {
        CheckInputDirection();
    }
    private void FixedUpdate()
    {
        Move();
    }
    void CheckInputDirection()
    {
        if (GameManager.instance.isPause || GameManager.instance.isGameOver || GameManager.instance.isChoosingUpgrade)
        {
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal"); // 1 pressed
        float moveY = Input.GetAxisRaw("Vertical");   // 0 not pressed\
        moveDir = new Vector2(moveX, moveY).normalized;
        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);
        }
        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0, lastVerticalVector);

        }
        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
    }
    void Move()
    {
        if (GameManager.instance.isPause || GameManager.instance.isGameOver || GameManager.instance.isChoosingUpgrade)
        {
            return;
        }
        rb.velocity = moveDir * DEFAULT_MOVESPEED * player.Stats.moveSpeed;
      //  rb.velocity = new Vector2(moveDir.x * player.Stats.moveSpeed, moveDir.y * player.Stats.moveSpeed);

    }

}
