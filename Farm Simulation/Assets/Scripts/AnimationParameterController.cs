using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParameterController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.ActionEvent += SetAnimationParameters;
    }

    private void OnDisable()
    {
        EventHandler.ActionEvent -= SetAnimationParameters;
    }

    private void SetAnimationParameters(float xInput, float yInput, bool isIdle, bool isWalking, bool isRunning, bool isHolding,
        bool idleRight, bool idleLeft, bool idleUp, bool idleDown,
        bool isToolRight, bool isToolLeft, bool isToolUp, bool isToolDown,
        bool isPullRight, bool isPullLeft, bool isPullUp, bool isPullDown,
        bool isWaterRight, bool isWaterLeft, bool isWaterUp, bool isWaterDown,
        bool isReapRight, bool isReapLeft, bool isReapUp, bool isReapDown)
    {
        animator.SetFloat("xInput", xInput);
        animator.SetFloat("yInput", yInput);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);
        if (idleRight) animator.SetTrigger("idleRight");
        if (idleLeft) animator.SetTrigger("idleLeft");
        if (idleUp) animator.SetTrigger("idleUp");
        if (idleDown) animator.SetTrigger("idleDown");
        if (isToolRight) animator.SetTrigger("isToolRight");
        if (isToolLeft) animator.SetTrigger("isToolLeft");
        if (isToolUp) animator.SetTrigger("isToolUp");
        if (isToolDown) animator.SetTrigger("isToolDown");
        if (isPullRight) animator.SetTrigger("isPullRight");
        if (isPullLeft) animator.SetTrigger("isPullLeft");
        if (isPullUp) animator.SetTrigger("isPullUp");
        if (isPullDown) animator.SetTrigger("isPullDown");
        if (isWaterRight) animator.SetTrigger("isWaterRight");
        if (isWaterLeft) animator.SetTrigger("isWaterLeft");
        if (isWaterUp) animator.SetTrigger("isWaterUp");
        if (isWaterDown) animator.SetTrigger("isWaterDown");
        if (isReapRight) animator.SetTrigger("isReapRight");
        if (isReapLeft) animator.SetTrigger("isReapLeft");
        if (isReapUp) animator.SetTrigger("isReapUp");
        if (isReapDown) animator.SetTrigger("isReapDown");
    }

    private void AnimationEvent()
    {
       
    }
}
