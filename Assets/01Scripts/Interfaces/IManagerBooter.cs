/*
작성자 : krokrai
작성일 : 26-05-20
수정일 : 26-05-20

역할 : ServiceLocator에 등록 및 해제
방식 : Interface를 이용해 선언된 manager들을 등록 해준다.
*/
public interface IManagerBooter
{
    /// <summary>
    /// ServiceLocator 등록
    /// </summary>
    public void Register();
    /// <summary>
    /// ServiceLocator 해제
    /// </summary>
    public void UnRegister();
}
