using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villain : MonoBehaviour
{
    bool move_delaying = false; //true일때 이동못함
    [SerializeField] Transform[] move_Pos; 
    void Start()
    {
        
    }
    public void Move(int pos_index)
    {
        if(move_delaying == true)
            return;
        transform.position = move_Pos[pos_index].position;
    }
}
