using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

        private static readonly int placement_window_width = 440;
        private static readonly int placement_middle_spacing = 12;
        private static readonly int placement_button_height = 50;

        private static readonly int placement_column_width = (placement_window_width - (window_margin_sides * 2) - (placement_middle_spacing * 2) - (button_margin_sides * 4)) / 3;
        private static readonly int placement_small_button_width = (placement_column_width / 2) - button_margin_sides;
        private static readonly int placement_middle_spacing_no_margin = placement_middle_spacing - (button_margin_sides * 2);

        private bool showSpawnMenu = false;
        private bool showPlacementMenu = false;

        private GameObject objectBeingPlaced = null;
        private static readonly string[] spawnableObjectNames = new string[]
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
        private List<GameObject> placedGameObjects = new List<GameObject>();
        private string customObjectName = "";
        private Vector2 scrollPosition = new Vector2();
        private bool isObjectBeingEdited = false;
        private float placementSensitivity = 1f;
        private TransformInfo originalTransform = null;


        private Rect spawnWindowRect = new Rect(0f, 0f, spawn_window_width, 0f);
        private Rect placementWindowRect = new Rect(0f, 0f, placement_window_width, 0f);

        private GUIStyle labelStyle = null;

        private bool generatedStyle = false;

        public void Start() {
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

            if (!generatedStyle) {
                labelStyle = new GUIStyle(ModMenu.fontSmall);
                labelStyle.margin.left = 3;
            }

            if (showSpawnMenu) {
                spawnWindowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), spawnWindowRect, RenderSpawnWindow, "Choose Object to Spawn", ModMenu.Instance.windowStyle, GUILayout.Width(spawn_window_width));
            }

            if (showPlacementMenu) {
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
                                    SpawnObject(name, false);
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
                                    SpawnObject(name, false);
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
                        SpawnObject(customObjectName, false);
                    }

                }
                GUILayout.EndHorizontal();

                if (placedGameObjects.Count > 0) {

                    GUILayout.Label("Modify Spawned Item:", labelStyle);
                    float maxHeight = Mathf.Min(208, placedGameObjects.Count * (spawn_button_height + button_margin_sides * 2) + (button_margin_sides * 2));

                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(maxHeight), GUILayout.ExpandHeight(true));
                    {

                        GUILayout.BeginVertical();
                        {

                            foreach (GameObject gameObject in this.placedGameObjects) {
                                if (GUILayout.Button(gameObject.name.Replace("(Clone)", ""), GUILayout.Height(spawn_button_height))) {
                                    SpawnObject(gameObject.name, true);
                                }
                            }

                        }
                        GUILayout.EndVertical();

                    }
                    GUILayout.EndScrollView();
                }

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
                            objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x + 0.2f * placementSensitivity, objectBeingPlaced.transform.position.y, objectBeingPlaced.transform.position.z);
                        }

                        GUILayout.BeginHorizontal();
                        {

                            if (GUILayout.Button("LEFT", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x, objectBeingPlaced.transform.position.y,objectBeingPlaced.transform.position.z + 0.2f * placementSensitivity);
                            }
                            if (GUILayout.Button("RIGHT", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x, objectBeingPlaced.transform.position.y, objectBeingPlaced.transform.position.z - 0.2f * placementSensitivity);
                            }

                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("BACKWARD", GUILayout.Height(placement_button_height))) {
                            objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x - 0.2f * placementSensitivity, objectBeingPlaced.transform.position.y, objectBeingPlaced.transform.position.z);
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.Space(placement_middle_spacing_no_margin);

                    GUILayout.BeginVertical(GUILayout.Width(placement_column_width));
                    {

                        if (GUILayout.Button("UP", GUILayout.Height(placement_button_height))) {
                            objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x, objectBeingPlaced.transform.position.y + 0.1f * placementSensitivity, objectBeingPlaced.transform.position.z);
                        }

                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("SIZE-", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.localScale /= 1f + 0.05f * placementSensitivity;
                            }
                            if (GUILayout.Button("SIZE+", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.localScale *= 1f + 0.05f * placementSensitivity;
                            }

                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("DOWN", GUILayout.Height(placement_button_height))) {
                            objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x, objectBeingPlaced.transform.position.y - 0.1f * placementSensitivity, objectBeingPlaced.transform.position.z);
                        }

                    }
                    GUILayout.EndVertical();

                    GUILayout.Space(placement_middle_spacing_no_margin);

                    GUILayout.BeginVertical(GUILayout.Width(placement_column_width));
                    {

                        GUILayout.BeginHorizontal();
                        {

                            if (GUILayout.Button("ROT-", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.Rotate(0f, 1f * placementSensitivity, 0f, Space.World);
                            }
                            if (GUILayout.Button("ROT+", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.Rotate(0f, -1f * placementSensitivity, 0f, Space.World);
                            }

                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {

                            if (GUILayout.Button("TILT-", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.Rotate(-placementSensitivity, 0f, 0f, Space.World);
                            }
                            if (GUILayout.Button("TILT+", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.Rotate(placementSensitivity, 0f, 0f, Space.World);
                            }

                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {

                            if (GUILayout.Button("ROLL-", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.Rotate(0f, 0f, -placementSensitivity, Space.World);
                            }
                            if (GUILayout.Button("ROLL+", GUILayout.Height(placement_button_height), GUILayout.Width(placement_small_button_width))) {
                                objectBeingPlaced.transform.Rotate(0f, 0f, placementSensitivity, Space.World);
                            }

                        }
                        GUILayout.EndHorizontal();

                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                {
                    GUILayout.Label("Placement Sensitivity:", labelStyle);
                    placementSensitivity = GUILayout.HorizontalSlider(placementSensitivity, 2.5f, 0.075f);
                    if (GUILayout.Button("SAVE OBJECT", GUILayout.Height(placement_button_height))) {
                        if (!isObjectBeingEdited) {
                            objectBeingPlaced.name = $"{objectBeingPlaced.name} {placedGameObjects.Count + 1}";
                            placedGameObjects.Add(objectBeingPlaced);
                        }
                        objectBeingPlaced = null;
                        showPlacementMenu = false;

                        showSpawnMenu = true;
                    }
                    if (isObjectBeingEdited) {
                        if (GUILayout.Button("DELETE", GUILayout.Height(placement_button_height))) {
                            placedGameObjects.Remove(objectBeingPlaced);
                            UnityEngine.Object.Destroy(objectBeingPlaced);
                            objectBeingPlaced = null;
                            showPlacementMenu = false;
                            showSpawnMenu = true;
                        }
                        if (GUILayout.Button("CANCEL", GUILayout.Height(placement_button_height))) {
                            originalTransform.ApplyTo(objectBeingPlaced.transform);
                            objectBeingPlaced = null;
                            showPlacementMenu = false;
                            showSpawnMenu = true;
                        }
                    } else {
                        if (GUILayout.Button("CANCEL/DELETE", GUILayout.Height(placement_button_height))) {
                            UnityEngine.Object.Destroy(objectBeingPlaced);
                            objectBeingPlaced = null;
                            showPlacementMenu = false;
                            showSpawnMenu = true;
                        }
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

        private Bounds GetMaxBounds(GameObject g) {
            var b = new Bounds(g.transform.position, Vector3.zero);
            foreach (Renderer r in g.GetComponentsInChildren<Renderer>()) {
                b.Encapsulate(r.bounds);
            }
            return b;
        }
        
        private void SpawnObject(string name, bool isEdit) {
            GameObject gameObject = FindGameObjectByName(name);
            if (gameObject != null) {
                isObjectBeingEdited = isEdit;
                if (isEdit) {
                    objectBeingPlaced = gameObject;
                    originalTransform = new TransformInfo(objectBeingPlaced.transform);
                } else {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.SetActive(true);
                    Vector3 position = GameObject.Find("GripTape").transform.position;
                    gameObject2.transform.position = new Vector3(position.x + 1.25f, position.y, position.z);
                    objectBeingPlaced = gameObject2;
                }

                showSpawnMenu = false;
                showPlacementMenu = true;
                return;
            }
            ModMenu.Instance.ShowMessage("GameObject Not Found: " + name);
        }
        
        private string CreateDisplayName(string name) {
            return Regex.Replace(name.Replace(" ", ""), "([a-z])([A-Z])", "$1 $2");
        }
    }
}
