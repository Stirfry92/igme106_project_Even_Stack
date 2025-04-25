using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.UserInterface
{
    internal class SkillElement : UIComponent
    {
        /// <summary>
        /// The name of the skill element.
        /// </summary
        internal readonly string Name;

        /// <summary>
        /// The description for the skill element.
        /// </summary>
        internal readonly string Description;

        /// <summary>
        /// Whether the skill has been learned by the player.
        /// </summary>
        internal Listener<bool> Learned { get; }

        /// <summary>
        /// The font used for the title.
        /// </summary>
        private readonly SpriteFont TitleFont;
        private readonly SpriteFont DescriptionFont;

        private readonly Vector2 TitleMeasure;
        private readonly Vector2 DescriptionMeasure;

        private Color BorderColor = Color.Gray;

        private Vector2 TitlePosition
        {
            get
            {
                
                return TopLeftCorner + new Vector2(Size.X * 0.5f, Size.Y * 0.2f) - (TitleMeasure * 0.5f); 
            }
        }
        private Vector2 DescriptionPosition
        {
            get
            {
                return TopLeftCorner + new Vector2(Size.X * 0.5f, Size.Y * 0.7f) - (DescriptionMeasure * 0.5f);
            }
        }


        /// <summary>
        /// Represents a skill element that the player can learned.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="description"></param>
        /// <param name="position"></param>
        /// <param name="origin"></param>
        public SkillElement(string name, string description, bool learned, Vector2 position, ComponentOrigin origin) : base(name, position, origin)
        {
            Name = name;
            Description = description;

            Learned = new Listener<bool>(learned);

            LocalContentManager lcm = LocalContentManager.Shared;
            //add in the fonts necessary

            TitleFont = lcm.GetFont("Standard");
            DescriptionFont = lcm.GetFont("Mini");

            //measure the strings
            TitleMeasure = TitleFont.MeasureString(Name);
            DescriptionMeasure = DescriptionFont.MeasureString(Description);

            //add in the buttons necessary
            Image = lcm.GetTexture("button");

            Size = new Vector2(Image.Width, Image.Height);

            Learned.OnValueChanged += () =>
            {
                if (Learned.Value)
                {
                    BorderColor = Color.White;
                }
                else
                {
                    BorderColor = Color.Gray;
                }
            };

        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, TopLeftCorner, BorderColor);
            batch.DrawString(TitleFont, Name, TitlePosition, BorderColor);

            if (Learned.Value)
            {
                batch.DrawString(DescriptionFont, Description, DescriptionPosition, BorderColor);
            }
        }

        internal override void Update(GameTime gameTime)
        {}
    }
}
