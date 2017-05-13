using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Score
{
    public string user;
    public string score;
    public string time;
}

public class ScoreService
{
    private static readonly string getScores = "get_scores";
    private static readonly string insertScore = "insert_score";

    private class FetchRequest: object
    {
        public string game_name = Endpoints.GAME_NAME;
    }

    private class SubmitRequest: object
    {
        public string user;
        public float score;
        public string game_name = Endpoints.GAME_NAME;
    }

    public static void FetchScores()
    {
        FetchRequest fetchRequest = new FetchRequest();
        string endpoint = Endpoints.SCORE_BOARD;
        string value = getScores;
        
        RequestHandler.MakeRequest(fetchRequest, endpoint, value);
    }

    public static void SubmitScore(float score)
    {
        SubmitRequest submitRequest = new SubmitRequest();
        submitRequest.user = System.Environment.UserName;
        submitRequest.score = score;

        RequestHandler.MakeRequest(submitRequest, Endpoints.SCORE_BOARD, insertScore);
    }
}
