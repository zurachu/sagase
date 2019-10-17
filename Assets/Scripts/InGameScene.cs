using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameScene : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text titleText;
    [SerializeField] private Prefectures prefectures;

    private List<PrefectureItem> prefectureItems;
    private NoneItem noneItem;
    private GameObject touchDefense;
    private List<int> indexes;

    void Start()
    {
        indexes = Enumerable.Range(0, prefectures.Count).ToList();

        CreatePrefectureItems();
        CreateNoneItem();
        StartGame();
    }

    private void StartGame()
    {
        Shuffle(indexes);

        var rightIndex = 40;
        titleText.text = $"<color='blue'>{prefectures.Get(rightIndex).name}</color>をさがせ！";

        var i = 0;
        foreach (var item in prefectureItems)
        {
            var index = indexes[i];
            var prefecture = prefectures.Get(index);
            item.Initialize(prefecture.sprite, prefecture.name, index == rightIndex, OnTap);
            i++;
        }

        noneItem.Initialize(indexes.FindIndex(_x => _x == rightIndex) >= prefectureItems.Count, OnTap);
    }

    private void OnTap(bool isRight)
    {
        if (isRight)
        {
            OnRight();
        }
    }

    private async Task OnRight()
    {
        ShowTouchDefense();

        await Task.Delay(1000);

        noneItem.ClearStatus();
        foreach (var item in prefectureItems)
        {
            item.ClearStatus();
            item.transform.DOLocalMove(Vector3.zero, 0.5f);
        }

        await Task.Delay(500);
        StartGame();

        var i = 0;
        foreach (var item in prefectureItems)
        {
            item.transform.DOLocalMove(GetItemLocalPosition(i), 0.5f);
            i++;
        }

        await Task.Delay(500);

        CloseTouchDefense();
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
            item.transform.localPosition = GetItemLocalPosition(i);
            prefectureItems.Add(item);
        }
    }

    private void CreateNoneItem()
    {
        var prefab = Resources.Load<NoneItem>("Prefabs/NoneItem");
        noneItem = Instantiate(prefab, canvas.transform);
        var localPosition = Vector3.zero;
        noneItem.transform.localPosition = GetItemLocalPosition(24);
    }

    private Vector3 GetItemLocalPosition(int index)
    {
        var localPosition = Vector3.zero;
        localPosition.x = (index % 5 - 2) * 450;
        localPosition.y = -(index / 5 - 2) * 450;
        return localPosition;
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
