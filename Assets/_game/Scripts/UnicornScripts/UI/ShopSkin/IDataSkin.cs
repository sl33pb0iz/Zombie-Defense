namespace Unicorn.UI.Shop
{
    public interface IDataSkin
    {
        public int GetIdEquipSkin(TypeEquipment type);
        public void SetIdEquipSkin(TypeEquipment type, int id);
        void SetUnlockSkin(TypeEquipment typeEquipment, int dataShopIDSkin);
        int GetVideoSkinCount(TypeEquipment typeEquipment, int dataShopIDSkin);
        void SetVideoSkinCount(TypeEquipment typeEquipment, int dataShopIDSkin, int numberWatchVideo);
    }
}