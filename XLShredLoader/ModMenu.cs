using UnityEngine;
using System;

namespace XLShredLoader {

    using Extensions.Components;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class ModMenu : MonoBehaviour {
        private bool showMenu = false;

        private ModMenu.TmpMessage tmpMessage;

        private float btnLastPressed;
        private float realtimeSinceStartup;

        private GUIStyle fontLarge;
        private GUIStyle fontMed;
        private GUIStyle fontSmall;

        private List<ModUIBox> uiBoxes;

        private bool showSpawnMenu = false;
        private bool showPlacementMenu = false;

        private GameObject objectBeingPlaced;
        private string[] spawnableObjectNames;
        private string customObjectName;

        private class TmpMessage {
            public string msg { get; set; }

            public float epoch { get; set; }
        }

        // Token: 0x040010D2 RID: 4306


        public void Start() {
            fontLarge = new GUIStyle();
            fontMed = new GUIStyle();
            fontSmall = new GUIStyle();
            fontLarge.fontSize = 28;
            fontLarge.normal.textColor = Color.red;
            fontMed.fontSize = 18;
            fontMed.normal.textColor = Color.red;
            fontSmall.fontSize = 14;
            fontSmall.normal.textColor = Color.yellow;
            uiBoxes = new List<ModUIBox>();


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

            ModUIBox uiBoxRafahel = new ModUIBox("Modifications by Rafahel Mello");
            uiBoxRafahel.AddLabel("G - Toggle Faster Grind Spin", ModUIBox.Side.left);
            uiBoxRafahel.AddLabel("L - Toggle Faster Body Spin", ModUIBox.Side.right);
            uiBoxRafahel.AddLabel("C - Toggle Dynamic Camera", ModUIBox.Side.left);
            uiBoxRafahel.AddLabel("M - Toggle Switch Flip Trick Positions", ModUIBox.Side.right);
            uiBoxRafahel.AddLabel("N - Toggle Realistic Flip Tricks Mode", ModUIBox.Side.left);
            AddUIBox(uiBoxRafahel);

            ModUIBox uiBoxKlepto = new ModUIBox("Modifications by Commander Klepto");
            uiBoxKlepto.AddLabel("Arrow UP/DOWN - Adjust Grind Pop Force", ModUIBox.Side.left);
            uiBoxKlepto.AddLabel("LB - Enable Slow Motion", ModUIBox.Side.right);
            uiBoxKlepto.AddLabel("Arrow LEFT/RIGHT - Adjust Manual Pop Force", ModUIBox.Side.left);
            AddUIBox(uiBoxKlepto);

            ModUIBox uiBoxFigzyy = new ModUIBox("Modifications by *Figzyy");
            uiBoxFigzyy.AddLabel("Page UP/DOWN - Adjust Push Speed", ModUIBox.Side.left);
            uiBoxFigzyy.AddLabel("+/- - Adjust Pop Force", ModUIBox.Side.right);
            AddUIBox(uiBoxFigzyy);

            ModUIBox uiBoxKubas = new ModUIBox("Modifications by kubas121");
            uiBoxKubas.AddLabel("S - Enable Automatic Slow Motion", ModUIBox.Side.left);
            AddUIBox(uiBoxKubas);

            ModUIBox uiBoxSalty = new ModUIBox("Modifications by Salty");
            uiBoxSalty.AddCustom(10f, 580f, 30f, (float xPos, float yPos) => {
                if (GUI.Button(new Rect(0f, yPos + 5f, 580f, 30f), "Open Map Object Spawner")) {
                    this.showMenu = false;
                    this.showSpawnMenu = true;
                }
            });
            AddUIBox(uiBoxSalty);
        }

        public void Update() {
            this.realtimeSinceStartup = Time.realtimeSinceStartup;
            this.processKeyPresses();
            if (Math.Round((double)Time.timeScale, 1) != (double)Main.settings.timeScaleTarget) {
                Time.timeScale += (Main.settings.timeScaleTarget - Time.timeScale) * Time.deltaTime * 10f;
                return;
            }
            Time.timeScale = Main.settings.timeScaleTarget;
        }

        private void showMessage(string msg) {
            Console.WriteLine(msg);
            realtimeSinceStartup = Time.realtimeSinceStartup;
            tmpMessage = new ModMenu.TmpMessage {
                msg = msg,
                epoch = realtimeSinceStartup
            };
        }

        public void AddUIBox(ModUIBox uiBox) {
            uiBoxes.Add(uiBox);
        }

        private void OnGUI() {
            if (this.tmpMessage != null) {
                this.realtimeSinceStartup = Time.realtimeSinceStartup;
                GUI.color = Color.white;
                GUI.Label(new Rect(20f, (float)(Screen.height - 50), 600f, 100f), this.tmpMessage.msg, this.fontLarge);
                if (this.realtimeSinceStartup - this.tmpMessage.epoch > 1f) {
                    this.tmpMessage = null;
                }
            }

            if (!this.showMenu && !this.showSpawnMenu && !this.showPlacementMenu) {
                Cursor.visible = false;
                return;
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (this.showMenu) {
                GUI.Box(new Rect(0f, 0f, 600f, 390f), "");
                GUI.Label(new Rect(5f, 2f, 200f, 40f), "Skater XL Shred Menu", this.fontLarge);
                
                float curY = 30;
                foreach (ModUIBox uiBox in uiBoxes) {
                    GUI.Box(new Rect(5f, curY, 590f, uiBox.height), uiBox.title);
                    foreach (ModUIBox.ModUILabel uiLabel in uiBox.labels) {
                        GUI.Label(new Rect(uiLabel.xPos, uiLabel.yPos + curY, 280f, 40f), uiLabel.text, this.fontSmall);
                    }
                    curY += 20f + uiBox.labelHeight;
                    foreach (ModUIBox.ModUICustom uiCustom in uiBox.customs) {
                        uiCustom.onGUI(uiCustom.xPos,curY);
                        curY += uiCustom.height;
                    }
                    curY += 30f;
                }

                GUI.Label(new Rect(425f, 360f, 280f, 40f), "discord.gg/mx2mE5h", this.fontMed);
            } else {
                if (this.showSpawnMenu) {
                    GUI.Label(new Rect(10f, 10f, 200f, 40f), "Choose item to spawn:", this.fontMed);
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
                    GUI.Label(new Rect(10f, 10f, 200f, 40f), "Refine Placement:", this.fontMed);
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
                        this.showMenu = false;
                        this.showSpawnMenu = true;
                    }
                    if (GUI.Button(new Rect(10f, 275f, 340f, 50f), "CANCEL/DELETE")) {
                        UnityEngine.Object.Destroy(this.objectBeingPlaced);
                        this.objectBeingPlaced = null;
                        this.showPlacementMenu = false;
                        this.showMenu = false;
                        this.showSpawnMenu = true;
                    }
                }
            }
        }

        // Token: 0x060016BC RID: 5820 RVA: 0x00071858 File Offset: 0x0006FA58
        private GameObject FindGameObjectByName(string name) {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
                if (gameObject.name == name) {
                    return gameObject;
                }
            }
            return null;
        }

