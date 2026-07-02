/*
 작성자 : krokrai
 작성일 : 26-06-19
 수정일 : 26-07-02

 역할 : 미니게임 상점 관리자
 방식 : 정해진 비용내에서 빠른 처리
 */
using TMPro;
using UnityEngine;

public class MiniGame_TokenShop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _token;

    [SerializeField] int _pigCost;
    [SerializeField] int _clockCost;
    [SerializeField] int _scissorsCost;
    [SerializeField] int _magicHatCost;
    [SerializeField] int _energyCost;
    [SerializeField] int _boxCost;
    [SerializeField] int _basketCost;

    [SerializeField] TextMeshProUGUI _pigText;
    [SerializeField] TextMeshProUGUI _clockText;
    [SerializeField] TextMeshProUGUI _scissorsText;
    [SerializeField] TextMeshProUGUI _magicHatText;
    [SerializeField] TextMeshProUGUI _energyText;
    [SerializeField] TextMeshProUGUI _boxText;
    [SerializeField] TextMeshProUGUI _basketText;

    int _tokenCount = 0;

    private void Awake()
    {
        _pigText.text = _pigCost.ToString();
        _clockText.text = _clockCost.ToString();
        _scissorsText.text = _scissorsCost.ToString();
        _magicHatText.text = _magicHatCost.ToString();
        _energyText.text = _energyCost.ToString();
        _boxText.text = _boxCost.ToString();
        _basketText.text = _basketCost.ToString();
    }

    private void OnEnable()
    {
        RefreshToken();
        RefreshUI();
    }

    public void Buy(string s)
    {
        switch(s)
        {
            case "Pig":
                if (_tokenCount - _pigCost < 0)
                    return;
                _tokenCount -= _pigCost;

                if (_tokenCount < _pigCost)
                    _pigText.color = Color.red;
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
                break;
            case "Clock":
                if (_tokenCount - _clockCost < 0)
                    return;
                _tokenCount -= _clockCost;

                if (_tokenCount < _clockCost)
                    _clockText.color = Color.red;
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
                break;
            case "Scissors":
                if (_tokenCount - _scissorsCost < 0)
                    return;
                _tokenCount -= _scissorsCost;

                if (_tokenCount < _scissorsCost)
                    _scissorsText.color = Color.red;
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
                break;
            case "MagicHat":
                if (_tokenCount - _magicHatCost < 0)
                    return;
                _tokenCount -= _magicHatCost;

                if (_tokenCount < _magicHatCost)
                    _magicHatText.color = Color.red;
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
                break;
            case "Energy":
                if (_tokenCount - _energyCost < 0)
                    return;
                _tokenCount -= _energyCost;

                if (_tokenCount < _energyCost)
                    _energyText.color = Color.red;
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
                break;
            case "Box":
                if (_tokenCount - _boxCost < 0)
                    return;
                _tokenCount -= _boxCost;

                if (_tokenCount < _boxCost)
                    _boxText.color = Color.red;
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
                break;
            case "Basket":
                if (_tokenCount - _basketCost < 0)
                    return;
                _tokenCount -= _basketCost;

                if (_tokenCount < _basketCost)
                    _basketText.color = Color.red;
                ServiceLocator.Get<IAudioManager>().PlaySFX(SFXAudiosEnum.PURCHASED);
                break;
        }
    }

    private void RefreshToken()
    {
        _tokenCount = ServiceLocator.Get<IDataManager>().UserDatas.Minigame.Token_Owned;
        _token.text = _tokenCount.ToString();
    }

    private void RefreshUI()
    {
        if (_tokenCount - _pigCost < 0)
            _pigText.color = Color.red;
        else
            _pigText.color = Color.black;
        
        if (_tokenCount - _clockCost < 0)
            _clockText.color = Color.red;
        else
            _clockText.color = Color.black;

        if (_tokenCount - _scissorsCost < 0)
            _scissorsText.color = Color.red;
        else
            _scissorsText.color = Color.black;
        
        if (_tokenCount - _magicHatCost < 0)
            _magicHatText.color = Color.red;
        else
            _magicHatText.color = Color.black;
        
        if (_tokenCount - _energyCost < 0)
            _energyText.color = Color.red;
        else
            _energyText.color = Color.black;
        
        if (_tokenCount - _boxCost < 0)
            _boxText.color = Color.red;
        else
            _boxText.color = Color.black;

        if (_tokenCount - _basketCost < 0)
            _basketText.color = Color.red;
        else
            _basketText.color = Color.black;
    }

    private void OnDisable()
    {
        ServiceLocator.Get<IDataManager>().UserDatas.Minigame.Token_Owned = _tokenCount;
    }

}
