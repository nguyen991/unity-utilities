using Paps.UnityToolbarExtenderUIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

namespace NUtilities.SceneMenuToolbar.Editor
{
    [MainToolbarElement(id: "SceneMenuToolbarBoostButton", alignment: ToolbarAlign.Right, order: 1)]
    public class SceneBooster : Button
    {
        public void InitializeElement()
        {
            text = "Boost";
            tooltip = "Scene Booster";
            clicked += () =>
            {
                SceneMenuToolbar.BoostScene();
            };
        }
    }
}
