using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelCheckpoint : MonoBehaviour
{
    public BoxCollider trigger;
    public Transform NextLevel;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("NextLevel");
            Player.Instance.SetCheckoutPoint(NextLevel);
            Player.Instance.Teleport();
            trigger.enabled = false;
        };
    }
}
