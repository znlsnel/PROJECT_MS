using FishNet;
using FishNet.Connection;
using TMPro;
using UnityEngine;

public class RoleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roleText;

    private void Start()
    {
        NetworkConnection conn = InstanceFinder.ClientManager.Connection;
        if(conn == null)
        {
            return;
        }

        PlayerRole role = NetworkGameSystem.Instance.GetPlayerRole(conn);
        UpdateUI(role);
    }

    public void UpdateUI(PlayerRole role)
    {
        switch(role)
        {
            case PlayerRole.Survival:
                roleText.text = "생존자";
                break;
            case PlayerRole.Imposter:
                roleText.text = "임포스터";
                break;
        }
    }
}
