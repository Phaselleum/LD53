using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementRecorder
{
    private Dictionary<int, MovementState> inputRecording;

    public MovementRecorder()
    {
        inputRecording = new();
    }

    public void AddEntry(int time, MovementState state)
    {
        try
        {
            inputRecording.Add(time, state);
        }
        catch (ArgumentException ae)
        {
            
        }
    }

    public Dictionary<int, MovementState> GetRecording()
    {
        return inputRecording;
    }
}

public enum MovementState {
    NOTHING,
    MOVEMENTSTART,
    MOVEMENTEND,
    BREAKINGSTART,
    BREAKINGEND
}