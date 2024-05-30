using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BetManager : MonoBehaviour
{
    public static event Action<bool> OnBetStarted;
    public static event Action OnBetEnded;
    public static event Action OnFinished;
    
    [SerializeField] private float _betExplosionTime;
    [SerializeField] private float _betMaxPercentageTime;
    [SerializeField] private float _betMaxPercentage;
    [SerializeField] private BetManagerView _betManagerView;
    [SerializeField] private float _money;
    
    private float _percentage = 1f;
    private int _currentBid;
    private float _betTimer;
    private Coroutine _betCoroutine;
    private bool _isBetSet;
    private bool _isBetCollected;
    
    private const float BET_MIN_PERCENTAGE = 1f;

    private void Start()
    {
        _betManagerView.UpdateMoneyText(_money);
    }

    private void OnEnable()
    {
        _betManagerView.BetButton.onClick.AddListener(SetBet);
    }

    private void OnDisable()
    {
        _betManagerView.BetButton.onClick.RemoveListener(SetBet);
    }

    private void SetBet()
    {
        _currentBid = int.Parse(_betManagerView.BetInputField.text);
        if (!CheckIfEnoughMoney())
        {
            return;
        }
        
        _isBetSet = true;
        _isBetCollected = false;
        _betManagerView.ActivateButton(false);
        LoseMoney(_currentBid);
        _betManagerView.UpdateMoneyText(_money);

        SubscribeToCollect();
    }

    private bool CheckIfEnoughMoney()
    {
        return _currentBid <= _money;
    }

    private void CollectBet()
    {
        _betManagerView.ActivateButton(false);
        _isBetCollected = true;
        WinCheck(_betTimer < _betExplosionTime);
        OnBetEnded?.Invoke();
        SubscribeToBet();
    }

    private void WinMoney(int bid)
    {
        _money += (float)Math.Round(bid * _percentage, 2);
    }
    
    private void LoseMoney(int bid)
    {
        _money -= bid;
    }

    private void WinCheck(bool isWin)
    {
        if (isWin)
        {
            WinMoney(_currentBid);
        }
        else
        {
            LoseMoney(_currentBid);
        }
        
        _betManagerView.UpdateMoneyText(_money);
    }

    private IEnumerator BetCoroutine()
    {
        if (_betManagerView.IsBetToggleOn && !_isBetSet)
        {
            SetBet();
        }
        
        OnBetStarted?.Invoke(_isBetSet);
        _betManagerView.ActivateButton(_isBetSet);
        
        _betTimer = 0f;
        _percentage = 0f;
        _betExplosionTime = Random.Range(1.5f, 5f);
        while (_betTimer < _betExplosionTime)
        {
            float t = _betTimer / _betMaxPercentageTime;
            _percentage = Mathf.Lerp(BET_MIN_PERCENTAGE, _betMaxPercentage, t);
            if (_betManagerView.IsWithdrawToggleOn && !_isBetCollected && _percentage >= float.Parse(_betManagerView.PercentageInputField.text))
            {
                CollectBet();
            }
            _betManagerView.UpdatePercentageText(_percentage);
            Debug.Log(_percentage);
            _betTimer += Time.deltaTime;
            
            yield return null;
        }

        SubscribeToBet();
        OnBetEnded?.Invoke();
        OnFinished?.Invoke();
        _isBetSet = false;
        _betManagerView.ActivateButton(true);
    }

    private void SubscribeToBet()
    {
        _betManagerView.BetButton.onClick.RemoveAllListeners();
        _betManagerView.BetButton.onClick.AddListener(SetBet);
    }
    
    private void SubscribeToCollect()
    {
        _betManagerView.BetButton.onClick.RemoveAllListeners();
        _betManagerView.BetButton.onClick.AddListener(CollectBet);
    }

    public Coroutine StartBetCoroutine()
    {
        _betCoroutine = StartCoroutine(nameof(BetCoroutine));
        return _betCoroutine;
    }
}