using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private List<Sounds> allSoundClips = new List<Sounds>();
    
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for(int i = 0; i < allSoundClips.Count; i++)
        {
            GameObject newOb = new GameObject(allSoundClips[i].name+" sound player");
            newOb.transform.parent = this.transform;
            newOb.transform.localPosition = Vector3.zero;
            newOb.AddComponent<AudioSource>();

            allSoundClips[i].SetSource(newOb.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(string soundName)
    {
        getSound(soundName).PlaySound();
    }
    public void PlaySoundOnLoop(string soundName)
    {
        getSound(soundName).PlaySoundOnLoop();
    }
    public void StopSoundLoop(string soundName)
    {
        getSound(soundName).StopLoop();
    }

    private Sounds getSound(string soundName)
    {
        for (int i = 0; i < allSoundClips.Count; i++)
        {
            if (allSoundClips[i].name == soundName)
            {
                return allSoundClips[i];
            }
        }

        return null;
    }
}

[System.Serializable]
class Sounds
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float soundVolume;
    [Range(0, 0.5f)]
    public float randomVolume;
    public bool mute;
    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.volume = soundVolume;
        source.clip = clip;
    }

    public void PlaySound()
    {
        if (!mute)
        {
            source.volume = soundVolume * (1 + Random.Range(-randomVolume / 2, randomVolume / 2));
            source.Play();
        }
    }
    public void PlaySoundOnLoop()
    {
        if (!mute)
        {
            source.volume = soundVolume * (1 + Random.Range(-randomVolume / 2, randomVolume / 2));
            source.loop = true;
            source.Play();
        }
    }
    public void StopLoop()
    {
        source.loop = true;
        source.Stop();
    }
}