using System.Collections;
using UnityEngine;

public class DMConversationRunner : MonoBehaviour
{
    [SerializeField] private DMChatUI chatUI;

    private Coroutine routeRoutine;

    public void OpenNpcDM()
    {
        if (routeRoutine != null)
            StopCoroutine(routeRoutine);

        chatUI.ClearChat();

        // NPC가 이미 보내둔 메시지
        chatUI.AddOpponentMessage("안녕");
        chatUI.AddOpponentMessage("지금 메시지 보여?");
        chatUI.AddOpponentMessage("이건 NPC가 이미 보내둔 DM이야.");

        // 유저가 읽은 뒤 답장 선택
        ShowFirstChoices();
    }

    private void ShowFirstChoices()
    {
        chatUI.ShowChoices(
            new string[]
            {
                "응 보여.",
                "지금은 조금 바빠.",
                "누구세요?"
            },
            choiceIndex =>
            {
                if (routeRoutine != null)
                    StopCoroutine(routeRoutine);

                routeRoutine = StartCoroutine(FirstChoiceRoute(choiceIndex));
            }
        );
    }

    private IEnumerator FirstChoiceRoute(int choiceIndex)
    {
        if (choiceIndex == 0)
        {
            chatUI.AddPlayerMessage("응 보여.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("좋아. 그럼 내 프로필은 다시 보여야 정상이고");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("이 메시지는 연속 메시지라 프로필이 없어야 해.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("그리고 선택지 이후 대화가 자연스럽게 이어지는지도 확인하면 돼.");
        }
        else if (choiceIndex == 1)
        {
            chatUI.AddPlayerMessage("지금은 조금 바빠.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("아, 알겠습니다.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("나중에 다시 연락드릴게요.");
        }
        else if (choiceIndex == 2)
        {
            chatUI.AddPlayerMessage("누구세요?");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("아... 저 기억 안 나세요?");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("전에 SNS에서 이야기했던 사람인데요.");
        }

        yield return new WaitForSeconds(1f);

        ShowSecondChoices();
    }

    private void ShowSecondChoices()
    {
        chatUI.ShowChoices(
            new string[]
            {
                "아 이제 기억나요.",
                "그래도 잘 모르겠어요."
            },
            choiceIndex =>
            {
                if (routeRoutine != null)
                    StopCoroutine(routeRoutine);

                routeRoutine = StartCoroutine(SecondChoiceRoute(choiceIndex));
            }
        );
    }

    private IEnumerator SecondChoiceRoute(int choiceIndex)
    {
        if (choiceIndex == 0)
        {
            chatUI.AddPlayerMessage("아 이제 기억나요.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("다행이다.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("그럼 다음에 또 연락할게요.");
        }
        else
        {
            chatUI.AddPlayerMessage("그래도 잘 모르겠어요.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("괜찮아요.");

            yield return new WaitForSeconds(1f);

            chatUI.AddOpponentMessage("천천히 생각해보세요.");
        }
    }
}