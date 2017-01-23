using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class RequestHandler
{
    public static void MakeRequest(string url)
    {
        //TODO: handle timeouts?
        UnityWebRequest request = new UnityWebRequest(url);
        request.Send();
    }
}

