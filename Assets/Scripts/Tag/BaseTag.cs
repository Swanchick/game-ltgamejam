using UnityEngine;

public class BaseTag : MonoBehaviour
{
    [SerializeField] private string tagName = "";

    public void OnEnter(Player player)
    {
        Debug.Log("Player has entered in " + tagName);

        player.ChangeState(PlayerState.Run);
    }

    public void OnStay(Player player)
    {
        
    }

    public void OnExit(Player player)
    {
        Debug.Log("Player has leaved from " + tagName);
        player.ChangeState(PlayerState.Walk);
    }

    public string GetTagName()
    {
        return tagName;
    }
}
