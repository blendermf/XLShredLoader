using UnityEngine;
using XLShredLib;
using XLShredLib.UI;

using System;

namespace XLShredRealisticVert {
    public struct LineDrawer {
        private LineRenderer lineRenderer;
        private float lineSize;

        public LineDrawer(float lineSize = 0.2f) {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }

        private void init(float lineSize = 0.2f) {
            if (lineRenderer == null) {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 dir, Color color) {
            if (lineRenderer == null) {
                init(0.2f);
            }

            //Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, start + dir);
        }

        public void Destroy() {
            if (lineRenderer != null) {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }

    class XLShredRealisticVert : MonoBehaviour {
        ModUIBox uiBox;
        ModUILabel uiLabelRealisticVert;
        LineDrawer lineDrawer;
        LineDrawer lineDrawer2;
        LineDrawer lineDrawer3;
        public void Start() {
            lineDrawer = new LineDrawer(0.02f);
            lineDrawer2 = new LineDrawer(0.02f);
            lineDrawer3 = new LineDrawer(0.02f);
            uiBox = ModMenu.Instance.RegisterModMaker("ghfear", "GHFear");
            uiLabelRealisticVert = uiBox.AddLabel(LabelType.Toggle, "Realistic Vert (V)", Side.left, () => Main.enabled, Main.settings.realisticVert && Main.enabled, (b) => Main.settings.realisticVert = b, 1);
        }

        public void Update() {
            if (Main.enabled) {
                ModMenu.Instance.KeyPress(KeyCode.V, 0.2f, () => {
                    Main.settings.realisticVert = !Main.settings.realisticVert;
                    uiLabelRealisticVert.SetToggleValue(Main.settings.realisticVert);
                    if (Main.settings.realisticVert) {
                        ModMenu.Instance.ShowMessage("Realistic Vert: ON");
                    } else {
                        ModMenu.Instance.ShowMessage("Realistic Vert: OFF");
                    }
                });
            }
        }

        public void FixedUpdate() {
            Rigidbody skaterBody = PlayerController.Instance.skaterController.skaterRigidbody;
            Vector3 popDir = new Vector3(0f, 1f, 0f) * 8;
            Vector3 p_up = PlayerController.Instance.boardController.GroundNormal;
            Vector3 forwardNoY = new Vector3(-PlayerController.Instance.GetGroundNormal().x, 0, -PlayerController.Instance.GetGroundNormal().z).normalized;
            float z = Vector3.Project(skaterBody.velocity.normalized * Mathf.Max(skaterBody.velocity.magnitude - 0.5f, 0), forwardNoY).magnitude * PlayerController.Instance.skaterController.skaterRigidbody.mass;
            float y = z / Mathf.Tan(Mathf.Asin(z / 8));

            if (Vector3.Angle(PlayerController.Instance.PlayerForward(), Camera.main.transform.forward) < 90f) {
                popDir = ((-forwardNoY * z) + (y * Vector3.up)).normalized * 8;
                //popDir.x = (Mathf.Abs(popDir.x) >= Mathf.Abs(forwardNoY.x * z * 8)) ? popDir.x : -forwardNoY.x * z * 8;
                //popDir.z = (Mathf.Abs(popDir.z) >= Mathf.Abs(forwardNoY.z * z * 8)) ? popDir.x : -forwardNoY.z * z * 8;
                lineDrawer2.DrawLineInGameView(PlayerController.Instance.skaterController.physicsBoardTransform.position, popDir * 2.0f, Color.blue);
                //Console.WriteLine($"{Vector3.Project(skaterBody.velocity, skaterBody.transform.forward).magnitude} {PlayerController.Instance.GetForwardSpeed()}");
            } else {
                popDir = ((-forwardNoY * z) + (y * Vector3.up)).normalized * 8;
                //popDir.x = (Mathf.Abs(popDir.x) >= Mathf.Abs(forwardNoY.x * z * 8)) ? popDir.x : -forwardNoY.x * z * 8;
                //popDir.z = (Mathf.Abs(popDir.z) >= Mathf.Abs(forwardNoY.z * z * 8)) ? popDir.x : -forwardNoY.z * z * 8;
                lineDrawer2.DrawLineInGameView(PlayerController.Instance.skaterController.physicsBoardTransform.position, popDir * 2.0f, Color.blue);
                //Console.WriteLine($"{Vector3.Project(skaterBody.velocity, skaterBody.transform.forward).magnitude} {PlayerController.Instance.GetForwardSpeed()}");
            }

            //lineDrawer.DrawLineInGameView(PlayerController.Instance.skaterController.physicsBoardTransform.position, p_up, Color.red);
            lineDrawer.DrawLineInGameView(PlayerController.Instance.skaterController.physicsBoardTransform.position, -forwardNoY * 2.0f, Color.red);
            //lineDrawer.DrawLineInGameView(PlayerController.Instance.skaterController.physicsBoardTransform.position, skaterBody.velocity, Color.blue);
        }
    }
}
