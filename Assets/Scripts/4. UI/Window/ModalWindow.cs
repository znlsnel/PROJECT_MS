using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalWindow : UIBase
{
    [SerializeField] private Image windowIconImage;
    [SerializeField] private Sprite windowIcon;
    [SerializeField] private TextMeshProUGUI windowTitleText;
    [SerializeField] private string windowTitle;
    [SerializeField] private TextMeshProUGUI windowDescriptionText;
    [SerializeField] private string windowDescription;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    
    // 버튼 표시 여부 옵션
    [SerializeField] private bool showConfirmButton = true;
    [SerializeField] private bool showCancelButton = true;
    
    // 이벤트
    public UnityEvent onConfirm;
    public UnityEvent onCancel;
    public UnityEvent onOpen;
    
    // 창이 닫힐 때 동작
    public enum CloseBehaviour { Destroy, Disable, DoNothing }
    [SerializeField] private CloseBehaviour closeBehaviour = CloseBehaviour.Disable;
    
    // 버튼 클릭 시 창 닫기 옵션
    [SerializeField] private bool closeOnConfirm = true;
    [SerializeField] private bool closeOnCancel = true;

    public void Start()
    {
        InitButtons();
        UpdateContent();
    }
    
    public void InitButtons()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() => {
                onConfirm?.Invoke();
                if (closeOnConfirm) CloseWindow();
            });
            confirmButton.gameObject.SetActive(showConfirmButton);
        }
        
        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() => {
                onCancel?.Invoke();
                if (closeOnCancel) CloseWindow();
            });
            cancelButton.gameObject.SetActive(showCancelButton);
        }
    }
    
    public void UpdateContent()
    {
        if (windowTitleText != null) windowTitleText.text = windowTitle;
        if (windowDescriptionText != null) windowDescriptionText.text = windowDescription;
    }
    
    public void SetTitle(string title)
    {
        windowTitle = title;
        if (windowTitleText != null) windowTitleText.text = title;
    }
    
    public void SetDescription(string description)
    {
        windowDescription = description;
        if (windowDescriptionText != null) windowDescriptionText.text = description;
    }
    
    public void SetIcon(Sprite icon)
    {
        if (windowIconImage != null) windowIconImage.sprite = icon;
    }
    
    public override void Show()
    {
        base.Show();
        onOpen?.Invoke();
    }
    
    private void CloseWindow()
    {
        switch (closeBehaviour)
        {
            case CloseBehaviour.Destroy:
                Destroy(gameObject);
                break;
                
            case CloseBehaviour.Disable:
                Hide();
                break;
                
            case CloseBehaviour.DoNothing:
                break;
        }
    }
    
    public override void Hide() => base.Hide();
}
