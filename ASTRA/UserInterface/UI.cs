using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTRA.UserInterface
{
    internal sealed class UI
    {
        /// <summary>
        /// The list of components stored in this UI.
        /// </summary>
        private List<UIComponent> Components;


        /// <summary>
        /// Creates a new UI, which will be associated with a set scene / stage.
        /// </summary>
        internal UI()
        {
            Components = new List<UIComponent>();
        }

        /// <summary>
        /// Adds a component to the UI.
        /// </summary>
        /// <param name="component"></param>
        internal void AddComponent(UIComponent component)
        {

            //tries to get a component with the same ID. "out _" just means we don't end up using the out variable so it's unnecessary.
            if (TryGetComponent(component.ID, out _))
            {
                throw new ArgumentException($"Duplicate Component ID, please use an ID different from {component.ID}.");
            }

            //add to list.
            Components.Add(component);
        }

        /// <summary>
        /// Tries to get a component with a set ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="component">If a result is found, then it will be passed out.</param>
        /// <returns></returns>
        internal bool TryGetComponent(string ID, out UIComponent component)
        {
            for (int i = 0; i < Components.Count; i++)
            {

                //if a match exists then return that element and return true.
                if (Components[i].ID == ID)
                {
                    component = Components[i];
                    return true;
                }
            }

            //did not contain any element by that name.
            component = default;
            return false;
            
        }

        /// <summary>
        /// Moves an element to the top of the UI if it exists.
        /// </summary>
        /// <param name="ID"></param>
        internal void MoveToTop(string ID)
        {
            if (TryGetComponent(ID, out UIComponent component))
            {
                Components.Remove(component);
                Components.Add(component);
            }
        }

        /// <summary>
        /// Gets all componenent that are of a particular subclass.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal List<T> GetSubComponents<T>() where T : UIComponent
        {
            List<T> subComponents = new List<T>();

            foreach (UIComponent component in Components)
            {
                if (component is T subcomponent)
                {
                    subComponents.Add(subcomponent);
                }
            }

            return subComponents;
        }



        /// <summary>
        /// Draws the UI to the screen.
        /// </summary>
        /// <param name="batch"></param>
        internal void Draw(SpriteBatch batch)
        {
            foreach (UIComponent component in Components)
            {
                component.Draw(batch);
            }
        }

        /// <summary>
        /// Updates the UI in its entirety.
        /// </summary>
        /// <param name="gameTime"></param>
        internal void Update(GameTime gameTime)
        {
            foreach (UIComponent component in Components)
            {
                component.Update(gameTime);
            }
        }

        /// <summary>
        /// Resets each UI component in the components list.
        /// </summary>
        internal void Reset()
        {
            foreach (UIComponent component in Components)
            {
                component.Reset();
            }
        }
    }
}
