
using FishNet;
using FishNet.Connection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleTypeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roleTypeText;
    [SerializeField] private Image image;
    
    private void Start()
    {
        NetworkConnection conn = InstanceFinder.ClientManager.Connection;
        if(conn == null)
        {
            return;
        }

        EPlayerRole role = NetworkGameSystem.Instance.GetPlayerRole(conn);
        SetRoleType(role);
    }

    public void SetRoleType(EPlayerRole role)
    {
        switch(role)
        {
            case EPlayerRole.Survival:
                roleTypeText.text = "생존자";
                image.color = MyColor.White; 
                break;
            case EPlayerRole.Imposter:
                roleTypeText.text = "마피아";
                image.color = MyColor.Red; 
                break;
        }
    }
}
