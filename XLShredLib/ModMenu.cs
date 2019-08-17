using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityModManagerNet;
public static class GameObjectExtensions {
    /// <summary>
    /// Checks if a GameObject has been destroyed.
    /// </summary>
    /// <param name="gameObject">GameObject reference to check for destructedness</param>
    /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
    public static bool IsDestroyed(this GameObject gameObject) {
        // UnityEngine overloads the == opeator for the GameObject type
        // and returns null when the object has been destroyed, but 
        // actually the object is still there but has not been cleaned up yet
        // if we test both we can determine if the object has been destroyed.
        return gameObject == null && !ReferenceEquals(gameObject, null);
    }
}
namespace XLShredLib {
    using System.IO;
    using UI;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class ModMenu : MonoBehaviour {
        public static readonly Color windowColor;
        private static readonly int window_margin_sides = 10;

        private static readonly int window_width = 600;
        private static readonly int spacing = 14;
       
        public static readonly int label_column_width = (window_width - (window_margin_sides * 2) - (spacing * 3)) / 2;

        public static readonly Color largeFontColor;
        public static readonly Color smallFontColor;
        private static readonly int largeFontSize = 28;
        private static readonly int medFontSize = 18;
        private static readonly int smallFontSize = 14;
        public GUIStyle fontLarge;
        public GUIStyle fontMed;
        public GUIStyle fontSmall;

        private static ModMenu _instance;

        private bool showMenu = false;
        private bool showOldMenu = false;
        private ModMenu.TmpMessage tmpMessage;

        private float btnLastPressed;
        private float realtimeSinceStartup;

        private HashSet<string> cursorVisibilityRegistry = new HashSet<string>();
        private HashSet<string> hideMenuRegistry = new HashSet<string>();
        private string exclusiveTimeScaleRegistry = null;

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
        public GUIStyle columnLeftStyle = GUIStyle.none;
        public GUIStyle columnStyle = GUIStyle.none;
        public GUIStyle boxStyle;
        public GUIStyle toggleStyle;

        private bool generatedStyle = false;

        public string menuModPath = "";
        AssetBundle mainMenuBundle = null;
        GameObject mainMenu = null;
        List<GameObject> mainMenuButtons = new List<GameObject>();

        float pausedTimescale = 1.0f;

        static ModMenu() {
            windowColor = new Color(0.2f, 0.2f, 0.2f);
            largeFontColor = Color.red;
            smallFontColor = Color.yellow;
        }

        public static ModMenu Instance {
            get {
                return ModMenu._instance;
            }
        }

        void OnEnable() {
            SceneManager.activeSceneChanged += OnSceneLoaded;
        }
        void OnSceneLoaded(Scene scene, Scene scene2) {
            LoadMainMenuAsset();
        }

        public void Start() {
            PromptController.Instance.menuthing.enabled = false;
        }

        private void Awake() {
            if (ModMenu._instance != null && ModMenu._instance != this) {
                Destroy(this);
                return;
            }
            ModMenu._instance = this;

            fontLarge = new GUIStyle() {
                fontSize = largeFontSize
            };
            fontLarge.normal.textColor = largeFontColor;
            fontMed = new GUIStyle() {
                fontSize = medFontSize
            };
            fontMed.normal.textColor = largeFontColor;
            fontSmall = new GUIStyle() {
                fontSize = smallFontSize,
                padding = new RectOffset(1, 0, 2, 0)
            };
            fontSmall.normal.textColor = smallFontColor;
        }

        public void LoadMainMenuAsset() {
            Console.WriteLine("Loading MainMenu Asset");
            bool bundleLoaded = mainMenuBundle != null;

            if (bundleLoaded) {
                mainMenuBundle.Unload(true);
                mainMenuBundle = null;
                bundleLoaded = false;
            }

            String bundlePath = Path.Combine(menuModPath, "mainmenu");
            Console.WriteLine(bundlePath);
            mainMenuBundle = AssetBundle.LoadFromFile(bundlePath);

            bundleLoaded = mainMenuBundle != null;

            if (!bundleLoaded) Console.WriteLine($"Failed to load Asset bundle: {bundlePath}");
            else {
                mainMenuBundle.LoadAllAssets<GameObject>();
                GameObject mainMenuPrefab = FindGameObjectByName("MainMenuCanvas");
                GameObject buttonPrefab = FindGameObjectByName("MainMenuButton");

                mainMenu = UnityEngine.Object.Instantiate<GameObject>(mainMenuPrefab);
                mainMenu.SetActive(false);
          
                Transform mainMenuPanel = mainMenu.transform.Find("MainMenuPanel");
                mainMenuButtons.Clear();
                for (int i = 0; i < 5; i++) {
                    GameObject button = UnityEngine.Object.Instantiate<GameObject>(buttonPrefab);

                    button.transform.SetParent(mainMenuPanel);
                    mainMenuButtons.Add(button);
                }
            }

        }

        private static GameObject FindGameObjectByName(string name) {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
                if (gameObject.name == name) {
                    return gameObject;
                }
            }
            return null;
        }

