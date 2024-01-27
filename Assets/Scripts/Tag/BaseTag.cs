
using UnityEngine;

public abstract class BaseTag : MonoBehaviour
{
    [SerializeField] private string tagName = "";
    [SerializeField] protected Transform decalProjection;
    [SerializeField] protected LayerMask groundLayer;

    public virtual void OnEnter(Player player) { }

    public virtual void OnStay(Player player) { }

    public virtual void OnExit(Player player) { }

    public string GetTagName()
    {
        return tagName;
    }
}
