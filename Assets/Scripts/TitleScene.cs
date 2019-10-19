using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] List<Image> prefectures;
    [SerializeField] Image saga;
    [SerializeField] CanvasGroup tapStart;

    private Tweener tapStartTweeener;
    private Tweener sagaColorTweener;

    private void Start()
    {
        sagaColorTweener = saga.DOColor(new Color(0.5f, 1f, 0.5f), 0.5f).From(new Color(0f, 0.875f, 0f)).SetLoops(-1, LoopType.Yoyo);
        tapStartTweeener = tapStart.DOFade(1f, 0.5f).From(0f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnTapStart()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/TouchDefense");
        Instantiate(prefab, canvas.transform);

        foreach (var prefecture in prefectures)
        {
            prefecture.transform.DOLocalMove(Vector3.zero, 0.5f);
            prefecture.transform.DOScale(Vector3.one, 0.5f);
        }

        sagaColorTweener.Kill();
        var sequence = DOTween.Sequence()
            .Append(saga.DOColor(new Color(0f, 0.75f, 0f), 0.25f))
            .AppendInterval(0.25f)
            .AppendCallback(() => { SceneManager.LoadScene("InGameScene"); })
            .Play();

        tapStartTweeener.Kill();
        tapStartTweeener = tapStart.DOFade(1f, 0.1f).From(0f).SetLoops(-1, LoopType.Yoyo);
    }
}
