using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    public class GameObject
    {
        public Transform Transform { get; private set; } = new Transform();
        private List<Component> components = new List<Component>();
        public string Tag { get; set; }

        public Component AddComponent(Component component)
        {
            component.GameObject = this;
            components.Add(component);

            return component;
        }

        public Component GetComponent<T>() where T : Component
        {
            return components.Find(x => x.GetType() == typeof(T));
        }

        public void Awake()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Awake();
            }
        }

        public void Start()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Start();
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Draw(spriteBatch);
            }
        }

        public Object Clone()
        {
            GameObject go = new GameObject();

            foreach (Component c in components)
            {
                go.AddComponent(c.Clone() as Component);
            }

            return go;
        }
    }

}
