using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Dreamteck.Splines;

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

        ModUIBox uiBox;

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

        AssetBundle objectBundle = null;

        private void ShowSpawnMenu() {
            showSpawnMenu = true;
            ModMenu.Instance.ShowCursor(Main.modId);
            ModMenu.Instance.EnableMenuHide(Main.modId);
        }

        private void HideSpawnMenu() {
            showSpawnMenu = false;
            ModMenu.Instance.HideCursor(Main.modId);
            ModMenu.Instance.DisableMenuHide(Main.modId);
        }

        private void ShowPlacementMenu() {
            showPlacementMenu = true;
            ModMenu.Instance.ShowCursor(Main.modId);
            ModMenu.Instance.EnableMenuHide(Main.modId);
        }

        private void HidePlacementMenu() {
            showPlacementMenu = false;
            ModMenu.Instance.HideCursor(Main.modId);
            ModMenu.Instance.DisableMenuHide(Main.modId);
        }

        void LoadPlaceableObjects() {
            Console.WriteLine("Loading Placeable Objects");
            bool bundleLoaded = objectBundle != null;
            if (bundleLoaded) {
                objectBundle.Unload(true);
                objectBundle = null;
                bundleLoaded = false;
            }
            
            String bundlePath = Path.Combine(Main.modPath, "CustomObjects");
            objectBundle = AssetBundle.LoadFromFile(bundlePath);

            bundleLoaded = objectBundle != null;

            if (!bundleLoaded) Console.WriteLine($"Failed to load Asset bundle: {bundlePath}");
            else {
                GameObject[] spawnableObjects = objectBundle.LoadAllAssets<GameObject>();
                foreach (GameObject o in spawnableObjects) {
                    Console.WriteLine(o.name);
                }
            }
        }

        void OnEnable() {
            Debug.Log("OnEnable called");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Debug.Log("Cleared placed objects after loading scene: " + scene.name);
            foreach (GameObject gameObject in this.placedGameObjects) {
                Destroy(objectBeingPlaced);
            }
            placedGameObjects.Clear();
            base.Invoke("LoadPlaceableObjects", 1.0f);
        }

        public void Start() {
            uiBox = ModMenu.Instance.RegisterModMaker("salty", "Salty", -1);
            uiBox.AddCustom("open-object-spawner", () => {
                if (GUILayout.Button("Open Map Object Spawner", GUILayout.Height(30f))) {
                    ShowSpawnMenu();
                }
            }, () => Main.enabled);
            
            LoadPlaceableObjects();
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
                    HideSpawnMenu();
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
                            AddGrindTriggers(objectBeingPlaced);
                        } else {
                            RecalculateGrindTriggers(objectBeingPlaced);
                        }
                        objectBeingPlaced = null;
                        HidePlacementMenu();
                        ShowSpawnMenu();
                    }
                    if (isObjectBeingEdited) {
                        if (GUILayout.Button("DELETE", GUILayout.Height(placement_button_height))) {
                            placedGameObjects.Remove(objectBeingPlaced);
                            UnityEngine.Object.Destroy(objectBeingPlaced);
                            objectBeingPlaced = null;
                            HidePlacementMenu();
                            showSpawnMenu = true;
                        }
                        if (GUILayout.Button("CANCEL", GUILayout.Height(placement_button_height))) {
                            originalTransform.ApplyTo(objectBeingPlaced.transform);
                            objectBeingPlaced = null;
                            HidePlacementMenu();
                            showSpawnMenu = true;
                        }
                    } else {
                        if (GUILayout.Button("CANCEL/DELETE", GUILayout.Height(placement_button_height))) {
                            UnityEngine.Object.Destroy(objectBeingPlaced);
                            objectBeingPlaced = null;
                            HidePlacementMenu();
                            ShowSpawnMenu();
                        }
                    }
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndVertical();
        }

        private void RecalculateGrindTriggers(GameObject go) {
            foreach (Transform transform in go.GetComponentsInChildren<Transform>()) {
                if (transform.name.Contains("GrindSpline") && !transform.name.Contains("Colliders")) {
                    Transform grindColliders = go.transform.Find(transform.name + "Colliders").transform;

                    Vector3[] grindPoints = new Vector3[transform.childCount];
                    SplinePoint[] splinePoints = new SplinePoint[grindPoints.Length];
                    for (int i = 0; i < grindPoints.Length; i++) {
                        grindPoints[i] = transform.GetChild(i).position;
                        splinePoints[i] = new SplinePoint(grindPoints[i]);
                    }

                    SplineComputer splineComputer = grindColliders.gameObject.GetComponent<SplineComputer>();
                    Vector3[] grindNormals = new Vector3[grindPoints.Length];
                    for (int i = 0; i < grindPoints.Length - 1; i++) {
                        GameObject grindCollider = grindColliders.Find("RailCol" + i).gameObject;
                        grindCollider.transform.position = grindPoints[i];
                        grindCollider.transform.LookAt(grindPoints[i + 1]);
                        BoxCollider boxCollider = grindCollider.GetComponent<BoxCollider>();
                        float segmentLength = Vector3.Distance(grindPoints[i], grindPoints[i + 1]);
                        boxCollider.size = new Vector3(0.08f / go.transform.lossyScale.x, 0.08f / go.transform.lossyScale.y, segmentLength);
                        boxCollider.center = Vector3.forward * segmentLength / 2f;
                        grindNormals[i] = grindCollider.transform.up;
                    }

                    grindNormals[grindNormals.Length - 1] = grindNormals[grindNormals.Length - 2];
                    splineComputer.SetPoints(splinePoints);
                    splineComputer.Evaluate(0.9);
                    for (int i = 0; i < grindPoints.Length; i++) {
                        splineComputer.SetPointNormal(i, splineComputer.GetPointNormal(i, SplineComputer.Space.World) + grindNormals[i], SplineComputer.Space.World);
                    }
                }
                if (transform.name.Contains("GrindCollider")) {
                    transform.localScale = new Vector3(1f / go.transform.lossyScale.x, 1f / go.transform.lossyScale.y, 1f);
                }
            }
        }

        private void AddGrindTriggers(GameObject go) {
            foreach (Transform transform in go.GetComponentsInChildren<Transform>()) {
                if (transform.name.Contains("GrindSpline")) {
                    Transform grindColliders = new GameObject(transform.name + "Colliders").transform;
                    grindColliders.parent = go.transform;
                    grindColliders.gameObject.layer = 12;

                    if (transform.name.Contains("Metal")) transform.tag = "Metal";
                    if (transform.name.Contains("Wood")) transform.tag = "Wood";
                    if (transform.name.Contains("Concrete")) transform.tag = "Concrete";

                    Vector3[] grindPoints = new Vector3[transform.childCount];
                    SplinePoint[] splinePoints = new SplinePoint[grindPoints.Length];
                    for (int i = 0; i < grindPoints.Length; i++) {
                        grindPoints[i] = transform.GetChild(i).position;
                        splinePoints[i] = new SplinePoint(grindPoints[i]);
                    }

                    SplineComputer splineComputer = grindColliders.gameObject.AddComponent<SplineComputer>();
                    splineComputer.type = Spline.Type.Linear;
                    Vector3[] grindNormals = new Vector3[grindPoints.Length];
                    for (int i = 0; i < grindPoints.Length - 1; i++) {
                        GameObject grindCollider = new GameObject("RailCol" + i);
                        grindCollider.layer = 12;
                        grindCollider.transform.position = grindPoints[i];
                        grindCollider.transform.LookAt(grindPoints[i + 1]);
                        BoxCollider boxCollider = grindCollider.AddComponent<BoxCollider>();
                        float segmentLength = Vector3.Distance(grindPoints[i], grindPoints[i + 1]);
                        boxCollider.size = new Vector3(0.08f / go.transform.lossyScale.x, 0.08f / go.transform.lossyScale.y, segmentLength);
                        boxCollider.center = Vector3.forward * segmentLength / 2f;
                        boxCollider.isTrigger = true;
                        grindCollider.transform.parent = grindColliders;
                        grindNormals[i] = grindCollider.transform.up;

                        if (transform.name.Contains("Metal")) grindCollider.tag = "Metal";
                        if (transform.name.Contains("Wood")) grindCollider.tag = "Wood";
                        if (transform.name.Contains("Concrete")) grindCollider.tag = "Concrete";
                    }

                    grindNormals[grindNormals.Length - 1] = grindNormals[grindNormals.Length - 2];
                    splineComputer.SetPoints(splinePoints);
                    splineComputer.Evaluate(0.9);
                    for (int i = 0; i < grindPoints.Length; i++) {
                        splineComputer.SetPointNormal(i, splineComputer.GetPointNormal(i, SplineComputer.Space.World) + grindNormals[i], SplineComputer.Space.World);
                    }
                }
                if (transform.name.Contains("GrindCollider")) {
                    transform.localScale = new Vector3(1f / go.transform.lossyScale.x, 1f / go.transform.lossyScale.y, transform.localScale.z);
                }
            }
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

                HideSpawnMenu();
                ShowPlacementMenu();
                return;
            }
            ModMenu.Instance.ShowMessage("GameObject Not Found: " + name);
        }
        
        private string CreateDisplayName(string name) {
            return Regex.Replace(name.Replace(" ", ""), "([a-z])([A-Z])", "$1 $2");
        }

        public void OnDestroy() {
            uiBox.RemoveCustom("open-object-spawner");
        }
    }
}
