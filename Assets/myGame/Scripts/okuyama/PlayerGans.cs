using System;
using System.Collections;
using UnityEngine;

/// <summary>vC[ͺ·ινβνΨθΦ¦πΗ·ιNX </summary>
public class PlayerGans : MonoBehaviour
{
    [Tooltip("zρμp")]
    const int INDEX_ONE = 1;
    private static int selectedIndex = 0;
    /// <summary>νΜzρ </summary>
    GameObject[] _gans = new GameObject[4];
    [SerializeField, Tooltip("νΜIconzρ")]
    GameObject[] _ganIcons;
    [SerializeField]
    private float _selectSize = 1.2f;
    [SerializeField]
    private float _idleSize = 0.8f;
    [Tooltip("IπOΜν")]
    GameObject before;
   
    public GameObject[] Weapons { get => _gans; }
    public static int SelectedIndex { get => selectedIndex; }

    private void Awake()
    {
        selectedIndex = 0;
        //νΨθΦ¦ΜόΝo^
        PlayerInput.SetEnterInput(InputType.LeftShoulder, LeftSelectGan);   //Ά
        PlayerInput.SetEnterInput(InputType.RightShoulder, SelectGan);@@@//E
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.LeftShoulder, LeftSelectGan);   //Ά
        PlayerInput.LiftEnterInput(InputType.RightShoulder, SelectGan);@@@//E
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

    public void SelectGan()//Eρθ
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
    public void LeftSelectGan()//Άρθ
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
