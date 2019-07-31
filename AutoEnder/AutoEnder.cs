using UnityEngine;
using UnityEngine.UI;

namespace AutoEnder
{
    internal class AutoEnder : ModMeta
    {
        public override void ConstructOptionsScreen(RectTransform parent, bool inGame)
        {
            Text text = WindowManager.SpawnLabel();
            text.text = "Created by mladjo97";
            WindowManager.AddElementToElement(text.gameObject, parent.gameObject, new Rect(0f, 0f, 400f, 128f),
                new Rect(0f, 0f, 0f, 0f));
        }

        public override string Name => "Auto-Ender";
    }
}
