using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Map_Editor
{
    public partial class Events : Form
    {
        public EventInfo tmpEvent = new EventInfo();
        Microsoft.Xna.Framework.Vector2 tmp;
        Event tmpEventCategory;
        public Events(Microsoft.Xna.Framework.Vector2 currentTile,Event category)
        {
            InitializeComponent();
            this.CancelButton = button2;
            tmp = currentTile;
            tmpEventCategory = category;
            textBox3.Text = tmp.X.ToString(); textBox4.Text = tmp.Y.ToString();
        }
        public Events(Event category)
        {
            InitializeComponent();
            tmpEventCategory = category;
            this.CancelButton = button2;
            textBox3.Text = tmp.X.ToString(); textBox4.Text = tmp.Y.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
        
            if (textBox1.TextLength != 0 && textBox2.TextLength != 0 && textBox3.TextLength != 0)
            {
                
                tmpEvent.name = textBox1.Text;
                tmpEvent.action = textBox2.Text;
                tmpEvent.tileIndex = tmp;
                tmpEvent.category = tmpEventCategory;
                MapEditor.GlobalEventList.Add(tmpEvent);
                UpdateList();
                this.Close();
            }
            
        }

        public static void UpdateList()
        {
            Form1.listBox4.ClearSelected();
            Form1.listBox4.Items.Clear();
            foreach (EventInfo item in MapEditor.GlobalEventList)
            {
                Form1.listBox4.Items.Add(item.name);
            }
        }



    }
}
