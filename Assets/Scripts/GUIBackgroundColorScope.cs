using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class GUIBackgroundColorScope : GUI.Scope
    {
        private Color beforeColor;

        public GUIBackgroundColorScope (Color color)
        {
            beforeColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }

        protected override void CloseScope ()
        {
            GUI.backgroundColor = beforeColor;
        }
    }
}