using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Awake()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.moveDir.x!=0 ||pm.moveDir.y!=0)
        {
            am.SetBool("Move",true);
     
        }
        else
        {
            am.SetBool("Move", false);

        }
        flipDirection();
    }
    void flipDirection()
    {
        if (pm.lastHorizontalVector<0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
        
    }
    // Allows us to update the default sprite and animations.
    public void SetSprites(Sprite sprite, RuntimeAnimatorController controller = null)
    {
        sr = GetComponent<SpriteRenderer>();
        if (controller)
        {
            am = GetComponent<Animator>();
            am.runtimeAnimatorController = controller;
        }
        sr.sprite = sprite;
    }
}
