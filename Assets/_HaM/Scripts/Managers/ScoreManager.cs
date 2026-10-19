using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int pointsPerMole = 10;
    [SerializeField] private TMP_Text scoreText;

    public int Score;

    private void Start()
    {
        EventManager.StartListening(Consts.Events.Game.Prepare, Prepare);
        EventManager.StartListening(Consts.Events.Mole.Hit, MoleHit);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Consts.Events.Game.Prepare, Prepare);
        EventManager.StopListening(Consts.Events.Mole.Hit, MoleHit);
    }

    private void Prepare()
    {
        Score = 0;
        scoreText.text = Score.ToString("D9");
    }

    private void MoleHit()
    {
        Score += pointsPerMole;
        scoreText.text = Score.ToString("D9");
    }
}
