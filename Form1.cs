namespace Picture_Viewer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

   
    public partial class Form1 : Form
    {
        // List to store pictures paths.
        
        internal List<string> filenames = new List<string>();

        // List to store pictures names only
        
        internal List<string> Picnames = new List<string>();

        // Public text box to show startup messages
        
        internal TextBox Message = new TextBox();

        public Form1()
        {
            InitializeComponent();

            Message.Text = "WELCOME  Please import some images";
            Message.Size = panel1.Size;
            panel1.Controls.Add(Message);
        }

        // Browse images button
        private void button1_Click(object sender, EventArgs e)
        {
            // Import images from windows
            try
            {
                using (OpenFileDialog fileD = new OpenFileDialog()
                {
                    Title = "Select a picture file",
                    Multiselect = true,
                    ValidateNames = true,
                    Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|All files (*.*)|*.*"
                })
                {
                    if (fileD.ShowDialog() == DialogResult.OK)
                    {
                        filenames.Clear();
                        listBox1.Items.Clear();
                        foreach (string fileName in fileD.FileNames)
                        {
                            FileInfo fName = new FileInfo(fileName);
                            filenames.Add(fName.FullName);
                            Picnames.Add(fName.Name);
                            listBox1.Items.Add(fName.Name);
                        }

                    }
                }
                Message.Text = "SELECT Images and Mode";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message);
            }
        }

        // Public picturebox object to ease use in slideshow and single
        internal PictureBox picbox1 = new PictureBox();

        // Single mode
        private void singleModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Uncheck another options
            UnCheck(singleModeToolStripMenuItem, slidshowToolStripMenuItem, multiPicturesToolStripMenuItem);

            if (listBox1.Items.Count == 0)
            {
                singleModeToolStripMenuItem.Checked = false;
                MessageBox.Show("Please Import images first");
            }
            else if (listBox1.SelectedItems.Count > 1)
                MessageBox.Show("Please choose only one");
            else
            {
                try
                {   // Clear panel from previous controls.
                    panel1.Controls.Clear();
                    // Deploy pic and add in control
                    picbox1.Size = panel1.Size;
                    picbox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    picbox1.Image = new Bitmap(Image.FromFile(filenames[listBox1.SelectedIndex]));
                    panel1.Controls.Add(picbox1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Pick a Picture please");
                    singleModeToolStripMenuItem.Checked = false;
                }
            }
        }

        // Slideshow mode
        private void slidshowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Uncheck another options
            UnCheck(slidshowToolStripMenuItem, singleModeToolStripMenuItem, multiPicturesToolStripMenuItem);

            if (listBox1.Items.Count == 0)
            {
                slidshowToolStripMenuItem.Checked = false;
                MessageBox.Show("Please Import images first");
            }
            else
            {   // Clear panel from previous controls.
                panel1.Controls.Clear();
                //Initalize timer
                toolStripStatusLabel1.Visible = true;
                i = 0;
                timer1.Interval = 100;
                timer1.Start();
                toolStripStatusLabel1.BackColor = Color.Cornsilk;
            }
        }


        internal int i = 0;

        /// The timer1_Tick.
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i < filenames.Count)
            { //Deploy pic and add in control
                picbox1.Size = panel1.Size;
                picbox1.Image = new Bitmap(Image.FromFile(filenames[i]));
                picbox1.SizeMode = PictureBoxSizeMode.StretchImage;
                panel1.Controls.Add(picbox1);
                toolStripStatusLabel1.Text = Picnames[i];
                i++;
            }
            else { timer1.Stop(); }
        }

        // MultiPictures mode
        private void multiPicturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Uncheck another options
            UnCheck(multiPicturesToolStripMenuItem, slidshowToolStripMenuItem, singleModeToolStripMenuItem);

            panel1.AutoScroll = true;
            ListBox.SelectedIndexCollection indexes = this.listBox1.SelectedIndices;
            if (listBox1.Items.Count == 0)
            {
                multiPicturesToolStripMenuItem.Checked = false;
                MessageBox.Show("Please Import images first");
            }
            else if (indexes.Count == 0)
            {
                multiPicturesToolStripMenuItem.Checked = false;
                MessageBox.Show("Pick at LEAST 1 picture");
            }
            else
            {   // Clear panel from previous controls.
                panel1.Controls.Clear();
                //Deploy Picturebox array
                PictureBox[] boxes = new PictureBox[indexes.Count];
                int y = 0, x = 0;

                for (int ind = 0; ind < boxes.Length; ind++, x++)
                {
                    // Add the index[i] pic in the control
                    boxes[ind] = new PictureBox();
                    panel1.Controls.Add(boxes[ind]);
                    if (ind % 3 == 0 && ind != 0)
                    {
                        y = y + (panel1.Width / 3) + 20; //to make 3 images per rows
                        x = 0;
                    }
                    // assign the size and location of the picturebox
                    boxes[ind].Location = new Point(x * (panel1.Width / 3), y);
                    boxes[ind].Size = new Size((panel1.Width / 3) - 30, (panel1.Width / 3));
                    boxes[ind].SizeMode = PictureBoxSizeMode.StretchImage;
                    boxes[ind].Image = new Bitmap(Image.FromFile(filenames[indexes[ind]]));
                }
            }
        }

        // Exit option
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Make tool strip clear>
        private void MODES_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        // Handle form resize
        private void Form1_Resize(object sender, EventArgs e)
        {

            if (multiPicturesToolStripMenuItem.Checked)
            {
                multiPicturesToolStripMenuItem.PerformClick(); multiPicturesToolStripMenuItem.Checked = true;
            }
            else if (singleModeToolStripMenuItem.Checked)
            {
                singleModeToolStripMenuItem.PerformClick(); singleModeToolStripMenuItem.Checked = true;
            }
            else if (slidshowToolStripMenuItem.Checked)
            {
                slidshowToolStripMenuItem.PerformClick(); slidshowToolStripMenuItem.Checked = true;
            }
            else { panel1.Controls.Clear(); panel1.Controls.Add(Message); }
        }

        // Uncheck unused Menu Item
        private void UnCheck(ToolStripMenuItem check, ToolStripMenuItem uncheck1, ToolStripMenuItem uncheck2)
        {
            check.Checked = true;
            uncheck1.Checked = false;
            uncheck2.Checked = false;
        }
    }
}
