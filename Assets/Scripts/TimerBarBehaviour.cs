using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;

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

    [SerializeField] private GameObject accelerateBarPrefab;
    [SerializeField] private GameObject brakeBarPrefab;
    [SerializeField] private GameObject mixedBarPrefab;
    [SerializeField] private Transform barParent;
    
    private bool moving;
    private bool braking;
    private Rect currentBarRect;
    private GameObject currentBar;
    private GameObject currentBarPrefab;
    private float currentRectWidth;

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
            RecordInputs();
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
        bool oldMoving = moving;
        bool oldBraking = braking;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.MOVEMENTSTART);
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.MOVEMENTEND);
            moving = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.BRAKINGSTART);
            braking = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            movementRecorder.AddEntry(fixedFrameCount, MovementState.BRAKINGEND);
            braking = false;
        }

        if (moving != oldMoving || braking != oldBraking)
        {
            if (!moving && !braking)
            {
                ((RectTransform)currentBar.transform).sizeDelta =
                    new Vector2((250 - rightPosition) - currentBar.transform.localPosition.x, 20);
                currentRectWidth = 0;
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
            currentBar.transform.localPosition = (250 - rightPosition) * Vector3.right;
            ((RectTransform)currentBar.transform).sizeDelta = new Vector2(0, 20);
            currentRectWidth = 0;

        } else if (moving || braking)
        {
            currentRectWidth += Time.fixedDeltaTime * 20;
            ((RectTransform)currentBar.transform).sizeDelta =
                new Vector2((250 - rightPosition) - currentBar.transform.localPosition.x, 20);
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
        moving = false;
        braking = false;
        currentRectWidth = 0;
        foreach(Transform child in barParent) Destroy(child.gameObject);
    }
}
