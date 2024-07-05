using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    // Movement
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector] // check diem cuoi di chuyen quay huong nao de flip + auto ban vu khi
    public float lastHorizontalVector;
    [HideInInspector] // check diem cuoi di chuyen quay huong nao de flip + auto ban vu khi
    public float lastVerticalVector;
    

    //References
    PlayerStats player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        player=GetComponent<PlayerStats>();
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
        moveDir = new Vector2 (moveX, moveY).normalized;
        if (moveDir.x != 0 )
        {
            lastHorizontalVector = moveDir.x;
        }
        if (moveDir.y != 0 )
        {
            lastVerticalVector = moveDir.y;

        }
        if(moveDir.x !=0 && moveDir.y != 0)
        {
        }
    }
    void Move()
    {
        if (GameManager.instance.isPause || GameManager.instance.isGameOver || GameManager.instance.isChoosingUpgrade)
        {
            return;
        }
        rb.velocity = new Vector2(moveDir.x*player.CurrentMoveSpeed, moveDir.y* player.CurrentMoveSpeed);
    }
}
