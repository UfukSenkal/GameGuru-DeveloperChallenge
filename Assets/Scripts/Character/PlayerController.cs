using GameGuru.FirstCase.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGuru.SecondCase.Character
{
    public class PlayerController : MonoBehaviour, IMovable, IAnimatorOwner
    {
        [SerializeField] public Animator animator;
        [SerializeField] public Rigidbody rb;
        [SerializeField] public float moveSpeed;


        public const string RUN_ANIM_NAME = "Run";
        public const string DANCE_ANIM_NAME = "dance";


        private void Update()
        {
            Move();
        }

        public void Move()
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        public void PlayAnim(string animName)
        {
            animator.Play(animName);
        }
    }


    public interface IMovable
    {
        public void Move();
    }
}
