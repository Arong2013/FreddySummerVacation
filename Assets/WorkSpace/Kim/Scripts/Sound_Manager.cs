using UnityEngine;

public enum AUDIO_INDEX
{
    NEIGHBOR_NOISE, //전용 오디오 소스
    WALKING,//전용 오디오 소스
    BGM, //기본 오디오 소스
    WARNING_SOUND,//기본 오디오 소스
}
public class Sound_Manager : Singleton<Sound_Manager>
{
/*     private static SoundManager instance;

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
    } */
    public AudioSource []audioSource;  // 오디오 소스 컴포넌트
    public AudioClip []audioClips;    // 재생할 사운드 클립

    public AudioClip GetAudioClip(AUDIO_INDEX index) 
    {
        return audioClips[(int)index];
    }

    void Start()
    {
        //audioSource.Play();
    }

    public void PlaySound(AUDIO_INDEX index)
    {
        audioSource[(int)index].PlayOneShot(audioClips[(int)index]);
        if(index == AUDIO_INDEX.WARNING_SOUND)
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
