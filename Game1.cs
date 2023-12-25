using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using ParticleEngine;
using System.Collections.Generic;

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

        private Texture2D _smoke;
        private Texture2D _smoke2;
        private Texture2D _smoke3;
        private Texture2D _smoke4;
        private Texture2D _smoke5;
        private Texture2D _smoke6;
        private Texture2D _smoke7;
        private Texture2D _smoke8;

        private Texture2D _rocketTexture;

        private bool _rocketFlying = false;
        private Vector2 _rocketPosition;
        private Vector2 _rocketDirection;
        private float _rocketAngle;
        private float _rocketScaling = 0.1f;


        private SpriteFont _renogare;

        private int _screenWidth;
        private int _screenHeight;

        private PlayerData[] _players;
        private int _numberOfPlayers = 4;
        private int _currentPlayer = 0;

        private float _playerScaling;

        private ParticleSystem _smokeTrail;

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

            _rocketTexture = Content.Load<Texture2D>("rocket");

            _smoke = Content.Load<Texture2D>("smoke");
            _smoke2 = Content.Load<Texture2D>("smoke_04");
            _smoke3 = Content.Load<Texture2D>("smoke_05");
            _smoke4 = Content.Load<Texture2D>("smoke_06");
            _smoke5 = Content.Load<Texture2D>("smoke_07");
            _smoke6 = Content.Load<Texture2D>("smoke_08");
            _smoke7 = Content.Load<Texture2D>("smoke_09");
            _smoke8 = Content.Load<Texture2D>("smoke_10");

            List<Texture2D> smokeTextures = new List<Texture2D>();
            smokeTextures.Add(_smoke);
            smokeTextures.Add(_smoke2);
            smokeTextures.Add(_smoke3);
            smokeTextures.Add(_smoke4);
            smokeTextures.Add(_smoke5);
            smokeTextures.Add(_smoke6);
            smokeTextures.Add(_smoke7);
            smokeTextures.Add(_smoke8);

            _renogare = Content.Load<SpriteFont>("renogare");

            _screenHeight = _graphics.PreferredBackBufferHeight;
            _screenWidth = _graphics.PreferredBackBufferWidth;

            SetUpPlayers();

            _playerScaling = 40f / (float)_carriageTexture.Width;
            _smokeTrail = new ParticleSystem(3, _rocketPosition, smokeTextures);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ProcessInput();
            UpdateRocket();
            base.Update(gameTime);
        }

        private void UpdateRocket()
        {
            Vector2 gravity = new Vector2(0, 1);
            if(_rocketFlying)
            {
              
                _rocketDirection += gravity / 10;
                int sign = Math.Sign(_rocketDirection.Y);
                _smokeTrail.Direction = sign;
                _rocketAngle = (float)Math.Atan2(_rocketDirection.X, -_rocketDirection.Y);
                _rocketPosition += _rocketDirection;

                _smokeTrail.EmitterLocation = _rocketPosition;
                _smokeTrail.Update();

            }
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

            if (_players[_currentPlayer].Angle >= MathHelper.PiOver2)
            {
                _players[_currentPlayer].Angle = MathHelper.PiOver2;
            }

            if (_players[_currentPlayer].Angle <= -MathHelper.PiOver2)
            {
                _players[_currentPlayer].Angle = -MathHelper.PiOver2;
            }

            if(keyboardState.IsKeyDown(Keys.Down))
            {
                _players[_currentPlayer].Power--;
            }
            if(keyboardState.IsKeyDown(Keys.Up))
            {
                _players[_currentPlayer].Power++;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.LeftShift))
            {
                _players[_currentPlayer].Power -= 20;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.LeftShift))
            {
                _players[_currentPlayer].Power += 20;
            }

            if (_players[_currentPlayer].Power >= 1000)
            {
                _players[_currentPlayer].Power = 1000;
            }

            if (_players[_currentPlayer].Power <= 0)
            {
                _players[_currentPlayer].Power = 0;
            }

            if(keyboardState.IsKeyDown(Keys.Enter) || keyboardState.IsKeyDown(Keys.Space))
            {
                _rocketFlying = true;
                _rocketPosition = _players[_currentPlayer].Position;
                _rocketPosition.X += 20;
                _rocketPosition.Y -= 10;
                _rocketAngle = _players[_currentPlayer].Angle;

                Vector2 up = new Vector2(0, -1);

                Matrix rotMatrix = Matrix.CreateRotationZ(_rocketAngle);

                _rocketDirection = Vector2.Transform(up, rotMatrix);

                _rocketDirection *= _players[_currentPlayer].Power / 50f;
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            DrawScenery();
            DrawPlayers();
            DrawRocket();

            PlayerData currentPlayer = _players[_currentPlayer];
            float angle = (float)Math.Round(MathHelper.ToDegrees(currentPlayer.Angle));
            float power = currentPlayer.Power;

            DrawText("Power: " + power, new Vector2(20, 20), currentPlayer.PlayerColor);

            DrawText("Angle: " + angle, new Vector2(20, 60), currentPlayer.PlayerColor);

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

        private void DrawText(string message, Vector2 position, Color textColor)
        {
            _spriteBatch.DrawString(_renogare, message, position, textColor);
        }

        private void DrawRocket()
        {
            if(_rocketFlying)
            {
                _spriteBatch.Draw(_rocketTexture, _rocketPosition, null, _players[_currentPlayer].PlayerColor,
                    _rocketAngle, new Vector2(42, 240), _rocketScaling, SpriteEffects.None, 1);
                _smokeTrail.Draw(_spriteBatch);
            }
        }
    }
}

