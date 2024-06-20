using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Used to count time up or down. Can be set and run from inspector, or can be manually set and run from another script
/// This script also holds an event for when the timer runs out, so that functionality can be easily extended.
/// </summary>
public class Timer : MonoBehaviour
{
    // Class Variables
    [Header("Timer Settings")]
    public bool countDown = true;              // Whether the timer counts down or up
    public float maxTime = 10.0f;              // Maximum time for the timer
    public bool finite = true;                 // Whether the timer should finish after reaching maxTime or count infinitely
    public bool startOnAwake = false;

    // Events
    public UnityEvent onTimerFinished;         // Event invoked when the timer finishes

    // Public Variables
    public bool IsRunning; // Flag to indicate if the timer is running
    public float CurrentTime; // Current time of the timer

    // Start is called before the first frame update
    void Start()
    {
        // Start the timer
        if(startOnAwake) StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            if (countDown)
            {
                CurrentTime -= Time.deltaTime;
                if (CurrentTime <= 0.0f)
                {
                    CurrentTime = 0.0f;
                    TimerFinished();
                }
            }
            else
            {
                CurrentTime += Time.deltaTime;
                if (finite && CurrentTime >= maxTime)
                {
                    CurrentTime = maxTime;
                    TimerFinished();
                }
            }
        }
    }

    // Start the timer
    public void StartTimer()
    {
        IsRunning = true;
        CurrentTime = countDown ? maxTime : 0.0f;
    }

    // Stop the timer
    public void StopTimer()
    {
        IsRunning = false;
    }

    // Function to manually finish the timer
    public void FinishTimer()
    {
        CurrentTime = countDown ? 0.0f : maxTime;
        TimerFinished();
    }

    // Timer finished callback
    private void TimerFinished()
    {
        IsRunning = false;
        onTimerFinished.Invoke();
    }

    // Format the time in mm:ss format
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(CurrentTime / 60.0f);
        int seconds = Mathf.FloorToInt(CurrentTime % 60.0f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Add your own code here for any additional functionality or customization
}
