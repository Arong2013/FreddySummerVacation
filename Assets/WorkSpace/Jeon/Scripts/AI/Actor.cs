using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    Rigidbody2D RB;
    Animator AN;
    SpriteRenderer SR;
    Dictionary<KeyCode, Action> keyActions = new Dictionary<KeyCode, Action>();

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        AN = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        // 이동
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        RB.velocity = movement * 4;
        SR.flipX = moveHorizontal == -1;

        AN.SetFloat("WalkX", moveHorizontal);
        AN.SetFloat("WalkY", moveVertical);


        foreach (var keyAction in keyActions)
        {
            if (Input.GetKeyDown(keyAction.Key))
            {
                keyAction.Value.Invoke();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Door>(out Door component))
        {
            keyActions.Add(KeyCode.Space, component.OpenDoor);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Door>(out Door component))
        {
            keyActions.Remove(KeyCode.Space);
        }
    }
}
