/*
작성자 : krokrai
작성일 : 26-05-20
수정일 : 26-05-20

역할 : ServiceLocator 기반 호출 용 Interface
방식 : ServiceLocator.Get<IUIManager>.PopUp(); 이런 식으로 호출
*/
public interface IUIManager
{
    /// <summary>
    /// 팝업 호출 시 이걸 호출
    /// </summary>
    public void PopUp(); // 어떤 팝업을 띄우고 어떤 데이터가 필요한지 구현

}
