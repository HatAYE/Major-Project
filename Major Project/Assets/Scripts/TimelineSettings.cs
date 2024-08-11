using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineSettings : MonoBehaviour
{
    PlayableDirector director;
    [SerializeField] KeyCode skipKey= KeyCode.Space;
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += Deactivate;
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state == PlayState.Playing && GameManager.instance != null)
        {
            GameManager.instance.ChangeState(gameStates.frozen);
        }
        if (Input.GetKeyDown(skipKey))
        {
            director.time = director.duration;
            director.Evaluate();
        }
    }

    void Deactivate(PlayableDirector director)
    {
        if(GameManager.instance != null)
        GameManager.instance.ChangeState(gameStates.playing);
        gameObject.SetActive(false);
    }
}
