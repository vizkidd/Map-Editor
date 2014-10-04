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

        public Form1()
        {
            InitializeComponent();

        }

        private void newMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewMap newmap = new NewMap())
            {
                newmap.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NewTexture newtexture = new NewTexture())
            {
                newtexture.ShowDialog(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
             for (int i = 0; i < MapEditor.TileList.Count; i++)
                {
                    if (MapEditor.TileList[i].texture.Name == listBox1.SelectedItem)
                    {
                        MapEditor.TileList.RemoveAt(i);
                        break;
                    }
                }
            
            listBox1.Items.Remove(listBox1.SelectedItem);
        }
        public static void UpdateList()
        {
            listBox1.ClearSelected();
            listBox1.Items.Clear();
            foreach (TileInfo item in MapEditor.TileList)
            {
                listBox1.Items.Add(item.texture.Name);
            }
        }

        private void mapEditor1_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Hide();
        }

        private void mapEditor1_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Show();
        }

    }
}
