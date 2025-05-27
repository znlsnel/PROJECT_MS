using UnityEngine;

public class PlayerOutlineHandler : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    
    private void Awake()
    {
        if (Managers.Player != null)
        {
            if (Managers.Player == GetComponent<AlivePlayer>())
                InvokeRepeating(nameof(UpdateProperty), 0f, 1f/30f);   
        }
        else
        {
            Managers.onChangePlayer += (p) =>{
                if (p == GetComponent<AlivePlayer>())
                    InvokeRepeating(nameof(UpdateProperty), 0f, 1f/30f);     
            }; 
        }
        
    }

    private void UpdateProperty()
    {
        if (targetMaterial != null) 
        {
            targetMaterial.SetVector("PlayerPos", transform.position);
        }
    } 
}
