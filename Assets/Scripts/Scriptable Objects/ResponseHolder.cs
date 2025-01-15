using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ApiResponseHolder", fileName = "ScriptableObjects/ApiResponseHolder")]
public class ResponseHolder : ScriptableObject
{

    public List<Response> apiResponses = new List<Response>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
    }

    public void AddReponse(string key, string response)
    {
        Response res = new Response();
        res.apiKey = key;
        res.response = response;
        apiResponses.Add(res);
    }

    public bool CheckResponse(string apiKey)
    {
        foreach (var response in apiResponses)
        {
            if (response.apiKey == apiKey)
            {
                return true;
            }
        }

        return false;
    }

    public string GetResponse(string apiKey)
    {
        foreach (var response in apiResponses)
        {
            if (response.apiKey == apiKey)
            {
                return response.response;
            }
        }

        return null;
    }

    public void ChangeReponse(string key, string response)
    {
        Response res = apiResponses.Find(x => x.apiKey == key);
        if (res != null)
            res.response = response;
    }

#if UNITY_EDITOR
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredPlayMode:
                apiResponses.Clear();
                break;

            case PlayModeStateChange.ExitingPlayMode:
                apiResponses.Clear();
                break;
        }
    }
#endif

    [Serializable]
    public class Response
    {
        public string apiKey;
        public string response;
    }
}
