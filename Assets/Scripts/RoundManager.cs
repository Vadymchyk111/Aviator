using System.Collections;
using TMPro;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private float _roundCooldown;
    [SerializeField] private BetManager _betManager;
    [SerializeField] private TextMeshProUGUI _secondsText;
    [SerializeField] private GameObject _delayPanel;
    [SerializeField] private GameObject _planePanel;

    private WaitForSeconds _waitForSeconds;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(1f);
    }

    private void Start()
    {
        StartCoroutine(nameof(RoundCoroutine));
    }

    private IEnumerator RoundCoroutine()
    {
        while (true)
        {
            yield return _betManager.StartBetCoroutine();
            yield return _waitForSeconds;
            yield return RoundDelayCoroutine();
        }
    }

    private IEnumerator RoundDelayCoroutine()
    {
        ActivateDelayPanel(true);
        float timer = 0f;
        while (timer<_roundCooldown)
        {
            _secondsText.text = Mathf.RoundToInt(_roundCooldown - timer).ToString();
            timer += Time.deltaTime;
            yield return null;
        }
        ActivateDelayPanel(false);
    }

    private void ActivateDelayPanel(bool isActive)
    {
        _delayPanel.SetActive(isActive);
        _planePanel.SetActive(!isActive);
    }
}
