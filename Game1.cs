using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CannonShooter
{
    struct PlayerData
    {
        public Vector2 Position;
        public bool isAlive;
        public Color PlayerColor;
        public float Angle;
        public float Power;
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GraphicsDevice _device;

        private Texture2D _backgroundTexture;
        private Texture2D _foregroundTexture;

        private Texture2D _carriageTexture;
        private Texture2D _cannonTexture;

        private int _screenWidth;
        private int _screenHeight;

        private PlayerData[] _players;
        private int _numberOfPlayers = 4;
        private int _currentPlayer = 0;

        private float _playerScaling;

        private Color[] _playerColors = new Color[10]
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Purple,
            Color.Yellow,
            Color.Orange,
            Color.BlueViolet,
            Color.Lavender,
            Color.DarkTurquoise,
            Color.Gold
        };

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.PreferredBackBufferWidth = 500;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Window.Title = "Cannon Shooter";

            base.Initialize();
        }

        private void SetUpPlayers()
        {
            _players = new PlayerData[_numberOfPlayers];

            for (int i = 0; i < _numberOfPlayers; i++)
            {
                _players[i].isAlive = true;
                _players[i].PlayerColor = _playerColors[i];
                _players[i].Angle = MathHelper.ToRadians(90);
                _players[i].Power = 100;
            }

            _players[0].Position = new Vector2(100, 193);
            _players[1].Position = new Vector2(200, 212);
            _players[2].Position = new Vector2(300, 361);
            _players[3].Position = new Vector2(400, 164);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _device = _graphics.GraphicsDevice;

            _backgroundTexture = Content.Load<Texture2D>("background");
            _foregroundTexture = Content.Load<Texture2D>("foreground");

            _carriageTexture = Content.Load<Texture2D>("carriage");
            _cannonTexture = Content.Load<Texture2D>("cannon");

            _screenHeight = _graphics.PreferredBackBufferHeight;
            _screenWidth = _graphics.PreferredBackBufferWidth;

            SetUpPlayers();

            _playerScaling = 40f / (float)_carriageTexture.Width;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void ProcessInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.Left))
            {
                _players[_currentPlayer].Angle -= 0.01f;
            }
            else if(keyboardState.IsKeyDown(Keys.Right))
            {
                _players[_currentPlayer].Angle += 0.01f;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            DrawScenery();
            DrawPlayers();
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, _screenWidth, _screenHeight);
            _spriteBatch.Draw(_backgroundTexture, screenRectangle, Color.White);
            _spriteBatch.Draw(_foregroundTexture, screenRectangle, Color.White);
        }

        private void DrawPlayers()
        {
            for (int i = 0; i < _numberOfPlayers; i++)
            {
                if (_players[i].isAlive)
                {
                    int xPos = (int)_players[i].Position.X;
                    int yPos = (int)_players[i].Position.Y;

                    Vector2 cannonOrigin = new Vector2(11, 50);
                    _spriteBatch.Draw(_carriageTexture, _players[i].Position, null, _players[i].PlayerColor,
                        0, new Vector2(0, _carriageTexture.Height), _playerScaling, SpriteEffects.None, 0);
                    _spriteBatch.Draw(_cannonTexture, new Vector2(xPos + 20, yPos - 10), null, _players[i].PlayerColor,
                        _players[i].Angle, cannonOrigin, _playerScaling, SpriteEffects.None, 1);

                }
            }
        }
    }
}

