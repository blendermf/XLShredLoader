using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;
using System.Text.RegularExpressions;

namespace XLShredObjectSpawner {

    public class XLShredObjectSpawner : MonoBehaviour {
        private static readonly int window_margin_sides = 10;
        private static readonly int button_margin_sides = 2;

        private static readonly int spawn_window_width = 330;
        private static readonly int spawn_middle_spacing = 8;
        private static readonly int spawn_button_height = 30;
        private static readonly int spawn_custom_button_width = 75;

        private static readonly int spawn_column_width = (spawn_window_width - (window_margin_sides * 2) - spawn_middle_spacing) / 2;
        private static readonly int spawn_middle_spacing_no_margin = spawn_middle_spacing - (button_margin_sides * 2);

        private static readonly int placement_window_width = 362;
        private static readonly int placement_middle_spacing = 12;
        private static readonly int placement_button_height = 50;

        private static readonly int placement_column_width = (placement_window_width - (window_margin_sides * 2) - placement_middle_spacing - (button_margin_sides * 2)) / 3;
        private static readonly int placement_small_button_width = (placement_column_width / 2) - button_margin_sides;
        private static readonly int placement_middle_spacing_no_margin = placement_middle_spacing - (button_margin_sides * 2);

        private bool showSpawnMenu = false;
        private bool showPlacementMenu = false;

        private GameObject objectBeingPlaced;
        private string[] spawnableObjectNames;
        private string customObjectName;

        private Rect spawnWindowRect = new Rect(0f, 0f, spawn_window_width, 0f);
        private Rect placementWindowRect = new Rect(0f, 0f, placement_window_width, 0f);

        public void Start() {  
            customObjectName = "";
            spawnableObjectNames = new string[]
            {
                "MiniRamp",
                "RoundRail",
                "Level",
                "Big Stairs",
                "CokeMachine",
                "Sign",
                "Plane",
                "Hubba"
            };

            ModMenu.Instance.RegisterTempHideMenu(Main.modId, () => (showSpawnMenu || showPlacementMenu) ? 1 : 0);

            ModUIBox uiBoxSalty = ModMenu.Instance.RegisterModMaker("salty", "Salty", -1);
            uiBoxSalty.AddCustom(() => {
                if (GUILayout.Button("Open Map Object Spawner", GUILayout.Height(30f))) {
                    this.showSpawnMenu = true;
                }
            }, () => Main.enabled);

            ModMenu.Instance.RegisterShowCursor(Main.modId, () => (showSpawnMenu || showPlacementMenu) ? 1 : 0);
        }

        private void OnGUI() {
            GUI.backgroundColor = ModMenu.windowColor;

            if (this.showSpawnMenu) {
                spawnWindowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), spawnWindowRect, RenderSpawnWindow, "Choose Object to Spawn", ModMenu.Instance.windowStyle, GUILayout.Width(spawn_window_width));
            }