        private class TmpMessage {
            public string Msg { get; set; }

            public float Epoch { get; set; }
        }

        /// <summary>
        /// Register the mod maker of your mod (used for display in the menu) and returns the <c>ModUIBox</c> for your section.
        /// </summary>
        /// <param name="identifier">A string representing your mod maker id.</param>
        /// <param name="name">A string representing a display friendly name for the mod maker.</param>
        /// <param name="priority">An int that represents the priority your section will take in the menu. Default value is 0, higher numbers = higher on the list. Unless you have a good reason, don't set it</param>
        /// <returns>The <c>ModUIBox</c> for your section of the menu.</returns>
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

        /// <summary>
        /// Add a <c>ModUIBox</c> to the menu. When you register a mod maker one is already created, so you don't usually need to use this.
        /// </summary>
        /// <param name="uiBox">The <c>ModUIBox</c> to add.</param>
        public void AddUIBox(ModUIBox uiBox) {
            uiBoxes.Add(uiBox);
        }

        /// <summary>
        /// Shows a message on screen temporarily (and writes it to the console).
        /// </summary>
        /// <param name="msg">A string representing the message.</param>
        public void ShowMessage(string msg) {
            Console.WriteLine(msg);
            realtimeSinceStartup = Time.realtimeSinceStartup;
            tmpMessage = new ModMenu.TmpMessage {
                Msg = msg,
                Epoch = realtimeSinceStartup
            };
        }

        public delegate void KeyPressAction();
        /// <summary>
        /// Checks for a key press, and performs action if pressed (with a buffer to prevent excessively rapid actions). 
        /// </summary>
        /// <param name="keyCode">The KeyCode you are checking</param>
        /// <param name="buttonPressBuffer">A float representing the time buffer between actions being called.</param>
        /// <param name="keyPressAction">The function to call when the key is pressed.</param>
        public void KeyPress(KeyCode keyCode, float buttonPressBuffer, KeyPressAction keyPressAction) {
            if (Input.GetKey(keyCode) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > buttonPressBuffer) {
                this.btnLastPressed = this.realtimeSinceStartup;
                keyPressAction();
            }
        }

