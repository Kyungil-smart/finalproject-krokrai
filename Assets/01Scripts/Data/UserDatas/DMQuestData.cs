/*
작성자 : 이종현
작성일 : 26-06-16
수정일 : 26-06-16

역할 : DM 퀘스트 생성 시간 저장
방식 : DM 퀘스트 생성 주기 계산을 위한 타임스탬프를 Firestore에 저장
*/

using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class DMQuestData
{
    private DateTime _dmGenerationTimestamp = DateTime.MinValue;

    [FirestoreProperty]
    public DateTime DMGenerationTimestamp
    {
        get
        {
            return _dmGenerationTimestamp;
        }
        set
        {
            _dmGenerationTimestamp = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }
}