using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultView : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private static readonly string Title = "『佐賀せ！』";

    private string message;

    public void Initialize(Prefectures.Prefecture prefecture, int score)
    {
        scoreText.text = $"SCORE: {score.ToString("#,0")}";
        if (score >= 0)
        {
            message = $"{Title}あなたの{prefecture.name}愛は{score.ToString("#,0")}点でした";
        }
        else
        {
            message = $"{Title}あなたの{prefecture.name}愛はマイナス{(-score).ToString("#,0")}点でした"
                + $"\n{prefecture.name}愛が不足しているのではないでしょうか？";
        }
    }

    public void OnTapTweet()
	{
        naichilab.UnityRoomTweet.Tweet("sagase", message, "unityroom", "unity1week");
    }

    public void OnTapTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
