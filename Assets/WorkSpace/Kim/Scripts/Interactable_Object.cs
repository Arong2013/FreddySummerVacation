using UnityEngine;

public class Interactable_Object : MonoBehaviour
{
    // 기본 속성
    public string objectName;
    public int objectID;


    // // 상호작용 시작/종료 시 이벤트
    // public delegate void InteractionHandler();
    // public event InteractionHandler OnInteract;
    // public event InteractionHandler OnStopInteract;

    // 상호작용 메서드
    public void Interact()
    {
        // 상호작용 이벤트 발생
        //OnInteract?.Invoke();
        Debug.Log($"{objectName} interacted with.");

        // 상호작용 로직 호출
        PerformInteraction();
    }

    // 상호작용 로직 (가상 메서드)
    protected virtual void PerformInteraction()
    {
        // 상속받는 클래스에서 이 메서드를 재정의하여 구체적인 상호작용 로직을 구현합니다.
    }

    // 상호작용 중지 메서드
    public void StopInteract()
    {
        // 상호작용 종료 이벤트 발생
        //OnStopInteract?.Invoke();
        Debug.Log($"{objectName} interaction stopped.");
    }
}
