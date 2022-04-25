using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// アニメーションが読まれてる時にアニメーションと紐づけたい情報をJumperに送る
/// 攻撃アニメーションのアニメーションステートにアタッチする
/// </summary>
public class AttackScript : StateMachineBehaviour
{
    [SerializeField] int _objectNumber;//攻撃時にアクティブにするコライダーの番号(使いたい当たり判定用のオブジェクトはAttackHitBoxColliderオブジェクトの何番目の子オブジェクトかで識別
    Jumper _player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
        {
            _player = animator.gameObject.GetComponent<Jumper>();
        }
        //現在の攻撃アニメーションに紐づけた当たり判定用オブジェクトの番号をJumperに送信
        _player.AttackAnimNum = _objectNumber;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
