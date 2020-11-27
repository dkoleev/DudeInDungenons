using UnityEngine;

namespace Runtime.Utilities {
    public class FpsCounter : MonoBehaviour {
        float _deltaTime = 0.0f;

        void Update() {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        void OnGUI() {
            int w = Screen.width, h = Screen.height;
            
            float msec = _deltaTime * 1000.0f;
            float fps = 1.0f / _deltaTime;
            
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.fontStyle = FontStyle.Bold;
            
            var color = Color.green;
            if (fps < 56) {
                color = Color.yellow;
            }
            if (fps < 50) {
                color = Color.red;
            }
            style.normal.textColor = color;

            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }
}