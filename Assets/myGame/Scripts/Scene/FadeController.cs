using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private static readonly int FRONT_LAYR = 200;
    private static readonly Vector2 IMAGE_SIZE = new Vector2(2000, 1100);
    [Tooltip("フェードスピード")]
    [SerializeField]
    private float _fadeSpeed = 1f;
    [Tooltip("フェードする画像")]
    [SerializeField]
    private Image _fadeImage = default;
    [Tooltip("開始時の色")]
    [SerializeField]
    private Color _startColor = Color.black;

    private static FadeController instance = default;
    public static FadeController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("FadeCanvas");
                obj.AddComponent<RectTransform>();
                var canvas = obj.AddComponent<Canvas>();
                canvas.sortingOrder = FRONT_LAYR;
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                var fadeController = obj.AddComponent<FadeController>();
                var imageObj = new GameObject("FadeImage");
                imageObj.transform.SetParent(obj.transform);
                var imageRect = imageObj.AddComponent<RectTransform>();
                var image = imageObj.AddComponent<Image>();
                imageRect.sizeDelta = IMAGE_SIZE;
                imageRect.localPosition = Vector2.zero;
                image.color = Color.black;
                fadeController._fadeImage = image;
                instance = fadeController;
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ChangeFadeSpeed(float speed)
    {
        _fadeSpeed = speed;
    }
    /// <summary>
    /// フェードイン後にアクションする
    /// </summary>
    /// <param name="action"></param>
    public static void StartFadeIn(Action action = null)
    {
        Instance.StartCoroutine(Instance.FadeIn(action));
    }
    /// <summary>
    /// フェードアウト後にアクションする
    /// </summary>
    /// <param name="action"></param>
    public static void StartFadeOut(Action action = null)
    {
        Instance.StartCoroutine(Instance.FadeOut(action));
    }
    public static void StartFadeOutIn(Action outAction = null, Action inAction = null)
    {
        Instance.StartCoroutine(Instance.FadeOutIn(outAction, inAction));
    }
    IEnumerator FadeIn(Action action)
    {
        _fadeImage.gameObject.SetActive(true);
        yield return FadeIn();
        action?.Invoke();
        _fadeImage.gameObject.SetActive(false);
    }
    IEnumerator FadeOut(Action action)
    {
        _fadeImage.gameObject.SetActive(true);
        yield return FadeOut();
        action?.Invoke();
    }
    IEnumerator FadeOutIn(Action outAction, Action inAction)
    {
        _fadeImage.gameObject.SetActive(true);
        yield return FadeOut();
        outAction?.Invoke();
        yield return FadeIn();
        inAction?.Invoke();
        _fadeImage.gameObject.SetActive(false);
    }
    IEnumerator FadeIn()
    {
        float clearScale = 1f;
        Color currentColor = _startColor;
        while (clearScale > 0f)
        {
            clearScale -= _fadeSpeed * Time.unscaledDeltaTime;
            if (clearScale <= 0f)
            {
                clearScale = 0f;
            }
            currentColor.a = clearScale;
            _fadeImage.color = currentColor;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float clearScale = 0f;
        Color currentColor = _startColor;
        while (clearScale < 1f)
        {
            clearScale += _fadeSpeed * Time.unscaledDeltaTime;
            if (clearScale >= 1f)
            {
                clearScale = 1f;
            }
            currentColor.a = clearScale;
            _fadeImage.color = currentColor;
            yield return null;
        }
    }
}
