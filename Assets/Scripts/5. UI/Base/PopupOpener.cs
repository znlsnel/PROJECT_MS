using UnityEngine;
using DG.Tweening;
public class PopupOpener : MonoBehaviour
{
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private GameObject popupAnimTarget;
    private PopupUI popup;

    
    public void Open()
    {
        if (popupAnimTarget != null)
        {
            popupAnimTarget.transform.DOKill();
            popupAnimTarget.transform.localScale = Vector3.one * 0.9f;
            popupAnimTarget.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack, 10f).onComplete += () =>
            {
                ShowPopup();
            }; 

        }
        else
        {
            ShowPopup();
        } 

    }

    private void ShowPopup()
    {
        if (popup == null)
            popup = Instantiate(popupPrefab).GetComponent<PopupUI>(); 

            Managers.UI.ShowPopupUI(popup); 
    }
    
}
