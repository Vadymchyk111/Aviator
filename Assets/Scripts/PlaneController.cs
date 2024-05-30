using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private Transform _planeStartPoint;
    [SerializeField] private Transform _planeEndPoint;
    [SerializeField] private Image _boomImage;
    [SerializeField] private float _timeToReachEndPoint;

    private Image _planeImage;

    private void Awake()
    {
        _planeImage = GetComponent<Image>();
    }

    private void StartFlying(bool isActive)
    {
        _planeImage.enabled = true;
        transform.localPosition = _planeStartPoint.localPosition;
        transform.DOLocalMove(_planeEndPoint.transform.localPosition, _timeToReachEndPoint);
    }

    private void OnEnable()
    {
        BetManager.OnFinished += CrushPlane;
        BetManager.OnBetStarted += StartFlying;
    }

    private void OnDisable()
    {
        BetManager.OnFinished -= CrushPlane;
        BetManager.OnBetStarted -= StartFlying;
    }

    private void CrushPlane()
    {
        DOTween.KillAll();
        _planeImage.enabled = false;
        _boomImage.gameObject.SetActive(true);
        Invoke(nameof(SetOffImage), 1f);
    }

    private void SetOffImage()
    {
        _boomImage.gameObject.SetActive(false);
    }
}
