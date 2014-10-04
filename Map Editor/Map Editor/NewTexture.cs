using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace Map_Editor
{
    public partial class NewTexture : Form
    {
        public NewTexture()
        {
            InitializeComponent();
            CancelButton = button2;
        }

        private void NewTexture_Load(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label4.Text = openFileDialog1.FileName;
                pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Load(openFileDialog1.FileName);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            TileInfo temp=new TileInfo();

            File.Copy(openFileDialog1.FileName, Path.GetDirectoryName(Application.ExecutablePath)+"\\Content\\"+openFileDialog1.SafeFileName,true);

            Stream str = TitleContainer.OpenStream("Content/" + openFileDialog1.SafeFileName);
           // using (MapEditor map = new MapEditor())
            //{
                temp.texture = MapEditor.LoadTexture(str);
           // }
            temp.texture.Name = textBox1.Text;
            temp.collision = checkBox1.Checked;
            
            MapEditor.TileList.Add(temp);
            Form1.UpdateList();
        }
    }
}
