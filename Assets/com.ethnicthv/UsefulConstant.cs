namespace com.ethnicthv
{
    public static class UsefulConstant
    {
        public const int MapSizeX = 10;
        public const int MapSizeY = 10;
        public static int MapSize => MapSizeX * MapSizeY;
        
        public const int MaxCollectable = 10;
        
        public const float CollectableRespawnTime = 1.0f;
        
        public const int GameOverScene= 2;
        public const int GamePlayScene = 1;
        public const int MainMenuScene = 0;
    }
}