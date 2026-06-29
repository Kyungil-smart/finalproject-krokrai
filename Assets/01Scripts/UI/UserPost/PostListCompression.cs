/*
 작성자 : krokrai
 작성일 : 26-06-08
 수정일 : 26-06-29

 역할 : 나눠어진 문자 table들을 압축하여 저장
 방식 : 나눠어져 있는 문자 table들을 하나로 합친 후 Dictionary에 보관 및 class로 통해서 한번에 여러개를 출력 가능하게 압축
 */
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PostListCompression : MonoBehaviour
{
    [Header("댓글 Root")]
    [SerializeField] AutoSOGen_ContaineSO _comment;
    [Header("해쉬태그 Root")]
    [SerializeField] AutoSOGen_ContaineSO _hashTag;
    [SerializeField] AutoSOGen_ContaineSO _hashTag_Table;
    [Header("npc")]
    [SerializeField] AutoSOGen_ContaineSO _npc_Table;

    Dictionary<int, List<PostComment>> _commentComp;
    Dictionary<int, Npc_TableSO> _npc;
    Dictionary<int, string> _hashTagComp;
    Dictionary<int, string> _hashTagSTR;

    public List<PostComment> GetComments(int postID)
    {
        return _commentComp[postID];
    }

    public string GetHashTags(int postID)
    {
        return _hashTagComp[postID];
    }

    public int GetCommentCounts(int postID)
    {
        return _commentComp[postID].Count;
    }

    private void Awake()
    {
        _commentComp = new Dictionary<int, List<PostComment>>(128);
        _hashTagComp = new Dictionary<int, string>(64);
        _hashTagSTR = new Dictionary<int, string>(64);
        foreach(var t in _hashTag_Table.scriptableObjects)
        {
            if (t is Hashtag_TableSO)
            {
                var tt = (Hashtag_TableSO) t;

                _hashTagSTR.Add(tt.hashtagId, tt.hashtagText);
            }
        }

        _npc = new Dictionary<int, Npc_TableSO>();
        foreach (var t in _npc_Table.scriptableObjects)
        {
            if (t is Npc_TableSO)
            {
                var tt = (Npc_TableSO)t;

                _npc.Add(tt.npcId, tt);
            }
        }

        compression();
        compHashtag();
    }

    private void compression()
    {
        int currentPostID = 0;
        List<PostComment> postComp = new List<PostComment>(8);

        var stringManager = ServiceLocator.Get<IString_TableManager>();

        for (int i = 0; i < _comment.scriptableObjects.Length; i++ )
        {
            if (_comment.scriptableObjects[i] is Post_Notification_ListSO)
            {
                var t = (_comment.scriptableObjects[i] as Post_Notification_ListSO);
                if (t.notiTemplate != 700005) continue;
                if (currentPostID == 0) currentPostID = t.postId;
                else if (currentPostID != t.postId)
                {
                    if (!_commentComp.ContainsKey(currentPostID))
                        _commentComp.Add(currentPostID, postComp);
                    currentPostID = t.postId;
                    postComp = new List<PostComment>(8);
                }

                if (t.commentText == string.Empty) continue;

                //postComp.comment.Add(stringManager.GetStringSO(t.commentText).KR);
                if (_npc.ContainsKey(t.referencedNpcId))
                {
                    if (stringManager.GetStringSO(t.commentText) == null)
                    {
                        postComp.Add(
                    new PostComment(stringManager.GetStringSO("STR_CMT_000005").KR,
                                                    _npc[400001]));
                    }
                    else
                    {
                        postComp.Add(
                        new PostComment(stringManager.GetStringSO(t.commentText).KR,
                                                        _npc[t.referencedNpcId]));
                    }
                }
                else
                {
                    if (stringManager.GetStringSO(t.commentText) == null)
                    {
                        postComp.Add(
                    new PostComment(stringManager.GetStringSO("STR_CMT_000005").KR,
                                                    _npc[400001]));
                    }
                    else
                    {
                        postComp.Add(
                        new PostComment(stringManager.GetStringSO(t.commentText).KR,
                                                        _npc[400001]));
                    }
                }
            }
        }
        _commentComp.Add(currentPostID, postComp);
    }

    private void compHashtag()
    {
        int currentPostID = 0;

        StringBuilder sb = new StringBuilder(128);
        int id;

        var stringManager = ServiceLocator.Get<IString_TableManager>();

        Log.Message($"{stringManager == null}");

        for (int i = 0; i < _hashTag.scriptableObjects.Length; i++)
        {
            if (_hashTag.scriptableObjects[i] is Post_Hashtag_ListSO)
            {
                var t = (_hashTag.scriptableObjects[i] as Post_Hashtag_ListSO);
                if (currentPostID == 0) currentPostID = t.postId;
                else if (currentPostID != t.postId)
                {
                    _hashTagComp.Add(currentPostID, sb.ToString());
                    currentPostID = t.postId;
                    sb.Clear();
                }

                Log.Message($"{stringManager.GetString(_hashTagSTR[t.hashtagId],SystemLanguage.Korean) == null}");
                sb.Append("#")
                    .Append(stringManager.GetStringSO(_hashTagSTR[t.hashtagId]).KR)
                    .Append(" ");
            }
        }
        _hashTagComp.Add(currentPostID, sb.ToString());
    }
}

public class PostComment
{
    public string comment { get; private set; }
    public Npc_TableSO so { get; private set; }

    public PostComment(string s, Npc_TableSO so)
    {
        comment = s;
        this.so = so;
    }
}