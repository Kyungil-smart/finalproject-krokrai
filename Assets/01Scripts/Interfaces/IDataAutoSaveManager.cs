/*
 작성자 : krokrai
 작성일 : 26-05-29
 수정일 : 26-06-09

 역할 : 자동 저장 구현에 간편함을 위해 Service Locator에 등록하기 위한 interface
 */

public interface IDataAutoSaveManager
{
    public void RequestSave();
    public void RequestRTDBSave();
    public void SetMainCurrencyController(MainCurrencyController mainCurrencyController);

#if UNITY_EDITOR
    public void SetTestMode();
#endif
}
