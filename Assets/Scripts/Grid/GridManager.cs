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

            float leftEdge = -cameraWidth / 2f + padding;
            float rightEdge = cameraWidth / 2f - padding;

            float realGridWidth = rightEdge - leftEdge;

            float cellSize = realGridWidth / gridSize;

            float cellSpacing = cellSize * cellSpacingRatio;
            cellSize -= cellSpacing;

            float gridHeight = cellSize * gridSize + cellSpacing * (gridSize - 1);

            if (gridHeight > cameraHeight / 2f)
            {
                float scaleRatio = (cameraHeight / 2f) / gridHeight;
                cellSize *= scaleRatio;
                cellSpacing *= scaleRatio;
            }

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    float xPos = leftEdge + (cellSize + cellSpacing) * i + cellSize / 2f;
                    float yPos = cameraHeight / 2f - padding - (cellSize + cellSpacing) * j - cellSize / 2f;

                    Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);
                    GameObject newCell = Instantiate(cellPrefab, spawnPosition, Quaternion.identity);
                    newCell.transform.parent = parent;
                    newCell.transform.localScale = new Vector3(cellSize, cellSize, 1f);
                }
            }
        }

    }
}
