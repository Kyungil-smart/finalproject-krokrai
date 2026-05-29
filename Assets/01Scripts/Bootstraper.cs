/*
작성자 : krokrai
작성일 : 26-05-20
수정일 : 26-05-20

역할 : 등록된 순서에 따라 순차적으로 생성 되어 생성 주기에 따른 Null 참조 방지
방식 : MonoBehaviour를 상속 받은 객체를 등록 후 배열을 순회하며, IManagerBooter를 이용해 ServiceLocator에 등록
*/
using UnityEngine;

public class Bootstraper : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _booters;

    private Bootstraper _bootstrapter;

    private bool _isInit;

    private void Awake()
    {
        if (_bootstrapter == null)
        {
            _isInit = true;
            _bootstrapter = this;
            DontDestroyOnLoad(this);
            InitManagers();
        }
        else
        {
            _isInit = false;
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        UnRegisterManagers();
    }

    private void InitManagers()
    {
        foreach (var manager in _booters)
        {
            if (!(manager is IManagerBooter))
            {
                Log.MessageColor($"IManagerBooter를 찾을 수 없습니다. {manager.name}", Color.red);
                return;
            }
            (manager as IManagerBooter).Register();
            Log.Message($"{manager.ToString()} 등록 완료");
        }
    }
    private void UnRegisterManagers()
    {
        if (!_isInit) return;
        foreach (var manager in _booters)
        {
            (manager as IManagerBooter).UnRegister();
        }
    }

    [ContextMenu("ManagerList")]
    private void PrintRegistedManagerList()
    {
        ServiceLocator.PrintServices();
    }
}
