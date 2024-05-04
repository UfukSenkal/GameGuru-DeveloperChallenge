using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameGuru.SecondCase.Platform
{
    [CustomEditor(typeof(PlatformManager))]
    public class EPlatformManager : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlatformManager platformManager = (PlatformManager)target;

            if (GUILayout.Button("Reposition Predefined Platforms"))
            {
                platformManager.RepositionPredefinedPlatforms();
            }
            if (GUILayout.Button("Find Predefined Platforms"))
            {
                platformManager.FindPredefinedPlatforms();
            }
        }
    }
}
