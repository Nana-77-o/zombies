using System;
using System.Collections;
using UnityEngine;

/// <summary>プレイヤーが所持する武器や武器切り替えを管理するクラス </summary>
public class PlayerGans : MonoBehaviour
{
    [Tooltip("配列操作用")]
    const int INDEX_ONE = 1;
    private static int selectedIndex = 0;
    /// <summary>武器の配列 </summary>
    GameObject[] _gans = new GameObject[4];
    [SerializeField, Tooltip("武器のIcon配列")]
    GameObject[] _ganIcons;
    [SerializeField]
    private float _selectSize = 1.2f;
    [SerializeField]
    private float _idleSize = 0.8f;
    [Tooltip("選択前の武器")]
    GameObject before;
   
    public GameObject[] Weapons { get => _gans; }
    public static int SelectedIndex { get => selectedIndex; }

    private void Awake()
    {
        selectedIndex = 0;
        //武器切り替えの入力登録
        PlayerInput.SetEnterInput(InputType.LeftShoulder, LeftSelectGan);   //左
        PlayerInput.SetEnterInput(InputType.RightShoulder, SelectGan);　　　//右
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.LeftShoulder, LeftSelectGan);   //左
        PlayerInput.LiftEnterInput(InputType.RightShoulder, SelectGan);　　　//右
    }
    IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;
        var gan = _gans[selectedIndex];
        gan.SetActive(true);
        ShowSelectIcon();
    }

    public void SelectGan()//右回り
    {
        if (selectedIndex + INDEX_ONE >= _gans.Length) { selectedIndex = 0; }
        else { selectedIndex++; }

        var gan = _gans[selectedIndex];
        gan.SetActive(true);
        if (selectedIndex - INDEX_ONE < 0)
        {
            before = _gans[_gans.Length - INDEX_ONE];
            before.SetActive(false);
        }
        else
        {
            before = _gans[selectedIndex - INDEX_ONE];
            before.SetActive(false);
        }
        ShowSelectIcon();
    }
    public void LeftSelectGan()//左回り
    {
        if (selectedIndex - INDEX_ONE < 0) { selectedIndex = _gans.Length - INDEX_ONE; }
        else { selectedIndex--; }

        var gan = _gans[selectedIndex];
        gan.SetActive(true);
        if (selectedIndex + INDEX_ONE >= _gans.Length)
        {
            before = _gans[0];
            before.SetActive(false);
        }
        else
        {
            before = _gans[selectedIndex + INDEX_ONE];
            before.SetActive(false);
        }
        ShowSelectIcon();
    }
    private void ShowSelectIcon()
    {
        if (_ganIcons == null || _ganIcons.Length == 0) { return; }
        foreach (var icon in _ganIcons)
        {
            icon.transform.localScale = Vector3.one * _idleSize;
        }
        _ganIcons[selectedIndex].transform.localScale = Vector3.one * _selectSize;
    }
}
