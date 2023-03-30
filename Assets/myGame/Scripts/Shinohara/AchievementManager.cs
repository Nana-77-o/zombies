using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

/// <summary>称号UI関連の処理を行うクラス</summary>
public class AchievementManager : MonoBehaviour
{
    /// <summary>実績リストの初期位置 </summary>
    readonly Vector2 ACHIEVEMENT_LIST_FIRST_POSITION = new Vector2(-670, 30);

    [SerializeField, Header("スクロールの速さ")]
    float _scrollSpeed = 0f;
    [SerializeField, Header("各実績のスクロール距離"), Tooltip("0.撃破数 1.救出数 2.階級")]
    float[] _scrollMoveAmounts = default;
    [SerializeField, Header("各実績のスクロールバーの移動距離"), Tooltip("0.撃破数 1.救出数 2.階級")]
    float[] _scrollbarMoveAmounts = default;
    [SerializeField, Header("各実績リストの名前")]
    string[] _listNames = default;
    [SerializeField, Tooltip("表示している称号")]
    Image _showImage = default;
    [SerializeField, Tooltip("実績未所得時の背景")]
    Image _hideImage = default;
    [SerializeField, Tooltip("実績未所得時のフレーム")]
    Image _hideFarme = default;
    [SerializeField, Tooltip("実績未所得時のクエスチョンマーク")]
    Text _hideText = default;
    [SerializeField, Tooltip("称号名を表示するテキスト")]
    Text _showAchievementText = default;
    [SerializeField, Tooltip("詳細を表示するテキスト1")]
    Text _showDetail1Text = default;
    [SerializeField, Tooltip("詳細を表示するテキスト2")]
    Text _showDetail2Text = default;
    [SerializeField, Tooltip("実績リストのラベル")]
    Text _achievementListLabel = default;
    [SerializeField, Tooltip("実績リストを変更するボタン　0=左 1=右")]
    Button[] _changeListButtons = default;
    [SerializeField, Tooltip("各実績でのボタン移動先（左）")]
    Button[] _leftButtonDestinations = default;
    [SerializeField, Tooltip("各実績でのボタン移動先（右）")]
    Button[] _rightButtonDestinations = default;
    [SerializeField, Tooltip("各実績リスト")]
    RectTransform[] _achievementLists = default;
    [SerializeField, Tooltip("現在の選択されている称号")]
    AchievementController _selectedAchievement = default;
    [SerializeField, Tooltip("実績を開いた時に選択されるボタン")]
    AchievementController _firstButton = default;
    [SerializeField, Tooltip("スクロールバー")]
    Scrollbar _scrollbar = default;
    [SerializeField, Tooltip("称号データ 0.撃破数 1.救出数 2.階級")]
    AchievementData[] _achievementData = default;
    [SerializeField, Tooltip("実績を表示するキャンバス")]
    Canvas _uiCanvas = default;

    /// <summary>実績リストを変更するボタンのテキスト　0=前 1= 次 </summary>
    Text[] _changeListButtonTexts = new Text[2];
    /// <summary>現在表示している実績リストのオブジェクト </summary>
    RectTransform _currnetShowAchievementList = default;
    /// <summary>現在参照している実績データ </summary>
    AchievementData _currentAchievementData = default;

    /// <summary>現在表示している実績リストの種類 </summary>
    AchievementType _currentType = AchievementType.Kill;
    /// <summary>各実績リストに合わせたスクロール距離にするための変数 </summary>
    float _currentMoveAmount = 0f;
    /// <summary>現在のスクロールバー移動距離 </summary>
    float _currentScrollberMoveAmount = 0f;
    /// <summary>スクロール可能かどうか </summary>
    bool _isScroll = false;

    /// <summary>実績を表示するキャンバス </summary>
    public Canvas UICanvas { get => _uiCanvas; }

