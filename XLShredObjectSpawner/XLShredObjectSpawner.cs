using UnityEngine;
using XLShredLib;

using System;
using System.Text.RegularExpressions;

namespace XLShredObjectSpawner {

    public class XLShredObjectSpawner : MonoBehaviour {

        private bool showSpawnMenu = false;
        private bool showPlacementMenu = false;

        private GameObject objectBeingPlaced;
        private string[] spawnableObjectNames;
        private string customObjectName;

        private Rect spawnWindowRect = new Rect(0f, 0f, 330f, 0f);
        private Rect placementWindowRect = new Rect(0f, 0f, 360f, 0f);


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

            if (this.showSpawnMenu) {
                spawnWindowRect = GUILayout.Window(2, spawnWindowRect, renderSpawnWindow, "Choose Object to Spawn", GUILayout.Width(330f));
            }

            if (this.showPlacementMenu) {
                placementWindowRect = GUILayout.Window(3, placementWindowRect, renderPlacementWindow, "Refine Placement", GUILayout.Width(360f));
            }
        }

        void renderSpawnWindow(int windowID) {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            GUILayout.BeginHorizontal();

            GUILayout.Space(10f);

            GUILayout.BeginVertical();

            GUILayout.Space(5f);

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();

            
            int i = 0;
            foreach (String name in spawnableObjectNames) {
                if (i % 2 == 0) {
                    
                    if (GUILayout.Button(createDisplayName(name),GUILayout.Height(30f),GUILayout.Width(150f))) {
                        SpawnObject(name);
                    }
                }
                i++;
            }
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.BeginVertical();

            foreach (String name in spawnableObjectNames) {
                if (i % 2 != 0) {
                    if (GUILayout.Button(createDisplayName(name), GUILayout.Height(30f),GUILayout.Width(150f))) {
                        SpawnObject(name);
                    }
                }
                i++;
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10f);

            GUILayout.BeginHorizontal();

            customObjectName = GUILayout.TextField(customObjectName, GUILayout.Height(30f));
            GUILayout.Space(10f);
            if (GUILayout.Button("Spawn", GUILayout.Height(30f), GUILayout.Width(75f))) {
                SpawnObject(customObjectName);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10f);

            if (GUILayout.Button("Close", GUILayout.Height(30f))) {
                showSpawnMenu = false;
            }

            GUILayout.Space(5f);

            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.EndHorizontal();
        }

        void renderPlacementWindow(int windowID) {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.BeginHorizontal();

            GUILayout.Space(10f);

            GUILayout.BeginVertical();

            GUILayout.Space(5f);

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(110f));
            if (GUILayout.Button("FORWARD", GUILayout.Height(50f), GUILayout.Width(110f))) {
               objectBeingPlaced.transform.position = new Vector3(objectBeingPlaced.transform.position.x + 0.2f, objectBeingPlaced.transform.position.y, objectBeingPlaced.transform.position.z);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("LEFT", GUILayout.Height(50), GUILayout.Width(55f))) {
                this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z + 0.2f);
            }
            if (GUILayout.Button("RIGHT", GUILayout.Height(50),GUILayout.Width(55f))) {
                this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z - 0.2f);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("BACKWARD", GUILayout.Height(50), GUILayout.Width(110f))) {
                this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x - 0.2f, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z);
            }
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.BeginVertical(GUILayout.Width(110f));
            if (GUILayout.Button("DOWN", GUILayout.Height(50f), GUILayout.Width(110f))) {
                this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y - 0.1f, this.objectBeingPlaced.transform.position.z);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ROT-", GUILayout.Height(50), GUILayout.Width(55f))) {
                this.objectBeingPlaced.transform.Rotate(0f, 1f, 0f, Space.World);
            }
            if (GUILayout.Button("ROT+", GUILayout.Height(50),GUILayout.Width(55f))) {
                this.objectBeingPlaced.transform.Rotate(0f, -1f, 0f, Space.World);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("SIZE-", GUILayout.Height(50), GUILayout.Width(110f))) {
                this.objectBeingPlaced.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(110f));
            if (GUILayout.Button("UP", GUILayout.Height(50f), GUILayout.Width(110f))) {
                this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y + 0.1f, this.objectBeingPlaced.transform.position.z);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("TILT-", GUILayout.Height(50), GUILayout.Width(55f))) {
                this.objectBeingPlaced.transform.Rotate(-1f, 0f, 0f, Space.World);
            }
            if (GUILayout.Button("TILT+", GUILayout.Height(50), GUILayout.Width(55f))) {
                this.objectBeingPlaced.transform.Rotate(1f, 0f, 0f, Space.World);
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("SIZE+", GUILayout.Height(50), GUILayout.Width(110f))) {
                this.objectBeingPlaced.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10f);

            GUILayout.BeginVertical();
            if (GUILayout.Button("DONE", GUILayout.Height(50))) {
                this.objectBeingPlaced = null;
                this.showPlacementMenu = false;
     
                this.showSpawnMenu = true;
            }
            if (GUILayout.Button("CANCEL/DELETE", GUILayout.Height(50))) {
                UnityEngine.Object.Destroy(this.objectBeingPlaced);
                this.objectBeingPlaced = null;
                this.showPlacementMenu = false;
                this.showSpawnMenu = true;
            }
            GUILayout.EndVertical();

            GUILayout.Space(5f);

            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.EndHorizontal();
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
        
        private string createDisplayName(string name) {
            return Regex.Replace(name.Replace(" ", ""), "([a-z])([A-Z])", "$1 $2");
        }
    }
}
