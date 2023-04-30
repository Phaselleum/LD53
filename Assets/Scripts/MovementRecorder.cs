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

    /// <summary>
    /// Adds entries to the recording Dictionary
    /// </summary>
    /// <param name="time">time in frames after the recording started</param>
    /// <param name="state">state to be recorded</param>
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

    /// <summary>
    /// returns the recording Dictionary
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, MovementState> GetRecording()
    {
        return inputRecording;
    }
}

public enum MovementState {
    NOTHING,
    MOVEMENT_START,
    MOVEMENT_END,
    BRAKING_START,
    BRAKING_END
}