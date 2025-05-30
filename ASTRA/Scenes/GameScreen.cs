﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ASTRA.UserInterface;

namespace ASTRA.Scenes
{
    internal class GameScreen : Scene
    {
        /// <summary>
        /// The ID for the game sequence
        /// </summary>
        internal new const string ID = "Game Screen";

        internal bool ResetGame = false;

        internal event EventHandler Buttons;

        /// <summary>
        /// Whether the game should be considered "over".
        /// </summary>
        internal bool IsGameOver
        {
            get
            {
                return player.Lives.Value == 0;
            }
        }

        /// <summary>
        ///The previous keyboard state.
        /// </summary>
        private KeyboardState PreviousKeyboardState;

        /// <summary>
        /// The instance of the player.
        /// </summary>
        private Player player;

        LevelLoader loader;

        internal GameScreen() : base()
        {
            loader = new LevelLoader();
            loader.LoadLevel("..\\..\\..\\DemoLevel.txt", Add, Remove);
            loader.reset += base.Reset;
            Buttons += loader.button;

            player = loader.Player;
            /*
            player = new Player(GameDetails.CenterOfScreen);
            player.AddToParent = Add;
            player.RemoveFromParent = Remove;

            this.Add(player);

            /*
            Throwable test = new Throwable(new Vector2(900, 750), new Vector2(0, -8));
            test.Remove = this.Remove;
            test.JustThrown = false;
            this.Add(test);
            

            for (int i = 600; i <= 1600; i += 50)
            {
                this.Add(new CollidableWall(new Vector2(i, 200), new Vector2(50, 50), "tile"));
                
                this.Add(new CollidableWall(new Vector2(i, 800), new Vector2(50, 50), "tile"));
                
            }
            for (int i = 200; i < 800; i += 50)
            {
                this.Add(new CollidableWall(new Vector2(1600, i), new Vector2(50, 50), "tile"));
                this.Add(new CollidableWall(new Vector2(600, i), new Vector2(50, 50), "tile"));
            }
            */
            TextComponent throwableCount = new TextComponent("player_throwCount", $"# Throwables: {player.ThrowableCount.Value}.", "Mini", new Vector2(0, 0), ComponentOrigin.TopLeft);

            player.ThrowableCount.OnValueChanged += () =>
            {
                throwableCount.SetText($"# Throwables: {player.ThrowableCount.Value}.");

                if (player.ThrowableCount.Value == 0)
                {
                    throwableCount.TextColor = Color.Red;
                }
                else
                {
                    throwableCount.TextColor = Color.White;
                }
            };

            UI.AddComponent(throwableCount);

            TextComponent lives = new TextComponent("player_lives", $"Lives: {player.Lives.Value}.", "Mini", new Vector2(0, 20), ComponentOrigin.TopLeft);
            player.Lives.OnValueChanged += () =>
            {
                lives.SetText($"Lives: {player.Lives.Value}.");
            };

            UI.AddComponent(lives);
        }

        internal override void Update(GameTime gameTime)
        {
            Buttons.Invoke(this, EventArgs.Empty);
            base.Update(gameTime);

            //Old Version:
            /*
            for (int i = 0; i < GameObjects.Count; i++)
            {
                //TODO: Very bad code right here: This should be shamed and exiled from the project forever.
                //However, we are on a time crunch so it is here for the playtest build because it works.
                if (GameObjects[i] is Player || GameObjects[i] is Throwable)
                {
                    ICollidable actor = (ICollidable)GameObjects[i];

                    foreach (ICollidable collidable in Collidables)
                    {
                        if (actor.CollidesWith(collidable) && collidable is CollidableWall wall)
                        {
                            actor.Collide(wall);
                        }
                        else if (actor.CollidesWith(collidable) && collidable is Throwable t)
                        {
                            if (actor is Player && t.CanPickup)
                            {
                                actor.Collide(t);
                                t.Collide(actor);
                                Remove(t);
                            }
                        }
                        else if (actor.CollidesWith(collidable) && !actor.Equals(collidable))
                        {
                            actor.Collide(collidable);
                            collidable.Collide(actor);
                        }
                    }
                }
            }
            */

            KeyboardState kbstate = Keyboard.GetState();

            //single press of the escape keyboard
            if (PreviousKeyboardState.IsKeyDown(Keys.Escape) && kbstate.IsKeyUp(Keys.Escape) || IsGameOver)
            {
                SetScene(PauseScreen.ID);
            }

            PreviousKeyboardState = kbstate;
            Clean();
        }

        internal override void Load()
        {
            if (loader.OnNoNextLevel == null) 
                loader.OnNoNextLevel = SetScene;

            if (loader.HasPlayerWon)
            {
                loader.LoadLevel("..\\..\\..\\DemoLevel.txt", Add, Remove);
            }
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        internal override void Reset()
        {
            if (ResetGame)
            {
                base.Reset();
                loader.ResetCurrentLevel();
            }

            ResetGame = false;
        }
    }
}
