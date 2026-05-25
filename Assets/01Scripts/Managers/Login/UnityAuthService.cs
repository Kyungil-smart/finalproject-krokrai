using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class UnityAuthService
{
    private const string PROVIDER_ID = "oidc-firebase";

    public static async Task InitializeAsync()
    {
        try
        {
            if (UnityServices.State == ServicesInitializationState.Initialized) return;

            await UnityServices.InitializeAsync();
            Log.Message("UnityAuthService: UGS Core 초기화 완료");
        }
        catch (Exception e)
        {
            Log.Message($"UnityAuthService: 초기화 실패 {e.Message}");
            throw;
        }
    }

    public static async Task SignInWithGoogleAsync(string firebaseToken)
    {
        try
        {
            if(AuthenticationService.Instance.IsSignedIn)
            {
                Log.Message($"UnityAuthService : 이미 로그인 상태, Player ID = {AuthenticationService.Instance.PlayerId}");
                return;
            }

            await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(PROVIDER_ID, firebaseToken);
            Log.Message($"UnityAuthService : UGS 로그인 완료, Player ID = {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Log.Message($"UnityAuthService: UGS 로그인 실패, 사유 : {e.Message}");
            throw;
        }
    }

    public static void SignOut()
    {
        if (!AuthenticationService.Instance.IsSignedIn) return;
        AuthenticationService.Instance.SignOut(clearCredentials : true); // true로 넘겨주면 로그아웃 후 토큰을 지워 자동 로그인을 막음
        Log.Message("UnityAuthService : UGS 로그아웃");
    }
}
