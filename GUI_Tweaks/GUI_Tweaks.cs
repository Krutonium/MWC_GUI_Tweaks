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
        public override string Version => "1.2"; // Version
        public override string Description => "Various GUI Tweaks"; // Short description of your mod
        public override Game SupportedGames => Game.MySummerCar_And_MyWinterCar; //Supported Games
        
        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.PostLoad, ApplySettings); // For whatever reason GUI can't be resized during OnLoad.
            SetupFunction(Setup.ModSettings, Mod_Settings);
        }

        private SettingsCheckBox SixteenByNine;
        private SettingsCheckBox TwentyOneByNine;
        private SettingsCheckBox ShowDirtyness;
        private SettingsCheckBox ShowTemperature;
        private SettingsColorPicker TextColor;
        
        private void Mod_Settings()
        {
            SixteenByNine = Settings.AddCheckBox("SixteenByNine", "Adjust GUI for 16:9", true);
            TwentyOneByNine = Settings.AddCheckBox("TwentyOneByNine", "Adjust GUI for 21:9", false);
            if (ModLoader.CurrentGame == Game.MyWinterCar)
            {
                ShowTemperature = Settings.AddCheckBox("ShowTemperature", "Show Temperature", false);
                ShowDirtyness = Settings.AddCheckBox("ShowDirtiness", "Show Dirtyness Meter", false);
            }

            if (ModLoader.CurrentGame == Game.MyWinterCar)
            {
                TextColor = Settings.AddColorPickerRGB("TextColor", "Text Color", Color.cyan);

            } else
            {
                TextColor = Settings.AddColorPickerRGB("TextColor", "Text Color", Color.yellow);
            }
            Settings.AddButton("Apply", ApplySettings, true);
        }
        private void ApplySettings()
        {
            //Lets build a Stack of Objects
            List<GameObject> ToWorkOut = new List<GameObject>();
            if (ModLoader.CurrentGame == Game.MyWinterCar)
            {
                DirtyMeter.SetActive(ShowDirtyness.GetValue());
                TempMeter.SetActive(ShowTemperature.GetValue());
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
            //Adjust the GUI elements to be up against the sides of a 16:9 display
            if (SixteenByNine.GetValue())
            {
                HUD.transform.position = HUDStartPos + new Vector3(-3f, 0f, 0f);
                FPS.transform.position = FPSStartPos + new Vector3(5f, 0f, 0f);
            }
            if (TwentyOneByNine.GetValue())
            {
                HUD.transform.position = HUDStartPos + new Vector3(-8.5f, 0f, 0f);
                FPS.transform.position = FPSStartPos + new Vector3(10f, 0f, 0f);
            }
            if(TwentyOneByNine.GetValue() == false && SixteenByNine.GetValue() == false)
            {
                FPS.transform.position = FPSStartPos;
                HUD.transform.position = HUDStartPos;
            }
            
            foreach (var item in HUDLabels)
            {
                item.color = TextColor.GetValue();
            }

        }

        private List<TextMesh> GetHUDLabels()
        {
            List<TextMesh> labels = new List<TextMesh>();
            SearchForHUDLabels(HUD.transform, labels);
            SearchForHUDLabels(MENU.transform, labels);
            return labels;
        }

        private void SearchForHUDLabels(Transform parent, List<TextMesh> labels)
        {
            foreach (Transform child in parent)
            {
                TextMesh textMesh = child.GetComponent<TextMesh>();
                // HUDLabel is in the HUD
                // GUITextLabel is the entire Settings UI, Except
                // GUITextlabel is for the Misc Menu. WHY TOPLESS!
                if (textMesh != null && (child.name == "HUDLabel" || child.name == "GUITextLabel" || child.name == "GUITextlabel"))
                {
                    labels.Add(textMesh);
                }
                SearchForHUDLabels(child, labels);
            }
        }

        private GameObject GUI;
        private GameObject HUD;
        private GameObject FPS;
        private GameObject MENU;
        private GameObject SweatMeter;
        private GameObject JailMeter;
        private GameObject DirtyMeter;
        private GameObject TempMeter;
        private List<TextMesh> HUDLabels;

        private static Vector3 HUDStartPos;
        private static Vector3 FPSStartPos;
        
        private void Mod_OnLoad()
        {
            GUI = GameObject.Find("GUI");
            HUD = GameObject.Find("HUD");
            MENU = GameObject.Find("Systems").gameObject.transform.Find("OptionsMenu").gameObject;
            FPS = HUD.transform.Find("FPS").gameObject;
            if (ModLoader.CurrentGame == Game.MyWinterCar)
            {
                SweatMeter = GameObject.Find("Sweat");
                TempMeter = HUD.transform.Find("Temp").gameObject;
            }
            DirtyMeter = HUD.transform.Find("Dirty").gameObject;
            JailMeter = HUD.transform.Find("Jailtime").gameObject;
            HUDStartPos = GUI.transform.position;
            FPSStartPos = FPS.transform.position;
            HUDLabels = GetHUDLabels();
            //Screen.SetResolution(1920, 823, false);  //Useful for testing other aspect ratios - That one is 21:9
    
        }
    }
}