using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityModManagerNet;

namespace XLShredLib {
    

    public class ModMenu : MonoBehaviour {
        private static readonly int window_margin_sides = 10;

        private static readonly int window_width = 600;
        private static readonly int spacing = 14;
       
        public static readonly int label_column_width = (window_width - (window_margin_sides * 2) - (spacing * 3)) / 2;

        public static readonly GUIStyle fontLarge;
        public static readonly GUIStyle fontMed;
        public static readonly GUIStyle fontSmall;
        public static readonly Color windowColor;

        private static ModMenu _instance;

        private bool showMenu = false;
        private ModMenu.TmpMessage tmpMessage;

        private float btnLastPressed;
        private float realtimeSinceStartup;

        private Dictionary<String, Func<int>> shouldShowCursorFuncs = new Dictionary<string, Func<int>>();
        private bool shouldShowCursor;

        private Dictionary<String, Func<float>> timeScaleTargets = new Dictionary<string, Func<float>>();
        private float timeScaleTarget = 1.0f;

        private Dictionary<String, Func<int>> tempHideFuncs = new Dictionary<string, Func<int>>();
        private bool tempHideMenu = false;

        private List<ModUIBox> uiBoxes = new List<ModUIBox>();
        private Dictionary<String, ModUIBox> modMakers = new Dictionary<String, ModUIBox>();

        private bool timeScaleExclusive = false;
        private Func<bool> timeScaleExclusiveFunc = null;

        private Rect windowRect = new Rect(0f, 0f, 600f, 0f);
        public GUIStyle windowStyle;
        public GUIStyle columnLeftStyle;
        public GUIStyle boxStyle;

        private bool generatedStyle = false;

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
            windowColor = new Color(0.2f, 0.2f, 0.2f);
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
            public string Msg { get; set; }

            public float Epoch { get; set; }
        }

        public void RegisterTimeScaleExclusive(Func<bool> func) {
            timeScaleExclusiveFunc = func;
            timeScaleExclusive = true;
        }

        public void RegisterTimeScaleTarget(String modid, Func<float> func) {
            timeScaleTargets[modid] = func;
        }

        public void RegisterShowCursor(String modid, Func<int> func) {
            shouldShowCursorFuncs[modid] = func;
        }

        public void RegisterTempHideMenu(String modid, Func<int> func) {
            tempHideFuncs[modid] = func;
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

            if (timeScaleExclusive) {
                bool timeScaleExclusiveVal = timeScaleExclusiveFunc.Invoke();

                if (!timeScaleExclusiveVal) {
                    timeScaleExclusiveFunc = null;
                    timeScaleExclusive = false;
                }
            } else {
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
        }

        public void ShowMessage(string msg) {
            Console.WriteLine(msg);
            realtimeSinceStartup = Time.realtimeSinceStartup;
            tmpMessage = new ModMenu.TmpMessage {
                Msg = msg,
                Epoch = realtimeSinceStartup
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
            GUI.backgroundColor = windowColor;

            if (!generatedStyle) {
                windowStyle = new GUIStyle(GUI.skin.window) {
                    padding = new RectOffset(10, 10, 25, 10),
                    contentOffset = new Vector2(0, -23.0f)
                };

                boxStyle = new GUIStyle(GUI.skin.box) {
                    padding = new RectOffset(14, 14, 24, 9),
                    contentOffset = new Vector2(0, -20f)
                };

                columnLeftStyle = new GUIStyle {
                    contentOffset = new Vector2(0f, 0f),
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0) {
                        right = spacing
                    }
                };

                generatedStyle = true;
            }


            if (this.tmpMessage != null) {
                this.realtimeSinceStartup = Time.realtimeSinceStartup;
                GUI.color = Color.white;
                GUI.Label(new Rect(20f, (float)(Screen.height - 50), 600f, 100f), this.tmpMessage.Msg, fontLarge);
                if (this.realtimeSinceStartup - this.tmpMessage.Epoch > 1f) {
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

            if (tempHideFuncs.Any()) {
                tempHideMenu = Enumerable.Max<Func<int>>(tempHideFuncs.Values, (f) => f.Invoke()) != 0;
            } else {
                tempHideMenu = false;
            }

            if (this.showMenu && !tempHideMenu) {
                windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, RenderWindow, "Skater XL Shred Menu", windowStyle, GUILayout.Width(600));
            }
        }

        void RenderWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) windowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.BeginVertical();
            {

                uiBoxes.Sort((box1, box2) => box2.priority.CompareTo(box1.priority));

                foreach (ModUIBox uiBox in uiBoxes) {
                    uiBox.Render();
                }

                GUILayout.BeginVertical("Discord Server", boxStyle);
                {

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();

                        GUILayout.Label("discord.gg/mx2mE5h", fontMed);

                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                }
                GUILayout.EndVertical();

            }
            GUILayout.EndVertical();
        }
    }
}