    /// <summary>実績UIを開いた時の処理 </summary>
    void SetupAchievement(GetAchievementList getAchievement)
    {
        var loadData = new List<bool[]>() { getAchievement._killAchievements, getAchievement._helpAchievements, getAchievement._rankAchievements };

        for (var i = 0; i < _achievementLists.Length; i++)
        {
            var target = _achievementLists[i];
            var loaddata = loadData[i];
            var achievementData = _achievementData[i];

            for (var k = 0; k < target.childCount; k++)
            {

                var achievement = target.GetChild(k).GetComponent<AchievementController>();
                var trigger = target.GetChild(k).GetComponent<EventTrigger>();

                EventTriggerEntry(trigger, EventTriggerType.Select, SelectAchievement, achievement);        //称号情報をセットする
                EventTriggerEntry(trigger, EventTriggerType.Select, ScrollAchievementList, achievement);    //スクロール機能を追加
                EventTriggerEntry(trigger, EventTriggerType.Deselect, ScrollAchievementList, achievement);

                achievement.OpenAchievementUI(loaddata[k], achievementData.AchievementInfos[k]._conditions);
            }
        }

        var firstAchievement = _achievementLists[0].GetChild(0).GetComponent<AchievementController>();  //実績を開いた時に選択されている実績フレームの色を変更する
        firstAchievement.SelectAchievement();

        SelectAchievement(firstAchievement);    //最初の実績を調べる

        _leftButtonDestinations[0].Select();
        _currentType = AchievementType.Kill;
        ChangeDestination();
        _isScroll = true;
    }


    /// <summary>実績未所得時に未所得イラストを表示する </summary>
    /// <param name="flag">所得しているかどうか</param>
    void AchievementHide(bool flag)
    {
        var hideImageColor = _hideImage.color;
        var hideFrameColor = _hideFarme.color;
        var hideTextColor = _hideText.color;

        if (flag)
        {
            hideImageColor.a = 0;
            hideFrameColor.a = 0;
            hideTextColor.a = 0;

            _hideImage.color = hideImageColor;
            _hideFarme.color = hideFrameColor;
            _hideText.color = hideTextColor;

            return;
        }

        hideImageColor.a = 1;
        hideFrameColor.a = 1;
        hideTextColor.a = 1;

        _hideImage.color = hideImageColor;
        _hideFarme.color = hideFrameColor;
        _hideText.color = hideTextColor;
    }


