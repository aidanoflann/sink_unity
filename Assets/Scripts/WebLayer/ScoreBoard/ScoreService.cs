using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScoreService
{
    private static readonly string getScores = "get_scores";

    private class FetchRequest: object
    {
        public string game_name;
        public FetchRequest()
        {
            game_name = "my_game";
        }   
    }

    public static void FetchScores()
    {
        string json = JsonUtility.ToJson(new FetchRequest());
        UnityWebRequest request = UnityWebRequest.Put(Endpoints.SCORE_BOARD + getScores, json);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        RequestHandler.MakeRequest(request);
    }
}
