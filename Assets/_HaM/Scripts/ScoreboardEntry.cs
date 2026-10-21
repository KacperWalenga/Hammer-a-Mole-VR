using TMPro;
using UnityEngine;

public class ScoreboardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text dateTime;
    
    private ScoreDataEntry scoreDataEntry;

    public void Init(ScoreDataEntry scoreData)
    {
        scoreDataEntry = scoreData;
        
        score.text = scoreDataEntry.Score.ToString();
        time.text = FormatTime(scoreDataEntry.Time);
        dateTime.text = scoreDataEntry.DateTime;
    }

    private string FormatTime(float timeToFormat)
    {
        var totalSeconds = Mathf.FloorToInt(timeToFormat);
        var minutes = totalSeconds / 60;
        var seconds = totalSeconds % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}
