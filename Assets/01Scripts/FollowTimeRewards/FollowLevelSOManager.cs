/*
 작성자 : cminhyeong1999
 작성일 : 26-06-15
 수정일 : 26-06-15

 역할 : FollowLevelSO를 쉽게 접근하기 위한 매니저
*/
using UnityEngine;

public class FollowLevelSOManager : MonoBehaviour
{
    [SerializeField] private Follow_Level_TableSO[] _followLevels;

    /// <summary>
    /// 현재 팔로워 레벨의 이름을 가져오는 메서드
    /// </summary>
    /// <param name="follow">현재 팔로워 수</param>
    /// <returns>현재 팔로워 레벨 이름</returns>
    public string GetFollowTierName(int follow)
    {
        int index = GetFollowLevel(follow);
        return _followLevels[index].tierName;
    }
    
    /// <summary>
    /// 현재 팔로워 레벨의 최대 에너지 보상치를 가져오는 메서드
    /// </summary>
    /// <param name="follow">현재 팔로워 수</param>
    /// <returns>현재 팔로워 레벨의 최대 에너지</returns>
    public int GetEnergyLevel(int follow)
    {
        int index = GetFollowLevel(follow);
        return _followLevels[index].maxEnergy;
    }

    /// <summary>
    /// 현재 팔로워 레벨의 최대 코인 보상치를 가져오는 메서드
    /// </summary>
    /// <param name="follow">현재 팔로워 수</param>
    /// <returns>현재 팔로워 레벨의 최대 코인</returns>
    public int GetCoinLevel(int follow)
    {
        int index = GetFollowLevel(follow);
        return _followLevels[index].maxCoin;
    }
    
    /// <summary>
    /// 현재 팔로워를 기준으로 다음 레벨 도달에 필요한 팔로워 수치 가져오는 메서드
    /// </summary>
    /// <param name="follow">현재 팔로워 수치</param>
    /// <returns>레벨업에 필요한 팔로워 수치</returns>
    public int GetNextTierFollowValue(int follow)
    {
        int index = GetFollowLevel(follow);
        int nextIndex = Mathf.Clamp(index + 1, 0, _followLevels.Length);
        return _followLevels[nextIndex].requiredFollowers;
    }

    /// <summary>
    /// 현재 팔로워 수가 어느 레벨인지 가져오는 메서드
    /// </summary>
    /// <param name="follow">현재 팔로워 수</param>
    /// <returns>현재 팔로워 레벨</returns>
    private int GetFollowLevel(int follow)
    {
        int result = follow switch
        {
            < 0             => -1,
            >= 0 and < 500  => 0,
            >= 500 and < 2000 => 1,
            >= 2000 and < 5000 => 2,
            >= 5000 and < 10000 => 3,
            >= 10000 and < 20000 => 4,
            >= 20000 and < 50000 => 5,
            >= 50000 and < 100000 => 6,
            >= 100000 and < 500000 => 7,
            >= 500000 and < 1000000 => 8,
            >= 1000000      => 9,
        };
        return result;
    }
}
