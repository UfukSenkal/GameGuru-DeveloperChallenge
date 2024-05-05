using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameGuru.SecondCase.Character;

namespace GameGuru.SecondCase.Helper
{
    public class FinishController : MonoBehaviour
    {
        [SerializeField] private Collider col;
        private void OnTriggerEnter(Collider other)
        {
            if (!PlayerController.TryGetPlayer(other, out var player)) return;

            col.enabled = false;
            GameController.Instance.FinishGame(true);
        }
    }
}
