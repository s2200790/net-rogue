using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
using RayGuiCreator;
using System.Xml.Linq;


namespace Rogue
{
    internal class Game
    {
        // List of possible class choices.
        MultipleChoiceEntry classChoices = new MultipleChoiceEntry(
        new string[] {"Duck" ,"Mongoose", "Elf" });

        MultipleChoiceEntry roleChoices = new MultipleChoiceEntry(
        new string[] { "Cook", "Smith", "Rogue" });

        // Volume value is modified by the volume slider
        float volume = 1.0f;

        // Textbox data for player's name
        TextBoxEntry playerNameEntry = new TextBoxEntry(15);

        // Is the spinner active or not. This is changed by the MenuCreator
        bool spinnerEditActive = false;

        Texture atlas;
        Texture hahmoAtlas;
        private PlayerCharacter player;
        private Map level01;
        public static readonly int tileSize = 16;
        bool enemyDrawing = false;
        bool itemDrawing = false;
        int game_width;
        int game_height;
        RenderTexture game_screen;

        int screen_width = 1280;
        int screen_height = 720;
        int enemyX = 0;
        int enemyY = 0;
        int itemX = 0;
        int itemY = 0;

        Options  myOptions;
        Pause myPause;
        public enum GameState
        {
            MainMenu,
            GameLoop,
            PlayerMenu,
            Options,
            Pause
        }

        Stack<GameState> stateStack = new Stack<GameState>();
        public static List<int> FloorTileNumbers;
        private void CreatePlayer()
        {
            string name;
            while (true)
            {
                Console.WriteLine("Enter your name:");
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsDigit) || name.Contains(" "))
                {
                    Console.WriteLine("Invalid name. Name cannot contain digits or spaces.");
                    continue;
                }
                break;
            }
            Species species;
            while (true)
            {
                Console.WriteLine("Choose your species:");
                Console.WriteLine("1. Duck");
                Console.WriteLine("2. Mongoose");
                Console.WriteLine("3. Elf");
                string vastaus = Console.ReadLine();

                int SpeciesNumber = -1;
                string[] raceNames = Enum.GetNames(typeof(Species));
                if (int.TryParse(vastaus, out SpeciesNumber))
                {

                    if (SpeciesNumber >= 1 && SpeciesNumber <= raceNames.Length)
                    {

                        species = Enum.Parse<Species>(raceNames[SpeciesNumber - 1]);
                        break;
                    }
                }

                if (!Enum.TryParse(vastaus, out species))
                {
                    Console.WriteLine("Invalid species.");
                    continue;
                }
            }
            Role role;
            while (true)
            {

                Console.WriteLine("Choose your role:");
                Console.WriteLine("1. Cook");
                Console.WriteLine("2. Smith");
                Console.WriteLine("3. Rogue");
                string vastaus = Console.ReadLine();

                int RoleNumber = -1;
                string[] raceNames = Enum.GetNames(typeof(Role));
                if (int.TryParse(vastaus, out RoleNumber))
                {

                    if (RoleNumber >= 1 && RoleNumber <= raceNames.Length)
                    {

                        role = Enum.Parse<Role>(raceNames[RoleNumber - 1]);
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid role.");
                    continue;
                }
            }

            player = new PlayerCharacter(name, species, role);
            Console.Clear();
            Console.WriteLine($"Player created: Name: {player.Name}, Species: {player.Species}, Role: {player.Role}");
            System.Threading.Thread.Sleep(4000);
            Console.Clear();
        }

        public void Run()
        {
            Init();
            
            GameLoop();
            Raylib.UnloadRenderTexture(game_screen);
        }