        // Token: 0x060016BD RID: 5821 RVA: 0x000718A0 File Offset: 0x0006FAA0
        private void SpawnObject(string name) {
            GameObject gameObject = this.FindGameObjectByName(name);
            if (gameObject != null) {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.SetActive(true);
                Vector3 position = GameObject.Find("GripTape").transform.position;
                gameObject2.transform.position = new Vector3(position.x, position.y, position.z);
                this.objectBeingPlaced = gameObject2;
                this.showMenu = false;
                this.showSpawnMenu = false;
                this.showPlacementMenu = true;
                return;
            }
            this.showMessage("GameObject Not Found: " + name);
        }

        // Token: 0x060016BE RID: 5822 RVA: 0x000116B2 File Offset: 0x0000F8B2
        private string createDisplayName(string name) {
            return Regex.Replace(name.Replace(" ", ""), "([a-z])([A-Z])", "$1 $2");
        }

        private void processKeyPresses() {
            if (Input.GetKey(KeyCode.F8) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.15) {
                this.btnLastPressed = this.realtimeSinceStartup;
                if (!this.showPlacementMenu) {
                    if (this.showSpawnMenu) {
                        this.showSpawnMenu = false;
                    } else {
                        this.showMenu = !this.showMenu;
                    }
                } else {
                    this.showMessage("Finish placing your object.");
                }
            }
            if (Input.GetKey(KeyCode.Equals) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.1) {
                this.btnLastPressed = this.realtimeSinceStartup;
                if (Main.settings.customPopForce <= 7.8f) {
                    Main.settings.customPopForce += 0.2f;
                }
                this.showMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
            }
            if (Input.GetKey(KeyCode.Minus) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.1) {
                this.btnLastPressed = this.realtimeSinceStartup;
                if (Main.settings.customPopForce >= 1.5f) {
                    Main.settings.customPopForce -= 0.2f;
                }
                this.showMessage("Pop Force: " + string.Format("{0:0.0}", Main.settings.customPopForce) + " Default: 3.0");
            }
            if (Input.GetKey(KeyCode.PageUp) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.1) {
                this.btnLastPressed = this.realtimeSinceStartup;
                if (Main.settings.customPushForce <= 300f) {
                    Main.settings.customPushForce += 0.2f;
                }
                this.showMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            }
            if (Input.GetKey(KeyCode.Delete) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.1) {
                Main.settings.customPushForce -= 10f;
                
                this.showMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            }
            if (Input.GetKey(KeyCode.Insert) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.1) {
                Main.settings.customPushForce += 10f;
                
                this.showMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            }
            if (Input.GetKey(KeyCode.PageDown) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.1) {
                this.btnLastPressed = this.realtimeSinceStartup;
                if (Main.settings.customPushForce >= 1f) {
                    Main.settings.customPushForce -= 0.2f;
                    
                }
                this.showMessage("Push Force: " + string.Format("{0:0.0}", Main.settings.customPushForce) + " Default: 8.0");
            }
            if (Input.GetKey(KeyCode.S) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.autoSlowmo = !Main.settings.autoSlowmo;
                if (Main.settings.autoSlowmo) {
                    this.showMessage("Automatic Slow Motion: ON");
                } else {
                    this.showMessage("Automatic Slow Motion: OFF");
                }
            }
            if (Input.GetKey(KeyCode.C) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.ToggleCameraModActive();
                if (Main.settings.GetCameraModActive()) {
                    this.showMessage("Dynamic Camera: ON");
                } else {
                    this.showMessage("Dynamic Camera: OFF");
                }
            }
            if (Input.GetKey(KeyCode.L) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.spinVelocityEnabled = !Main.settings.spinVelocityEnabled;
                if (Main.settings.spinVelocityEnabled) {
                    this.showMessage("Faster Body Spin: ON");
                } else {
                    this.showMessage("Faster Body Spin: OFF");
                }
            }
            if (Input.GetKey(KeyCode.G) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.grindSpinVelocityEnabled = !Main.settings.grindSpinVelocityEnabled;
                if (Main.settings.grindSpinVelocityEnabled) {
                    this.showMessage("Faster Grind Spin: ON");
                } else {
                    this.showMessage("Faster Grind Spin: OFF");
                }
            }
            if (Input.GetKey(KeyCode.M) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.fixedSwitchFlipPositions = !Main.settings.fixedSwitchFlipPositions;
                if (Main.settings.fixedSwitchFlipPositions) {
                    this.showMessage("Switch Flip Trick Positions: CHANGED");
                } else {
                    this.showMessage("Switch Flip Trick Positions: DEFAULT");
                }
            }
            if (Input.GetKey(KeyCode.N) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.realisticFlipTricks = !Main.settings.realisticFlipTricks;
                if (Main.settings.realisticFlipTricks) {
                    this.showMessage("Realistic Flip Tricks: ACTIVATED");
                } else {
                    this.showMessage("Realistic Flip Tricks: DEACTIVATED");
                }
            }
            if (Input.GetKey(KeyCode.UpArrow) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.IncreaseGrindPopForce();
                this.showMessage("Grind Pop Force: " + Main.settings.customGrindPopForce + " Default: 2.0");
            }
            if (Input.GetKey(KeyCode.DownArrow) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.DecreaseGrindPopForce();
                this.showMessage("Grind Pop Force: " + Main.settings.customGrindPopForce + " Default: 2.0");
            }
            if (Input.GetKey(KeyCode.RightArrow) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.IncreaseManualPopForce();
                this.showMessage("Manual Pop Force: " + Main.settings.customManualPopForce + " Default: 2.5");
            }
            if (Input.GetKey(KeyCode.LeftArrow) && (double)(this.realtimeSinceStartup - this.btnLastPressed) > 0.2) {
                this.btnLastPressed = this.realtimeSinceStartup;
                Main.settings.DecreaseManualPopForce();
                this.showMessage("Manual Pop Force: " + Main.settings.customManualPopForce + " Default: 2.5");
            }
            if (Main.settings.autoSlowmo) {
                if (!PlayerController.Instance.boardController.AllDown) {
                    Main.settings.timeScaleTarget = 0.6f;
                } else {
                    Main.settings.timeScaleTarget = 1f;
                }
            }
            if (PlayerController.Instance.inputController.player.GetButtonSinglePressDown("LB")) {
                Main.settings.fixedSlowmo = !Main.settings.fixedSlowmo;
                if (Main.settings.fixedSlowmo) {
                    Main.settings.timeScaleTarget = 0.6f;
                    return;
                }
                Main.settings.timeScaleTarget = 1f;
            }
        }
    }
}
