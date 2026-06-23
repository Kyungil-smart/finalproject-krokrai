/*
 작성자 : krokrai
 작성일 : 26-05-21
 수정일 : 26-05-21

 역할 : Post_Table에서 작성자와 획득 경로 분류용 enum
 */

public enum ProfileTypeEnum : byte
{
    NPC,USER
}

public enum Route : byte
{
    STORY, EVENT, CAPTURE
}
