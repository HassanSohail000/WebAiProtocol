using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class Console : MonoBehaviour
{
    public TMP_Text consoleText;
    public Button clearButton;
    public ScrollRect scrollRect;
    private Queue<string> logQueue = new Queue<string>();
    private const int maxLogCount = 20;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        clearButton.onClick.AddListener(ClearLog);
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        clearButton.onClick.RemoveListener(ClearLog);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            string detailedLog = $"[{type}] {logString}\n{stackTrace}\nScript: {GetScriptName()}\nObject: {gameObject.name}";
            logQueue.Enqueue(detailedLog);
            if (logQueue.Count > maxLogCount)
            {
                logQueue.Dequeue();
            }

            consoleText.text = string.Join("\n", logQueue.ToArray());
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    void ClearLog()
    {
        logQueue.Clear();
        consoleText.text = string.Empty;
    }

    string GetScriptName()
    {
        return GetType().Name;
    }
}
