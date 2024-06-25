using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;

    private void Update()
    {
        // 이동
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        if (movement != Vector2.zero)
        {
            RB.velocity = movement * 4;
            AN.SetBool("walk", true);
            SR.flipX = moveHorizontal == -1;
        }
    }


}
