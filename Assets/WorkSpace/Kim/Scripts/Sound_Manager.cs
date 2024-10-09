using UnityEngine;

public enum SFX_SOURCE_INDEX
{
    NORMAL_SFX,    // 위치 상관없이 들리는 소리
    NEIGHBOR_NOISE, // 위에서 들리는 소리
    DOOR_SFX,      // 문쪽에서 들리는 소리
}

public class Sound_Manager : Singleton<Sound_Manager>
{
    public AudioSource[] sfxSources;  // SFX 오디오 소스 컴포넌트 배열
    public AudioSource bgmSource;     // BGM 오디오 소스 (배경 음악은 하나의 소스 사용)
    public float bgmVolume = 1.0f;    // BGM 소리 크기
    public float sfxVolume = 1.0f;    // SFX 소리 크기

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); // 씬 전환 시 오디오 유지
    }

    // BGM 재생 메소드
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;  // BGM 클립 설정
            bgmSource.loop = true;     // 배경 음악은 루프되도록 설정
            bgmSource.volume = bgmVolume;  // BGM 소리 크기 설정
            bgmSource.Play();
            Debug.Log("BGM 재생됨: " + bgmClip.name);
        }
        else
        {
            Debug.LogError("BGM 클립이 유효하지 않습니다.");
        }
    }

    // SFX 재생 메소드 (sourceIndex가 0이면 빈 곳에 재생)
    public void PlaySFX(AudioClip sfxClip, int sourceIndex = 0)
    {
        if (sfxClip != null)
        {
            // sourceIndex가 0일 경우, 빈 오디오 소스를 찾아 재생
            if (sourceIndex == 0)
            {
                foreach (AudioSource source in sfxSources)
                {
                    if (!source.isPlaying)
                    {
                        source.volume = sfxVolume;  // SFX 소리 크기 설정
                        source.PlayOneShot(sfxClip);  // SFX 재생
                        Debug.Log("SFX 재생됨 (빈 소스 사용): " + sfxClip.name);
                        return;  // 첫 번째 빈 소스에 재생 후 메소드 종료
                    }
                }
                Debug.LogWarning("모든 SFX 소스가 사용 중입니다. 클립을 재생할 수 없습니다.");
            }
            else if (sourceIndex < sfxSources.Length)
            {
                // sourceIndex가 0이 아니고 유효한 인덱스일 경우 해당 소스에서 재생
                sfxSources[sourceIndex].volume = sfxVolume;  // SFX 소리 크기 설정
                sfxSources[sourceIndex].PlayOneShot(sfxClip);
                Debug.Log("SFX 재생됨 (지정된 소스): " + sfxClip.name);
            }
            else
            {
                Debug.LogError("지정된 SFX 소스 인덱스가 유효하지 않습니다.");
            }
        }
        else
        {
            Debug.LogError("SFX 클립이 유효하지 않습니다.");
        }
    }


    // 현재 오디오 소스에서 특정 SFX가 재생 중인지 확인
    public bool IsPlayingAudioSource(AudioClip sfxClip)
    {
        // 모든 sfxSources를 순회하며 주어진 sfxClip이 재생 중인지 확인
        foreach (AudioSource source in sfxSources)
        {
            if (source.isPlaying && source.clip == sfxClip)
            {
                return true; // 해당 클립이 재생 중이면 true 반환
            }
        }
        return false; // 클립이 재생 중이지 않으면 false 반환
    }
    // BGM 정지 메소드
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
            Debug.Log("BGM 중지됨");
        }
    }

    // BGM 일시정지 메소드
    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();
            Debug.Log("BGM 일시정지됨");
        }
    }

    // BGM 소리 크기 조절 메소드 (슬라이더 연동)
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.volume = bgmVolume;  // 재생 중인 BGM 소리 크기 업데이트
        }
        Debug.Log("BGM 소리 크기 설정됨: " + bgmVolume);
    }

    // SFX 소리 크기 조절 메소드 (슬라이더 연동)
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        for (int i = 0; i < sfxSources.Length; i++)
        {
            sfxSources[i].volume = sfxVolume;  // 모든 SFX 소스에 대해 볼륨 조정
        }
        Debug.Log("SFX 소리 크기 설정됨: " + sfxVolume);
    }

    // 특정 SFX 정지 메소드
    public void StopSFX(int sourceIndex = 0)
    {
        if (sourceIndex < sfxSources.Length && sfxSources[sourceIndex].isPlaying)
        {
            sfxSources[sourceIndex].Stop();
            Debug.Log("SFX 정지됨: SourceIndex " + sourceIndex);
        }
        else
        {
            Debug.LogError("유효하지 않은 SFX 소스입니다.");
        }
    }
}
