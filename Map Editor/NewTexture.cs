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

        public List<TileInfo> tmpList;
        public NewTexture(List<TileInfo> list)
        {
            InitializeComponent();
            tmpList = new List<TileInfo>();
            tmpList = list;
            this.CancelButton = button2;
        }

        private void NewTexture_Load(object sender, EventArgs e)
        {


            openFileDialog1.Title = "Choose an Image!";
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Images|*.png;*.jpg|All Files|*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openFileDialog1.CheckFileExists == true)
                    {
                        label4.Text = openFileDialog1.FileName;
                        pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.Load(openFileDialog1.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Choose File Properly!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                this.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.TextLength != 0)
            {
                this.Close();
                TileInfo temp = new TileInfo();
                try
                {
                    File.Copy(openFileDialog1.FileName, Path.GetDirectoryName(Application.ExecutablePath) + "\\Content\\" + openFileDialog1.SafeFileName, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Choose File Properly!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Stream str = TitleContainer.OpenStream("Content/" + openFileDialog1.SafeFileName);
                // using (MapEditor map = new MapEditor())
                //{
                temp.texture = MapEditor.LoadTexture(str);
                // }
                temp.texture.Name = textBox1.Text+Path.GetExtension(openFileDialog1.FileName);
                temp.collision = checkBox1.Checked;

                //MapEditor.TileList.Add(temp);
                tmpList.Add(temp);
            }
        }
    }
}
