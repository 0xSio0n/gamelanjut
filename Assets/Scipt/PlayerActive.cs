using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActive : MonoBehaviour
{
    public Vector3 MoveUpdate;
    public float Speed;

    public bool IsIdle;
    public bool IsFall;
    public bool IsJump;
    public bool IsWalk;

    private CharacterController body;
    private Animator anim;

    private void Movement(Vector3 vector)
    {
        Fall();
        IsIdle = vector.x == 0 && vector.z == 0;
        if (IsIdle)
        {
            Idle();
        } else
        {
            Run(vector);
            if (!IsJump)
            {
                MoveUpdate = new Vector3(vector.x, MoveUpdate.y, vector.z);
                if (!IsWalk) 
                {
                    var rotate = Quaternion.LookRotation(vector);
                    transform.rotation = Quaternion.Slerp(rotate, transform.rotation, Time.deltaTime);
                }
            }
        }
        body.Move(Speed * MoveUpdate * Time.deltaTime);
    }

    private void Fall()
    {
        if (IsFall)
        {
            MoveUpdate.y += -7f * Time.deltaTime;
            body.Move(MoveUpdate * Time.deltaTime);
            if (body.isGrounded)
            {
                MoveUpdate = Vector3.zero;
                IsJump = false;
                anim.SetBool("IsJump", IsJump);
            }
        }
    }

    private IEnumerator JumpCoroutine()
    {
        if (!IsJump)
        {
            IsFall = false;
            IsJump = true;
            anim.SetBool("IsJump", IsJump);
            MoveUpdate.y++;
            body.Move(MoveUpdate * Time.deltaTime);
            yield return new WaitForSeconds(0.5f);

            IsFall = true;
        }
    }

    private void Jump()
    {
        StartCoroutine(JumpCoroutine());
    }

    private void Start()
    {
        body = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Input_GamePlay.Instance.OnMovement = Movement;
        Input_GamePlay.Instance.OnAction = Jump;
    }

    private void Idle()
    {
        anim.SetFloat("Moving", 0);
        anim.SetFloat("AxisX", 0);
        anim.SetFloat("AxisZ", 0);
    }

    private void Run(Vector3 vector)
    {
        anim.SetBool("IsWalk", IsWalk);
        anim.SetFloat("Moving", IsWalk ? 1 : 2);
        anim.SetFloat("AxisX", IsWalk ? vector.x : 0);
        anim.SetFloat("AxisZ", IsWalk ? vector.y : 0);
        Speed = IsWalk ? 3 : 6;
    }
}
