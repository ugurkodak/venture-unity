using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Form : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();       
    }

    void Start()
    {
        animator.SetTrigger("open");
    }

    public void Apply()
    {
        animator.SetTrigger("apply");
    }

    public void Discard()
    {
        animator.SetTrigger("discard");
    }
}
