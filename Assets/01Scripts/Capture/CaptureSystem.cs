/*
 작성자 : 23M-RFT68
 작성일 : 26-06-18
 수정일 : 26-06-18

 역할 : Object_TableSO와 Post_TableSO를 이용하여, 초회 획득 여부를 판단하고 캡처를 진행해주는 역할
 방식 : SO 데이터를 연결해서 데이터 조회 후, 초회 획득 여부를 판단하고 맞다면 캡처 후 미리보기 스크립트에게 전달하고,
       아니라면 토스트 메시지 스크립트에 전달 후 토스트 메시지 뜨게 하는 방식
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureSystem : MonoBehaviour
{
    [Header("데이터 테이블")] 
    [SerializeField] private AutoSOGen_ContaineSO _objectTable;   // Object_TableSO_Root
    [SerializeField] private AutoSOGen_ContaineSO _postTable;     // Post_TableSO_Root

    [Header("연출 컴포넌트")] 
    [SerializeField] private CaptureFlashEffect _captureFlash;      // 반짝 연출
    [SerializeField] private ToastMessage _toastMessage;            // 토스트 메시지(중복 획득)
    [SerializeField] private ToastMessage _errorToast;              // 토스트 메시지(에러)
    [SerializeField] private CapturePreviewPopup _previewPopup;     // 미리보기 팝업
    [SerializeField] private ObjectEditMode _editMode;
    
    private bool _isCapturing = false;  // 캡처 중 중복 클릭 방지
    
    /// <summary>
    /// 캡처 버튼 눌렀을때 실행되게 하기
    /// 캡처 버튼 직접 프리팹 버튼에서 Onclick()연결
    /// Inspector에서 int 값 지정 예정
    /// </summary>
    public void OnCaptureClick(int objectImg)
    {
        if (_isCapturing) return;  // 중복 클릭 방지
        StartCoroutine(CaptureRoutine(objectImg));
    }

    // 캡처 전체 루틴
    private IEnumerator CaptureRoutine(int objectImg)
    {
        _isCapturing = true;
        
        // 1. PostUserTableSO에서 postId 찾기
        Object_TableSO objectSO = FindObjectSOByObjectImg(objectImg);
        if (objectSO == null)
        {
            Log.Message($"postId {objectImg}를 찾을 수 없음");
            _isCapturing = false;
            yield break;
        }
        
        // 2. 반짝 연출 시작 (0.5초) + DB 조회 병렬 진행
        bool isFirstCapture = false;
        bool dbCheckDone = false;
        
        // 반짝 연출
        StartCoroutine(_captureFlash.PlayFlash());
        
        // DB 조회 (병렬)
        StartCoroutine(CheckImgListRoutine(objectSO.getImg, result =>
        {
            isFirstCapture = result;
            dbCheckDone = true;
        }));
        
        // 반짝 연출 0.5초 대기
        yield return new WaitForSeconds(0.5f);
        
        // 3. DB 조회 완료 대기 (최대 5초)
        float waitTime = 0f;
        while (!dbCheckDone && waitTime < 5f)
        {
            // 로딩 아이콘 표시
            if (waitTime > 0f)
                _editMode.ShowLoadingIcon(true);        // 로딩 시작
            
            waitTime += Time.deltaTime;
            yield return null;
        }
        
        _editMode.ShowLoadingIcon(false);
        
        // 4. 5초 내 완료 안 됨 -> 오류처리
        if (!dbCheckDone)
        {
            _errorToast.Show();
            _isCapturing = false;
            yield break;
        }
        
        // 5. 최초 획득 vs 중복 획득 분기
        if (isFirstCapture)
        {
            // 최초 획득 -> ImgList에 추가 -> 미리보기 팝업
            yield return StartCoroutine(FirstCaptureRoutine(objectSO.getImg));
        }
        else
        {
            // 중복 획득 -> 토스트 메시지
            _toastMessage.Show();
        }
        
        _isCapturing = false;
    }

    // ImgList에 이미 있는지 확인
    // result: true = 최초 획득, false = 중복
    private IEnumerator CheckImgListRoutine(int getImg, System.Action<bool> result)
    {
        try
        {
            var dataManager = ServiceLocator.Get<IDataManager>();
            var imgList = dataManager.UserDatas.ImgList;
            result(!imgList.ContainsKey(getImg.ToString()));
        }
        catch (Exception e)
        {
                Log.Message($"DB 조회 오류: {e.Message}");
                result(false);
        }

        yield return null;
    }

    // 최초 획득 처리
    private IEnumerator FirstCaptureRoutine(int getImg)
    {
        // 데이터 매니저와 리스트 미리 확보
        var dataManager = ServiceLocator.Get<IDataManager>();
        string imgKey = getImg.ToString();
        
        
        // getImg로 Post_TableSO에서 postId 찾기
        int postId = FindPostIdByGetImg(getImg);
        if (postId == 0)
        {
            Log.Message("포스트 ID가 없음");
            yield break ;
        }

        // 미리보기 팝업 표시
        
        _previewPopup.gameObject.SetActive(true);

        yield return null;
        
        _previewPopup.Show(getImg, postId, _editMode);
        
        
                
        // ImgList에 추가
        dataManager.UserDatas.ImgList[imgKey] = new ImageState();
        Log.Message($"이미지 획득! key: {getImg}");
    }

    // objectImg로 Object_TableSO찾기
    private Object_TableSO FindObjectSOByObjectImg(int objectImg)
    {
        for (int i = 0; i < _objectTable.scriptableObjects.Length; i++)
        {
            if (_objectTable.scriptableObjects[i] is Object_TableSO so)
                if (so.objectImg == objectImg)
                    return so;
        }
        return null;
    }
    
    // getImg로 Post_TableSO에서 postId 찾기
    private int FindPostIdByGetImg(int getImg)
    {
        for (int i = 0; i < _postTable.scriptableObjects.Length; i++)
            if (_postTable.scriptableObjects[i] is Post_TableSO so)
                if (so.postImage == getImg)
                    return so.postID;
        
        Log.Message($"getImg {getImg} 에 해당하는 postId가 없음");
        return 0;
    }
}
