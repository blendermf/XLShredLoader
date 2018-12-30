using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLShredLib {
    public class ModUIBox {
        public String modMaker;
        public int priority;
        public List<ModUILabel> labelsLeft;
        public List<ModUILabel> labelsRight;
        public List<ModUICustom> customs;

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
        }

        public struct ModUICustom {
            public delegate void OnGUI();
            public OnGUI onGUI;
            public IsEnabled isEnabled;
            public int priority;
        }

        public enum Side {
            left,
            right
        }

        
        public void AddLabel(String text, Side side, IsEnabled isEnabled, int priority = 0) {

            ModUILabel uiLabel = new ModUILabel {
                text = text,
                isEnabled = isEnabled,
                priority = priority
            };
            
            if (side == Side.left) {
                labelsLeft.Add(uiLabel);
            } else {
                labelsRight.Add(uiLabel);
            }
        }

        public void AddCustom(ModUICustom.OnGUI onGUI, IsEnabled isEnabled, int priority = 0) {
            customs.Add(new ModUICustom {
                onGUI = onGUI,
                isEnabled = isEnabled,
                priority = priority
            });
        }
    }
}
