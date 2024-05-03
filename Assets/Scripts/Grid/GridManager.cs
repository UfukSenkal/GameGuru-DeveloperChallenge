using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace GameGuru.FirstCase.Grid
{

    public class GridManager : MonoBehaviour
    {
        public GameObject cellPrefab; 
        public int gridSize; 
        public float cellSpacingRatio = .1f;
        public float paddingRatio = .2f;
        public Transform parent;
        void Start()
        {
            SpawnGrid();
        }

       public void SpawnGrid()
        {
            var mainCamera = Camera.main;

            float cameraDistance = Mathf.Abs(mainCamera.transform.position.z); 
            float cameraHeight = 2f * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * cameraDistance;
            float cameraWidth = cameraHeight * mainCamera.aspect; 

           
            float padding = cameraWidth * paddingRatio;

            
            float realGridWidth = cameraWidth - 2f * padding;

           
            float cellSize = realGridWidth / gridSize;

           
            float cellSpacing = cellSize * cellSpacingRatio;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                   
                    float xPos = -cameraWidth / 2f + padding + (cellSize + cellSpacing) * i + cellSize / 2f;
                    float yPos = cameraHeight / 2f - padding - (cellSize + cellSpacing) * j - cellSize / 2f;

                    
                    Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
                    GameObject newCell = Instantiate(cellPrefab, spawnPosition, Quaternion.identity);
                    newCell.transform.localScale = new Vector3(cellSize, cellSize, 1f);
                }
            }
        }

    }
}
