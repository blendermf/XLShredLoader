using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLShredLib {
    public class ModUIBox {
        public String modMaker;

        public List<ModUILabel> labelsLeft;
        public List<ModUILabel> labelsRight;
        public List<ModUICustom> customs;

        public ModUIBox(String modMaker) {
            labelsLeft = new List<ModUILabel>();
            labelsRight = new List<ModUILabel>();
            customs = new List<ModUICustom>();
            this.modMaker = modMaker;
        }
        public delegate bool IsEnabled();

        public struct ModUILabel {
            public String text;
            public IsEnabled isEnabled;
            public uint priority;
        }

        public struct ModUICustom {
            public delegate void OnGUI();
            public OnGUI onGUI;
            public IsEnabled isEnabled;
            public uint priority;
        }

        public enum Side {
            left,
            right
        }

        
        public void AddLabel(String text, Side side, IsEnabled isEnabled, uint priority = 0) {

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

        public void AddCustom(ModUICustom.OnGUI onGUI, IsEnabled isEnabled, uint priority = 0) {
            customs.Add(new ModUICustom {
                onGUI = onGUI,
                isEnabled = isEnabled,
                priority = priority
            });
        }
    }
}
