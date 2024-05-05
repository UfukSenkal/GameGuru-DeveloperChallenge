using GameGuru.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameGuru.FirstCase.Grid
{

    public class GridController : MonoBehaviour
    {
        [SerializeField] private CellModule cellModule;
        [SerializeField] private InputControl inputControl;

        void Start()
        {
            cellModule.Initiliaze();
        }

        public void SpawnGrid()
        {
            cellModule.Initiliaze();
        }

        private void Update()
        {
            if (!inputControl.IsDown) return;

            var coord = cellModule.ToCoordinate(inputControl.WorldPosition);

            if (!cellModule.ContainsCoordinate(coord)) return;

            Debug.Log(coord.x + " - " + coord.y);

            if (cellModule[coord].isMarked) return;

            cellModule[coord].isMarked = true;
            bool isMatch = cellModule.CheckMatches(coord);

            if (!isMatch)
                cellModule[coord].cellInstance.PlayAnim(CellController.MARKED_ANIM_NAME);


        }



    }


}
