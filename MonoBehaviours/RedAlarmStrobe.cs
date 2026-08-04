using System;
using UnityEngine;

namespace Wingytone.MonoBehaviours
{
    public class RedAlarmStrobe : MonoBehaviour
    {
        # region static
        public static bool Enabled;
        # endregion

        public bool Strobing;
        public CC_Levels Levels;

        public float StrobeInterval = 1;
        public float StrobeTime;

        // update function runs every frame the ui updates 
        public void Update()
        {
            if (!Enabled)
            {
                return;
            }

            // deltaTime is the milliseconds passing, and here we're adding it up
            StrobeTime += Time.deltaTime;

            if (StrobeTime >= StrobeInterval)
            {
                Strobing = !Strobing;

                if (Strobing)
                {
                    Levels.enabled = true;
                    Levels.isRGB = true;
                    Levels.outputMinR = 255;
                }
                else
                {
                    Levels.enabled = false;
                }
            }
        }

        public void Start()
        {
            // gameObject is like parentobject but for UI elements
            if (!gameObject.TryGetComponent(out Levels))
            {
                var ushader = Shader.Find("Hidden/CC_Levels");
                
                if (!ushader)
                {
                    Debug.LogError("String shader was Null.");
                    return;
                } 
                
                Levels = gameObject.AddComponent<CC_Levels>();
                Levels.shader = Shader.Find("Hidden/CC_Levels");
            }
        }
    }
}