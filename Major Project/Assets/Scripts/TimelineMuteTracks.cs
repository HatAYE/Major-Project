using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineMuteTracks : MonoBehaviour
{
    PlayableDirector playableDirector; // Add reference to attached PlayableDirector component from Inspector
    TimelineAsset someTimelineAsset; 
    TrackAsset someTimelineTrackAsset;

    // Use this for initialization
    void Start()
    {
        playableDirector=GetComponent<PlayableDirector>();
        someTimelineAsset = (TimelineAsset)playableDirector.playableAsset;

    }

    public void GetTrack(int trackIndex)
    {
        someTimelineTrackAsset = someTimelineAsset.GetOutputTrack(trackIndex);
    }
    public void MuteUnmuteTrack(bool mute)
    {
        someTimelineTrackAsset.muted = mute;

        double t = playableDirector.time; // Store elapsed time
        playableDirector.RebuildGraph(); // Rebuild graph
        playableDirector.time = t; // Restore elapsed time
    }
}
