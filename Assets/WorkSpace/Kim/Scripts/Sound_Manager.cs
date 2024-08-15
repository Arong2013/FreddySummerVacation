using UnityEngine;

public enum SOUND_INDEX
{
    WARNING_SOUND,
    NEIGHBOR_NOISE
}
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    // SoundManager 인스턴스에 접근할 수 있는 프로퍼티
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Scene에서 SoundManager 찾아서 인스턴스화한다.
                instance = FindObjectOfType<SoundManager>();

                // Scene에 SoundManager 없으면 새로 생성한다.
                if (instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }
    public AudioSource audioSource;  // 오디오 소스 컴포넌트
    public AudioClip []soundEffect;    // 재생할 사운드 클립

    void Start()
    {
        // 게임 시작 시 배경음악 재생
        //audioSource.Play();
    }

    public void PlaySound(SOUND_INDEX index)
    {
        // 특정 이벤트에서 효과음 재생
        //audioSource.PlayOneShot(soundEffect[(int)index]);아직 사운드 없음
        if(index == SOUND_INDEX.WARNING_SOUND)
        {
            Debug.Log("경보 재생됨");
            Villain_Manager.Instance.Warning();
        }
        else
        {
            Debug.Log("층간소음 재생됨");
        }
        
    }
}
