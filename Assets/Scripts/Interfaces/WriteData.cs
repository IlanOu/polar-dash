using TMPro;
using UnityEngine;

public class WriteData : MonoBehaviour
{
    public TextMeshProUGUI textScore;
    private string defaultTextScore = "SCORE\n";
    public TextMeshProUGUI textBestScore;
    private string defaultTextBestScore = "BEST\n";
    public TextMeshProUGUI leftScore;
    public TextMeshProUGUI rightScore;
    void Start()
    {
        textScore.SetText(defaultTextScore + ScoreManager.instance.score.ToString());
        textBestScore.SetText(defaultTextBestScore + ScoreManager.instance.bestScore.ToString());
        leftScore.SetText(
            "JUMP : " + ScoreManager.instance.leftPlayer["jump"].ToString() + "\n" +
            "SQUAT : " + ScoreManager.instance.leftPlayer["squat"].ToString() + "\n" +
            "TOP : " + ScoreManager.instance.leftPlayer["top"].ToString() + "\n" +
            "BOTTOM : " + ScoreManager.instance.leftPlayer["bottom"].ToString() + "\n" +
            "SIDE : " + ScoreManager.instance.leftPlayer["side"].ToString()
        );
        rightScore.SetText(
            "JUMP : " + ScoreManager.instance.rightPlayer["jump"].ToString() + "\n" +
            "SQUAT : " + ScoreManager.instance.rightPlayer["squat"].ToString() + "\n" +
            "TOP : " + ScoreManager.instance.rightPlayer["top"].ToString() + "\n" +
            "BOTTOM : " + ScoreManager.instance.rightPlayer["bottom"].ToString() + "\n" +
            "SIDE : " + ScoreManager.instance.rightPlayer["side"].ToString()
        );
    }
}
