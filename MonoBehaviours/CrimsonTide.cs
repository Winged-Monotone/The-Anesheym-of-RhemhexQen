using System;
using UnityEngine;
using XRL;

namespace Wingytone.MonoBehaviours
{
    public class CrimsonTide : MonoBehaviour
    {
        # region static
        public static bool Enabled;
        # endregion

        public bool FadingIn;
        public CC_Levels Levels;
        public CC_Frost Frost;
        
        public WeakReference<XRLGame> Game;

        public float Interval = 7;
        public float FadeInTime;

        // update function runs every frame the ui updates 
        public void Update()
        {
            if (!Enabled)
            {
                if (Levels && Frost && Levels.enabled && Frost.enabled)
                {
                    Levels.enabled = false;
                    Frost.enabled = false;
                }
                
                return;
            }
            
            if (Game == null || !Game.TryGetTarget(out var game) || game != The.Game || !game.Running)
            {
                Levels.enabled = false;
                Frost.enabled = false;
                Enabled = false;
                return;
            }
            
            if (FadeInTime >= Interval)
            {
                return;
            }
            
            // deltaTime is the milliseconds passing between each frame, and here we're adding it up
            
            FadeInTime += Time.deltaTime;

            Levels.enabled = true;
            
            Frost.enableVignette = true;
            Levels.isRGB = true;
            
            Levels.outputMinR = Mathf.Lerp(0, 200, FadeInTime/Interval);
            Frost.darkness = Mathf.Lerp(0, 100, FadeInTime/Interval);
            Frost.scale = Mathf.Lerp(0, 16, FadeInTime/Interval);
            
            // Levels is how its explained, think photoshop or CS, lets you modify gradient color scaling, contrast scaling, etc.
            // Needs to be enabled in order to mess with it
            // isRGB() switches it to Color Leveling. Otherwise, it would be greyscale?

            // Levels.enabled = true;
            // Levels.isRGB = true;

            // Output defines color you are manipulating. R for example is Red.
            // As much as I hate it, "Lerp" means "linear Interpolation" for some fucking reason.
            // Ternary Operation or "bool ? (true) : (false)" 

            // Levels.outputMinR = Mathf.Lerp(FadingIn ? 0 : 255, FadingIn ? 255 : 0, Easing.BounceEaseInOut(FadeInTime/StrobeInterval));
        }

        public void Start()
        {
            // in this context or class behavior, gameObject is like ParentObject but for UI elements
            if (!gameObject.TryGetComponent(out Frost))
            {
                var ushader = Shader.Find("Hidden/CC_Frost");
                var ushaderb = Shader.Find("Hidden/CC_Levels");

                
                if (!ushader || !ushaderb)
                {
                    Debug.LogError("String shader was Null.");
                    return;
                }

                Levels = gameObject.AddComponent<CC_Levels>();
                Levels.shader = Shader.Find("Hidden/CC_Levels");

                Frost = gameObject.AddComponent<CC_Frost>();
                Frost.shader = Shader.Find("Hidden/CC_Frost");
            }
        }
    }
}