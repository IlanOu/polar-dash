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
        textScore.SetText(defaultTextScore + DataStorage.instance.score.ToString());
        textBestScore.SetText(defaultTextBestScore + DataStorage.instance.bestScore.ToString());
        leftScore.SetText(
            "JUMP : " + DataStorage.instance.leftPlayer["jump"].ToString() + "\n" +
            "SQUAT : " + DataStorage.instance.leftPlayer["squat"].ToString() + "\n" +
            "TOP : " + DataStorage.instance.leftPlayer["top"].ToString() + "\n" +
            "BOTTOM : " + DataStorage.instance.leftPlayer["bottom"].ToString() + "\n" +
            "SIDE : " + DataStorage.instance.leftPlayer["side"].ToString()
        );
        rightScore.SetText(
            "JUMP : " + DataStorage.instance.rightPlayer["jump"].ToString() + "\n" +
            "SQUAT : " + DataStorage.instance.rightPlayer["squat"].ToString() + "\n" +
            "TOP : " + DataStorage.instance.rightPlayer["top"].ToString() + "\n" +
            "BOTTOM : " + DataStorage.instance.rightPlayer["bottom"].ToString() + "\n" +
            "SIDE : " + DataStorage.instance.rightPlayer["side"].ToString()
        );
    }
}
