using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] ScoreDisplayPopUp popUpPrefab;

    public int Score
    {
        get => score;
        set
        {
            var diff = value - score;
            if (diff != 0)
            {
                var popUp = Instantiate<ScoreDisplayPopUp>(popUpPrefab, transform);
                popUp.Initialize(diff);
            }

            score = value;
            scoreText.text = score.ToString("#,0");
            scoreText.color = score >= 0 ? Color.black : new Color(0.5f, 0f, 0f);
        }
    }
    private int score;

    private void Start()
    {
        Score = 0;
    }
}
