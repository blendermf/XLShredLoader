using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityModManagerNet;

namespace XLShredLib {
    

    public class ModMenu : MonoBehaviour {
        public bool showMenu = false;

        private ModMenu.TmpMessage tmpMessage;

        private float btnLastPressed;
        private float realtimeSinceStartup;


        private Dictionary<String, Func<int>> shouldShowCursorFuncs = new Dictionary<string, Func<int>>();
        private bool shouldShowCursor;

        private Dictionary<String, Func<float>> timeScaleTargets = new Dictionary<string, Func<float>>();
        private float timeScaleTarget = 1.0f;

        public static readonly GUIStyle fontLarge;
        public static readonly GUIStyle fontMed;
        public static readonly GUIStyle fontSmall;

        private Rect windowRect = new Rect(0f, 0f, 600f, 0f);

        private List<ModUIBox> uiBoxes = new List<ModUIBox>();

        private Dictionary<String, ModUIBox> modMakers = new Dictionary<String, ModUIBox>();

        private static ModMenu _instance;

        static ModMenu() {
            fontLarge = new GUIStyle();
            fontMed = new GUIStyle();
            fontSmall = new GUIStyle();
            fontLarge.fontSize = 28;
            fontLarge.normal.textColor = Color.red;
            fontMed.fontSize = 18;
            fontMed.normal.textColor = Color.red;
            fontSmall.fontSize = 14;
            fontSmall.normal.textColor = Color.yellow;
        }

        public static ModMenu Instance {
            get {
                return ModMenu._instance;
            }
        }

        private void Awake() {
            if (ModMenu._instance != null && ModMenu._instance != this) {
                Destroy(this);
                return;
            }
            ModMenu._instance = this;
        }

        private class TmpMessage {
            public string msg { get; set; }

            public float epoch { get; set; }
        }

        public void RegisterTimeScaleTarget(String modid, Func<float> func) {
            timeScaleTargets[modid] = func;
        }

        public void RegisterShowCursor(String modid, Func<int> func) {
            shouldShowCursorFuncs[modid] = func;
        }

        public ModUIBox RegisterModMaker(String identifier, String name, int priority = 0) {
            if (!modMakers.ContainsKey(identifier)) {
                ModUIBox uiBox = new ModUIBox(name, priority);
                modMakers.Add(identifier, uiBox);
                AddUIBox(uiBox);
                return uiBox;
            } else {
                return modMakers[identifier];
            }
        }

        public void Update() {
            this.realtimeSinceStartup = Time.realtimeSinceStartup;

            KeyPress(KeyCode.F8, 0.15f, () => {
                this.showMenu = !this.showMenu;
            });

            if (timeScaleTargets.Any()) {
                timeScaleTarget = Enumerable.Min<Func<float>>(timeScaleTargets.Values, (f) => f.Invoke());
            } else {
                timeScaleTarget = 1.0f;
            }
            if (Math.Round((double)Time.timeScale, 1) != timeScaleTarget) {
                Time.timeScale += (timeScaleTarget - Time.timeScale) * Time.deltaTime * 10f;
                return;
            }
            Time.timeScale = timeScaleTarget;
        }

        public void ShowMessage(string msg) {
            Console.WriteLine(msg);
            realtimeSinceStartup = Time.realtimeSinceStartup;
            tmpMessage = new ModMenu.TmpMessage {
                msg = msg,
                epoch = realtimeSinceStartup
            };
        }

        public delegate void KeyPressAction();
        public void KeyPress(KeyCode keyCode, float buttonPressBuffer, KeyPressAction keyPressAction) {
            if (Input.GetKey(keyCode) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > buttonPressBuffer) {
                this.btnLastPressed = this.realtimeSinceStartup;
                keyPressAction();
            }
        }

        public void AddUIBox(ModUIBox uiBox) {
            uiBoxes.Add(uiBox);
        }

