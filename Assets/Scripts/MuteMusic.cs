using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteMusic : MonoBehaviour {

    public bool muted = false;
    
    public void ButtonMute()
    {
        muted = !muted;
        if (muted)
        {
            Debug.Log("mute value: " + muted);
            GameObject.Find("BGM1").GetComponent<AudioSource>().Pause();
        }
        else
        {
            Debug.Log("mute value: " + muted);
            GameObject.Find("BGM1").GetComponent<AudioSource>().UnPause();
        }
    }
    
}
