using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameScene : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text titleText;
    [SerializeField] private Text gameCountText;
    [SerializeField] private ScoreDisplay scoreDisplay;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private Prefectures prefectures;

    private static readonly int TotalGameCount = 10;
    private static readonly float MaxTimeForBonus = 5f;

    private List<PrefectureItem> prefectureItems;
    private NoneItem noneItem;
    private GameObject touchDefense;
    private List<int> indexes;
    private Prefectures.Prefecture rightPrefecture;
    private int gameCount;
    private float timeForBonus;

    AudioClip rightAudio;
    AudioClip missAudio;

    private void Start()
    {
        rightAudio = Resources.Load<AudioClip>("Audio/right2");
        missAudio = Resources.Load<AudioClip>("Audio/mistake");
        audioSource.clip = Resources.Load<AudioClip>("Audio/thinkingtime4");
        audioSource.Play();

        indexes = Enumerable.Range(0, prefectures.Count).ToList();

        CreatePrefectureItems();
        CreateNoneItem();
        ShowTouchDefense();
        StartGame();
    }

    private void Update()
    {
        timeForBonus -= Time.deltaTime;
    }

    private void StartGame()
    {
        ShowTouchDefense();
        noneItem.gameObject.SetActive(false);

        Shuffle(indexes);

        var rightIndex = 40;
        rightPrefecture = prefectures.Get(rightIndex);
        titleText.text = $"<color='blue'>{rightPrefecture.name}</color>をさがせ！";
        gameCount++;
        gameCountText.text = $"{gameCount}/{TotalGameCount}";

        var i = 0;
        foreach (var item in prefectureItems)
        {
            var index = indexes[i];
            var prefecture = prefectures.Get(index);
            item.Initialize(prefecture.sprite, prefecture.name, index == rightIndex, OnTap);
            i++;
        }

        noneItem.Initialize(indexes.FindIndex(_x => _x == rightIndex) >= prefectureItems.Count, OnTap);

        MovePrefectureItemToPosition();
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .AppendCallback(() => {
                noneItem.gameObject.SetActive(true);
                CloseTouchDefense();
                timeForBonus = MaxTimeForBonus;
            })
            .Play();
    }

    private void OnTap(bool isRight)
    {
        if (isRight)
        {
            OnRight();
        }
        else
        {
            OnMiss();
        }
    }

    private void OnRight()
    {
        ShowTouchDefense();

        var score = Mathf.Max((int)(timeForBonus * 100), 100);
        scoreDisplay.Score += score;

        audioSource.PlayOneShot(rightAudio);

        if (gameCount >= TotalGameCount)
        {
            DOTween.Sequence()
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    var prefab = Resources.Load<ResultView>("Prefabs/ResultView");
                    var resultView = Instantiate(prefab, canvas.transform);
                    resultView.Initialize(rightPrefecture, scoreDisplay.Score);
                })
                .Append(audioSource.DOFade(0f, 1f))
                .Play();
        }
        else
        {
            DOTween.Sequence()
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    foreach (var item in prefectureItems)
                    {
                        item.ClearStatus();
                        item.transform.DOLocalMove(Vector3.zero, 0.5f);
                    }

                    noneItem.gameObject.SetActive(false);
                    noneItem.ClearStatus();
                })
                .AppendInterval(0.5f)
                .AppendCallback(StartGame)
                .Play();
        }
    }

    private void OnMiss()
    {
        scoreDisplay.Score -= 100;

        audioSource.PlayOneShot(missAudio);
    }

    private void Shuffle(List<int> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var j = Random.Range(0, list.Count);
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }

    private void CreatePrefectureItems()
    {
        var prefab = Resources.Load<PrefectureItem>("Prefabs/PrefectureItem");
        prefectureItems = new List<PrefectureItem>();

        for (var i = 0; i < 24; i++)
        {
            var item = Instantiate(prefab, canvas.transform);
            item.transform.localPosition = Vector3.zero;
            prefectureItems.Add(item);
        }
    }

    private void CreateNoneItem()
    {
        var prefab = Resources.Load<NoneItem>("Prefabs/NoneItem");
        noneItem = Instantiate(prefab, canvas.transform);
        noneItem.transform.localPosition = GetItemLocalPosition(24);
    }

    private Vector3 GetItemLocalPosition(int index)
    {
        var localPosition = Vector3.zero;
        localPosition.x = (index % 5 - 2) * 450;
        localPosition.y = -(index / 5 - 2) * 450;
        return localPosition;
    }

    private void MovePrefectureItemToPosition()
    {
        var i = 0;
        foreach (var item in prefectureItems)
        {
            item.transform.DOLocalMove(GetItemLocalPosition(i), 0.5f);
            i++;
        }
    }

    private void ShowTouchDefense()
    {
        if (touchDefense == null)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/TouchDefense");
            touchDefense = Instantiate(prefab, canvas.transform);
        }
    }

    private void CloseTouchDefense()
    {
        Destroy(touchDefense.gameObject);
        touchDefense = null;
    }


}
