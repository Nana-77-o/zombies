using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ダメージ表示クラス
/// </summary>
public class DamageView : MonoBehaviour
{
    /// <summary> 初期プール数 </summary>
    private static readonly int DEFAULT_COUNT = 20;
    /// <summary> 表示位置ぶれ幅 </summary>
    private static readonly float VIEW_DIFFUS = 1f;
    /// <summary> 位置調整軸 </summary>
    private static readonly Vector3 VIEW_DIR = new Vector3(1, 1, 0);
    /// <summary> プールを保持するオブジェクト </summary>
    private static GameObject viewBase = null;
    /// <summary> ダメージ表示Prefab </summary>
    private static DamageView damageViewPrefab = null;
    /// <summary> プールリスト </summary>
    private static List<DamageView> damageViewList = new List<DamageView>();
    [SerializeField,Tooltip("表示用テキスト")]
    private Text _viewText = default;
    [SerializeField,Header("表示時間")]
    private float _lifeTime = 1f;
    /// <summary> タイマー </summary>
    private float _timer = 0;
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0) //表示時間経過で非表示化
        {
            _timer = 0;
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 座標を指定し、ダメージ表示を行う
    /// </summary>
    /// <param name="pos">/param>
    /// <param name="damage"></param>
    public static void Show(Vector3 pos, float damage)
    {
        if (damageViewPrefab == null) //初回はPrefabをロードしプールを生成する
        {
            damageViewPrefab = Resources.Load<DamageView>("Prefab/DamageView");
            if (damageViewPrefab == null)
            {
                Debug.Log("null! Resources/Prefab/DamageView");
                return;
            }
            viewBase = new GameObject("DamageViewBase");
            for (int i = 0; i < DEFAULT_COUNT; i++) //初期プール生成
            {
                var newView = Instantiate(damageViewPrefab, viewBase.transform);
                newView.gameObject.SetActive(false);
                damageViewList.Add(newView);
            }
            DontDestroyOnLoad(viewBase);
        }
        //Vector3.Diffusivityは拡張メソッド、ランダムに座標をずらす。ダメージ表示の重なりを抑えるため
        GetView(pos.Diffusivity(VIEW_DIR, VIEW_DIFFUS) + Vector3.up + Vector3.back).View(damage);
    }
    /// <summary>
    /// プールからDamageViewを取り出す
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private static DamageView GetView(Vector3 pos)
    {
        foreach (DamageView view in damageViewList)
        {
            if (view.gameObject.activeInHierarchy == true)
            {
                continue;
            }
            view.transform.position = pos;
            return view;
        }
        //不足分は追加生成
        var newView = Instantiate(damageViewPrefab, viewBase.transform);
        newView.transform.position = pos;
        damageViewList.Add(newView);
        return newView;
    }
    /// <summary>
    /// ダメージ表示
    /// </summary>
    /// <param name="damage"></param>
    public void View(float damage)
    {
        _viewText.text = damage.ToString("F0");
        _timer = _lifeTime;
        gameObject.SetActive(true);
    }
}
