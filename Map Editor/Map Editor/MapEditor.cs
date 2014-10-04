using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Map_Editor
{
    struct TileInfo
    {
        public Texture2D texture;
        public bool collision;
        public uint index;
    }
    struct CharacterInfo
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 startingPosition;
    }


    class MapEditor : GraphicDeviceControl
    {
        //ContentBuilder contentBuilder;
        ContentManager content;
        Texture2D baseTexture,selection;
        TileInfo wall,box;
        SpriteBatch spriteBatch;
        SpriteFont font;
        public static Viewport viewport;
        Camera camera;
        KeyboardState previousKeyboardState, curKerboardState;
        Color customColor = new Color();
        MouseState curMouse,prevMouse;
        Rectangle mouseRectangle;
        public static GraphicsDevice graphics;

        int initialize = 0;

        public static List<TileInfo> TileList;
        public static List<CharacterInfo> CharacterList;
        //List<TileInfo> ObjectList;
        Vector2 mapResolution=new Vector2(1000,1000);
        Vector2 numberOfTiles = new Vector2(50, 50);
        Vector2 currentTile;
        int tileWidth, tileHeight;
        Texture2D[,] BaseMap;
        Texture2D[,] TileMap;
        Texture2D[,] ObjectMap;
        Rectangle[,] RectangleMap;

        public MapEditor()
        {
                       /* string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = Path.GetFullPath(relativePath);*/
            //contentBuilder = new ContentBuilder();
            content = new ContentManager(Services, "Content");
            
            camera = new Camera();
            //content.RootDirectory = "Content";
            tileWidth = (int)(mapResolution.X / numberOfTiles.X);
            tileHeight = (int)(mapResolution.Y / numberOfTiles.Y);
            
        }

        protected override void Initialize()
        {
            viewport.Bounds = new Rectangle(0, 0, this.Width, this.Height);
            graphics = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Mouse.WindowHandle = this.Handle;
            camera.Pos = new Vector2(0f, 0f);

            //wall = new TileInfo();
            TileList = new List<TileInfo>();
            CharacterList = new List<CharacterInfo>();
            TileMap = new Texture2D[(int)numberOfTiles.X, (int)numberOfTiles.Y];
            BaseMap = new Texture2D[(int)numberOfTiles.X, (int)numberOfTiles.Y];
            RectangleMap = new Rectangle[(int)numberOfTiles.X, (int)numberOfTiles.Y];
            font = content.Load<SpriteFont>("hudFont");
            baseTexture = content.Load<Texture2D>("base");
            selection = content.Load<Texture2D>("select");
            wall.texture = content.Load<Texture2D>("new");
            wall.texture.Name = "Wall";
            box.texture = content.Load<Texture2D>("box");
            box.texture.Name = "Box";
            TileList.Add(wall);
            TileList.Add(box);
            
           for (int i = 0; i < numberOfTiles.X; i++)
            {
                for (int j = 0; j < numberOfTiles.Y; j++)
                {
                    TileMap[i, j] = null;
                    BaseMap[i, j] = content.Load<Texture2D>("base");
                    RectangleMap[i, j] = new Rectangle((int)i * tileWidth, (int)j * tileHeight, tileWidth, tileHeight);
                }

            }
           
           TileMap[1, 1] = TileList[0].texture;
           TileMap[4, 4] = TileList[1].texture;
           foreach (TileInfo item in TileList)
           {
               Form1.listBox1.Items.Add(item.texture.Name);
           }
           initialize = 1;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (initialize == 1)
            {
                
                prevMouse = curMouse;
                curMouse = Mouse.GetState();
                curKerboardState = previousKeyboardState;

                curKerboardState = Keyboard.GetState();


                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    camera.pos.X += 0.009f;
                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    camera.pos.X -= 0.009f;
                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    camera.pos.Y -= 0.009f;
                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    camera.pos.Y += 0.009f;
                }

                if (curMouse.ScrollWheelValue > prevMouse.ScrollWheelValue)
                {
                    camera.Zoom += 0.1f;
                }

                if (curMouse.ScrollWheelValue < prevMouse.ScrollWheelValue)
                {
                    camera.Zoom -= 0.1f;
                }

                for (int i = 0; i < numberOfTiles.X; i++)
                {
                    for (int j = 0; j < numberOfTiles.Y; j++)
                    {
                        if (mouseRectangle.Intersects(RectangleMap[i, j]))
                        {
                            BaseMap[i, j] = selection;
                            currentTile = new Vector2(i, j);
                        }
                        else
                        {
                            BaseMap[i, j] = baseTexture;
                        }

                       
                    }
                }

                if (curMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                {
                    ApplyTexture(Form1.listBox1.Items[Form1.listBox1.SelectedIndex].ToString(),currentTile);
                }
                mouseRectangle = new Rectangle(curMouse.X, curMouse.Y, 1, 1);
            }



            base.WndProc(ref m);
        }



        protected override void Draw()
        {
            
            GraphicsDevice.Clear(Color.Black);

            /*string message = (mouse.X + this.FindForm().PointToClient(this.Parent.PointToScreen(this.Location)).X).ToString() + "," + (mouse.Y + this.FindForm().PointToClient(this.Parent.PointToScreen(this.Location)).Y).ToString();*/
            /*string message = (mouse.X - this.FindForm().Location.X).ToString() + "," + (mouse.Y - this.FindForm().Location.Y).ToString();*/
            
            
            //string message = (curMouse.X - this.FindForm().Location.Y - this.Location.X - 7).ToString() + "," + (curMouse.Y - this.FindForm().Location.X - this.Location.Y - 30).ToString();
            
            string message = curMouse.X.ToString() + "," + curMouse.Y.ToString();
            Form1.toolStripStatusLabel1.Text = message;


            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        camera.GetTransformation(GraphicsDevice,mapResolution));
            //spriteBatch.DrawString(font, message, new Vector2(23, 23), Color.White);
            /*spriteBatch.Draw(texture, new Rectangle(100, 100, 200, 200), Color.White);*/
            
            for (int i = 0; i < numberOfTiles.X; i++)
            {
                for (int j = 0; j < numberOfTiles.Y; j++)
                {
                    
                    if (TileMap[i, j] != null)
                    {
                        spriteBatch.Draw(TileMap[i, j], RectangleMap[i, j], Color.White);
                    }
                    spriteBatch.Draw(BaseMap[i, j], RectangleMap[i, j], Color.White);
                }
            }
            
            spriteBatch.End();
            this.Invalidate();
        }

   

        public static Texture2D LoadTexture(Stream str)
        {
            return Texture2D.FromStream(graphics, str);
        }
        public void ApplyTexture(string name,Vector2 currentTile)
        {
            for (int i = 0; i < MapEditor.TileList.Count; i++)
            {
                if (TileList[i].texture.Name == name)
                {
                    TileMap[(int)currentTile.X,(int)currentTile.Y]= TileList[i].texture;
                    this.Invalidate();
                    break;
                }
                
            }
        }
    }
}
