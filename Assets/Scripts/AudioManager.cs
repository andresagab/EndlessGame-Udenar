using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
        }

        PlaySound("MainTheme");

    }

    public void PlaySound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
                sound.source.Play();
        }
    }


}