        private void OnGUI() {
            if (this.tmpMessage != null) {
                this.realtimeSinceStartup = Time.realtimeSinceStartup;
                GUI.color = Color.white;
                GUI.Label(new Rect(20f, (float)(Screen.height - 50), 600f, 100f), this.tmpMessage.msg, fontLarge);
                if (this.realtimeSinceStartup - this.tmpMessage.epoch > 1f) {
                    this.tmpMessage = null;
                }
            }

            if (shouldShowCursorFuncs.Any()) {
                shouldShowCursor = Enumerable.Max<Func<int>>(shouldShowCursorFuncs.Values, (f) => f.Invoke()) != 0;
            } else {
                shouldShowCursor = false;
            }

            if (!this.showMenu && !(UnityModManager.UI.Instance != null && UnityModManager.UI.Instance.Opened) && !shouldShowCursor) {
                Cursor.visible = false;
                return;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (this.showMenu) {
                windowRect.height = 0;
                windowRect = GUILayout.Window(1, windowRect, renderWindow, "Skater XL Shred Menu", GUILayout.Width(600));
            }
        }

        void renderWindow(int windowID) {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.BeginVertical();

            uiBoxes.Sort((box1, box2) => box2.priority.CompareTo(box1.priority));

            foreach (ModUIBox uiBox in uiBoxes) {
                int labelLeftCount = Enumerable.Count<ModUIBox.ModUILabel>(uiBox.labelsLeft, (l) => l.isEnabled());
                int labelRightCount = Enumerable.Count<ModUIBox.ModUILabel>(uiBox.labelsRight, (l) => l.isEnabled());
                int customCount = Enumerable.Count<ModUIBox.ModUICustom>(uiBox.customs, (l) => l.isEnabled());

                uiBox.labelsLeft.Sort((lbl1, lbl2) => lbl2.priority.CompareTo(lbl1.priority));
                uiBox.labelsRight.Sort((lbl1, lbl2) => lbl2.priority.CompareTo(lbl1.priority));
                uiBox.customs.Sort((ctm1, ctm2) => ctm2.priority.CompareTo(ctm1.priority));

                if (labelLeftCount + labelRightCount + customCount > 0) {
                    GUILayout.BeginVertical(String.Format("Modifications by {0}", uiBox.modMaker), "Box");

                    if (labelLeftCount + labelRightCount > 0) {
                        GUILayout.Space(20f);

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10f);
                        GUILayout.BeginVertical(GUILayout.Width(285f));

                        foreach (ModUIBox.ModUILabel uiLabel in uiBox.labelsLeft) {
                            if (uiLabel.isEnabled()) {
                                GUILayout.Label(uiLabel.text, fontSmall);
                            }
                        }
                        if (labelLeftCount == 0) {
                            foreach (ModUIBox.ModUILabel uiLabel in uiBox.labelsRight) {
                                if (uiLabel.isEnabled()) {
                                    GUILayout.Label(uiLabel.text, fontSmall);
                                }
                            }
                        }
                        GUILayout.Space(5f);
                        GUILayout.EndVertical();
                        GUILayout.Space(10f);
                        GUILayout.BeginVertical(GUILayout.Width(285f));
                        if (labelLeftCount != 0) {
                            foreach (ModUIBox.ModUILabel uiLabel in uiBox.labelsRight) {
                                if (uiLabel.isEnabled()) {
                                    GUILayout.Label(uiLabel.text, fontSmall);
                                }
                            }
                        }
                        GUILayout.Space(5f);
                        GUILayout.EndVertical();
                        GUILayout.Space(10f);
                        GUILayout.EndHorizontal();
                    }

                    if (uiBox.customs.Any()) {
                        GUILayout.Space(20f);

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10f);
                        GUILayout.BeginVertical();
                        foreach (ModUIBox.ModUICustom uiCustom in uiBox.customs) {
                            if (uiCustom.isEnabled()) {
                                uiCustom.onGUI();
                            }
                        }
                        GUILayout.Space(10f);
                        GUILayout.EndVertical();
                        GUILayout.Space(10f);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                }
            }
            GUILayout.BeginVertical("Discord Server", "Box");
            GUILayout.Space(20f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("discord.gg/mx2mE5h", fontMed);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }
    }
}