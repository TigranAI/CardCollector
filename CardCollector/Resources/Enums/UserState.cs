namespace CardCollector.Resources.Enums
{
    /* Представление состояний пользователя */
    public enum UserState
    {
        /* Пользователь в меню коллекции */
        CollectionMenu,
        /* Пользователь в меню магазина */
        ShopMenu,
        /* Пользователь в меню аукциона */
        AuctionMenu,
        /* Пользователь, загружающий файл */
        UploadFile,
        CombineMenu,
        ProductMenu,
        
        /* Пользователь в базовом состоянии (по умолчанию) */
        Default,

        UploadSticker,
        LoadForSaleSticker,
        UploadPackPreview,
        EditSticker,
        GiveSticker,
        CreateGiveaway,
        SelectGiveawayPrize,
        SendDistributionSticker,
        UploadPackGifPreview,
        UploadStickerPackZip,
        AddPuzzle,
        SendPuzzleArchive
    }
}