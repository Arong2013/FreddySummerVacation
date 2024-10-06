using UnityEngine;

public enum SFX_SOURCE_INDEX
{
    NORMAL_SFX,///위치 상관없이 들리는 소리
    NEIGHBOR_NOISE,///위에서 들릴 소리
    DOOR_SFX,///문쪽에서 들릴 소리
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
    }
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;  // BGM 클립 설정
            bgmSource.loop = true;     // 배경음악은 루프되도록 설정
            bgmSource.volume = bgmVolume;  // BGM 소리 크기 설정
            bgmSource.Play();
            Debug.Log("BGM 재생됨: " + bgmClip.name);
        }
        else
        {
            Debug.LogError("BGM 클립이 유효하지 않습니다.");
        }
    }

    // SFX 재생 메소드
    public void PlaySFX(AudioClip sfxClip, int sourceIndex = 0)
    {
        if (sfxClip != null && sourceIndex < sfxSources.Length)
        {
            sfxSources[sourceIndex].volume = sfxVolume;  // SFX 소리 크기 설정
            sfxSources[sourceIndex].PlayOneShot(sfxClip);
            Debug.Log("SFX 재생됨: " + sfxClip.name);
        }
        else
        {
            Debug.LogError("SFX 소스나 클립이 유효하지 않습니다.");
        }
    }
    public bool IsPlayingAudioSource(AudioClip sfxClip, int sourceIndex = 0)
    {
        if (sfxSources[sourceIndex].isPlaying && sfxSources[sourceIndex].clip == sfxClip)
        {
            return true;
        }
        return false;
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
        Debug.Log("SFX 소리 크기 설정됨: " + sfxVolume);
    }
}
