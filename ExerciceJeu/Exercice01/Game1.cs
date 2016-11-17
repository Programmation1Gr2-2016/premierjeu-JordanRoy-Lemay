using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace Exercice01
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle fenetre;
        GameObject heros;
        GameObject badboy;
        GameObject arrow;
        SoundEffect son;
        SoundEffectInstance dying;
        SoundEffectInstance musique;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;


            this.graphics.ToggleFullScreen();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;

            Song song = Content.Load<Song>("LinkMusic");
            MediaPlayer.Play(song);
            son = Content.Load<SoundEffect>("LTTP_Link_Dying");
            dying = son.CreateInstance();

            heros = new GameObject();
            heros.estVivant = true;
            heros.vitesse = 10;
            heros.sprite = Content.Load<Texture2D>("LinkGood.png");
            heros.position = heros.sprite.Bounds;
            heros.position.Offset(650, 300);

            UpdateHeros();

            badboy = new GameObject();
            badboy.estVivant = true;
            badboy.vitesse = 1;
            badboy.sprite = Content.Load<Texture2D>("LinkBad.png");
            badboy.position = badboy.sprite.Bounds;
            badboy.position.Offset(0,0);

            arrow = new GameObject();
            arrow.estVivant = true;
            arrow.vitesse = 25;
            arrow.sprite = Content.Load<Texture2D>("ArrowMonogame.png");
            arrow.position = arrow.sprite.Bounds;

            // TODO: use this.Content to load your game content here
        }

        


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //DÉPLACEMENT DU HERO
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                heros.position.X += heros.vitesse;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                heros.position.X -= heros.vitesse;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                heros.position.Y -= heros.vitesse;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                heros.position.Y += heros.vitesse;
            }
            //DÉPLACEMENT DU BADBOY
            if (badboy.position.X == 0)
            {
                badboy.vitesse = 10;
            }
            if (badboy.position.X >= fenetre.Right - badboy.sprite.Width)
            {
                badboy.vitesse = -10;
            }
            badboy.position.X += badboy.vitesse;

            //DÉPLACEMENT ET RESPAWN ARROW
            arrow.position.Y += arrow.vitesse;

            if (arrow.position.Y > fenetre.Bottom - arrow.sprite.Height)
            {
                arrow.position.X = badboy.position.X;
                arrow.position.Y = badboy.position.Y;
                arrow.position.Y += arrow.vitesse;
            }
            //MORT DU HERO
            if (heros.position.Intersects(arrow.position))
            {
                heros.estVivant = false;
                dying.Play();
            }
            if (heros.position.Intersects(badboy.position))
            {
                heros.estVivant = false;
                dying.Play();
            }

            UpdateHeros();

            base.Update(gameTime);
        }
        
        protected void UpdateHeros()
        {
            //EMPECHE QUE LE HERO SORTE
            if (heros.position.X < fenetre.Left)
            {
                heros.position.X = fenetre.Left;
            }
            if (heros.position.X+ heros.sprite.Bounds.Width > fenetre.Right)
            {
                heros.position.X = fenetre.Right - heros.sprite.Bounds.Width;
            }
            if (heros.position.Y < fenetre.Top)
            {
                heros.position.Y = fenetre.Top;
            }
            if (heros.position.Y + heros.sprite.Bounds.Height > fenetre.Bottom)
            {
                heros.position.Y = fenetre.Bottom - heros.sprite.Bounds.Height;
            }
            


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            if (heros.estVivant == true)
            {
                spriteBatch.Draw(heros.sprite, heros.position, Color.White);
            }
            spriteBatch.Draw(badboy.sprite, badboy.position, Color.White);
            spriteBatch.Draw(arrow.sprite, arrow.position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
