using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    private void OnCollisionEnter(Collision collision)
    {
        Animator.Play("Collect");
        Debug.Log("Playing Collect animation");
    }

    public void RemoveCollectible()
    {
        Destroy(gameObject);

    }
}
