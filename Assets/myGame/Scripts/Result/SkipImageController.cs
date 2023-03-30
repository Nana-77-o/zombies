using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipImageController : MonoBehaviour
{
    [SerializeField]
    private ResultController _resultController = default;

    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }
    void Update()
    {
        if (!_resultController.IsSkip)
        {
            _image.fillAmount = _resultController.SkipTimer / _resultController.SkipTime;
        }

        if (_resultController.IsSkip || _resultController.IsEnd)
        {
            _image.enabled = false;
        }
    }
}
