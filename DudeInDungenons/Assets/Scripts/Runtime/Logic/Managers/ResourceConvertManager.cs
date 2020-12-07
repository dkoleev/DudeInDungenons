using System.Collections.Generic;
using Runtime.Logic.Converters;

namespace Runtime.Logic.Managers {
    public class ResourceConvertManager : ManagerBase {
        private List<IResourceConverter> _converters = new List<IResourceConverter>();
        
        public ResourceConvertManager(GameController gameController) : base(gameController) {
            var energyRestore = new TimeToResourceConverter(GameController.ItemReference.EnergyData, GameController.Inventory);
            var expToLevelConverter = new ResourceConverter(GameController.ItemReference.ExpData, 
                GameController.ItemReference.LevelData, 
                GameController.SettingsReference.LevelUp.LevelByExp, GameController.Inventory);


            _converters.Add(energyRestore);
            _converters.Add(expToLevelConverter);

            foreach (var converter in _converters) {
                converter.ApplyGameOutProgress(GameController.Progress);
            }
        }

        public override void Update() {
            foreach (var converter in _converters) {
                converter.Update();
            }
        }

        public override void Dispose() {
        }
    }
}