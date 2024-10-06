using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 인스턴스가 없는 경우 씬에서 찾는다
                instance = FindObjectOfType<T>();
                
                // 그래도 없으면 새로운 인스턴스를 생성한다
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }

                DontDestroyOnLoad(instance.gameObject); // 생성된 인스턴스는 파괴되지 않도록 설정
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject); // 첫 인스턴스는 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로운 객체를 파괴
        }
    }
}
