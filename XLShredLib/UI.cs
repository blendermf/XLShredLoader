using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XLShredLib.UI {

    public interface IModUIControlText {
        string Text {
            get; set;
        }
    }

    public class ModUIControl {
        public String id = "";
        public int priority = 0;
    }

    public class ModUILabel : ModUIControl, IModUIControlText {
        public string Text { get; set; }

        public ModUILabel(string id, String text, int priority = 0) {

        }

        #region Deprecated Fields
        [ObsoleteAttribute("This field is obsolete. Add or remove the ModUIBox or label when enabled/disabled", false)]
        public Func<bool> isEnabled = null;

        [ObsoleteAttribute("This field is obsolete. There is no concept of side in the new menu.", false)]
        public Side side;

        [ObsoleteAttribute("This field is obsolete. ModUILabel is now meant only to be a label.", false)]
        public LabelType labelType = LabelType.Text;

        [ObsoleteAttribute("This field is obsolete. Use dedicated toggle control instead of ModUILabel", false)]
        private bool oldToggleValue = false;

        [ObsoleteAttribute("This field is obsolete. Use dedicated toggle control instead of ModUILabel", false)]
        private bool toggleValue = false;

        [ObsoleteAttribute("This field is obsolete. Use dedicated controls that include actions instead of ModUILabel", false)]
        public Action<bool> action = null;
        #endregion

        #region Deprecated Methods
        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the ModUILabel(string id, String text, int priority = 0) version, or one of the specific controls for Buttons/Toggles.", false)]
        public ModUILabel(string id, String text, Side side, Func<bool> isEnabled, int priority = 0) :
            this(id, LabelType.Text, text, side, isEnabled, false, null, priority) { }

        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the ModUILabel(string id, String text, int priority = 0) version, or one of the specific controls for Buttons/Toggles.", false)]
        public ModUILabel(string id, LabelType type, String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {
            this.id = id;
            this.labelType = type;
            this.Text = text;
            this.side = side;
            this.isEnabled = isEnabled;
            this.toggleValue = initToggle;
            this.oldToggleValue = initToggle;
            this.action = action;
            this.priority = priority;
        }


        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the ModUILabel(string id, ...) version.", false)]
        public ModUILabel(String text, Side side, Func<bool> isEnabled, int priority = 0) :
            this("", LabelType.Text, text, side, isEnabled, false, null, priority) { }

        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the ModUILabel(string id, ...) version.", false)]
        public ModUILabel(LabelType type, String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {
            this.id = "";
            this.labelType = type;
            this.Text = text;
            this.side = side;
            this.isEnabled = isEnabled;
            this.toggleValue = initToggle;
            this.oldToggleValue = initToggle;
            this.action = action;
            this.priority = priority;
        }

        [ObsoleteAttribute("This method is obsolete. Use dedicated toggle control instead of ModUILabel", false)]
        public void SetToggleValue(bool val) {
            oldToggleValue = toggleValue;
            toggleValue = val;
        }
        #endregion

        public void Render() {
            if (isEnabled != null && isEnabled()) {
                switch (labelType) {
                    case LabelType.Text:
                        GUILayout.Label(Text, ModMenu.Instance.fontSmall);
                        break;
                    case LabelType.Toggle:
                        oldToggleValue = toggleValue;
                        toggleValue = GUILayout.Toggle(toggleValue, Text, ModMenu.Instance.toggleStyle);
                        if (toggleValue != oldToggleValue && action != null) action(toggleValue);
                        break;
                    case LabelType.Button:
                        if (GUILayout.Button(Text, ModMenu.Instance.fontSmall) && action != null) action(true);
                        break;
                }
            }
        }
    }

    public class ModUICustom : ModUIControl {
        public Action onGUI = null;
        public Func<bool> isEnabled = null;

        public ModUICustom(string id, Action onGUI, Func<bool> isEnabled, int priority = 0) {
            this.id = id;
            this.onGUI = onGUI;
            this.isEnabled = isEnabled;
            this.priority = priority;
        }

        #region Deprecated Methods
        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the ModUICustom(string id, ...) version.", false)]
        public ModUICustom(Action onGUI, Func<bool> isEnabled, int priority = 0) {
            this.id = "";
            this.onGUI = onGUI;
            this.isEnabled = isEnabled;
            this.priority = priority;
        }
        #endregion

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
        public String modMaker = "";
        public string modName = "";
        public int priority;
        public List<ModUILabel> labelsLeft;
        public List<ModUILabel> labelsRight;
        public List<ModUICustom> customs;
        public List<ModUIControl> controls;
        public Dictionary<string, ModUILabel> labelsById;
        public Dictionary<string, ModUICustom> customsById;

        int labelLeftEnabledCount = 0;
        int labelRightEnabledCount = 0;
        int customEnabledCount = 0;

        public ModUIBox(String modName, String modMaker, int priority = 0) {
            labelsLeft = new List<ModUILabel>();
            labelsRight = new List<ModUILabel>();
            customs = new List<ModUICustom>();
            labelsById = new Dictionary<string, ModUILabel>();
            customsById = new Dictionary<string, ModUICustom>();
            this.modName = modName;
            this.modMaker = modMaker;
            this.priority = priority;
        }

        public ModUIBox(String modMaker, int priority = 0) {
            labelsLeft = new List<ModUILabel>();
            labelsRight = new List<ModUILabel>();
            customs = new List<ModUICustom>();
            labelsById = new Dictionary<string, ModUILabel>();
            customsById = new Dictionary<string, ModUICustom>();
            this.modMaker = modMaker;
            this.priority = priority;
        }

        public void AddLabel(ModUILabel uiLabel) {
            if (labelsById.ContainsKey(uiLabel.id)) {
                ModUILabel oldLabel = labelsById[uiLabel.id];
                if (oldLabel.side == uiLabel.side) {
                    if (uiLabel.side == Side.left) {
                        labelsLeft[labelsLeft.FindIndex(l => l.id == uiLabel.id)] = uiLabel;
                    } else {
                        labelsRight[labelsRight.FindIndex(l => l.id == uiLabel.id)] = uiLabel;
                    }
                } else {
                    if (oldLabel.side == Side.left) {
                        labelsLeft.Remove(oldLabel);
                        labelsRight.Add(uiLabel);
                    } else {
                        labelsRight.Remove(oldLabel);
                        labelsLeft.Add(uiLabel);
                    }
                }
            } else {
                if (uiLabel.id != "") labelsById[uiLabel.id] = uiLabel;

                if (uiLabel.side == Side.left) {
                    labelsLeft.Add(uiLabel);
                } else {
                    labelsRight.Add(uiLabel);
                }
            }

            UpdateSort();
        }

        public ModUILabel AddLabel(String id, String text, Side side, Func<bool> isEnabled, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel(id, text, side, isEnabled, priority);
            AddLabel(uiLabel);
            return uiLabel;
        }

        public ModUILabel AddLabel(String id, LabelType type, String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel(id, type, text, side, isEnabled, initToggle, action, priority);
            AddLabel(uiLabel);
            UpdateSort();

            return uiLabel;
        }

        public ModUILabel AddToggle(String id, String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {
            ModUILabel uiLabel = new ModUILabel(id, LabelType.Toggle, text, side, isEnabled, initToggle, action, priority);
            AddLabel(uiLabel);
            UpdateSort();

            return uiLabel;
        }

        public void RemoveLabel(string id) {
            ModUILabel label = labelsById[id];
            labelsById.Remove(id);
            labelsLeft.Remove(label);
            labelsRight.Remove(label);
        }

        public void AddCustom(ModUICustom uiCustom) {
            if (customsById.ContainsKey(uiCustom.id)) {
                ModUICustom oldCustom = customsById[uiCustom.id];
                customs[customs.FindIndex(c => c.id == uiCustom.id)] = uiCustom;
              
            } else {
                if (uiCustom.id != "") customsById[uiCustom.id] = uiCustom;

                customs.Add(uiCustom);
            }
            
            UpdateSort();
        }

        public ModUICustom AddCustom(String id, Action onGUI, Func<bool> isEnabled, int priority = 0) {
            ModUICustom uiCustom = new ModUICustom(id, onGUI, isEnabled, priority);
            AddCustom(uiCustom);
            return uiCustom;
        }

        public void RemoveCustom(string id) {
            ModUICustom custom = customsById[id];
            customsById.Remove(id);
            customs.Remove(custom);
        }

        #region Deprecated Methods
        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the AddLabel(string id, ...) version.", false)]
        public ModUILabel AddLabel(String text, Side side, Func<bool> isEnabled, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel(text, side, isEnabled, priority);
            AddLabel(uiLabel);
            return uiLabel;
        }

        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the AddLabel(string id, ...) version.", false)]
        public ModUILabel AddLabel(LabelType type, String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel(type, text, side, isEnabled, initToggle, action, priority);
            AddLabel(uiLabel);
            UpdateSort();

            return uiLabel;
        }

        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the AddLabel(string id, ...) version.", false)]
        public ModUILabel AddToggle(String text, Side side, Func<bool> isEnabled, bool initToggle = false, Action<bool> action = null, int priority = 0) {
            ModUILabel uiLabel = new ModUILabel(LabelType.Toggle, text, side, isEnabled, initToggle, action, priority);
            AddLabel(uiLabel);
            UpdateSort();

            return uiLabel;
        }

        [ObsoleteAttribute("This method is obsolete (and will eventually go away). Use the AddCustom(string id, ...) version.", false)]
        public ModUICustom AddCustom(Action onGUI, Func<bool> isEnabled, int priority = 0) {
            ModUICustom uiCustom = new ModUICustom(onGUI, isEnabled, priority);
            AddCustom(uiCustom);
            return uiCustom;
        }
        #endregion

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
