using GameGuru.FirstCase.Grid;
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
        [SerializeField] public RotateAround rotateAround;


        private float _centerXPos;
        private Vector3 _startPosition;
        private bool _isGameOver = false;

        public const string RUN_ANIM_NAME = "Run";
        public const string DANCE_ANIM_NAME = "dance";

        private void Awake()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            if (_isGameOver) return;

            Move();
            SideMove();
            Gravity();

            if (!_isGameOver && IsFalling())
            {
                _isGameOver = true;
                GameManager.Instance.FinishGame(false);
            }
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

        public void Revive()
        {
            characterController.enabled = false;
            transform.position = _startPosition;
            characterController.enabled = true;
            _isGameOver = false;
        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!hit.collider)
            {
                ///current platform
            }
        }
        private bool IsFalling()
        {
            RaycastHit hit;

            return Physics.Raycast(transform.position, Vector3.down * 5f, out hit);
        }

    }
    public class TriggerChecker
    {

    }

    public interface IMovable
    {
        public void Move();
    }
}