    /// <summary>EventTriggerに関数を登録する処理 </summary>
    void EventTriggerEntry(EventTrigger trigger, EventTriggerType type, Action<AchievementController> action, AchievementController achievement)
    {
        var entry = new EventTrigger.Entry();       //EventTriggerに関数を登録する
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => action.Invoke(achievement));
        trigger.triggers.Add(entry);
    }

    /// <summary>実績UIを開いた時の処理　初期化処理 </summary>
    public async UniTask OpenAchievementUI()
    {
        var achievementData = await JsonProcessClass.TestLoadData<GetAchievementList>(DataType.Achievement); //データを読み込む

        _changeListButtonTexts[0] = _changeListButtons[0].transform.GetChild(0).GetComponent<Text>();
        _changeListButtonTexts[1] = _changeListButtons[1].transform.GetChild(0).GetComponent<Text>();
        _currnetShowAchievementList = _achievementLists[0];
        _currentMoveAmount = _scrollMoveAmounts[0];
        _currentScrollberMoveAmount = _scrollbarMoveAmounts[0];
        _currentAchievementData = _achievementData[0];

        UpdateAchievementListName(_achievementLists.Length);
        ChangeDestination();
        SetupAchievement(achievementData);

        _uiCanvas.enabled = true;
    }

    /// <summary>表示している実績リストに合わせてボタンの移動先を変更する </summary>
    void ChangeDestination()
    {
        var liftNavi = _changeListButtons[0].navigation;
        var rightNavi = _changeListButtons[1].navigation;
        var nextIndex = (int)_currentType % _achievementLists.Length;

        liftNavi.selectOnDown = _leftButtonDestinations[nextIndex];
        rightNavi.selectOnDown = _rightButtonDestinations[nextIndex];

        _changeListButtons[0].navigation = liftNavi;
        _changeListButtons[1].navigation = rightNavi;


        _leftButtonDestinations[nextIndex].Select();    //表示している実績リストが変更されたら左上にあるボタンを選択する
    }

    /// <summary>リスト名を表示するテキストを更新する処理 </summary>
    void UpdateAchievementListName(int index)
    {
        if (index <= 0)
        {
            index = _achievementLists.Length;
        }

        _achievementListLabel.text = $"{_listNames[index % _achievementLists.Length]} 実績";

        //リストを変更するボタンのテキストを更新
        _changeListButtonTexts[0].text = _listNames[(index - 1) % _achievementLists.Length];
        _changeListButtonTexts[1].text = _listNames[(index + 1) % _achievementLists.Length];
    }

    /// <summary>称号リストをスクロールさせる </summary>
    public void ScrollAchievementList(AchievementController controller)
    {
        if (!_isScroll) { return; }

        if (_selectedAchievement.transform.position.y < controller.transform.position.y)     //リストを上に移動させる
        {
            _currnetShowAchievementList.DOComplete();
            _currnetShowAchievementList.DOAnchorPosY(_currnetShowAchievementList.localPosition.y - _currentMoveAmount, _scrollSpeed);
            _scrollbar.value -= _currentScrollberMoveAmount;
        }
        else if (controller.transform.position.y < _selectedAchievement.transform.position.y)  //リストを下に移動させる
        {
            _currnetShowAchievementList.DOComplete();
            _currnetShowAchievementList.DOAnchorPosY(_currnetShowAchievementList.localPosition.y + _currentMoveAmount, _scrollSpeed);
            _scrollbar.value += _currentScrollberMoveAmount;
        }

        _selectedAchievement = controller;
    }

    /// <summary>称号情報をセットする </summary>
    public void SelectAchievement(AchievementController controller)
    {
        if (controller.IsOpen)
        {
            _showAchievementText.text = $"称号名 : {_currentAchievementData.AchievementInfos[controller.ID]._label}";
            _showDetail1Text.text = $"{_currentAchievementData.AchievementInfos[controller.ID]._detail1}";
            _showDetail2Text.text = $"{_currentAchievementData.AchievementInfos[controller.ID]._detail2}";
            _showImage.sprite = controller.AchievementImage.sprite;
        }
        else
        {
            _showAchievementText.text = $"解放条件";
            _showDetail1Text.text = $"{_currentAchievementData.AchievementInfos[controller.ID]._conditions}";
            _showDetail2Text.text = $" ";
        }

        AchievementHide(controller.IsOpen);
    }

    /// <summary>
    /// 表示する実績リストを変更する
    /// ボタンから呼び出す
    /// </summary>
    public void ChangeShowAchievementList(int index)
    {
        _currnetShowAchievementList.DOComplete();

        var nextIndex = (int)_currentType + index;

        if (nextIndex < 0)
        {
            nextIndex = _achievementLists.Length - 1;
        }

        _currnetShowAchievementList.gameObject.SetActive(false);
        _currnetShowAchievementList = _achievementLists[nextIndex % _achievementLists.Length];
        _currnetShowAchievementList.gameObject.SetActive(true);
        _currentMoveAmount = _scrollMoveAmounts[nextIndex % _achievementLists.Length];  //スクロールの距離を変更する
        _currentScrollberMoveAmount = _scrollbarMoveAmounts[nextIndex % _achievementLists.Length]; //スクロールバーの移動距離を変更する
        _currentAchievementData = _achievementData[nextIndex % _achievementLists.Length];

        UpdateAchievementListName(nextIndex);

        _currentType = (AchievementType)nextIndex;
        ChangeDestination();
    }
     
    /// <summary>実績UIを閉じる </summary>
    public void CloseUI()
    {
        for (var i = 0; i < _achievementLists.Length; i++)
        {
            if (i <= 0)
            {
                _achievementLists[i].gameObject.SetActive(true);
                _achievementLists[i].localPosition = ACHIEVEMENT_LIST_FIRST_POSITION;
                continue;
            }

            _achievementLists[i].gameObject.SetActive(false);
            _achievementLists[i].localPosition = ACHIEVEMENT_LIST_FIRST_POSITION;
        }

        SelectAchievement(_firstButton);

        _isScroll = false;
        _selectedAchievement = _firstButton;
        _scrollbar.value = 0;
    }
}

/// <summary>実績の種類 </summary>
public enum AchievementType
{
    /// <summary>倒した数 </summary>
    Kill = 0,
    /// <summary>救出数</summary>
    Help = 1,
    /// <summary>階級</summary>
    Rank = 2,
}
