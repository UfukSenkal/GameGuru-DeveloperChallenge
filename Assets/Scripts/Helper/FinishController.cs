using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameGuru.SecondCase.Helper
{
    public class FinishController : MonoBehaviour
    {
        [SerializeField] private Collider col;
        private void OnTriggerEnter(Collider other)
        {
            if (!Character.PlayerController.TryGetPlayer(other, out var player)) return;

            col.enabled = false;
            GameController.Instance.FinishGame(true);
        }
    }
}
