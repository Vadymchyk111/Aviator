using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BidButton
{
    public Button Button;
    public float MoneyCount;
}

public class BetManagerView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _betInputField;
    [SerializeField] private TMP_InputField _percentageInputField;
    [SerializeField] private Button _betButton;
    [SerializeField] private BidButton[] _bidButtons;
    [SerializeField] private TextMeshProUGUI _betButtonText;
    [SerializeField] private TextMeshProUGUI _betMoneyText;
    [SerializeField] private TextMeshProUGUI _betPercentageText;
    [SerializeField] private Toggle _betToggle;
    [SerializeField] private Toggle _withdrawToggle;
    [SerializeField] private Sprite _collectSprite;
    [SerializeField] private Sprite _betSprite;
    [SerializeField] private Image _betButtonImage;

    public TMP_InputField BetInputField => _betInputField;
    public TMP_InputField PercentageInputField => _percentageInputField;
    public Button BetButton => _betButton;
    public bool IsBetToggleOn => _betToggle.isOn;
    public bool IsWithdrawToggleOn => _withdrawToggle.isOn;

    private void OnEnable()
    {
        BetManager.OnBetStarted += SetCollectButton;
        BetManager.OnBetEnded += SetBetButton;
        foreach (BidButton bidButton in _bidButtons)
        {
            bidButton.Button.onClick.AddListener(() => _betInputField.text = bidButton.MoneyCount.ToString(CultureInfo.InvariantCulture));
        }
    }

    private void OnDisable()
    {
        BetManager.OnBetStarted -= SetCollectButton;
        BetManager.OnBetEnded -= SetBetButton;
    }

    private void SetCollectButton(bool isBetSet)
    {
        if (!isBetSet)
        {
            return;
        }
        _betButtonText.text = "Collect";
        _betButtonImage.sprite = _collectSprite;
    }
    
    private void SetBetButton()
    {
        _betButtonText.text = "BET";
        _betButtonImage.sprite = _betSprite;
    }

    public void UpdateMoneyText(float money)
    {
        _betMoneyText.text = Math.Round(money, 2).ToString(CultureInfo.InvariantCulture);
    }
    
    public void UpdatePercentageText(float percentage)
    {
        _betPercentageText.text = $"{Math.Round(percentage, 2).ToString(CultureInfo.InvariantCulture)}x";
    }

    public void ActivateButton(bool isActive)
    {
        _betButton.interactable = isActive;
    }
}
