using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VILLAIN_DIFFICULTY
{
    EASY,
    NORMAL,
    HARD
}
public enum VILLAIN_INDEX
{
    E
}
public class Villain_Manager : MonoBehaviour
{
    private static Villain_Manager instance;

    // Villain_Manager 인스턴스에 접근할 수 있는 프로퍼티
    public static Villain_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                // Scene에서 Villain_Manager 찾아서 인스턴스화한다.
                instance = FindObjectOfType<Villain_Manager>();

                // Scene에 Game_Manager 없으면 새로 생성한다.
                if (instance == null)
                {
                    GameObject obj = new GameObject("Villain_Manager");
                    instance = obj.AddComponent<Villain_Manager>();
                }
            }
            return instance;
        }
    }
    [SerializeField] Villain[] villains;
    Coroutine coroutine;

    public Villain GetVillain(VILLAIN_INDEX index)
    {
        return villains[(int)index];
    }
    public void StartMove(VILLAIN_INDEX index)
    {
        villains[(int)index].Initialize();
    }
    public void SetTimer_Villain_E(bool StartOrStop)
    {
        if(StartOrStop)
        {
            coroutine = StartCoroutine(villains[(int)VILLAIN_INDEX.E].CheckTime());
        }
        else
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }
    }
    public void SetVillainDifficulty(VILLAIN_INDEX index, VILLAIN_DIFFICULTY difficulty)
    {
        villains[(int)index].SetDifficulty(difficulty);
    }
}
