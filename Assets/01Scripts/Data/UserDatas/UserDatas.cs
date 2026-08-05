/*
 작성자 : krokrai
 수정자 : 이종현
 작성일 : 26-05-28
 수정일 : 26-06-15

 역할 : FireStore 내부에 UserDatas 하위에 있는 데이터를 script에 저장
 방식 : FireStore에서 정보를 불러온 후 ConvertTo<UserDatas>()를 해주면, Google 쪽에서 자동으로 scrip에 저장
 */
using Firebase.Firestore;
using System;
using System.Collections.Generic;

[Serializable, FirestoreData]
public class UserDatas
{
    [FirestoreProperty]
    public Attendance Attendance { get; set; } = new();

    [FirestoreProperty]
    public Event_Missions Event_Mission { get; set; } = new();

    [FirestoreProperty]
    public ProFile Profile { get; set; } = new();

    [FirestoreProperty]
    public Dictionary<string, ImageState> ImgList { get; set; } = new();

    //public List<int> UserPost { get; set; } = new();
    private List<int> _userPost = new();

    [FirestoreProperty]
    public List<int> UserPost
    {
        get
        {
            return _userPost;
        }
        set
        {
            _userPost = value;
            ServiceLocator.Get<IDataAutoSaveManager>().RequestSave();
        }
    }

    //public Dictionary<string, UserPostState> UserPost { get; set; } = new();

    [FirestoreProperty]
    public Dictionary<string, DMProgress> DMProgress { get; set; } = new();
    
    [FirestoreProperty]
    public DMQuestData DMQuest { get; set; } = new();
    
    [FirestoreProperty]
    public Dictionary<string, ImageState> NPCPost { get; set; } = new();

    [FirestoreProperty]
    public FollowerData Follower { get; set; } = new();

    [FirestoreProperty]
    public NyangBakeryDatas NyangBakery { get; set; } = new();

    [FirestoreProperty]
    public MiniGameDatas Minigame { get; set; } = new();
}