        private void Init()
        {
            myOptions = new Options();
            myPause = new Pause();
            myOptions.BackButtonPressedEvent += this.OnOptionsBackButtonPressed;
            myPause.BackButtonPressedEvent += this.OnPauseBackButtonPressed;
            myPause.OptionsButtonPressedEvent += this.OnPauseOptionsButtonPressed;
            stateStack.Push(GameState.MainMenu);

            MapLoader loader = new MapLoader();
            level01 = loader.LoadMapFromTiledFile("Map/NewRogueMap.tmj");

            Raylib.InitWindow(screen_width, screen_height, "Rogue");
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            game_width = 22 * 22 - 4;
            game_height = 17 * 19 - 4;
            Raylib.SetWindowMinSize(game_width, game_height);
            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);
            Raylib.SetTargetFPS(30);
            atlas = Raylib.LoadTexture("Map/Dungeon_Tileset.png");
            hahmoAtlas = Raylib.LoadTexture("Images/DungeonTile.png");
            
        }
        void OnOptionsBackButtonPressed(object sender, EventArgs args)
        {
            stateStack.Pop();
        }
        void OnPauseBackButtonPressed(object sender, EventArgs args)
        {
            stateStack.Pop();
        }
        void OnPauseOptionsButtonPressed(object sender, EventArgs args)
        {
            stateStack.Push(GameState.Options);
        }
        private void DrawGameToTexture()
        {
            Raylib.BeginTextureMode(game_screen);
            Raylib.ClearBackground(Raylib.RAYWHITE);
            int KenttäX = 0;// 16;
            int KenttäY = 0;// 16;
            int KenttäTilesPerRow = 10;
            int KenttäTileSize = 16;


            for (int y = 0; y < level01.MapHeight; y++)
            {
                for (int x = 0; x < level01.mapWidth; x++)
                {
                    int tileId = level01.GetTileAt(x, y);
                    if (tileId == 0)
                    {
                        continue;
                    }
                    else
                    {
                        tileId -= 1;
                    }
                    Rectangle destRect = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                    Color Color = (tileId == 1) ? Raylib.LIGHTGRAY : Raylib.GRAY;
                    Raylib.DrawRectangleRec(destRect, Color);

                    int atlasIndex = tileId;

                    int imageX = atlasIndex % KenttäTilesPerRow;
                    int imageY = (int)(atlasIndex / KenttäTilesPerRow);
                    int imagePixelX = KenttäX + imageX * KenttäTileSize;
                    int imagePixelY = KenttäY + imageY * KenttäTileSize;

                    Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, KenttäTileSize, KenttäTileSize);

                    Raylib.DrawTextureRec(atlas, imageRect, new Vector2(destRect.x, destRect.y), Raylib.WHITE);
                }
            }
            
            DrawEnemy();
            DrawItem();

            int pixelX = player.X * tileSize;
            int pixelY = player.Y * tileSize;
            int HahmoX = 127;
            int HahmoY = 15;
            int HahmoTilesPerRow = 9;
            int HahmoTileSizeL = 17;
            int HahmoTileSizeK = 28;
            Rectangle playerRect = new Rectangle(pixelX, pixelY, tileSize, tileSize);


            int atlasIndexP = 18;

            int PlayerImageX = atlasIndexP % HahmoTilesPerRow;
            int PlayerImageY = (int)(atlasIndexP / HahmoTilesPerRow);
            int HahmoImagePixelX = HahmoX + PlayerImageX * HahmoTileSizeL;
            int HahmoImagePixelY = HahmoY + PlayerImageY * HahmoTileSizeK;

            Rectangle playerSpriteRect = new Rectangle(HahmoImagePixelX, HahmoImagePixelY + 2, HahmoTileSizeL, HahmoTileSizeK - 5);
            Raylib.DrawTextureRec(hahmoAtlas, playerSpriteRect, new Vector2(pixelX - 2, pixelY - 8), Raylib.WHITE);

            Raylib.EndTextureMode();
            DrawGameScaled();
        }
        private void DrawGameScaled()
        {

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            int draw_width = Raylib.GetScreenWidth();
            int draw_height = Raylib.GetScreenHeight();
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);

            Rectangle source = new Rectangle(0.0f, 0.0f,
                game_screen.texture.width,
                game_screen.texture.height * -1.0f);

            Rectangle destination = new Rectangle(
                (draw_width - (float)game_width * scale) * 0.5f,
                (draw_height - (float)game_height * scale) * 0.5f,
                game_width * scale,
                game_height * scale);

            Raylib.DrawTexturePro(game_screen.texture,
                source, destination,
                new Vector2(0, 0), 0.0f, Raylib.WHITE);
            
            Raylib.EndDrawing();
        }
        private void UpdateGame()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
                MovePlayer(0, -1);
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
                MovePlayer(0, 1);
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                MovePlayer(-1, 0);
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
                MovePlayer(1, 0);

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
            {
                stateStack.Push(GameState.Pause);
            }
        }

        private void MovePlayer(int moveX, int moveY)
        {
            int newX = player.X + moveX;
            int newY = player.Y + moveY;

            if (IsTileWalkable(newX, newY))
            {
                if ((newX, newY) != (enemyX, enemyY))
                {
                    player.Move(moveX, moveY);
                }
                else
                {
                    Console.WriteLine("Vihollinen");
                }
                if ((newX, newY) == (itemX, itemY))
                {
                    Console.WriteLine("Esine");
                    itemDrawing = false;
                }
            }
        }

        private bool IsTileWalkable(int x, int y)
        {
            FloorTileNumbers = new List<int> { 8, 9, 12, 18, 19};
            return level01.GetTileAt(x, y) == 8;
        }

        public void GameLoop()
        {
                while (!Raylib.WindowShouldClose())
            {
                switch (stateStack.Peek())
                {
                    case GameState.MainMenu:
                        DrawMainMenu();
                        break;

                    case GameState.GameLoop:
                        UpdateGame();
                        DrawGameToTexture();
                        break;
                    case GameState.PlayerMenu:
                        ShowPlayerMenu();
                        break;
                    case GameState.Options:
                        myOptions.DrawMenu();
                        break;
                    case GameState.Pause:
                        myPause.DrawMenu();
                        break;

                }

                //UpdateGame();
                //DrawGameToTexture();
            }

            Raylib.CloseWindow();
        }

        private void DrawMainMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            int elementX = 40;
            int elementY = 40;
            int menuWidth = Raylib.GetScreenWidth() * 4;
            MenuCreator creator = new MenuCreator(elementX, elementY, 20, menuWidth );
            creator.Label("Rogue");
            creator.Label("Controls by Arrows");

                if (creator.LabelButton("Start"))
                {
                stateStack.Push(GameState.PlayerMenu);

                }
                if (creator.LabelButton("Options"))
                {
                stateStack.Push(GameState.Options);

                }

            elementY += 30;
                if(creator.LabelButton("Quit"))
                {
                }
            Raylib.EndDrawing();
        }

        public void DrawEnemy()
        {
            Random random = new Random();
            if (!enemyDrawing)
            {
                do
                {
                    enemyX = random.Next(0, level01.mapWidth);
                    enemyY = random.Next(0, level01.MapHeight);
                } while (level01.GetTileAt(enemyX, enemyY) != 8);
                enemyDrawing = true;
            }

            int EnemyX = 127;
            int EnemyY = 144;
            int EnemyTilesPerRow = 9;
            int EnemyTileSizeL = 17;
            int EnemyTileSizeK = 28;

            int atlasIndexP = 18;

            int EnemyImageX = atlasIndexP % EnemyTilesPerRow;
            int EnemyImageY = (int)(atlasIndexP / EnemyTilesPerRow);
            int EnemyImagePixelX = EnemyX + EnemyImageX * EnemyTileSizeL;
            int EnemyImagePixelY = EnemyY + EnemyImageY * EnemyTileSizeK;

            Rectangle EnemySpriteRect = new Rectangle(EnemyImagePixelX, EnemyImagePixelY + 2, EnemyTileSizeL, EnemyTileSizeK - 5);
            Raylib.DrawTextureRec(hahmoAtlas, EnemySpriteRect, new Vector2(enemyX * tileSize - 2, enemyY * tileSize - 8), Raylib.WHITE);
        }
        private void DrawItem()
        {
            Random random = new Random();
            if (!itemDrawing)
            {
                while (true)
                {
                    do
                    {
                        itemX = random.Next(0, level01.mapWidth);
                        itemY = random.Next(0, level01.MapHeight);
                    } while (level01.GetTileAt(itemX, itemY) != 8);
                    if (itemX != enemyX && itemY != enemyY)
                    {
                        break;
                    }
                }
                itemDrawing = true;
            }

            int ItemX = 15;
            int ItemY = 116;
            int ItemTilesPerRow = 10;
            int ItemTileSizeL = 16;
            int ItemTileSizeK = 16;

            int atlasIndexP = 18;

            int ItemImageX = atlasIndexP % ItemTilesPerRow;
            int ItemImageY = (int)(atlasIndexP / ItemTilesPerRow);
            int ItemImagePixelX = ItemX + ItemImageX * ItemTileSizeL;
            int ItemImagePixelY = ItemY + ItemImageY * ItemTileSizeK;

            Rectangle ItemSpriteRect = new Rectangle(ItemImagePixelX, ItemImagePixelY, ItemTileSizeL, ItemTileSizeK);
            Raylib.DrawTextureRec(atlas, ItemSpriteRect, new Vector2(itemX * tileSize, itemY * tileSize), Raylib.WHITE);
        }
        public void ShowPlayerMenu()
        {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(MenuCreator.GetBackgroundColor());

                DrawPlayerMenu();

                Raylib.EndDrawing();
        }
        public void Print()
        {
            Console.WriteLine(" Menu values: ");
            Console.WriteLine(
            $"Volume: {volume}\n" +
            $"Player name: \"{playerNameEntry}\"\n" +
            $"Player class {classChoices.GetIndex()}: {classChoices.GetSelected()}"
            );
        }

        private void DrawPlayerMenu()
        {
            int width = Raylib.GetScreenWidth() / 2;
            // Fit 22 rows on the screen
            int rows = 22;
            int rowHeight = Raylib.GetScreenHeight() / rows;
            // Center the menu horizontally
            int x = (Raylib.GetScreenWidth() / 2) - (width / 2);
            // Center the menu vertically
            int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;
            // 3 pixels between rows, text 3 pixels smaller than row height
            MenuCreator c = new MenuCreator(x, y, rowHeight, width, 3, -3);
            c.Label("Main menu");

            c.Label("Player name");
            c.TextBox(playerNameEntry);

            if (c.Button("Honk!"))
            {
                Console.Write("Honk!");
            }

            c.Label("Character class");
            c.DropDown(classChoices);

            c.Label("Role class");
            c.DropDown(roleChoices);

            c.Label("Volume");
            c.Slider("Quiet", "Max", ref volume, 0.0f, 1.0f);

            if (c.LabelButton(">>Print values to console"))
            {
                Print();
            }

            if (c.Button("Start"))
            {
                if (playerNameEntry.ToString != null)
                {
                    player = new PlayerCharacter(playerNameEntry.ToString(),Enum.Parse<Species>(classChoices.GetSelected()),Enum.Parse<Role>(roleChoices.GetSelected()));
                    stateStack.Push(GameState.GameLoop);
                }
            }

            // Draws open dropdowns over other menu items
            int menuHeight = c.EndMenu();

            // Draws a rectangle around the menu
            int padding = 2;
            Raylib.DrawRectangleLines(
                x - padding,
                y - padding,
                width + padding * 2,
                menuHeight + padding * 2,
                MenuCreator.GetLineColor());
        }
    }
}


 