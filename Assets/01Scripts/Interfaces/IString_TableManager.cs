/*
 작성자 : krokrai
 작성일 : 26-06-04

 역할 : 문자열을 직접 탐색하거나, 캐싱하는 것을 막기 위한 String_TableManager의 interface 전역적 접근
 */
using UnityEngine;

public interface IString_TableManager
{
    public String_TableSO GetStringSO(string s);
    public string GetString(string s, SystemLanguage language);
}
