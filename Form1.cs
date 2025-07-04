﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Para_06._09
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private GamePlus gameplus;

        Timer timer1 = new Timer();

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("Crimson");
            comboBox1.Items.Add("Green");
            comboBox1.Items.Add("Blue");
            comboBox1.Items.Add("Pink");
            comboBox1.Items.Add("White");

            timer1.Tick += new EventHandler(timer1_Tick);

        }
        private void StartGame()
        {
            if (timer1.Enabled) return;

            NUDDensity.Enabled = false;
            NUDResolution.Enabled = false;
            BStart.Enabled = false;
            comboBox1.Enabled = false;
            resolution = (int)NUDResolution.Value;

            gameplus = new GamePlus
                (
                    rows: pictureBox1.Height / resolution,
                    cols: pictureBox1.Width / resolution,
                    density: (int)(NUDDensity.Minimum) + (int)(NUDDensity.Maximum) - (int)NUDDensity.Value
                );

            Text = $"Generation{gameplus.CurrentGeneration}";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void DrawNextGeneration()
        {
            var color = Brushes.Aqua;

            switch (comboBox1.SelectedIndex)
            {
                case 0: color = Brushes.Crimson; break;
                case 1: color = Brushes.Green; break;
                case 2: color = Brushes.Blue; break;
                case 3: color = Brushes.Pink; break;
                case 4: color = Brushes.White; break;
            }
            graphics.Clear(Color.Black);
            var field = gameplus.GetCurrentGeneration();
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(color, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }
            pictureBox1.Refresh();
            Text = $"Generation {gameplus.CurrentGeneration}";
            gameplus.NextGeneration();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void StopGame()
        {
            if (!timer1.Enabled) return;
            timer1.Stop();
            NUDDensity.Enabled = true;
            NUDResolution.Enabled = true;
            BStart.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled) return;
            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameplus.AddCell(x, y);
            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameplus.RemoveCell(x, y);
            }
        }

        private void BContinue_Click(object sender, EventArgs e)
        {
            resolution = (int)NUDResolution.Value;

            gameplus = new GamePlus
                (
                    rows: pictureBox1.Height / resolution,
                    cols: pictureBox1.Width / resolution,
                    density: (int)(NUDDensity.Minimum) + (int)(NUDDensity.Maximum) - (int)NUDDensity.Value
                );

            Text = $"Generation{gameplus.CurrentGeneration}";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
        }

        private void BStart_Click_1(object sender, EventArgs e)
        {
            StartGame();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Application.Restart();
            StopGame();
            StartGame();
        }

        private void BStop_Click_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            splitContainer1.Height = this.Height - 10;
            splitContainer1.Width = this.Width - 10;
            pictureBox1.Height = splitContainer1.Panel2.Height;
            pictureBox1.Width = splitContainer1.Panel2.Width;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
