using UnityEngine;

public abstract class BaseTag : MonoBehaviour
{
    [SerializeField] private string tagName = "";
    [SerializeField] protected Transform decalProjection;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask tagLayer;
    [SerializeField] protected float distance = 0.5f;
    
    protected AudioSource tagSource;

    private void Start()
    {
        tagSource = GetComponent<AudioSource>();
        if (tagSource)
            tagSource.Play();

        decalProjection.localRotation = Quaternion.Euler(90f, Random.Range(0, 360), 0);

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, tagLayer);

        foreach (Collider c in colliders) 
        {
            BaseTag baseTag = c.GetComponent<BaseTag>();

            if (baseTag.GetTagName() != tagName)
            {
                Debug.Log($"{c.name} has been destroyed");

                Destroy(c.gameObject);
            }
        }
    }

    public virtual void OnEnter(Player player) { }

    public virtual void OnStay(Player player) { }

    public virtual void OnExit(Player player) { }

    public string GetTagName()
    {
        return tagName;
    }
}
