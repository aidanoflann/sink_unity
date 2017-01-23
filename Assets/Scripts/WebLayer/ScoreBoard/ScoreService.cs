using UnityEngine;
using System.Collections;

public class ScoreService
{
    private static readonly string getScores = "get_scores";

    public static void GetScores()
    {
        RequestHandler.MakeRequest(Endpoints.SCORE_BOARD + getScores);
    }
}
