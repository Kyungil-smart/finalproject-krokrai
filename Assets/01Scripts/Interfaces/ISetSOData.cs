/*
작성자 : krokrai
작성일 : 26-05-21
수정일 : 26-05-21

역할 : SO에 데이터 주입 용 interface
방식 : 문자열을 주입하여 SO 내부에서 처리
*/
public interface ISetSOData
{
    public void SetData(string[] datas);
}
