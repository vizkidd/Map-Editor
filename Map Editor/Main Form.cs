using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;

namespace Map_Editor
{
    public partial class Form1 : Form
    {
        Map map = new Map();

        public Form1()
        {
            InitializeComponent();

        }

        private void newMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewMap newmap = new NewMap())
            {
                newmap.ShowDialog();
                //reset
                MapEditor.mapName = newmap.textBox7.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NewTexture newtexture = new NewTexture(MapEditor.TileList))
            {
                newtexture.ShowDialog(this);
                Form1.UpdateList(ref MapEditor.TileList,listBox1);
            }
            mapEditor1.Invalidate();
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
             for (int i = 0; i < MapEditor.TileList.Count; i++)
                {
                    if (MapEditor.TileList[i].texture.Name == (string)listBox1.SelectedItem)
                    {
                        mapEditor1.RemoveUnreferredTextures(MapEditor.TileMap,MapEditor.TileList[i].texture.Name);
                        MapEditor.TileList.RemoveAt(i);
                 
                        break;
                    }

                }
            
            listBox1.Items.Remove(listBox1.SelectedItem);
            mapEditor1.Invalidate();
        }
        public static void UpdateList(ref List<TileInfo> mainList,ListBox listBox)
        {
            listBox.ClearSelected();
            listBox.Items.Clear();
            foreach (TileInfo item in mainList)
            {
                listBox.Items.Add(item.texture.Name);
            }
           
            
        }

        private void mapEditor1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
         
            MapEditor.active = true;
            mapEditor1.Invalidate();
        }

        private void mapEditor1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
            MapEditor.active = false;
            mapEditor1.Invalidate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (NewTexture newtexture = new NewTexture(MapEditor.ObjectList))
            {
                newtexture.ShowDialog(this);
                Form1.UpdateList(ref MapEditor.ObjectList,listBox2);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MapEditor.ObjectList.Count; i++)
            {
                if (MapEditor.ObjectList[i].texture.Name == (string)listBox2.SelectedItem)
                {
                    mapEditor1.RemoveUnreferredTextures(MapEditor.ObjectMap,MapEditor.ObjectList[i].texture.Name);
                    MapEditor.ObjectList.RemoveAt(i);

                    break;
                }

            }

            listBox2.Items.Remove(listBox2.SelectedItem);
            mapEditor1.Invalidate();
        }

        private void checkBox1_checkedChangeed(object sender, EventArgs e)
        {
            mapEditor1.Invalidate();
        }
        private void checkBox5_checkedChangeed(object sender, EventArgs e)
        {
            mapEditor1.Invalidate();
        }
        private void checkBox2_checkedChangeed(object sender, EventArgs e)
        {
            mapEditor1.Invalidate();
        }

         private void button11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MapEditor.GlobalEventList.Count; i++)
            {
                if (MapEditor.GlobalEventList[i].name == (string)listBox4.SelectedItem)
                {
                    MapEditor.GlobalEventList.RemoveAt(i);
                    break;
                }

            }

            listBox4.Items.Remove(listBox4.SelectedItem);
            
            mapEditor1.Invalidate();
        }

         private void button8_Click(object sender, EventArgs e)
         {
             using (NewCharacter character = new NewCharacter())
             {
                 character.ShowDialog();
             }
         }


         private void button3_Click(object sender, EventArgs e)
         {
             using (NewCharacter c=new NewCharacter())
             {
                 CharacterInfo ch = new CharacterInfo();
                 c.comboBox1.Enabled = false;
                 c.comboBox1.Visible = false;
                 c.label3.Visible = false;
                 c.label3.Enabled = false;
                 c.ShowDialog();
                 ch.name = c.textBox1.Text;
                 ch.category = Character.Player;
                 MapEditor.player = ch;
             }
         }
         private void button9_Click(object sender, EventArgs e)
         {
             int j = mapEditor1.LocalCharacterList.Count - 1;
             while (j >= 0)
             {
                 if (mapEditor1.LocalCharacterList[j].name == (string)listBox3.SelectedItem)
                 {
                     mapEditor1.LocalCharacterList.RemoveAt(j);

                 }
                 j--;
             }
             for (int i = 0; i < MapEditor.GlobalCharacterList.Count; i++)
             {
                 if (MapEditor.GlobalCharacterList[i].name == (string)listBox3.SelectedItem)
                 {
                     MapEditor.GlobalCharacterList.RemoveAt(i);
                 }

             }

             listBox3.Items.Remove(listBox3.SelectedItem);
             mapEditor1.Invalidate();
         }

         private void button5_Click(object sender, EventArgs e)
         {
             mapEditor1.CreateEvent(Event.Characters);
         }
         private void button4_Click(object sender, EventArgs e)
         {
             mapEditor1.CreateEvent(Event.Player);
         }
    }
}
