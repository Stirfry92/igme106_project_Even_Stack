using ASTRA.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Emit;

namespace ASTRA;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //the scene manager that will be used throughout the application
    private SceneManager sceneManager;

    /// <summary>
    /// The required textures for the game.
    /// TODO: add any texture that is required up here!
    /// </summary>
    private string[] requiredTextures = { "blank", "button", "editedAstronaut", "astralogo"};

    /*********************
     *     TEST ZONE     *
     * *******************/
    //Using this variable to test player movement
    //-Sterling
    private Player player;
    private Player player2;
    //Eason
    LevelLoader level = new LevelLoader();
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = GameDetails.GameWidth;
        _graphics.PreferredBackBufferHeight = GameDetails.GameHeight;
        _graphics.IsFullScreen = true;

        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {

        LocalContentManager lcm = LocalContentManager.Shared;

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        //FROM STERLING: Testing basic player requires a texture, I made a temporary texture right here: Remove if needed
        //but preferably integrate it elsewhere.

        foreach (string texture in requiredTextures)
        {
            lcm.Add<Texture2D>(texture, Content.Load<Texture2D>(texture));
        }

        lcm.Add<SpriteFont>("Standard", Content.Load<SpriteFont>("Standard"));


        player = new Player(new Vector2(400, 400));
        player2 = new Player(new Vector2(200, 200));
        // TODO: use this.Content to load your game content here

        //after content has been loaded, the scenes can be loaded
        sceneManager = new SceneManager(Exit);
        
        //Eason LevelLoader testing 
        level.LoadLevel("../../../DemoLevel.txt");
        //
    }

    protected override void Update(GameTime gameTime)
    {

        sceneManager.CurrentScene.Update(gameTime);
        /*
        // TODO: Add your update logic here
        player.Update(gameTime);

        if (player.CollisionBounds.Intersects(player2.CollisionBounds))
        {
            player.Collide(player2);
        }
        */

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        
        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        //ZACH - delete if need to test gameplay
        sceneManager.CurrentScene.Draw(_spriteBatch);// un comment after testing 

        //Eason levelLoader testing
        //level.DrawLevel(_spriteBatch);
        //
        /*

        player.Draw(_spriteBatch);
        
        player2.Draw(_spriteBatch);
        */
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
