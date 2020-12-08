using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Utilities {
    public static class LoadHelper {
        public static Sprite CreateSprite(Texture2D texture) {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2f);
        }

        public static void InstantiateAsset<T>(string id, ItemsReference itemsReference, Action<T> onCreated, 
            Transform parent = null) where T : Component {
            var data = itemsReference.GetItem(id);
            InstantiateAsset(data.Asset, onCreated, parent);
        }
        
        public static void InstantiateAsset<T>(AssetReference asset, Action<T> onCreated, 
            Transform parent = null) where T : Component {
            asset.InstantiateAsync(parent).Completed += handle => {
                var resultObject = handle.Result.GetComponent<T>();
                resultObject.transform.localPosition = Vector3.zero;
                resultObject.transform.localRotation = Quaternion.identity;
                
                onCreated?.Invoke(resultObject);
            };
        }
    }
}
