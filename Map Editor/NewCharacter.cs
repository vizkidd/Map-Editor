using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Map_Editor
{
    public partial class NewCharacter : Form
    {
        public NewCharacter()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CancelButton = button2;
        }

        private void NewEnemy_Load(object sender, EventArgs e)
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
                        label1.Text = openFileDialog1.FileName;
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
            CharacterInfo temp = new CharacterInfo();
            if (this.textBox1.TextLength != 0)
            {
                if (openFileDialog1.CheckFileExists == true)
                {
                    this.Close();
                    try
                    {
                        File.Copy(openFileDialog1.FileName, Path.GetDirectoryName(Application.ExecutablePath) + "\\Content\\" + openFileDialog1.SafeFileName, true);
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Choose File Properly!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                        Stream str = TitleContainer.OpenStream("Content/" + openFileDialog1.SafeFileName);
                        temp.category = (Character)comboBox1.SelectedIndex;
                        temp.name = textBox1.Text + Path.GetExtension(openFileDialog1.FileName);
                        temp.texture = MapEditor.LoadTexture(str);
                        MapEditor.GlobalCharacterList.Add(temp);
                        Form1.listBox3.Items.Add(temp.name);
                    

                     
                }
            }
        }
    }
}
