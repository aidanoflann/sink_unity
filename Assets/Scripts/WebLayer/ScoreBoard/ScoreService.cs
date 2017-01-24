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
        FetchRequest fetchRequest = new FetchRequest();
        string endpoint = Endpoints.SCORE_BOARD;
        string value = getScores;
        
        RequestHandler.MakeRequest(fetchRequest, endpoint, value);
    }
}
