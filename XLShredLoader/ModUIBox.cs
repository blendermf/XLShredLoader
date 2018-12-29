using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLShredLoader {
    public class ModUIBox {
        public String title;
        public float height = 30f;
        public float labelHeight = 0f;
        public float customHeight = 0f;
        public float yPos;
        public int numLabels;
        public int numLabelsLeft;
        public int numLabelsRight;
        public int numCustoms;

        public List<ModUILabel> labels;
        public List<ModUICustom> customs;

        public ModUIBox(String title) {
            labels = new List<ModUILabel>();
            customs = new List<ModUICustom>();
            this.title = title;
        }

        public struct ModUILabel {
            public String text;
            public float xPos;
            public float yPos;
        }

        public struct ModUICustom {
            public float xPos;
            public float yPos;
            public float width;
            public float height;
            public delegate void OnGUI(float xPos, float yPos);
            public OnGUI onGUI;
        }

        public enum Side {
            left,
            right
        }

        public void AddLabel(String text, Side side) {
            int numLabelsOnSide = (side == Side.left) ? numLabelsLeft : numLabelsRight;

            labels.Add(new ModUILabel {
                text = text,
                xPos = (side == Side.left) ? 10f : 290f,
                yPos = 20f + (20f * numLabelsOnSide)
            });

            labelHeight = Math.Max(numLabelsLeft, numLabelsRight) * 20f;
            height = 30f + labelHeight + customHeight;
            
            numLabels++;
            if (side == Side.left) {
                numLabelsLeft++;
            } else {
                numLabelsRight++;
            }
        }

        public void AddCustom(float xPos, float width, float height, ModUICustom.OnGUI onGUI) {
            customs.Add(new ModUICustom {
                xPos = xPos,
                width = width,
                height = height,
                onGUI = onGUI
            });

            customHeight += height;
            this.height = 30f + labelHeight + customHeight;
        }
    }
}
