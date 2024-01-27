using UnityEngine;

public class JumpTag : BaseTag
{
    [SerializeField] private float jumpImpulse = 10f;

    private Vector3 jumpDirection = Vector3.zero;

    public override void OnEnter(Player player)
    {

        Vector3 upDirection = transform.up;
        if (Vector3Utils.IsNormalWall(transform.up))
        {
            upDirection += Vector3.up;
            upDirection.Normalize();
        }

        Debug.Log(upDirection);

        player.Jump(upDirection, jumpImpulse);
    }
}
