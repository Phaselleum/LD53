using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TimerBarBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform elapsedRect;
    private float rightPosition = 500;
    public static TimerBarBehaviour Instance;

    private bool timerStarted = false;
    private MovementRecorder movementRecorder;
    private int recordingStartTime;
    private int playbackFrameCount;
    private bool recordingMovement = false;
    private bool playbackMovement = false;
    private int fixedFrameCount = 0;
    private Dictionary<int, MovementState> inputRecording;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TruckBehaviour.Instance.car.isKinematic = true;
    }

    private void Update()
    {
        if (timerStarted && recordingMovement) RecordInputs();
        if (timerStarted && !recordingMovement) return;
        if (Input.GetKeyDown(KeyCode.Space) && !timerStarted)
        {
            StartTimer();
            movementRecorder = new();
            recordingStartTime = 0;
            fixedFrameCount = 0;
            movementRecorder.AddEntry(0, MovementState.MOVEMENTSTART);
        }

        if (!timerStarted) return;
        
        if (rightPosition == 0) return;
        recordingMovement = true;
        //elapsedRect.position += Time.deltaTime * Vector3.right * elapsedRect.rect.width * .2f;
        rightPosition -= Time.deltaTime * 100;
        if (rightPosition <= 0)
        {
            rightPosition = 0;
            elapsedRect.localPosition = -250 * Vector3.right;
            recordingMovement = false;
            playbackMovement = true;
            rightPosition = 500;
            TruckBehaviour.Instance.car.isKinematic = false;
            playbackFrameCount = 0;
            inputRecording = movementRecorder.GetRecording();
            Debug.Log(string.Join(Environment.NewLine, inputRecording));
        }
        elapsedRect.localPosition = (250 - rightPosition) * Vector3.right;
    }

    private void RecordInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.MOVEMENTSTART);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.MOVEMENTEND);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.BRAKINGSTART);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.BRAKINGEND);
        }
    }

    private void FixedUpdate()
    {
        if (playbackMovement) PlayBackFixedFrame();
        if (!recordingMovement) return;
        if (!timerStarted) return;
        fixedFrameCount++;
    }

    private void PlayBackFixedFrame()
    {
        rightPosition -= Time.fixedDeltaTime * 100;
        if (rightPosition <= 0)
        {
            rightPosition = 0;
            timerStarted = false;
            elapsedRect.localPosition = -250 * Vector3.right;
            playbackMovement = false;
        }
        elapsedRect.localPosition = (250 - rightPosition) * Vector3.right;
        //Debug.Log("Playback!");
        if (inputRecording.ContainsKey(playbackFrameCount))
        {
            Debug.Log($"Input: {inputRecording[playbackFrameCount]}");
            TruckBehaviour.Instance.Move(inputRecording[playbackFrameCount]);
        }
        playbackFrameCount++;
        if (playbackFrameCount > fixedFrameCount)
        {
            TruckBehaviour.Instance.EndMovement();
        }
    }

    public void StartTimer()
    {
        timerStarted = true;
    }

    public void Reset()
    {
        elapsedRect.localPosition = -250 * Vector3.right;
        rightPosition = 500;
        timerStarted = false;
        recordingMovement = false;
        recordingStartTime = 0;
        playbackFrameCount = 0;
        fixedFrameCount = 0;
        TruckBehaviour.Instance.car.isKinematic = true;
    }
}
