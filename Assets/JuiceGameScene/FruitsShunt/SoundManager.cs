using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
    public static SoundManager _instance = null;
    public AudioClip[] juiceGameSound;
    AudioSource myAudio;

    public enum SOUNDNUM
    {
        FX_FRUIT_BURST, FX_FRUIT_MOVE, FX_JUICE_DISCARD, FX_JUICE_FILLSTART,
        FX_JUICE_FILLSTOP, FX_JUICE_OVERFLOW, FX_JUICE_SELECT, FX_SERVE_BED,
        FX_SERVE_COMBO, FX_SERVE_COOL, FX_END
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static SoundManager Instance
    {
        get
        {
            if (null == _instance)
                _instance = GameObject.FindObjectOfType<SoundManager>();

            return _instance;
        }
    }

    public void PlayOneShot(SOUNDNUM _SoundNum)
    {
        myAudio.PlayOneShot(juiceGameSound[(int)_SoundNum]);
    }

    // Use this for initialization
    void Start () {
        myAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
