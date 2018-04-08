using UnityEngine;
using System.Collections;

public class MyLog : MonoBehaviour
{
    public int limit = 8;
    string myLog;
    Queue myLogQueue = new Queue();

    void Start()
    {
        Debug.Log("Logger Started");
        Debug.Log(" ");
        Debug.Log(" ");
        Debug.Log(" ");
        Debug.Log(" ");
        Debug.Log(QualitySettings.GetQualityLevel());
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }
        if (myLogQueue.Count > limit)
        {
            myLogQueue.Dequeue();
        }
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    void OnGUI()
    {
        GUILayout.Label(myLog);
    }
}

