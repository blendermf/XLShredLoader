using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XLShredLib.UI {

    public class ModUILabel {
        public LabelType labelType = LabelType.Text;
        public String text = "";
        public Side side;
        public Func<bool> isEnabled = null;
        private bool oldToggleValue = false;
        private bool toggleValue = false;
        public Action<bool> action = null;
        public int priority = 0;

        public ModUILabel(String text, Side side, Func<bool> isEnabled, int priority = 0) :
            this(LabelType.Text, text, side, isEnabled, false, null, priority) { }

        public ModUILabel(LabelType type, String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {
            this.labelType = type;
            this.text = text;
            this.side = side;
            this.isEnabled = isEnabled;
            this.toggleValue = initToggle;
            this.oldToggleValue = initToggle;
            this.action = action;
            this.priority = priority;
        }

        public void SetToggleValue(bool val) {
            oldToggleValue = toggleValue;
            toggleValue = val;
        }

        public void Render() {
            if (isEnabled != null && isEnabled()) {
                switch (labelType) {
                    case LabelType.Text:
                        GUILayout.Label(text, ModMenu.fontSmall);
                        break;
                    case LabelType.Toggle:
                        oldToggleValue = toggleValue;
                        toggleValue = GUILayout.Toggle(toggleValue, text, ModMenu.Instance.toggleStyle);
                        if (toggleValue != oldToggleValue && action != null) action(toggleValue);
                        break;
                    case LabelType.Button:
                        if (GUILayout.Button(text, ModMenu.fontSmall) && action != null) action(true);
                        break;
                }
            }
        }
    }

    public class ModUICustom {
        public Action onGUI = null;
        public Func<bool> isEnabled = null;
        public int priority = 0;

        public ModUICustom(Action onGUI, Func<bool> isEnabled, int priority = 0) {
            this.onGUI = onGUI;
            this.isEnabled = isEnabled;
            this.priority = priority;
        }

        public void Render() {
            if (isEnabled != null && isEnabled()) onGUI?.Invoke();
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

        public void AddLabel(ModUILabel uiLabel) {
            if (uiLabel.side == Side.left) {
                labelsLeft.Add(uiLabel);
            } else {
                labelsRight.Add(uiLabel);
            }
            UpdateSort();
        }

        public ModUILabel AddLabel(String text, Side side, Func<bool> isEnabled, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel(text, side, isEnabled, priority);
            AddLabel(uiLabel);
            return uiLabel;
        }

        public ModUILabel AddLabel(LabelType type, String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel(type, text, side, isEnabled, initToggle, action, priority);
            AddLabel(uiLabel);
            UpdateSort();

            return uiLabel;
        }
        public ModUILabel AddToggle(String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {
            ModUILabel uiLabel = new ModUILabel(LabelType.Toggle, text, side, isEnabled, initToggle, action, priority);
            AddLabel(uiLabel);
            UpdateSort();

            return uiLabel;
        }

        public void AddCustom(ModUICustom uiCustom) {
            customs.Add(uiCustom);
            UpdateSort();
        }

        public ModUICustom AddCustom(Action onGUI, Func<bool> isEnabled, int priority = 0) {
            ModUICustom uiCustom = new ModUICustom(onGUI, isEnabled, priority);
            AddCustom(uiCustom);
            return uiCustom;
        }

        public void UpdateEnabledCounts() {
            labelLeftEnabledCount = Enumerable.Count<ModUILabel>(labelsLeft, (l) => l.isEnabled());
            labelRightEnabledCount = Enumerable.Count<ModUILabel>(labelsRight, (l) => l.isEnabled());
            customEnabledCount = Enumerable.Count<ModUICustom>(customs, (l) => l.isEnabled());
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

                            foreach (ModUILabel uiLabel in labelsLeft) {
                                uiLabel.Render();
                            }

                            if (labelLeftEnabledCount == 0) {
                                foreach (ModUILabel uiLabel in labelsRight) {
                                    uiLabel.Render();
                                }
                            }

                        }

                        GUILayout.EndVertical();

                        GUILayout.BeginVertical(GUILayout.Width(ModMenu.label_column_width));
                        {

                            if (labelLeftEnabledCount != 0) {
                                foreach (ModUILabel uiLabel in labelsRight) {
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

                            foreach (ModUICustom uiCustom in customs) {
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
