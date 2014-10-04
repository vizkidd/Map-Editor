using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Map_Editor
{
    public class Camera
    {
        
            protected float zoom; // Camera Zoom
            public Matrix transform; // Matrix Transform
            public Vector2 pos; // Camera Position
            protected float rotation; // Camera Rotation

            public Camera()
            {
                zoom = 1.0f;
                rotation = 0.0f;
                pos = Vector2.Zero;
               
            }

            // Sets and gets zoom
            public float Zoom
            {
                get { return zoom; }
                set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
            }

            public float Rotation
            {
                get { return rotation; }
                set { rotation = value; }
            }

            public void Clamp(float value,float min, float max)
            {
                MathHelper.Clamp(value, min, max);
            }

            // Auxiliary function to move the camera
            public void Move(Vector2 amount)
            {
                pos += amount;
            }
            // Get set position
            public Vector2 Pos
            {
                get { return pos; }
                set { pos = value; }
            }
            public Matrix GetTransformation(GraphicsDevice graphicsDevice)
            {
                if (pos.X < 0)
                {
                    pos.X = 0.0f;
                }
                if (pos.Y < 0)
                {
                    pos.Y = 0.0f;
                }
            
                       // Thanks to o KB o for this solution
                return Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                                             Matrix.CreateRotationZ(Rotation) *
                                             Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                             Matrix.CreateTranslation(new Vector3(MapEditor.viewport.Width * 0.0f, MapEditor.viewport.Height * 0.0f, 0));
                 
            }
            public void Update(GraphicsDevice g)
            {
                transform = GetTransformation(g);
            }
            public void GoTo(Rectangle rect)
            {
                transform =
                                              Matrix.CreateRotationZ(Rotation) *
                                              Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                              Matrix.CreateTranslation((rect.Width * 0.5f) - pos.X, (rect.Height * 0.5f) - pos.Y, 0f);
            }
    }

}
