/*
 작성자 : krokrai
 수정자 : 
 작성일 : 26-05-25
 수정일 : 26-05-25

 역할 : audioclip들을 한 곳에 저장 및 관리하기 위한 SO
*/
using UnityEngine;

[CreateAssetMenu(fileName = "SoundContainer", menuName = "Scriptable Objects/SoundConatiner")]
public class SoundsSO : ScriptableObject
{
    public AudioClip[] sfx;
    public AudioClip[] bgm;
}
