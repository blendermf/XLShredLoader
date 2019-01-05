using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XLShredLib {
    public class ModUIBox {
        public String modMaker;
        public int priority;
        public List<ModUILabel> labelsLeft;
        public List<ModUILabel> labelsRight;
        public List<ModUICustom> customs;

        int labelLeftEnabledCount = 0;
        int labelRightEnabledCount = 0;
        int customEnabledCount = 0;

        public ModUIBox(String modMaker, int priority = 0) {
            labelsLeft = new List<ModUILabel>();
            labelsRight = new List<ModUILabel>();
            customs = new List<ModUICustom>();
            this.modMaker = modMaker;
            this.priority = priority;
        }

        public delegate bool IsEnabled();

        public struct ModUILabel {
            public String text;
            public IsEnabled isEnabled;
            public int priority;
            public LabelType labelType;
            public Action<bool> action;
            public bool toggleValue;

            public void Render() {
                if (isEnabled()) {
                    GUILayout.Label(text, ModMenu.fontSmall);
                }
            }
        }

        public struct ModUICustom {
            public delegate void OnGUI();
            public OnGUI onGUI;
            public IsEnabled isEnabled;
            public int priority;

            public void Render() {
                if (isEnabled()) {
                    onGUI();
                }
            }
        }

        public enum Side {
            left,
            right
        }

        public enum LabelType {
            Text,
            Toggle,
            Button
        }

        
        public void AddLabel(String text, Side side, IsEnabled isEnabled, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel {
                labelType = LabelType.Text,
                text = text,
                isEnabled = isEnabled,
                action = null,
                priority = priority,
                toggleValue = false
            };
            
            if (side == Side.left) {
                labelsLeft.Add(uiLabel);
            } else {
                labelsRight.Add(uiLabel);
            }

            UpdateSort();
        }

        public void AddLabel(LabelType type, String text, Side side, IsEnabled isEnabled, Action<bool> action = null, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel {
                labelType = type,
                text = text,
                isEnabled = isEnabled,
                action = action,
                priority = priority,
                toggleValue = false
            };

            if (side == Side.left) {
                labelsLeft.Add(uiLabel);
            } else {
                labelsRight.Add(uiLabel);
            }
            UpdateSort();
        }

        public void AddCustom(ModUICustom.OnGUI onGUI, IsEnabled isEnabled, int priority = 0) {
            customs.Add(new ModUICustom {
                onGUI = onGUI,
                isEnabled = isEnabled,
                priority = priority
            });

            UpdateSort();
        }

        public void UpdateEnabledCounts() {
            labelLeftEnabledCount = Enumerable.Count<ModUIBox.ModUILabel>(labelsLeft, (l) => l.isEnabled());
            labelRightEnabledCount = Enumerable.Count<ModUIBox.ModUILabel>(labelsRight, (l) => l.isEnabled());
            customEnabledCount = Enumerable.Count<ModUIBox.ModUICustom>(customs, (l) => l.isEnabled());
        }

        public void UpdateSort() {
            labelsLeft.Sort((lbl1, lbl2) => lbl2.priority.CompareTo(lbl1.priority));
            labelsRight.Sort((lbl1, lbl2) => lbl2.priority.CompareTo(lbl1.priority));
            customs.Sort((ctm1, ctm2) => ctm2.priority.CompareTo(ctm1.priority));
        }

        public void Render() {
            UpdateEnabledCounts();

            if (labelLeftEnabledCount + labelRightEnabledCount + customEnabledCount > 0) {
                GUILayout.BeginVertical($"Modifications by {modMaker}", ModMenu.Instance.boxStyle);

                if (labelLeftEnabledCount + labelRightEnabledCount > 0) {

                    GUILayout.BeginHorizontal();
                    {

                        GUILayout.BeginVertical(ModMenu.Instance.columnLeftStyle, GUILayout.Width(ModMenu.label_column_width));
                        {

                            foreach (ModUIBox.ModUILabel uiLabel in labelsLeft) {
                                uiLabel.Render();
                            }

                            if (labelLeftEnabledCount == 0) {
                                foreach (ModUIBox.ModUILabel uiLabel in labelsRight) {
                                    uiLabel.Render();
                                }
                            }

                        }

                        GUILayout.EndVertical();

                        GUILayout.BeginVertical(GUILayout.Width(ModMenu.label_column_width));
                        {

                            if (labelLeftEnabledCount != 0) {
                                foreach (ModUIBox.ModUILabel uiLabel in labelsRight) {
                                    uiLabel.Render();
                                }
                            }

                        }
                        GUILayout.EndVertical();

                    }
                    GUILayout.EndHorizontal();
                }

                if (customs.Any()) {

                    GUILayout.BeginHorizontal();
                    {

                        GUILayout.BeginVertical();
                        {

                            foreach (ModUIBox.ModUICustom uiCustom in customs) {
                                uiCustom.Render();
                            }

                        }
                        GUILayout.EndVertical();

                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
        }
    }
}
