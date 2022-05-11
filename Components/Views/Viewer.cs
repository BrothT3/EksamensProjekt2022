using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EksamensProjekt2022
{

    public class Viewer : Component
    {
        public int CurrentIndex { get; private set; }
        private float elapsed;
        private SpriteRenderer _spriteRenderer;
        private Dictionary<string, View> _views = new Dictionary<string, View>();
        private View _currentView;

        public override void Start()
        {
            _spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        }


        public override void Update(GameTime gameTime)
        {
            
        }

        public void AddView(View view)
        {
            _views.Add(view.Name, view);

            if (_currentView == null)
            {
                _currentView = view;
            }
        }

        public void ChangeAnimation(string viewName)
        {
            if(viewName != _currentView.Name)
            {
                _currentView = _views[viewName];
              //  elapsed = 0;
               // CurrentIndex = 0;
            }
        }

    }




}
