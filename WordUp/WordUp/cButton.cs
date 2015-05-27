using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;

namespace WordUp
{
    class cButton
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;

        // Used to channge the alpha color for mouseovers
        Color color = new Color(255, 255, 255, 255);

        public Vector2 size;

        public cButton(Texture2D newTexture, GraphicsDevice graphics)
        {

            texture = newTexture;

            size = new Vector2(texture.Width, texture.Height);

        }

        bool down;
        bool clicked;

        public bool isClicked
        {
            get { return this.clicked; }
        }

        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                        (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3; else color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) clicked = true;

            }
            else if (color.A < 255)
            {
                color.A += 3;
                clicked = false;
            }
        }
        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }


    }
}

