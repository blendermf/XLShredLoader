using UnityEngine;
using XLShredLib;

using System;
using System.Text.RegularExpressions;

namespace XLShredLoader {

    using Extensions.Components;

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

            ModUIBox uiBoxRafahel = ModMenu.Instance.RegisterModMaker("com.rafahel_mello", "Rafahel Mello");
            uiBoxRafahel.AddLabel("G - Toggle Faster Grind Spin", ModUIBox.Side.left, () => Main.enabled);
            uiBoxRafahel.AddLabel("L - Toggle Faster Body Spin", ModUIBox.Side.right, () => Main.enabled);
            uiBoxRafahel.AddLabel("C - Toggle Dynamic Camera", ModUIBox.Side.left, () => Main.enabled);
            uiBoxRafahel.AddLabel("M - Toggle Switch Flip Trick Positions", ModUIBox.Side.right, () => Main.enabled);
            uiBoxRafahel.AddLabel("N - Toggle Realistic Flip Tricks Mode", ModUIBox.Side.left, () => Main.enabled);

            ModUIBox uiBoxFigzyy = ModMenu.Instance.RegisterModMaker("com.figzy","*Figzyy");
            uiBoxFigzyy.AddLabel("Page UP/DOWN - Adjust Push Speed", ModUIBox.Side.left, () => Main.enabled);
            uiBoxFigzyy.AddLabel("+/- - Adjust Pop Force", ModUIBox.Side.right, () => Main.enabled);

            ModUIBox uiBoxKubas = ModMenu.Instance.RegisterModMaker("com.kubas121", "kubas121");
            uiBoxKubas.AddLabel("S - Enable Automatic Slow Motion", ModUIBox.Side.left, () => Main.enabled);

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

            ModMenu.Instance.KeyPress(KeyCode.PageUp, 0.1f, () => {
                if (Main.settings.customPushForce <= 300f) {
                    Main.settings.customPushForce += 0.2f;
                }
                ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            });

            ModMenu.Instance.KeyPress(KeyCode.Delete, 0.1f, () => {
                Main.settings.customPushForce -= 10f;

                ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            });

            ModMenu.Instance.KeyPress(KeyCode.Insert, 0.1f, () => {
                Main.settings.customPushForce += 10f;

                ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            });

            ModMenu.Instance.KeyPress(KeyCode.PageDown, 0.1f, () => {
                if (Main.settings.customPushForce >= 1f) {
                    Main.settings.customPushForce -= 0.2f;

                }
                ModMenu.Instance.ShowMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            });

            ModMenu.Instance.KeyPress(KeyCode.S, 0.2f, () => {
                Main.settings.autoSlowmo = !Main.settings.autoSlowmo;
                if (Main.settings.autoSlowmo) {
                    ModMenu.Instance.ShowMessage("Automatic Slow Motion: ON");
                } else {
                    ModMenu.Instance.ShowMessage("Automatic Slow Motion: OFF");
                }
            });

            ModMenu.Instance.KeyPress(KeyCode.C, 0.2f, () => {
                Main.settings.ToggleCameraModActive();
                if (Main.settings.GetCameraModActive()) {
                    ModMenu.Instance.ShowMessage("Dynamic Camera: ON");
                } else {
                    ModMenu.Instance.ShowMessage("Dynamic Camera: OFF");
                }
            });

            ModMenu.Instance.KeyPress(KeyCode.L, 0.2f, () => {
                Main.settings.spinVelocityEnabled = !Main.settings.spinVelocityEnabled;
                if (Main.settings.spinVelocityEnabled) {
                    ModMenu.Instance.ShowMessage("Faster Body Spin: ON");
                } else {
                    ModMenu.Instance.ShowMessage("Faster Body Spin: OFF");
                }
            });

            ModMenu.Instance.KeyPress(KeyCode.G, 0.2f, () => {
                Main.settings.grindSpinVelocityEnabled = !Main.settings.grindSpinVelocityEnabled;
                if (Main.settings.grindSpinVelocityEnabled) {
                    ModMenu.Instance.ShowMessage("Faster Grind Spin: ON");
                } else {
                    ModMenu.Instance.ShowMessage("Faster Grind Spin: OFF");
                }
            });

            ModMenu.Instance.KeyPress(KeyCode.M, 0.2f, () => {
                Main.settings.fixedSwitchFlipPositions = !Main.settings.fixedSwitchFlipPositions;
                if (Main.settings.fixedSwitchFlipPositions) {
                    ModMenu.Instance.ShowMessage("Switch Flip Trick Positions: CHANGED");
                } else {
                    ModMenu.Instance.ShowMessage("Switch Flip Trick Positions: DEFAULT");
                }
            });

            ModMenu.Instance.KeyPress(KeyCode.N, 0.2f, () => {
                Main.settings.realisticFlipTricks = !Main.settings.realisticFlipTricks;
                if (Main.settings.realisticFlipTricks) {
                    ModMenu.Instance.ShowMessage("Realistic Flip Tricks: ACTIVATED");
                } else {
                    ModMenu.Instance.ShowMessage("Realistic Flip Tricks: DEACTIVATED");
                }
            });



            if (Main.settings.autoSlowmo) {
                if (!PlayerController.Instance.boardController.AllDown) {
                    ModMenu.Instance.timeScaleTarget = 0.6f;
                } else {
                    ModMenu.Instance.timeScaleTarget = 1f;
                }
            }

        }
    }
}
