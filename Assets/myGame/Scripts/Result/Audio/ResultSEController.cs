using UnityEngine;

public class ResultSEController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _soundEffect = default;
    [SerializeField]
    private float _seVolume = 0.2f;

    public void PlaySE(int index)
    {
        if (index >= 0 && index < _soundEffect.Length)
        {
            AudioSource.PlayClipAtPoint(_soundEffect[index], transform.position, _seVolume);
        }
        else
        {
            Debug.LogError("範囲外です");
        }
    }
}
