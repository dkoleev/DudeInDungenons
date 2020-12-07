using Runtime.Data.Items;
using Runtime.Logic.GameProgress.Progress.Items;

namespace Runtime.Logic.Factories {
    public static class ItemProgressFactory {
        public static ItemProgress Create(ItemStack itemStack) {
            var item = itemStack.Item;
            
            if (item is ItemAction) {
                return new ItemProgress(item.Id, itemStack.Amount);
            }
            if (item is ItemRestoreByTime itemRestoreByTime) {
                return new ItemTimeProgress(item.Id, itemStack.Amount, itemRestoreByTime.RestoreTime.TotalMilliseconds);
            }
            
            return new ItemProgress(item.Id, itemStack.Amount);
        }
    }
}