            if (this.showPlacementMenu) {
                placementWindowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), placementWindowRect, RenderPlacementWindow, "Refine Placement", ModMenu.Instance.windowStyle, GUILayout.Width(placement_window_width));
            }
        }

        void RenderSpawnWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) spawnWindowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.BeginVertical();
            {

                GUILayout.BeginHorizontal();
                {

                    GUILayout.BeginVertical();
                    {

                        int i = 0;
                        foreach (String name in spawnableObjectNames) {
                            if (i % 2 == 0) {

                                if (GUILayout.Button(CreateDisplayName(name), GUILayout.Height(spawn_button_height), GUILayout.Width(spawn_column_width))) {
                                    SpawnObject(name);
                                }
                            }
                            i++;
                        }

                    }
                    GUILayout.EndVertical();

                    GUILayout.Space(spawn_middle_spacing_no_margin);

                    GUILayout.BeginVertical();
                    {

                        int i = 0;
                        foreach (String name in spawnableObjectNames) {
                            if (i % 2 != 0) {
                                if (GUILayout.Button(CreateDisplayName(name), GUILayout.Height(spawn_button_height), GUILayout.Width(spawn_column_width))) {
                                    SpawnObject(name);
                                }
                            }
                            i++;
                        }

                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(spawn_middle_spacing_no_margin);

                GUILayout.BeginHorizontal();
                {

                    customObjectName = GUILayout.TextField(customObjectName, GUILayout.Height(spawn_button_height));

                    GUILayout.Space(spawn_middle_spacing_no_margin);


                    if (GUILayout.Button("Spawn", GUILayout.Height(spawn_button_height), GUILayout.Width(spawn_custom_button_width))) {
                        SpawnObject(customObjectName);
                    }

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(spawn_middle_spacing_no_margin);


                if (GUILayout.Button("Close", GUILayout.Height(spawn_button_height))) {
                    showSpawnMenu = false;
                }

            }
            GUILayout.EndVertical();
        }

        void RenderPlacementWindow(int windowID) {
            if (Event.current.type == EventType.Repaint) placementWindowRect.height = 0;

            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.BeginVertical();
            {

                GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
                {

                    GUILayout.BeginVertical(GUILayout.Width(placement_column_width));
                    {
                        if (GUILayout.Button("FORWARD", GUILayout.Height(placement_button_height))) {
                            objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x + 0.2f, objectBeingPlaced.transform.position.y, objectBeingPlaced.transform.position.z);
                        }

                        GUILayout.BeginHorizontal();
                        {

                            if (GUILayout.Button("LEFT", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z + 0.2f);
                            }
                            if (GUILayout.Button("RIGHT", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z - 0.2f);
                            }

                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("BACKWARD", GUILayout.Height(placement_button_height))) {
                            this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x - 0.2f, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z);
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.Space(placement_middle_spacing_no_margin);

                    GUILayout.BeginVertical(GUILayout.Width(placement_column_width));
                    {

                        if (GUILayout.Button("DOWN", GUILayout.Height(placement_button_height))) {
                            this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y - 0.1f, this.objectBeingPlaced.transform.position.z);
                        }

                        GUILayout.BeginHorizontal();
                        {

                            if (GUILayout.Button("ROT-", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                this.objectBeingPlaced.transform.Rotate(0f, 1f, 0f, Space.World);
                            }
                            if (GUILayout.Button("ROT+", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                this.objectBeingPlaced.transform.Rotate(0f, -1f, 0f, Space.World);
                            }

                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("SIZE-", GUILayout.Height(placement_button_height))) {
                            this.objectBeingPlaced.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                        }

                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(placement_column_width));
                    {

                        if (GUILayout.Button("UP", GUILayout.Height(placement_button_height))) {
                            this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y + 0.1f, this.objectBeingPlaced.transform.position.z);
                        }

                        GUILayout.BeginHorizontal();
                        {

                            if (GUILayout.Button("TILT-", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                this.objectBeingPlaced.transform.Rotate(-1f, 0f, 0f, Space.World);
                            }
                            if (GUILayout.Button("TILT+", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                this.objectBeingPlaced.transform.Rotate(1f, 0f, 0f, Space.World);
                            }

                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("SIZE+", GUILayout.Height(placement_button_height))) {
                            this.objectBeingPlaced.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                        }

                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                {

                    if (GUILayout.Button("DONE", GUILayout.Height(placement_button_height))) {
                        this.objectBeingPlaced = null;
                        this.showPlacementMenu = false;

                        this.showSpawnMenu = true;
                    }
                    if (GUILayout.Button("CANCEL/DELETE", GUILayout.Height(placement_button_height))) {
                        UnityEngine.Object.Destroy(this.objectBeingPlaced);
                        this.objectBeingPlaced = null;
                        this.showPlacementMenu = false;
                        this.showSpawnMenu = true;
                    }

                }
                GUILayout.EndVertical();

            }
            GUILayout.EndVertical();
        }

        
        private GameObject FindGameObjectByName(string name) {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
                if (gameObject.name == name) {
                    return gameObject;
                }
            }
            return null;
        }
        
        private void SpawnObject(string name) {
            GameObject gameObject = this.FindGameObjectByName(name);
            if (gameObject != null) {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.SetActive(true);
                Vector3 position = GameObject.Find("GripTape").transform.position;
                gameObject2.transform.position = new Vector3(position.x, position.y, position.z);
                this.objectBeingPlaced = gameObject2;
                this.showSpawnMenu = false;
                this.showPlacementMenu = true;
                return;
            }
            ModMenu.Instance.ShowMessage("GameObject Not Found: " + name);
        }
        
        private string CreateDisplayName(string name) {
            return Regex.Replace(name.Replace(" ", ""), "([a-z])([A-Z])", "$1 $2");
        }
    }
}
