using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private BoxCollider trigger;
    private bool canSave = true;

    private void Start()
    {
        trigger = GetComponent<BoxCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canSave)
        {
            Player.Instance.SetCheckoutPoint(transform);
            
            canSave = false;
        }
    }
}
