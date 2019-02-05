using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get { return _instance; }}
    private static AudioManager _instance;

    public List<AudioClip> noteClips;
    private List<AudioSource> sources = new List<AudioSource>();
    private int currentSource = 0;

    public float starDelayTime = 0.1f;

    private const int NUM_SOURCES = 8;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < NUM_SOURCES; i++){
            var s = gameObject.AddComponent<AudioSource>();
            s.volume = 0.75f;
            sources.Add(s);
        }
    }

    public void PlayStarNote(string note){
        AudioClip c = null;
        for (int i=0; i < noteClips.Count; i++){
            if (noteClips[i].name == note){
                c = noteClips[i];
                break;
            }
        }
        if (c != null){
            sources[currentSource].clip = c;
            sources[currentSource].PlayDelayed(starDelayTime);
            currentSource++;
            currentSource = currentSource % sources.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
