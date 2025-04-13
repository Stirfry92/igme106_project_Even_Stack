using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA
{
    internal class LocalContentManager
    {

        /// <summary>
        /// The singleton instance of the local content manager.
        /// </summary>
        internal static LocalContentManager Shared
        {
            get
            {
                if (shared == null)
                    shared = new LocalContentManager();

                return shared;
            }
        }

        /// <summary>
        /// The private instance of the local content manager.
        /// </summary>
        private static LocalContentManager shared;


        /// <summary>
        /// Initializes the shared instance of the local content manager.
        /// </summary>
        private LocalContentManager()
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
        }

        /// <summary>
        /// The list of textures from Content.
        /// </summary>
        private Dictionary<string, Texture2D> Textures;

        /// <summary>
        /// The list of fonts from Content.
        /// </summary>
        private Dictionary<string, SpriteFont> Fonts;

        /// <summary>
        /// Adds an item to the local content manager. This should be directly imported from Content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        internal void Add<T>(string name, T item)
        {
            if (item is Texture2D texture)
            {
                Textures.Add(name, texture);
                return;
            }

            if (item is SpriteFont font)
            {
                Fonts.Add(name, font);
            }

            throw new ArgumentException("Argument is not a type that can be stored inside the local content manager.");
        }

        /// <summary>
        /// Gets a texture from the local content manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Texture2D GetTexture(string name)
        {
            if (Textures.TryGetValue(name, out Texture2D value))
            {
                return value;
            }

            throw new KeyNotFoundException($"No texture was found with the name {name} inside the local content manager.");
        }

        /// <summary>
        /// Gets a font from the local content manager.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        internal SpriteFont GetFont(string name)
        {
            if (Fonts.TryGetValue(name, out SpriteFont value))
            {
                return value;
            }

            throw new KeyNotFoundException($"No font was found with the name {name} inside the local content manager.");
        }


    }
}
