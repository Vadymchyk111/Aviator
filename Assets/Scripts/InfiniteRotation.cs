using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteRotation : MonoBehaviour
{
    private Image _propellerImage;

    private void Awake()
    {
        _propellerImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _propellerImage.transform.rotation = new Quaternion(0f, 0f, 0f, 0);
        _propellerImage.transform.DORotate(Vector3.back * 360, 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}