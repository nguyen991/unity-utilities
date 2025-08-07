using System;
using System.Collections.Generic;
using UnityEngine;

namespace NUtilities.SceneMenuToolbar.Editor
{
    public class SceneMenuToolbarObject : ScriptableObject
    {
        public List<string> sceneDirectories = new List<string>(); 
        public string sceneBoostPath = "";
    }
}