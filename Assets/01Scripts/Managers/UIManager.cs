/*
작성자 : krokrai
작성일 : 26-05-20
수정일 : 26-05-20

역할 : Pop UI의 재사용을 위한 Manager
방식 : PopUp을 GameManager에 등록하고 PopUp()에서 구현
*/
using UnityEngine;

public class UIManager : MonoBehaviour, IUIManager, IManagerBooter
{
    [SerializeField] private GameObject _popups;

    /// <summary>
    /// Interface 구현용 팝업 함수 실제 호출은 ServiceLocator에서 호출
    /// </summary>
    public void PopUp() // TODO : 임시 작업되어 있으니 나중에 다시 작업
    {
        // 여기에 팝업 상세 구현
        Log.MessageColor("팝업!", Color.red);
    }

    public void Register() => ServiceLocator.Register<IUIManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IUIManager>(this);
}
