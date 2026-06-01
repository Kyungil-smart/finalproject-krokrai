/*
 작성자 : cminhyeong1999
 작성일 : 26-05-29

 역할 : Realtime Database 내부에 User 하위에 있는 데이터를 script에 저장
 방식 :  1. GetRawJsonValue로 데이터를 string A로 저장
        2. JsonUtility.FromJson<RealTimeUserDatas>(A)로 Realtime Database에 존재하는 데이터의 이름이 같은 변수에 저장
            ex) Realtime Database 에 Energy : 30 이 존재하고, 현재 클래스에 Energy가 존재하면 이 클래스의 Energy에 30 저장
*/

using Firebase.Firestore;
using System;

[Serializable, FirestoreData]
public class RealTimeUserDatas
{
    public int Energy;
    public int Coin;
    public int Gem;
    public int Stone;
    public int FurDoll;
    public int Claw;

    /// <summary>
    /// 보유한 재화량을 문자열로 출력하기 위하여 ToString을 override함<br/>
    /// $"[재화 보유 현황]\n" + $"[Energy 보유 현황 : {Energy}]\n" + $"[Coin 보유 현황 : {Coin}]\n" + ... 형식<br/>
    /// 실제로 RTDB가 읽고 쓰이는지를 확인하기 위함, 추후 제거할 가능성 있음
    /// </summary>
    public override string ToString()
    {
        return $"[재화 보유 현황]\n" +
               $"[Energy 보유 현황 : {Energy}]\n" +
               $"[Coin 보유 현황 : {Coin}]\n" +
               $"[Gem 보유 현황 : {Gem}]\n" +
               $"[Stone 보유 현황 : {Stone}]\n" +
               $"[FurDoll 보유 현황 : {FurDoll}]\n" +
               $"[Claw 보유 현황 : {Claw}]\n";
    }
}