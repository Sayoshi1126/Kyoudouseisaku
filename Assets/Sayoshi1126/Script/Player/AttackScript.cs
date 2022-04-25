using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �A�j���[�V�������ǂ܂�Ă鎞�ɃA�j���[�V�����ƕR�Â���������Jumper�ɑ���
/// �U���A�j���[�V�����̃A�j���[�V�����X�e�[�g�ɃA�^�b�`����
/// </summary>
public class AttackScript : StateMachineBehaviour
{
    [SerializeField] int _objectNumber;//�U�����ɃA�N�e�B�u�ɂ���R���C�_�[�̔ԍ�(�g�����������蔻��p�̃I�u�W�F�N�g��AttackHitBoxCollider�I�u�W�F�N�g�̉��Ԗڂ̎q�I�u�W�F�N�g���Ŏ���
    Jumper _player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null)
        {
            _player = animator.gameObject.GetComponent<Jumper>();
        }
        //���݂̍U���A�j���[�V�����ɕR�Â��������蔻��p�I�u�W�F�N�g�̔ԍ���Jumper�ɑ��M
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
