using System;
using UnityEngine;
using XRL;
using XRL.World;

namespace Wingytone.MonoBehaviours
{
    public class RedAlarmStrobe : MonoBehaviour
    {
        # region static
        public static bool Enabled;
        # endregion

        public bool Strobing;
        public CC_Levels Levels;

        public WeakReference<XRLGame> Game;
        
        public float StrobeInterval = 1;
        public float StrobeTime;

        // update function runs every frame the ui updates 
        public void Update()
        {
            if (!Enabled)
            {
                if (Levels && Levels.enabled)
                {
                    Levels.enabled = false;
                }
                return;
            }

            
            if (Game == null || !Game.TryGetTarget(out var game) || game != The.Game || !game.Running)
            {
                Levels.enabled = false;
                Enabled = false;
                return;
            }
    
            // deltaTime is the milliseconds passing between each frame, and here we're adding it up
            StrobeTime += Time.deltaTime;
            
            // Levels is how its explained, think photoshop or CS, lets you modify gradient color scaling, contrast scaling, etc.
            // Needs to be enabled in order to mess with it
            // isRGB() switches it to Color Leveling. Otherwise, it would be greyscale?

            Levels.enabled = true;
            Levels.isRGB = true;
            
            if (StrobeTime >= StrobeInterval)
            {
                Strobing = !Strobing;
                StrobeTime = 0;
            }
            
            // Output defines color you are manipulating. R for example is Red.
            // As much as I hate it, "Lerp" means "linear Interpolation" for some fucking reason.
            // Ternary Operation or "bool ? (true) : (false)" 
            
            Levels.outputMinR = Mathf.Lerp(Strobing ? 0 : 255, Strobing ? 255 : 0, Easing.BounceEaseInOut(StrobeTime/StrobeInterval));
        }

        public void Start()
        {
            // in this context or class behavior, gameObject is like ParentObject but for UI elements
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