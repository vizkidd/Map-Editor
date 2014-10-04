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
    public struct TileInfo
    {
        public Texture2D texture;
        public bool collision;
        public uint index;
    }
    public struct CharacterInfo
    {
        public string name;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 startingPosition;
        public Character category;
    }
    public struct EventInfo
    {
        public object variable, value;
        public Vector2 tileIndex;
        public string name, action;
        public float time;
        public Event category;
    }
    public enum Event { General = 0, Player = 1, Characters = 2 };
    public enum Character { Player = 2, Enemy = 0, NPC = 1 };
    class MapEditor : GraphicDeviceControl
    {
        //ContentBuilder contentBuilder;
        ContentManager content;
        public static string mapName="null";
        Texture2D baseTexture,selection;
        SpriteBatch spriteBatch;
        SpriteFont font;
        public static Viewport viewport;
        Camera camera;
        KeyboardState previousKeyboardState, curKerboardState;
        MouseState curMouse,prevMouse;
        Rectangle mouseRectangle;
        public static GraphicsDevice graphics;
        public static bool active;
        int initialize = 0;
        Texture2D eventTexture,charTexture;
        public static List<TileInfo> TileList;
        public static List<TileInfo> ObjectList;
        public static List<CharacterInfo> GlobalCharacterList;
        public List<CharacterInfo> LocalCharacterList;
        public static List<EventInfo> GlobalEventList;
        public static CharacterInfo player;
        Vector2 mapResolution=new Vector2(1000,1000);
        public static Vector2 numberOfTiles = new Vector2(50, 50);
        public Vector2 currentTile;
        public int tileWidth, tileHeight;
        public Texture2D[,] BaseMap;
        public static Texture2D[,] TileMap;
        public static Texture2D[,] ObjectMap;
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
            camera.pos.X++;
            camera.Zoom++;
            //wall = new TileInfo();
            TileList = new List<TileInfo>();
            ObjectList = new List<TileInfo>();
            GlobalCharacterList = new List<CharacterInfo>();
            LocalCharacterList = new List<CharacterInfo>();
            GlobalEventList = new List<EventInfo>();
            TileMap = new Texture2D[(int)numberOfTiles.X, (int)numberOfTiles.Y];
            BaseMap = new Texture2D[(int)numberOfTiles.X, (int)numberOfTiles.Y];
            ObjectMap = new Texture2D[(int)numberOfTiles.X, (int)numberOfTiles.Y];
            RectangleMap = new Rectangle[(int)numberOfTiles.X, (int)numberOfTiles.Y];
            font = content.Load<SpriteFont>("hudFont");
            baseTexture = content.Load<Texture2D>("base");
            selection = content.Load<Texture2D>("select");
            eventTexture = content.Load<Texture2D>("event");
            charTexture = content.Load<Texture2D>("character");
           for (int i = 0; i < numberOfTiles.X; i++)
            {
                for (int j = 0; j < numberOfTiles.Y; j++)
                {
                    TileMap[i, j] = null;
                    ObjectMap[i, j] = null;
                    BaseMap[i, j] = content.Load<Texture2D>("base");
                    RectangleMap[i, j] = new Rectangle((int)i * tileWidth, (int)j * tileHeight, tileWidth, tileHeight);
                }

            }
 
           initialize = 1;
           camera.Zoom = 1.0f;
           camera.pos.X++;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (initialize == 1)
            {
                camera.Update(GraphicsDevice);
                if (active)
                {
                    //Microsoft.Xna.Framework.Input.Mouse.WindowHandle = Handle;
                    
                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    camera.pos.X += 0.09f;
                    this.Invalidate();
                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    camera.pos.X -= 0.09f;
                    this.Invalidate();
                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    camera.pos.Y -= 0.09f;
                    this.Invalidate();
                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    camera.pos.Y += 0.09f;
                    this.Invalidate();
                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Z))
                {
                    camera.Zoom += 0.001f;
                    this.Invalidate();

                }

                if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    camera.Zoom -= 0.001f;
                    this.Invalidate();
                }

                    prevMouse = curMouse;
                    curMouse = Mouse.GetState();
                    curKerboardState = previousKeyboardState;

                    curKerboardState = Keyboard.GetState();

                    /*if (curMouse.X <= camera.pos.X + viewport.Width && curMouse.X <= camera.pos.Y + viewport.Height)
                    {
                        mouseRectangle.X += (int)camera.pos.X;
                        mouseRectangle.Y += (int)camera.pos.Y;
                    }*/


                    CalculateCurrentTile();


                    if (curKerboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G) && previousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.G))
                    {
                        camera.GoTo(mouseRectangle);
                        spriteBatch.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       null,
                       null,
                       null,
                       null,
                       camera.transform);
                        spriteBatch.End();
                        Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)currentTile.X * (int)tileWidth, (int)tileHeight * (int)currentTile.Y);
                    }

                    if (curMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[0] && Form1.listBox1.SelectedIndex != -1 && Form1.checkBox1.CheckState == CheckState.Checked)
                    {
                        ApplyTexture(ref TileMap,MapEditor.TileList,Form1.listBox1.Items[Form1.listBox1.SelectedIndex].ToString(), currentTile);
                        Invalidate();
                    }
                    if (curMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[0] && Form1.listBox1.SelectedIndex != -1 && Form1.checkBox1.CheckState == CheckState.Checked)
                    {
                        RemoveTexture(ref TileMap,currentTile);
                        Invalidate();
                    }


                    if (curMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[1] && Form1.listBox2.SelectedIndex != -1 && Form1.checkBox2.CheckState == CheckState.Checked)
                    {
                        ApplyTexture(ref ObjectMap, MapEditor.ObjectList, Form1.listBox2.Items[Form1.listBox2.SelectedIndex].ToString(), currentTile);
                        Invalidate();
                    }
                    if (curMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[1] && Form1.listBox2.SelectedIndex != -1 && Form1.checkBox2.CheckState == CheckState.Checked)
                    {
                        RemoveTexture(ref ObjectMap, currentTile);
                        Invalidate();
                    }

                    if (curMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[4] && Form1.checkBox5.CheckState == CheckState.Checked)
                    {
                        using (NewEvent newEvent = new NewEvent(currentTile,Event.General))
                        {
                            newEvent.ShowDialog();
                        }
                        Invalidate();
                    }
                    if (curMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[2] && Form1.listBox3.SelectedIndex != -1 && Form1.checkBox4.CheckState == CheckState.Checked)
                    {
                        foreach (CharacterInfo item in GlobalCharacterList)
                        {
                            if ((string)Form1.listBox3.SelectedItem == item.name)
                            {
                                CharacterInfo tmpcharinfo=new CharacterInfo();
                                tmpcharinfo=item;
                                tmpcharinfo.position=currentTile;
                                tmpcharinfo.startingPosition=currentTile;
                                LocalCharacterList.Add(tmpcharinfo);
                               
                            }

                        }
                        Invalidate();
                    }
                    if (curMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[2] && Form1.checkBox4.CheckState == CheckState.Checked)
                    {
                        for (int i = 0; i < LocalCharacterList.Count; i++)
                        {
                            if (currentTile == LocalCharacterList[i].position)
                            {
                                LocalCharacterList.Remove(LocalCharacterList[i]);
                            }
                        }

                    }

                    if(curMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && prevMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Form1.tabControl1.SelectedTab == Form1.tabControl1.TabPages[3])
                    {
                        //Form1.textBox1.Text = currentTile.X.ToString() + "," + currentTile.Y.ToString();
                        player.position = player.startingPosition = currentTile;
                    }
                    mouseRectangle = new Rectangle(curMouse.X, curMouse.Y, 1, 1);
                    if (curMouse.X < GraphicsDevice.Viewport.X || curMouse.Y < GraphicsDevice.Viewport.Y)
                    {
                        Mouse.SetPosition(curMouse.X + 10, curMouse.Y + 10);
                    }
                    mouseRectangle.X = (int)MathHelper.Clamp(mouseRectangle.X, (int)camera.pos.X, mapResolution.X);
                    mouseRectangle.Y = (int)MathHelper.Clamp(mouseRectangle.Y, (int)camera.pos.Y, mapResolution.Y);
                }

            }
            //Invalidate();
            base.WndProc(ref m);
        }



        protected override void Draw()
        {
            
            GraphicsDevice.Clear(Color.Black);

            /*string message = (mouse.X + this.FindForm().PointToClient(this.Parent.PointToScreen(this.Location)).X).ToString() + "," + (mouse.Y + this.FindForm().PointToClient(this.Parent.PointToScreen(this.Location)).Y).ToString();*/
            /*string message = (mouse.X - this.FindForm().Location.X).ToString() + "," + (mouse.Y - this.FindForm().Location.Y).ToString();*/
            
            
            //string message = (curMouse.X - this.FindForm().Location.Y - this.Location.X - 7).ToString() + "," + (curMouse.Y - this.FindForm().Location.X - this.Location.Y - 30).ToString();

            string message = "Mouse : " + curMouse.X.ToString() + "," + curMouse.Y.ToString() + "|" + "Tile : " + currentTile.X.ToString() + "," + currentTile.Y.ToString();
            
            
            Form1.toolStripStatusLabel1.Text = message;
            

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        camera.transform);
            //spriteBatch.DrawString(font, message, new Vector2(23, 23), Color.White);
            /*spriteBatch.Draw(texture, new Rectangle(100, 100, 200, 200), Color.White);*/
            
            for (int i = 0; i < numberOfTiles.X; i++)
            {
                for (int j = 0; j < numberOfTiles.Y; j++)
                {

                    spriteBatch.Draw(selection,new Rectangle((int)curMouse.X,(int)curMouse.Y,tileWidth/2,tileHeight/2),Color.White);
                    if (Form1.checkBox5.CheckState == CheckState.Checked)
                    {
                        foreach (EventInfo item in GlobalEventList)
                        {

                            if (item.tileIndex.X == i && item.tileIndex.Y == j)
                            {
                                BaseMap[i, j] = eventTexture;
                            }
                        }
                    }
                    
                     spriteBatch.Draw(BaseMap[i, j], RectangleMap[i, j], Color.White); 
                    if (Form1.checkBox1.CheckState == CheckState.Checked)
                    {
                        if (TileMap[i, j] != null)
                        {
                            spriteBatch.Draw(TileMap[i, j], RectangleMap[i, j], Color.White);
                        }
                    }
                    if (Form1.checkBox2.CheckState == CheckState.Checked)
                    {
                        if (ObjectMap[i, j] != null)
                        {
                            spriteBatch.Draw(ObjectMap[i, j], RectangleMap[i, j], Color.White);
                        }
                    }
                    if (Form1.checkBox4.CheckState == CheckState.Checked)
                    {
                        foreach (CharacterInfo ch in LocalCharacterList)
                        {
                            spriteBatch.Draw(ch.texture, new Rectangle((int)ch.position.X*tileWidth, (int)ch.position.Y* tileHeight, tileWidth, tileHeight), Color.White);
                        }
                    }
                    if (Form1.checkBox3.CheckState == CheckState.Checked)
                    {
                        if(player.texture!= null && player.position != Vector2.Zero)
                        spriteBatch.Draw(player.texture, player.position, Color.White);
                    }
                }
            }
        

            spriteBatch.End();
        }

   

        public static Texture2D LoadTexture(Stream str)
        {
            return Texture2D.FromStream(graphics, str);
        }
        public void ApplyTexture(ref Texture2D[,] texture,List<TileInfo> list,string name,Vector2 currentTile)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].texture.Name == name)
                {
                    texture[(int)currentTile.X,(int)currentTile.Y]= list[i].texture;
                    //Invalidate();
                    break;
                }
                
            }
        }
        public void RemoveTexture(ref Texture2D[,] texture,Vector2 currentTile)
        {

            texture[(int)currentTile.X, (int)currentTile.Y] = null;
            //Invalidate();

        }

        public void CreateEvent(Event category)
        {
            using (Map_Editor.NewEvent events = new Map_Editor.NewEvent(category))
            {
                events.textBox3.Enabled = true;
                events.textBox3.ReadOnly = false;
                events.textBox4.Enabled = true;
                events.textBox4.ReadOnly = false;
                events.tmpEvent.tileIndex = currentTile;
                events.ShowDialog();
            }


            
        }

        public void CalculateCurrentTile()
        {

            for (int i = 0; i < numberOfTiles.X; i++)
            {
                for (int j = 0; j < numberOfTiles.Y; j++)
                {
                    if (mouseRectangle.Intersects(RectangleMap[i, j]))
                    {
                        //this.Invalidate();
                        BaseMap[i, j] = selection;
                        currentTile = new Vector2(i, j);
                        this.Invalidate();
                    }
                    else
                    {
                        BaseMap[i, j] = baseTexture;
                        this.Invalidate();
                    }


                }
            }
        }

        public void RemoveUnreferredTextures(Texture2D[,] texture,string name)
        {
            if (texture != null)
            {
                for (int i = 0; i < numberOfTiles.X; i++)
                {
                    for (int j = 0; j < numberOfTiles.Y; j++)
                    {
                        if (texture[i, j] != null)
                        {
                            if (texture[i, j].Name == name)
                            {
                                texture[i, j] = null;
                                
                            }
                        }
                    }

                }
            }
            Invalidate();
        }

    }
}
