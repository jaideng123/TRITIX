using System;
using UnityEngine;

// Custom yield instruction to wait until a callback is called
// by Jackson Dunstan, http://JacksonDunstan.com/articles/3678
// License: MIT
public class WaitForCallback<TResult> : CustomYieldInstruction
{
    /// <summary>
    /// If the callback has been called
    /// </summary>
    private bool done;

    /// <summary>
    /// Immediately calls the given Action and passes it another Action. Call that Action with the
    /// result of the callback function. Doing so will cause <see cref="keepWaiting"/> to be set to
    /// false and <see cref="Result"/> to be set to the value passed to the Action.
    /// </summary>
    /// <param name="callCallback">Action that calls the callback function. Pass the result to
    /// the Action passed to it to stop waiting.</param>
    public WaitForCallback(Action<Action<TResult>> callCallback)
    {
        callCallback(r => { Result = r; done = true; });
    }

    /// <summary>
    /// If the callback is still ongoing. This is set to false when the Action passed to the
    /// Action passed to the constructor is called.
    /// </summary>
    override public bool keepWaiting { get { return done == false; } }

    /// <summary>
    /// Result of the callback
    /// </summary>
    public TResult Result { get; private set; }
}