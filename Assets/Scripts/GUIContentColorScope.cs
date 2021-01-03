using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public sealed class GUIContentColorScope : GUI.Scope
    {
        private Color beforeColor;

        public GUIContentColorScope (Color color)
        {
            beforeColor = GUI.contentColor;
            GUI.contentColor = color;
        }

        protected override void CloseScope ()
        {
            GUI.contentColor = beforeColor;
        }
    }
}