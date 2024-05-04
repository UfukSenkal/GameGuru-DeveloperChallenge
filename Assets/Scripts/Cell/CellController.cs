using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGuru.FirstCase.Grid
{
    public class CellController : MonoBehaviour, IAnimatorOwner
    {
        [SerializeField] private Animator animator;


        public const string MARKED_ANIM_NAME = "marked";
        public const string UNMARKED_ANIM_NAME = "unmarked";

        public void PlayAnim(string animName)
        {
            animator.Play(animName);
        }
    }

    public interface IAnimatorOwner
    {
        public void PlayAnim(string animName);
    }
}
