using UnityEngine;

public abstract class Letter : MonoBehaviour
{
    public abstract void Open();
    public abstract void Submit();
    public abstract void Postpone();
    public abstract void Discard();
}