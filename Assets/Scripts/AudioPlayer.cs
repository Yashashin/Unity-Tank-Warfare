using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer:MonoBehaviour { 
   public void playAudio(AudioClip audio,Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(audio, pos);
    }
}
