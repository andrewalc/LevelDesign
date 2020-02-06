using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelinePlayer : MonoBehaviour
{
    public TimelineAsset asset;
    private PlayableDirector director;

    public bool playOnce;
    private bool hasPlayedOnce;
    // Start is called before the first frame update
    void Start()
    {
        director = FindObjectOfType<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("No timeline director found!");
        }
    }

    public void PlayAsset()
    {
        if (playOnce && hasPlayedOnce) return; // Play the timeline asset once if flag set
        director.playableAsset = asset;
        //rebuild for runtime playing
        director.RebuildGraph();
        director.time = 0.0;
        print("play" + asset.name);
        director.Play();
        hasPlayedOnce = true;
    }
}