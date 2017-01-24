using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class RequestHandler
{
    public static void MakeRequest(object requestObject, string endPoint, string value)
    {
        string json = JsonUtility.ToJson(requestObject);
        UnityWebRequest request = UnityWebRequest.Put(endPoint + value, json);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        //TODO: handle timeouts?
        request.Send();
    }
}

