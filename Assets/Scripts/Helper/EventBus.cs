using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameGuru.Helper
{
    public class EventBus
    {
        public static UnityEvent<int> OnMatchCountChanged = new UnityEvent<int>();
        public static UnityEvent<int> OnStartGridSizeSet = new UnityEvent<int>();
        public static UnityEvent<int> OnRebuildButtonClicked = new UnityEvent<int>();

        /// <summary>
        /// First Case Match Count Changed
        /// </summary>
        /// <param name="matchCount"></param>
        public static void TriggerMatchCountChanged(int matchCount)
        {
            OnMatchCountChanged.Invoke(matchCount);
        }

        /// <summary>
        /// First Case Set Start Grid Size
        /// </summary>
        /// <param name="gridSize"></param>
        public static void TriggerStartGridSizeSet(int gridSize)
        {
            OnStartGridSizeSet.Invoke(gridSize);
        }

        /// <summary>
        /// First Case Rebuild Button Clicked
        /// </summary>
        /// <param name="gridSize"></param>
        public static void RebuildButtonClicked(int gridSize)
        {
            OnRebuildButtonClicked.Invoke(gridSize);
        }
    }
}