using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class TimerBarBehaviour : MonoBehaviour
{
    public static TimerBarBehaviour Instance;
    
    private static readonly int barWidth = 1080;
    private static readonly int barHeight = 90;
    private static readonly int halfBarWidth = barWidth / 2;
    
    /// <summary> bar time indicator </summary>
    [SerializeField] private RectTransform elapsedRect;
    /// <summary> bar time indicator position (from R) </summary>
    private float rightPosition = barWidth;

    public bool timerStarted = false;
    private MovementRecorder movementRecorder;
    private int recordingStartTime;
    private int playbackFrameCount;
    public bool recordingMovement = false;
    public bool playbackMovement = false;
    private int fixedFrameCount = 0;
    private Dictionary<int, MovementState> inputRecording;

    [SerializeField] private GameObject accelerateBarPrefab;
    [SerializeField] private GameObject brakeBarPrefab;
    [SerializeField] private GameObject mixedBarPrefab;
    [SerializeField] private Transform barParent;
    
    [SerializeField] private AudioSource pingAudio;
    
    [SerializeField] private bool moving;
    [SerializeField] private bool braking;
    private Rect currentBarRect;
    private GameObject currentBar;
    private GameObject currentBarPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //TruckBehaviour.Instance.car.isKinematic = true;
    }

    private void Update()
    {
        //if (GrabberBehaviour.Instance.grabberIsActive) return;
        //handle input recording
        if (timerStarted && recordingMovement) RecordInputs();
        if (timerStarted && !recordingMovement) return;
        //start input recording
        if (Input.GetKeyDown(KeyCode.Space) && !timerStarted)
        {
            timerStarted = true;
            movementRecorder = new();
            recordingStartTime = 0;
            fixedFrameCount = 0;
            RecordInputs();
        }

        if (!timerStarted) return;
        
        if (rightPosition == 0) return;
        recordingMovement = true;
        rightPosition -= Time.deltaTime * barWidth * .2f;
        //end condition for input recording
        if (rightPosition <= 0)
        {
            rightPosition = 0;
            elapsedRect.localPosition = (-halfBarWidth - 18) * Vector3.right;
            recordingMovement = false;
            playbackMovement = true;
            rightPosition = barWidth;
            TruckBehaviour.Instance.car.isKinematic = false;
            playbackFrameCount = 0;
            inputRecording = movementRecorder.GetRecording();
            Debug.Log(string.Join(Environment.NewLine, inputRecording));
        }
        elapsedRect.localPosition = (halfBarWidth - rightPosition - 18) * Vector3.right;
    }

    private void FixedUpdate()
    {
        //handle playback mode
        if (playbackMovement) PlayBackFixedFrame();
        
        if (!recordingMovement) return;
        if (!timerStarted) return;
        fixedFrameCount++;
    }

    /// <summary>
    /// Converts inputs into recorder entry requests. Also handles bar visuals.
    /// </summary>
    private void RecordInputs()
    {
        bool oldMoving = moving;
        bool oldBraking = braking;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.MOVEMENT_START);
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.MOVEMENT_END);
            moving = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.BRAKING_START);
            braking = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.BRAKING_END);
            braking = false;
        }

        //handle bar visuals
        if (moving != oldMoving || braking != oldBraking)
        {
            if (!moving && !braking)
            {
                ((RectTransform)currentBar.transform).sizeDelta =
                    new Vector2((halfBarWidth - rightPosition) - currentBar.transform.localPosition.x, barHeight);
                return;
            }
            if (moving && braking)
            {
                currentBarPrefab = mixedBarPrefab;
            } else if (moving)
            {
                currentBarPrefab = accelerateBarPrefab;
            } else if (braking)
            {
                currentBarPrefab = brakeBarPrefab;
            }
            
            currentBar = Instantiate(currentBarPrefab, barParent);
            currentBar.transform.localPosition = (halfBarWidth - rightPosition) * Vector3.right;
            ((RectTransform)currentBar.transform).sizeDelta = new Vector2(0, barHeight);

        } else if (moving || braking)
        {
            ((RectTransform)currentBar.transform).sizeDelta =
                new Vector2((halfBarWidth - rightPosition) - currentBar.transform.localPosition.x, barHeight);
        }
    }

    /// <summary>
    /// Handles playback of recorded inputs
    /// </summary>
    private void PlayBackFixedFrame()
    {
        rightPosition -= Time.fixedDeltaTime * barWidth * .2f;
        //playback end condition
        if (rightPosition <= 0)
        {
            rightPosition = 0;
            timerStarted = false;
            elapsedRect.localPosition = (-halfBarWidth - 18) * Vector3.right;
            playbackMovement = false;
            TruckBehaviour.Instance.EndMovement();
            if(TruckBehaviour.Instance.isTouchingFlag)
            {
                ResetTimerBar();
                TruckBehaviour.Instance.ResetActiveMovement();
                pingAudio.Play();
            }
            //GrabberBehaviour.Instance.GrabVehicle();
        }
        //bar indicator
        elapsedRect.localPosition = (halfBarWidth - rightPosition - 18) * Vector3.right;
        if (inputRecording.ContainsKey(playbackFrameCount))
        {
            Debug.Log($"Input: {inputRecording[playbackFrameCount]}");
            TruckBehaviour.Instance.Move(inputRecording[playbackFrameCount]);
        }
        playbackFrameCount++;
    }

    /// <summary>
    /// Resets all timers and bars as well as vehicle physics
    /// </summary>
    public void ResetTimerBar()
    {
        elapsedRect.localPosition = (-halfBarWidth - 18) * Vector3.right;
        rightPosition = barWidth;
        timerStarted = false;
        recordingMovement = false;
        playbackMovement = false;
        recordingStartTime = 0;
        playbackFrameCount = 0;
        fixedFrameCount = 0;
        //TruckBehaviour.Instance.car.isKinematic = true;
        moving = false;
        braking = false;
        inputRecording.Clear();
        foreach(Transform child in barParent) Destroy(child.gameObject);
    }
}
