using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    private void OnCollisionEnter(Collision collision)
    {
        Animator.SetBool("IsCollected", true);
    }

    public void RemoveCollectible()
    {
        Destroy(gameObject);
    }
}
