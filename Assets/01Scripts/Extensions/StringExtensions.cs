/*
작성자 : krokrai
수정자 : NekioEmilia
작성일 : 26-05-21
수정일 : 26-05-29

역할 : 문자열을 변환 및 예외 처리 간편화
방식 : TryParse를 이용해 분기 처리
*/
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class StringExtensions
{
    public static void SetData(this string s, out int i,
        [CallerFilePath] string filePath = "")
    {

        if (s == "" || s == null)
        {
            i = default;
            Log.Message("기본 값 적용 됌 : int");
        }
        else if (int.TryParse(s, out i))
            return;
        else
        {
            Debug.LogError($"[{Path.GetFileNameWithoutExtension(filePath)}] 문자열 변환 실패 / 실패한 문자열 : {s}");
        }
    }
    public static void SetData(this string s, out float f,
        [CallerFilePath] string filePath = "")
    {
        if (s == "" || s == null)
        {
            f = default;
            Log.Message("기본 값 적용 됌 : float");
        }
        else if (float.TryParse(s, out f))
            return;
        else
        {
            Debug.LogError($"[{Path.GetFileNameWithoutExtension(filePath)}] 문자열 변환 실패 / 실패한 문자열 : {s}");
        }
    }
    public static void SetData(this string s, out bool b,
        [CallerFilePath] string filePath = "")
    {
        if (s == "1" || s == "0")
        {
            b = s == "1" ? true : false;
            return;
        }
        
        else if (s == "" || s == null)
        {
            b = default;
            Log.Message("기본 값 적용 됌 : bool");
        }
        else if (bool.TryParse(s, out b))
            return;
        else
        {
            Debug.LogError($"[{Path.GetFileNameWithoutExtension(filePath)}] 문자열 변환 실패 / 실패한 문자열 : {s}");
        }
    }

    public static void SetData<T>(this string s, out T e,
        [CallerFilePath] string filePath = "") where T : struct
    {
        if (s == "" || s == null)
        {
            e = default;
            Log.Message("기본 값 적용 됌 : enum");
        }
        else if (Enum.TryParse<T>(s,true,out e))
            return;
        else
        {
            Debug.LogError($"[{Path.GetFileNameWithoutExtension(filePath)}] 문자열 변환 실패 / 실패한 문자열 : {s}");
        }
    }
}
