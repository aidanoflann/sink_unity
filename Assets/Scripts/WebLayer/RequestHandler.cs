using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class RequestHandler
{
    public static void MakeRequest(UnityWebRequest request)
    {
        //TODO: handle timeouts?
        request.Send();
    }
}

