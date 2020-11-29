using UnityEngine;

namespace Runtime.Utilities {
    public static class LoadHelper {
        public static Sprite CreateSprite(Texture2D texture) {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2f);
        }
    }
}