        /// <summary>
        /// Register your mod for exclusive control over <c>Time.timeScale</c>. 
        /// Disables <c>timeScale</c> functions from all other Mod Menu mods while this is set.
        /// You can then manually modify <c>Time.timeScale</c> from your mod.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        public void EnableExclusiveTimeScale(string modid) {
            exclusiveTimeScaleRegistry = modid;
        }

        /// <summary>
        /// Disables exclusive <c>Time.timeScale</c> control.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        public void DisableExclusiveTimeScale(string modid) {
            if (modid == exclusiveTimeScaleRegistry) exclusiveTimeScaleRegistry = null;
        }

        /// <summary>
        /// Register a target timescale for your mod. The slowest target from all mods will be used.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        /// <param name="func">A function that returns the target timescale.</param>
        public void RegisterTimeScaleTarget(string modid, Func<float> func) {
            timeScaleTargets[modid] = func;
        }

        /// <summary>
        /// Unregister your mod's timescale target.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        public void UnregisterTimeScaleTarget(string modid) {
            timeScaleTargets.Remove(modid);
        }

        /// <summary>
        /// Tells the menu your mod wants the cursor to be visible.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        public void ShowCursor(string modid) {
            cursorVisibilityRegistry.Add(modid);
        }

        /// <summary>
        /// Tells the menu your mod no longer needs the cursor to be visible (other mods still may have it showing).
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        public void HideCursor(string modid) {
            cursorVisibilityRegistry.Remove(modid);
        }

        /// <summary>
        /// Tells the menu you want it to be temporarily hidden (usually when your mod is showing it's own window/interface)
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        public void EnableMenuHide(string modid) {
            hideMenuRegistry.Add(modid);
        }

        /// <summary>
        /// Tells the menu you no longer need it to be hidden (other mods may still have it hidden).
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        public void DisableMenuHide(string modid) {
            hideMenuRegistry.Remove(modid);
        }

        #region Deprecated Methods

        /// <summary>
        /// Register a function for your mod that tells the menu whether it wants the cursor visible or not. This will eventually be removed, use <c>ShowCursor</c>.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        /// <param name="func">A function that returns 1 or 0 to indicate if the cursor should be visible or not.</param>
        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use ShowCursor instead.", false)]
        public void RegisterShowCursor(string modid, Func<int> func) {
            shouldShowCursorFuncs[modid] = func;
        }

        /// <summary>
        /// Unregister your show cursor function. This will eventually be removed, use <c>HideCursor</c>.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use HideCursor instead.", false)]
        public void UnregisterShowCursor(string modid) {
            shouldShowCursorFuncs.Remove(modid);
        }

        /// <summary>
        /// Register a function for your mod that tells the menu whether you want it to be temporarily hidden or not. This will eventually be removed, use <c>EnableMenuHide</c>.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        /// <param name="func"></param>
        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use EnableMenuHide instead.", false)]
        public void RegisterTempHideMenu(String modid, Func<int> func) {
            tempHideFuncs[modid] = func;
        }

        /// <summary>
        /// Unregister your temporary menu hide function. This will eventually be removed, use <c>DisableMenuHide</c>.
        /// </summary>
        /// <param name="modid">A string representing your mod's id.</param>
        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use DisableMenuHide instead.", false)]
        public void UnregisterTempHideMenu(String modid) {
            tempHideFuncs.Remove(modid);
        }

        #endregion

        public void Update() {
            this.realtimeSinceStartup = Time.realtimeSinceStartup;

            if (PlayerController.Instance.inputController.player.GetButtonDown("Start")) {
                if (this.showMenu) {
                    FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
                    mainMenu.SetActive(false);
                    showMenu = false;
                    Time.timeScale = pausedTimescale;
                } else if (!this.showMenu) {
                    mainMenu.SetActive(true);
                    FindObjectOfType<EventSystem>().SetSelectedGameObject(mainMenuButtons[0]);
                    showMenu = true;
                    pausedTimescale = Time.timeScale;
                }
            }


            KeyPress(KeyCode.F8, 0.15f, () => {
                this.showOldMenu = !this.showOldMenu;
            });

            if (showMenu) {
                Time.timeScale = 0;
            } else if (exclusiveTimeScaleRegistry == null) {
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

                columnLeftStyle.margin.right = spacing;

                toggleStyle = new GUIStyle(GUI.skin.toggle) {
                    fontSize = smallFontSize,
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(20, 0, 2, 0),
                    contentOffset = new Vector2(0, 0)
                };
                toggleStyle.normal.textColor = toggleStyle.active.textColor = toggleStyle.hover.textColor = largeFontColor;
                toggleStyle.onNormal.textColor = toggleStyle.onActive.textColor = toggleStyle.onHover.textColor = smallFontColor;


                toggleStyle.padding.left = 20;
                toggleStyle.imagePosition = ImagePosition.TextOnly;

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

            shouldShowCursor = cursorVisibilityRegistry.Any();

            if (shouldShowCursorFuncs.Any()) {
                shouldShowCursor = shouldShowCursor || Enumerable.Max<Func<int>>(shouldShowCursorFuncs.Values, (f) => f.Invoke()) != 0;
            }

            if (!this.showOldMenu && !(UnityModManager.UI.Instance != null && UnityModManager.UI.Instance.Opened) && !shouldShowCursor) {
                Cursor.visible = false;
                return;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            tempHideMenu = hideMenuRegistry.Any();

            if (tempHideFuncs.Any()) {
                tempHideMenu = tempHideMenu || Enumerable.Max<Func<int>>(tempHideFuncs.Values, (f) => f.Invoke()) != 0;
            }

            if (this.showOldMenu && !tempHideMenu) {
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