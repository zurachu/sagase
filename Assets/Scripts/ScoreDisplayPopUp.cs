using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreDisplayPopUp : MonoBehaviour
{
    [SerializeField] Text scoreText;

    public void Initialize(int score)
    {
        scoreText.text = score.ToString("#,0");
        scoreText.color = score >= 0 ? Color.blue : Color.red;

        var sequence = DOTween.Sequence()
            .Append(scoreText.transform.DOLocalMove(new Vector3(0f, 200f, 0f), 0.5f))
            .Join(scoreText.DOFade(0f, 0.5f))
            .AppendCallback(() =>
            {
                Destroy(gameObject);
            })
            .Play();
    }
}
