/*
작성자 : 이종현
작성일 : 26-06-16

역할 : DM 퀘스트 보상 메시지 출력 상태 구분
방식 : 보상 메시지 출력 여부를 enum으로 관리
*/

public enum QuestRewardStateEnum
{
    None = 0,
    RewardMessagePrinted = 1
}