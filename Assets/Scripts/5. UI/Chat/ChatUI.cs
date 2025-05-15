using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject chatMessagePrefab; // 채팅  UI 프리팹
    [SerializeField] private Transform chatRoot; // 채팅 부모 객체
    [SerializeField] private TMP_InputField chatInput; // 채팅 입력창
    [SerializeField] private string playerNickname = "사람1"; // 플레이어 닉네임

    [Header("Settings")]
    [SerializeField] private int maxMessages = 8; // 최대 표시할 채팅 수

    private bool inputActive = false;
    private List<GameObject> chatMessages = new List<GameObject>();

    // 네트워크 확장 위한 델리게이트 (네트워크 기능 추가 시 사용)
    public delegate void OnChatMessageSent(string nickname, string message);
    public static OnChatMessageSent onChatMessageSent;

    private void Start()
    {
        chatInput.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enter 키 처리
        {
            if (!inputActive)
            {
                chatInput.gameObject.SetActive(true); // 채팅 입력창 활성화
                chatInput.ActivateInputField();
                inputActive = true;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(chatInput.text)) // 채팅 입력창에 내용이 있으면 전송
                {
                    SendChatMessage(chatInput.text);
                    chatInput.text = "";
                }

                chatInput.DeactivateInputField(); // 채팅 입력창 비활성화
                chatInput.gameObject.SetActive(false);
                inputActive = false;
            }
        }
    }

    public void SendChatMessage(string message) // 채팅 전송 및 표시
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        DisplayMessage(playerNickname, message); // 채팅 생성 및 표시

        onChatMessageSent?.Invoke(playerNickname, message); // 네트워크 확장을 위한 이벤트 발생 (네트워크 기능 추가 시 사용)
    }

    public void SetNickname(string nickname) // 닉네임 설정
    {
        if (!string.IsNullOrWhiteSpace(nickname))
        {
            playerNickname = nickname;
        }
    }

    public void DisplayMessage(string nickname, string message) // 채팅 UI에 표시
    {
        string formattedMessage = $"[{nickname}] {message}";

        GameObject chatObj = Instantiate(chatMessagePrefab, chatRoot); // 새 채팅 생성
        TextMeshProUGUI textComponent = chatObj.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = formattedMessage;

        chatMessages.Add(chatObj); // 채팅 목록에 추가

        if (chatMessages.Count > maxMessages) // 최대 채팅 수 초과 시 오래된 채팅 제거
        {
            GameObject oldestMessage = chatMessages[0];
            chatMessages.RemoveAt(0);
            Destroy(oldestMessage);
        }
    }
}
