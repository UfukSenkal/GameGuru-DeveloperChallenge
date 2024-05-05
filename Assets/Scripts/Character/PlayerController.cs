using GameGuru.FirstCase.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace GameGuru.SecondCase.Character
{
    public class PlayerController : MonoBehaviour, IMovable, IAnimatorOwner
    {

        public static bool TryGetPlayer(Collider c, out PlayerController playerController)
        {
            playerController = c.GetComponent<PlayerController>();
            return playerController != null;
        }


        [SerializeField] public Animator animator;
        [SerializeField] public CharacterController characterController;
        [SerializeField] public float moveSpeed;
        [SerializeField] public float sideSpeed;
        [SerializeField] public float gravity;


        private float _centerXPos;
        private Vector3 _startPosition;
        private bool _canMove;

        public Action onCharacterDead;

        public const string RUN_ANIM_NAME = "Run";
        public const string IDLE_ANIM_NAME = "Idle";
        public const string DANCE_ANIM_NAME = "dance";

        private void Awake()
        {
            _startPosition = transform.position;
            _canMove = false;
        }

        private void Update()
        {
            if (!_canMove) return;

            Gravity();
            Move();

          
            bool isFalling = transform.position.y < _startPosition.y;
            if (isFalling)
            {
                onCharacterDead?.Invoke();
                onCharacterDead = null;
                return;
            }

            SideMove();


        }
        public void StartMovement()
        {
            ResetRotation();
            PlayAnim(RUN_ANIM_NAME);
            _canMove = true;
        }
        public void Dance()
        {
            _canMove = false;
            PlayAnim(DANCE_ANIM_NAME);
        }

        public void Move()
        {
            characterController.Move(transform.forward * moveSpeed * Time.deltaTime);
        }
        private void SideMove()
        {
            var pos = Vector3.zero;
            pos.x = _centerXPos - transform.position.x;

            if (pos.magnitude <= .1f) return;

            characterController.Move(pos.normalized * sideSpeed * Time.deltaTime);
        }
        private void Gravity()
        {
            characterController.Move(Vector3.up * gravity * Time.deltaTime);
        }
        public void SetCenterOfPlatform(float xPos)
        {
            _centerXPos = xPos;
        }
        public void PlayAnim(string animName)
        {
            animator.Play(animName);
        }
        public void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
        }

        public void Revive()
        {
            _canMove = false;
            PlayAnim(IDLE_ANIM_NAME);
            characterController.enabled = false;
            transform.position = _startPosition;
            _centerXPos = _startPosition.x;
            characterController.enabled = true;
        }


    }

 
}
