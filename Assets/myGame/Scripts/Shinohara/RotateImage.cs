using UnityEngine;

/// <summary>オブジェクトをカメラの方向に向かせる為のクラス </summary>
public class RotateImage : MonoBehaviour
{
    /// <summary>カメラオブジェクトの名前 </summary>
    const string CAMERA_NAME = "CMVC1";

    Transform _cameraPosition;
    Quaternion _rotationValue;

    void Start()
    {
        _cameraPosition = GameObject.Find(CAMERA_NAME).GetComponent<Transform>();
        _rotationValue = Quaternion.Euler(_cameraPosition.localEulerAngles.x, 0f, 0);
        transform.rotation = _rotationValue;
    }

    private void Update()
    {
        _rotationValue =  Quaternion.Euler(_cameraPosition.localEulerAngles.x, 0f, 0);
        transform.rotation = _rotationValue;
    }
}
