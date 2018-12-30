using UnityEngine;
using XLShredLib;

using System;
using System.Text.RegularExpressions;

namespace XLShredLoader {

    public class ModMenuXLLoader : MonoBehaviour {

        private bool showSpawnMenu = false;
        private bool showPlacementMenu = false;

        private GameObject objectBeingPlaced;
        private string[] spawnableObjectNames;
        private string customObjectName;


        public void Start() {  
            this.customObjectName = "";
            this.spawnableObjectNames = new string[]
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



            ModUIBox uiBoxFigzyy = ModMenu.Instance.RegisterModMaker("com.figzy","*Figzyy");
            uiBoxFigzyy.AddLabel("+/- - Adjust Pop Force", ModUIBox.Side.right, () => Main.enabled);



           ModUIBox uiBoxSalty = ModMenu.Instance.RegisterModMaker("com.salty", "Salty");
            uiBoxSalty.AddCustom(() => {
                if (GUILayout.Button("Open Map Object Spawner", GUILayout.Height(30f))) {
                    ModMenu.Instance.showMenu = false;
                    this.showSpawnMenu = true;
                }
            }, () => Main.enabled);
        }

        public void Update() {
            this.processKeyPresses();
        }

        private void OnGUI() {

            if (this.showSpawnMenu) {
                Cursor.visible = true;
                GUI.Label(new Rect(10f, 10f, 200f, 40f), "Choose item to spawn:", ModMenu.fontMed);
                float num = 0f;
                for (int i = 0; i < this.spawnableObjectNames.Length; i++) {
                    if (i % 2 == 0) {
                        if (GUI.Button(new Rect(10f, 40f + num, 150f, 30f), this.createDisplayName(this.spawnableObjectNames[i]))) {
                            this.SpawnObject(this.spawnableObjectNames[i]);
                        }
                    } else {
                        if (GUI.Button(new Rect(170f, 40f + num, 150f, 30f), this.createDisplayName(this.spawnableObjectNames[i]))) {
                            this.SpawnObject(this.spawnableObjectNames[i]);
                        }
                        num += 35f;
                    }
                }
                num += 35f;
                this.customObjectName = GUI.TextField(new Rect(10f, 40f + num, 200f, 30f), this.customObjectName);
                if (GUI.Button(new Rect(220f, 40f + num, 100f, 30f), "Spawn")) {
                    this.SpawnObject(this.customObjectName);
                }
            }

            if (this.showPlacementMenu) {
                Cursor.visible = true;
                GUI.Label(new Rect(10f, 10f, 200f, 40f), "Refine Placement:", ModMenu.fontMed);
                if (GUI.Button(new Rect(10f, 50f, 100f, 50f), "FORWARD")) {
                    this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x + 0.2f, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z);
                }
                if (GUI.Button(new Rect(10f, 100f, 50f, 50f), "LEFT")) {
                    this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z + 0.2f);
                }
                if (GUI.Button(new Rect(60f, 100f, 50f, 50f), "RIGHT")) {
                    this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z - 0.2f);
                }
                if (GUI.Button(new Rect(10f, 150f, 100f, 50f), "BACKWARD")) {
                    this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x - 0.2f, this.objectBeingPlaced.transform.position.y, this.objectBeingPlaced.transform.position.z);
                }
                if (GUI.Button(new Rect(150f, 50f, 100f, 50f), "DOWN")) {
                    this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y - 0.1f, this.objectBeingPlaced.transform.position.z);
                }
                if (GUI.Button(new Rect(250f, 50f, 100f, 50f), "UP")) {
                    this.objectBeingPlaced.transform.position = new Vector3(this.objectBeingPlaced.transform.position.x, this.objectBeingPlaced.transform.position.y + 0.1f, this.objectBeingPlaced.transform.position.z);
                }
                if (GUI.Button(new Rect(150f, 100f, 50f, 50f), "ROT-")) {
                    this.objectBeingPlaced.transform.Rotate(0f, 1f, 0f, Space.World);
                }
                if (GUI.Button(new Rect(200f, 100f, 50f, 50f), "ROT+")) {
                    this.objectBeingPlaced.transform.Rotate(0f, -1f, 0f, Space.World);
                }
                if (GUI.Button(new Rect(250f, 100f, 50f, 50f), "TILT-")) {
                    this.objectBeingPlaced.transform.Rotate(-1f, 0f, 0f, Space.World);
                }
                if (GUI.Button(new Rect(300f, 100f, 50f, 50f), "TILT+")) {
                    this.objectBeingPlaced.transform.Rotate(1f, 0f, 0f, Space.World);
                }
                if (GUI.Button(new Rect(150f, 150f, 100f, 50f), "SIZE-")) {
                    this.objectBeingPlaced.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                }
                if (GUI.Button(new Rect(250f, 150f, 100f, 50f), "SIZE+")) {
                    this.objectBeingPlaced.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                }
                if (GUI.Button(new Rect(10f, 225f, 340f, 50f), "DONE")) {
                    this.objectBeingPlaced = null;
                    this.showPlacementMenu = false;
                    ModMenu.Instance.showMenu = false;
                    this.showSpawnMenu = true;
                }
                if (GUI.Button(new Rect(10f, 275f, 340f, 50f), "CANCEL/DELETE")) {
                    UnityEngine.Object.Destroy(this.objectBeingPlaced);
                    this.objectBeingPlaced = null;
                    this.showPlacementMenu = false;
                    ModMenu.Instance.showMenu = false;
                    this.showSpawnMenu = true;
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
        
        private void SpawnObject(string name) {
            GameObject gameObject = this.FindGameObjectByName(name);
            if (gameObject != null) {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.SetActive(true);
                Vector3 position = GameObject.Find("GripTape").transform.position;
                gameObject2.transform.position = new Vector3(position.x, position.y, position.z);
                this.objectBeingPlaced = gameObject2;
                ModMenu.Instance.showMenu = false;
                this.showSpawnMenu = false;
                this.showPlacementMenu = true;
                return;
            }
            ModMenu.Instance.ShowMessage("GameObject Not Found: " + name);
        }
        
        private string createDisplayName(string name) {
            return Regex.Replace(name.Replace(" ", ""), "([a-z])([A-Z])", "$1 $2");
        }

        private void processKeyPresses() {
            ModMenu.Instance.KeyPress(KeyCode.F8, 0.15f, () => {
                if (!this.showPlacementMenu) {
                    if (this.showSpawnMenu) {
                        this.showSpawnMenu = false;
                    } else {
                        ModMenu.Instance.showMenu = !ModMenu.Instance.showMenu;
                    }
                } else {
                    ModMenu.Instance.ShowMessage("Finish placing your object.");
                }
            });

            ModMenu.Instance.KeyPress(KeyCode.Equals, 0.1f, () => {
                if (Main.settings.customPopForce <= 7.8f) {
                    Main.settings.customPopForce += 0.2f;
                }
                ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
            });

            ModMenu.Instance.KeyPress(KeyCode.Minus, 0.1f, () => {
                if (Main.settings.customPopForce >= 1.5f) {
                    Main.settings.customPopForce -= 0.2f;
                }
                ModMenu.Instance.ShowMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
            });
        }
    }
}
