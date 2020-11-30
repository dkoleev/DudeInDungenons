﻿using Runtime.Logic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Data.Items {
    [CreateAssetMenu(fileName = "Item", menuName = "Data/Items/Item")]
    public class Item : ScriptableObject {
        [SerializeField]
        private ResourceId _id;
        [SerializeField]
        private bool _haveAsset;
        [SerializeField, ShowIf("_haveAsset")]
        private AssetReference _asset;
        [SerializeField]
        [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
        private Texture2D _icon;

        public ResourceId Id => _id;
        public AssetReference Asset => _asset;
        public Texture2D Icon => _icon;
    }
}