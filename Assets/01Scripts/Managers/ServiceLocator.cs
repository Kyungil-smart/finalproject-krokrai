/*
작성자 : krokrai
작성일 : 26-05-20
수정일 : 26-05-20

역할 : 전역적인 접근을 위한 class
방식 : Static으로 선언되어 있어 Runtime 전에 생성 및 전역적인 접근 가능
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new(6);

    /// <summary>
    /// ServiceLocator에 구독 / 반드시 미리 선정된 manager들만 호출 할 것
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public static void Register<T>(T service)
    {
        var t = typeof(T);
        if (!_services.ContainsKey(t)) _services.Add(t, service);
    }

    /// <summary>
    /// ServiceLocator에 구독 해제 / 구독과 같이 미리 선정된 manager에서만 호출
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    public static void UnRegister<T>(T service)
    {
        var t = typeof (T);
        if (_services.TryGetValue(t,out var current) && ReferenceEquals(current,service))
            _services.Remove(t);
    }

    /// <summary>
    /// 등록된 Manager Interface 기입하여 호출 후 및 내부 함수 호출
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>()
    {
        var t = typeof(T);
        if (_services.TryGetValue(t, out var service))
            return (T)service;
        return default;
    }

    /// <summary>
    /// 등록된 Manager 전부 표시
    /// </summary>
    public static void PrintServices()
    {
        Debug.Log("<color=red>[ServiceLocator] Print Services -----</color>");
        foreach( var service in _services)
        {
            Debug.Log($"{service.Key} : {service.Value}");
        }
        Debug.Log("<color=red>[ServiceLocator] Print Services End</color>");
    }
}
