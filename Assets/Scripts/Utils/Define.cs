#region Enums
public enum ESound
{
    Bgm,
    Effect,
    MaxCount,   
}


[GoogleSheet.Core.Type.UGS(typeof(EStatType))]
public enum EStatType 
{
    Health,
    Speed,
}
#endregion



#region Interface
public interface IInteractable
{
    void Interact();
}

public interface IDamageable
{
    void TakeDamage(float damage); 
}

#endregion