using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (_instance==null)
        {
            _instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public static MusicManager instance()
    {
        return _instance;
    }

    [SerializeField]
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
