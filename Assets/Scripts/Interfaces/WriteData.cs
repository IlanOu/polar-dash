using TMPro;
using UnityEngine;

public class WriteData : MonoBehaviour
{
    public TextMeshProUGUI textScore;
    private string defaultTextScore = "SCORE\n";
    public TextMeshProUGUI textBestScore;
    private string defaultTextBestScore = "BEST\n";
    void Start()
    {
        textScore.SetText(defaultTextScore + ScoreManager.instance.score.ToString());
        textBestScore.SetText(defaultTextBestScore + ScoreManager.instance.bestScore.ToString());
    }
}
