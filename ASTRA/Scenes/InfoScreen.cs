using ASTRA.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.Scenes
{
    internal class InfoScreen : Scene
    {
        internal new const string ID = "Info Screen";

        internal Listener<int> ScrollValue;

        private bool ProperlyInitialized = false;

        internal InfoScreen() : base()
        {
            ScrollValue = new Listener<int>(0);
            Button goback = new Button("goback", "Go Back", GameDetails.CenterOfScreen * 2 - new Vector2(5, 5), ComponentOrigin.BottomRight);
            goback.OnClick += () => { SetScene(HomeScreen.ID); };

            TextComponent scrollwheel = new TextComponent("scrollwheel", $"Scroll Value: {ScrollValue.Value}.", "Mini", new Vector2(5, 5), ComponentOrigin.TopLeft);


            ScrollValue.OnValueChanged += () =>
            {
                scrollwheel.SetText($"Scroll Value: {ScrollValue.Value}.");
            };


            UI.AddComponent(scrollwheel);
            UI.AddComponent(goback);
        }

        internal override void Load()
        {
            // this is where file IO should occur.
            //ScrollValue.Value = Mouse.GetState().ScrollWheelValue;

            LocalContentManager lcm = LocalContentManager.Shared;

            Texture2D ref_buttonTexture = lcm.GetTexture("button");

            Vector2 Position = new Vector2(5, 5);
            Listener<int> index = new Listener<int>(0);
            index.OnValueChanged += () =>
            {
                if (index.Value % 4 == 0)
                {
                    Position.X = 5;
                    Position.Y += ref_buttonTexture.Height * 1.1f;
                }

                else
                {
                    Position.X += ref_buttonTexture.Width * 1.1f;
                }

            };


            StreamReader rdr = null;
            string line = null;
            string[] args;
            UIComponent element = null;
            try
            {

                //read in the file
                rdr = new StreamReader("..\\..\\..\\player_skillTree.txt");

                while ((line = rdr.ReadLine()) != null)
                {
                    args = line.Split(",");

                    //if properly initialized, the element already exists
                    if (ProperlyInitialized)
                    {
                        if (!UI.TryGetComponent(args[0], out element))
                        {
                            throw new Exception("Inaccurate name when comparing the info screen and the skill tree.");
                        }

                        (element as SkillElement).Learned.Value = args[2] == "1";
                        UpdateSkillPosition(element as SkillElement);
                    }
                    else
                    {
                        element = new SkillElement(args[0], args[1], args[2] == "1", Position, ComponentOrigin.TopLeft);


                        ScrollValue.OnValueChanged += () =>
                        {
                            UpdateSkillPosition((SkillElement)element);
                        };

                        UI.AddComponent(element);

                        
                    }

                    index.Value++;
                }
               
            }

            catch (Exception ex)
            {
                ProperlyInitialized = false;
                Debug.WriteLine(ex.Message);
                return;
            }

            rdr?.Close();
            ProperlyInitialized = true;

            UI.MoveToTop("scrollwheel");
            UI.MoveToTop("goback");


        }

        /// <summary>
        /// Updates a singular skill's position
        /// </summary>
        /// <param name="element"></param>
        private void UpdateSkillPosition(SkillElement element)
        {
            element.Position = element.DefaultPosition - new Vector2(0, ScrollValue.Value * 0.1f);
        }

        internal override void Draw(SpriteBatch batch)
        {
            if (!ProperlyInitialized)
                return;
            
            base.Draw(batch);
        }

        internal override void Update(GameTime gameTime)
        {
            int scrollWheelValue = Mouse.GetState().ScrollWheelValue;


            ScrollValue.Value = scrollWheelValue;
            base.Update(gameTime);

            

        }

        internal override void Reset()
        {
            
        }
    }
}
