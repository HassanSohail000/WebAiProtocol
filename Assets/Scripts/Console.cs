using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class Console : MonoBehaviour
{
    public TMP_Text consoleText;
    private Queue<string> logQueue = new Queue<string>();
    private const int maxLogCount = 20;

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
        logQueue.Enqueue(logString);
        if (logQueue.Count > maxLogCount)
        {
            logQueue.Dequeue();
        }

        consoleText.text = string.Join("\n", logQueue.ToArray());
    }
}
