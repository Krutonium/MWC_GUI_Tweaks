using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSCLoader;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace GUI_Tweaks {
    public class GUI_Tweaks : Mod {
        public override string ID => "GUI_Tweaks"; // Your (unique) mod ID 
        public override string Name => "GUI Tweaks"; // Your mod name
        public override string Author => "Krutonium"; // Name of the Author (your name)
        public override string Version => "1.0"; // Version
        public override string Description => "Various GUI Tweaks"; // Short description of your mod
        public override Game SupportedGames => Game.MyWinterCar; //Supported Games
        
        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.ModSettings, Mod_Settings);
        }

        private SettingsCheckBox SixteenByNine;
        private SettingsCheckBox ShowDirtyness;
        private SettingsCheckBox ShowTemperature;
        private void Mod_Settings()
        {
            SixteenByNine = Settings.AddCheckBox("SixteenByNine", "Adjust GUI for 16:9", false);
            ShowDirtyness = Settings.AddCheckBox("ShowDirtiness", "Show Dirtyness Meter", false);
            ShowTemperature = Settings.AddCheckBox("ShowTemperature", "Show Temperature", false);
            Settings.AddButton("Apply", ApplySettings, true);
        }
        private void ApplySettings()
        {
            //Adjust the GUI elements to be up against the sides of a 16:9 display
            if (SixteenByNine.GetValue())
            {
                HUD.transform.position = HUDStartPos + new Vector3(-3f, 0f, 0f);
                FPS.transform.position = FPSStartPos + new Vector3(5f, 0f, 0f);
            }
            else
            {
                FPS.transform.position = FPSStartPos;
                HUD.transform.position = HUDStartPos;
            }
            
            DirtyMeter.SetActive(ShowDirtyness.GetValue());
            TempMeter.SetActive(ShowTemperature.GetValue());
            
           
            
            //Lets build a Stack of Objects
            List<GameObject> ToWorkOut = new List<GameObject>();
            if (ShowDirtyness.GetValue())
            {
                ToWorkOut.Add(DirtyMeter);
            }
            if (ShowTemperature.GetValue())
            {
                ToWorkOut.Add(TempMeter);
            }
            ToWorkOut.Add(JailMeter);

            var startPos = SweatMeter.transform.position;
            foreach (var obj in ToWorkOut)
            {
                startPos += new Vector3(0, -0.4f, 0);
                obj.transform.position = startPos;
            }
        }

        private GameObject GUI;
        private GameObject HUD;
        private GameObject FPS;
        private GameObject SweatMeter;
        private GameObject JailMeter;
        private GameObject DirtyMeter;
        private GameObject TempMeter;

        private static Vector3 HUDStartPos;
        private static Vector3 FPSStartPos;
        
        private void Mod_OnLoad()
        {
            GUI = GameObject.Find("GUI");
            HUD = GameObject.Find("HUD");
            FPS = HUD.transform.Find("FPS").gameObject;
            SweatMeter = GameObject.Find("Sweat");
            DirtyMeter = HUD.transform.Find("Dirty").gameObject;
            JailMeter = HUD.transform.Find("Jailtime").gameObject;
            TempMeter = HUD.transform.Find("Temp").gameObject;
            HUDStartPos = GUI.transform.position;
            FPSStartPos = FPS.transform.position;
            
        }
    }
}