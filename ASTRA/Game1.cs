using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ASTRA;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    /*********************
     *     TEST ZONE     *
     * *******************/
    //Using this variable to test player movement
    //-Sterling
    private Player player;
    private Player player2;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.IsFullScreen = true;

        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        //FROM STERLING: Testing basic player requires a texture, I made a temporary texture right here: Remove if needed
        //but preferably integrate it elsewhere.
        LocalContentManager.Shared.Add<Texture2D>("blank", Content.Load<Texture2D>("blank"));
        player = new Player(new Vector2(400, 400));
        player2 = new Player(new Vector2(200, 200));
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        player.Update(gameTime);

        IDrawable i = (IDrawable)player;
        if (player.CollisionBounds.Intersects(player2.CollisionBounds))
        {
            player.Collide(player2);
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        IDrawable i = (IDrawable)player;
        i.Draw(_spriteBatch);
        i = (IDrawable)player2;
        i.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
