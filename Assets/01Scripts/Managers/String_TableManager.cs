/*
 작성자 : krokrai
 작성일 : 26-06-04
 수정일 : 26-06-29

 역할 : 문자열을 직접 탐색하거나, 캐싱하는 것을 막기 위한 전역적 접근
 방식 : Dictionary에 저장 및 SO 또는 string 형태로 반환
 */
using System.Collections.Generic;
using UnityEngine;

public class String_TableManager : MonoBehaviour, IString_TableManager, IManagerBooter
{
    [SerializeField] private AutoSOGen_ContaineSO _strTableSO;

    private Dictionary<string, String_TableSO> _strings = new(480);

    private void Awake()
    {
        String_TableSO s;
        for (int i = 0; i < _strTableSO.scriptableObjects.Length; i++)
        {
            if (_strTableSO.scriptableObjects[i] is String_TableSO)
            {
                s = (_strTableSO.scriptableObjects[i] as String_TableSO);
                _strings.Add(s.stringId, s);
            }
        }
    }

    /// <summary>
    /// stringID를 입력하면 SO를 반환
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public String_TableSO GetStringSO(string s)
    {
        if (_strings.ContainsKey(s))
        {
            return _strings[s];
        }
        return null;
    }

    /// <summary>
    /// 가급적 SO로 반환 받을 것
    /// </summary>
    /// <param name="s"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public string GetString(string s, SystemLanguage language)
    {
        if (_strings.ContainsKey(s))
        {
            switch(language)
            {
                case SystemLanguage.English:
                    return _strings[s].EN;
                case SystemLanguage.Korean:
                    return _strings[s].KR;
            }
        }
        return null;
    }

    public void Register() => ServiceLocator.Register<IString_TableManager>(this);

    public void UnRegister() => ServiceLocator.UnRegister<IString_TableManager>(this);
}
