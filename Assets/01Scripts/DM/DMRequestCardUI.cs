/*
작성자 : 이종현
작성일 : 26-06-17

역할 : DM 요청 카드 UI 출력 담당
방식 : Request_TableSO 데이터를 받아 요청 아이템과 보상 아이템 정보를 카드 UI에 표시
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DMRequestCardUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text requestDescriptionText;
    [SerializeField] private TMP_Text requestItemNameText;
    [SerializeField] private TMP_Text rewardItemNameText;
    [SerializeField] private TMP_Text requestItemCountText;
    [SerializeField] private TMP_Text rewardItemCountText;

    [Header("Image")]
    [SerializeField] private Image requestItemImage;
    [SerializeField] private Image rewardItemImage;
    
    private IString_TableManager _stringManager;
    
    private void Awake()
    {
        _stringManager = ServiceLocator.Get<IString_TableManager>();
    }
    
    private string GetString(string stringKey)
    {
        if (_stringManager == null)
        {
            Log.Message("String_TableManager를 찾을 수 없습니다.");
            return stringKey;
        }

        string text = _stringManager.GetString(stringKey, SystemLanguage.Korean);

        if (string.IsNullOrEmpty(text))
        {
            Log.Message($"String Key를 찾을 수 없습니다 : {stringKey}");
            return stringKey;
        }

        return text;
    }

    ///<summary>
    /// Request_TableSO 데이터를 요청 카드 UI에 반영합니다.
    ///</summary>
    public void SetData(Request_TableSO requestData, string npcName)
    {
        if (requestData == null)
        {
            Log.Message("Request 데이터가 없습니다.");
            return;
        }

        if (npcNameText != null)
            npcNameText.text = $"{npcName}님의 요청";

        if (requestDescriptionText != null)
            requestDescriptionText.text = "다음 아이템을 전달해 주세요.";

        if (requestItemNameText != null)
            requestItemNameText.text = $"({GetString(requestData.reqItemId)})";

        if (rewardItemNameText != null)
            rewardItemNameText.text = $"({GetString(requestData.rewardItemId)})";

        if (requestItemCountText != null)
            requestItemCountText.text = $"x{requestData.reqItemCount}";

        if (rewardItemCountText != null)
            rewardItemCountText.text = $"x{requestData.rewardItemCount}";

        LoadItemImage(requestData.reqImgId.ToString(), requestItemImage);
        LoadItemImage(requestData.rewardImgId.ToString(), rewardItemImage);
    }

    private void LoadItemImage(string imageKey, Image targetImage)
    {
        if (targetImage == null)
            return;

        if (string.IsNullOrEmpty(imageKey))
            return;

        IAddressableManager addressableManager =
            ServiceLocator.Get<IAddressableManager>();

        if (addressableManager == null)
        {
            Log.Message("AddressableManager를 찾을 수 없습니다.");
            return;
        }

        addressableManager.LoadImageSprite(imageKey, targetImage);
    }
}