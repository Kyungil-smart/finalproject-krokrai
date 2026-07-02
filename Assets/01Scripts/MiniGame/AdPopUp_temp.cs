using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AdPopUp_temp : MonoBehaviour
{
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private GameObject _adPopUp;
    //[SerializeField] private 

    TaskCompletionSource<bool> _taskResult;

    const string _basicNotic = "광고 중... ";

    int _count = 3;

    public async Task<bool> OnAd()
    {
        _adPopUp.SetActive(true);

        _taskResult?.TrySetCanceled();
        _taskResult = new();
        _closeButton.SetActive(false);

        StartCoroutine(Waiter());

        return await _taskResult.Task;
    }

    IEnumerator Waiter()
    {
        for (int i = _count; 0 < i; i--)
        {
            _loadingText.text = $"{_basicNotic} {i}";
            yield return new WaitForSeconds(1f);
        }
        
        _loadingText.text = $"{_basicNotic} {0}";

        _closeButton.SetActive(true);
    }

    public void OnCloseClick()
    {
        ServiceLocator.Get<IDataManager>().UserGoods.Claw_ += 1;
        _adPopUp.SetActive(false);
        _taskResult.TrySetResult(true);
    }
}
