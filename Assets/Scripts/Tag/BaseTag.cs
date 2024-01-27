
using UnityEngine;

public abstract class BaseTag : MonoBehaviour
{
    [SerializeField] private string tagName = "";
    [SerializeField] protected Transform decalProjection;
    [SerializeField] protected LayerMask groundLayer;

    private void Start()
    {
        decalProjection.localRotation = Quaternion.Euler(90f, Random.Range(0, 360), 0);
    }

    public virtual void OnEnter(Player player) { }

    public virtual void OnStay(Player player) { }

    public virtual void OnExit(Player player) { }

    public string GetTagName()
    {
        return tagName;
    }
}
