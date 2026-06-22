/*
작성자 : NekioEmilia
수정자 : 
 
작성일 : 26-06-19
수정일 : 

역할 : ServiceLocator에 연결하기 위해 public 메서드를 모아둔 인터페이스
방식 : PlayTimeTracker에서 상속 받아 시뮬레이터와 연결
*/

using System;

public interface IPlayTimeTracker
{
    public void OnTimeAdvanced(DateTime time);
}