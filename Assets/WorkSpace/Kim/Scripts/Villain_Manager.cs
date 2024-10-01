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
    A,
    B,
    C,
    D,
    E
}
public class Villain_Manager : Singleton<Villain_Manager>
{
/*     private static Villain_Manager instance;

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
    } */
    [SerializeField] Villain[] villains;
    Coroutine coroutine;
    Coroutine cycle_Coroutine;

    public Villain GetVillain(VILLAIN_INDEX index)
    {
        return villains[(int)index];
    }
    public void StartMove(VILLAIN_INDEX index, VILLAIN_DIFFICULTY dIFFICULTY)
    {
        villains[(int)index].Initialize(dIFFICULTY);
    }
    public void Stop_All_Villain()
    {
        foreach(var v in villains)
            v.Stop();
    }
    public void Door_Closing()
    {
        foreach(var v in villains)
            v.IsClosing = true;
    }
    public void Warning()
    {
        foreach(var v in villains)
            v.IsWaring = true;
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
    public void villain_Cycle(int day)
    {
        if(day == 1)
        {
            cycle_Coroutine = StartCoroutine(villain_Cycle_Timer(new VILLAIN_INDEX[] {VILLAIN_INDEX.A, VILLAIN_INDEX.B, VILLAIN_INDEX.C, VILLAIN_INDEX.E}));
        }
        else if(day == 2)
        {
            cycle_Coroutine = StartCoroutine(villain_Cycle_Timer(new VILLAIN_INDEX[] {VILLAIN_INDEX.A, VILLAIN_INDEX.C, VILLAIN_INDEX.B, VILLAIN_INDEX.D, VILLAIN_INDEX.E}));
        }
    }
    IEnumerator villain_Cycle_Timer(VILLAIN_INDEX[] index_list)
    {
        for(int i = 0; i < index_list.Length; i++)
        {
            villains[(int)index_list[i]].StartMove();
            yield return new WaitForSeconds(villains[(int)index_list[i]].GetMoveDelay);
        }
    }
}
