/*
 작성자 : krokrai
 작성일 : 26-05-29

 역할 : test 및 sample용 Login UI
 방식 : UnityAuthService script에 접근하여 Google 로그인을 시도 및 기기에 남아 있는 Token을 사용해 자동 로그인
*/
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private Button _gameStart;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _logoutButton;
    [SerializeField] private TextMeshProUGUI _statusText;

    private bool _isProcessing;

    private void Awake() => UpdateStatus("초기화...");
    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();
    private async void Start() => await TryAutoLoginAsync();

    private void BindButtonEvents()
    {
        _gameStart.interactable = false;
        _gameStart.onClick.AddListener(OnGameStartClicked);
        _loginButton.onClick.AddListener(OnLoginClicked);
        _logoutButton.onClick.AddListener(OnLogoutClicked);
    }

    private void UnbindButtonEvents()
    {
        _gameStart.onClick.RemoveListener(OnGameStartClicked);
        _loginButton.onClick.RemoveListener(OnLoginClicked);
        _logoutButton.onClick.RemoveListener(OnLogoutClicked);
    }

    private void OnGameStartClicked()
    {
        SceneManager.LoadScene(1);
    }

    private async Task TryAutoLoginAsync()
    {
        bool firebaseOk = await ServiceLocator.Get<IBackendManager>().ReadyTask;
        if (!firebaseOk)
        {
            UpdateStatus("Firebase 초기화 실패");
            return;
        }

        try
        {
            await UnityAuthService.InitializeAsync();

            FirebaseUser user = ServiceLocator.Get<IBackendManager>().Auth.CurrentUser;
            if (user == null)
            {
                UpdateStatus("대기");
                return;
            }

            await RestoreSessionAsync(user);
        }
        catch (Exception e)
        {
            UpdateStatus($"자동 로그인 실패: {e.Message}");
        }
    }

    private async Task RestoreSessionAsync(FirebaseUser user)
    {
        UpdateStatus("세션 복원...");
        string firebaseIdToken = await user.TokenAsync(false);
        await UnityAuthService.SignInWithGoogleAsync(firebaseIdToken);
        UpdateStatus($"환영합니다, {GetDisplayName(user)}님");
        _gameStart.interactable = true;
    }

    private async void OnLoginClicked()
    {
        if (_isProcessing) return;
        UpdateStatus($"로그인 시도 중...");
        _isProcessing = true;
        SetButtonsInteractable(false);

        try
        {
            await PerformLoginAsync();
        }
        catch (Exception e)
        {
            UpdateStatus($"로그인 실패: {e.Message}");
        }
        finally
        {
            _isProcessing = false;
            SetButtonsInteractable(true);
            SceneManager.LoadScene(0);
        }
    }

    private async Task PerformLoginAsync()
    {
        UpdateStatus("UGS 초기화...");
        await UnityAuthService.InitializeAsync();

        UpdateStatus("Google 로그인 시도...");
        FirebaseUser user = await GoogleSignInService.SignInAsync();

        UpdateStatus("UGS 인증 시도...");
        string firebaseIdToken = await user.TokenAsync(false);
        await UnityAuthService.SignInWithGoogleAsync(firebaseIdToken);

        UpdateStatus($"환영합니다, {GetDisplayName(user)}님");
        _gameStart.interactable = true;
    }

    private void OnLogoutClicked()
    {
        if (_isProcessing) return;
        _gameStart.interactable = false;
        UnityAuthService.SignOut();
        GoogleSignInService.SignOut();
        UpdateStatus("로그아웃");
    }

    private static string GetDisplayName(FirebaseUser user)
        => string.IsNullOrEmpty(user.DisplayName) ? user.Email : user.DisplayName;

    private void UpdateStatus(string message)
        => _statusText.text = message;

    private void SetButtonsInteractable(bool interactable)
    {
        _loginButton.interactable = interactable;
        _logoutButton.interactable = interactable;
    }
}
