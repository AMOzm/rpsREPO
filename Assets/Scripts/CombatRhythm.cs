using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRhythm : MonoBehaviour
{
    public bool PlayerSuccess;
    public bool PlayerFail;
    public bool PlayerTied;
    public bool isPlaying = false; // Set this to true to start the metronome
    private float bpm = 100f;      // Beats per minute
    private int beatCount = 0;     // Current beat (1, 2, 3, 4)

    void Start()
    {
        // Calculate interval between beats based on BPM
        float interval = 60f / bpm;

        // Start the metronome cycle
        InvokeRepeating("PlayBeat", interval, interval);
    }

    void PlayBeat()
    {
        if (isPlaying)
        {
            // Increment the beat count (cycles through 1 to 8)
            beatCount = (beatCount % 8) + 1;

            // Trigger actions based on the current beat
            // switch (beatCount)
            // {
            //     case 1:
            //         EnemyPOne();
            //         break;
            //     case 2:
            //         EnemyPTwo();
            //         break;
            //     case 3:
            //         PlayerPOne();
            //         break;
            //     case 4:
            //         PlayerPTwo();
            //         break;
            //     case 5:
            //         Results();
            //         break;
            //     else:
            //         break;
            // }

            // Output the beat count for debugging
            //Debug.Log(beatCount);
        }
    }

    void OnDisable()
    {
        // Ensure the repeating invocation stops when the script is disabled
        CancelInvoke("PlayBeat");
    }
    private void EnemyPOne(){
        Debug.Log("call enemy play if play on 1");
    }
    private void EnemyPTwo(){
        Debug.Log("call enemy play if play on 2");
    }
    private void PlayerPOne(){
        Debug.Log("call player play if enemy play on 1");
    }
    private void PlayerPTwo(){
        Debug.Log("call player play if enemy play on 2");
    }
    private void results(){

    }
}
