using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerRun : StateMachineBehaviour
{
    private PlayerController _controller;
    private PlayerData _data;
    private FxPooler pooler;
    private Vector2 fxSpawnPosition;
    private float moveFxSpawnInterval = 0.3f;
    private float time;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = _controller ?? animator.GetComponentInParent<PlayerController>();
        _data = _data ?? animator.GetComponentInParent<PlayerData>();
        Debug.Log("Player Run State");

        pooler = GameManager.Instance.FxPooler;
        fxSpawnPosition = _controller.BoundCenter;
        fxSpawnPosition.y = _controller.BoundCenter.y;
        Vector3 newScale = new Vector3(Mathf.Sign(_controller.Input.X), 1, 1);
        pooler.GetFx("MoveFx", fxSpawnPosition,Quaternion.identity, newScale);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (_data.MoveParticle?.isPlaying == false)
        //{
        //    _data.MoveParticle?.Play();
        //}
        if (_controller.Rig2D.velocity.y < 0 && _controller.CollisionInfo.IsGrounded == false)
        {
            animator.SetTrigger(_controller.Id_FallAnimationParameter);
            return;
        }

        if (Input.GetMouseButtonDown(1) && _data.CanDash)
        {
            animator.SetTrigger(_controller.Id_DashAnimationParameter);
            return;
        }

        if (_controller.Input.X == 0)
        {
            animator.SetTrigger(_controller.Id_IdleAnimationParameter);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _controller.CollisionInfo.IsGrounded)
        {
            animator.SetTrigger(_controller.Id_JumpAnimationParameter);
            return;
        }

        _controller.HorizontalMovement.HorizontalMove();
        time -= Time.deltaTime;
        if(time <= 0f)
        {
            time = moveFxSpawnInterval;
            fxSpawnPosition = _controller.BoundCenter;
            fxSpawnPosition.y = _controller.BoundCenter.y;
            Vector3 newScale = new Vector3(Mathf.Sign(_controller.Input.X), 1, 1);
            pooler.GetFx("MoveFx", fxSpawnPosition, Quaternion.identity, newScale);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = moveFxSpawnInterval;
    }
}
