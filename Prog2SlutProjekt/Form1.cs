using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace Prog2SlutProjekt03
{
    public partial class Form1 : Form
    {
        // Object that decides properties of everything
        private static BlackBox referenceBox = new BlackBox(new Rectangle(new Point(0, 0), new Size(25, 25)));

        Stopwatch stopwatch;
        List<BlackBox> blackBoxesList = new List<BlackBox>();
        List<PictureBox> blackBoxes = new List<PictureBox>();
        PlayerBox player = new PlayerBox(new Rectangle(referenceBox.box.Location, new Size(referenceBox.box.Width - (referenceBox.box.Width / 6), referenceBox.box.Height - (referenceBox.box.Height / 6))));
        PictureBox playerBox = new PictureBox();
        PictureBox goalBox = new PictureBox();
        PictureBox keyCollisionBox;
        DatabaseOperations DatabaseOperations = new DatabaseOperations();
        Goal goal;

        Random rnd = new Random();

        bool gameActive = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void clearBlackBoxes()
        {
            foreach (var square in blackBoxes)
            {
                Controls.Remove(square);
            }

            blackBoxes.Clear();
            blackBoxesList.Clear();

        }

        private Point boxLocation(Size windowSize, List<Point> positionsCheck)
        {
            Point location = new Point();

            do
            {
                location.X = (rnd.Next(1, 12)) * (windowSize.Width - (3 * player.box.Width)) / 12;
                location.Y = (rnd.Next(1, 12)) * (windowSize.Height - (3 * player.box.Height)) / 12;
            } while (positionsCheck.Contains(location) && player.box.Location == location);

            positionsCheck.Add(location);

            return location;
        }

        private void initializeGame(int squareCount, bool keyColor)
        {
            clearBlackBoxes();
            keyCollisionBox = new PictureBox();
            stopwatch = new Stopwatch();

            Size windowSize = ClientSize;
            int[] keys = new int[] { 87, 65, 83, 68 };  // keys "w a s d" values
            goal = new Goal(new Rectangle(new Point(windowSize.Width - referenceBox.box.Width * 2, windowSize.Height - referenceBox.box.Height * 2), referenceBox.box.Size));

            List<Point> positionsCheck = new List<Point>();

            label2.Visible = false;
            label3.Text = textBox1.Text;
            label3.Location = new Point(label3.Height, windowSize.Height - label3.Height * 2);
            label4.Visible = false;
            textBox1.Visible = false;
            button4.Visible = false;

            for (int i = 0; i < squareCount; i++)
            {
                blackBoxes.Add(new PictureBox());
            }

            // Generate all black boxes with all requred PictureBox values
            int index = 0;
            foreach (var square in blackBoxes)
            {
                Controls.Add(square);
                square.Size = referenceBox.box.Size;
                square.Location = boxLocation(windowSize, positionsCheck);
                blackBoxesList.Add(new BlackBox(new Rectangle(square.Location, referenceBox.box.Size)));
                square.BackColor = Color.Black;
                square.BorderStyle = BorderStyle.None;
                square.TabIndex = 1;
                square.TabStop = false;

                // Give the first black box the key
                if (index == 0)
                {
                    blackBoxesList[0].hasKey = true;
                    blackBoxesList[0].keyPosition = keys[rnd.Next(0, 4)];
                    if (keyColor)
                    {
                        square.BackColor = Color.Pink;
                    }
                    index++;
                }
            }

            // Check so that the key has space for the player. If not, re-roll direction of the key and check again
            int retries = 0;
            restart:
            blackBoxesList[0].checkKeySpacing();
            for (int i = 1; i < blackBoxesList.Count; i++)
            {
                if (retries == 6)
                {
                    blackBoxes[0].Location = boxLocation(windowSize, positionsCheck);
                    blackBoxesList[0].box.Location = blackBoxes[0].Location;
                    retries = 0;
                    goto restart;
                } else if (blackBoxesList[0].keySpacing.IntersectsWith(blackBoxesList[i].box))
                {
                    blackBoxesList[0].keyPosition = keys[rnd.Next(0, 4)];

                    retries++;
                    goto restart;
                }
            }

            // Create the Player with all its required PictureBox values
            Controls.Add(playerBox);
            playerBox.Size = player.box.Size;
            playerBox.Location = player.box.Location;
            playerBox.BackColor = Color.Red;
            playerBox.BorderStyle = BorderStyle.None;
            playerBox.TabIndex = 1;
            playerBox.TabStop = false;

            // Create the Goal with all its required PictureBox values
            Controls.Add(goalBox);
            goalBox.Size = goal.box.Size;
            goalBox.Location = goal.box.Location;
            goalBox.BackColor = Color.Lime;
            goalBox.BorderStyle = BorderStyle.None;
            goalBox.TabIndex = 0;
            goalBox.TabStop = false;

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;

            // Start stopwatch for score
            stopwatch.Start();
            gameActive = true;  // Activate movement
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initializeGame(10, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            initializeGame(18, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            initializeGame(20, false);
        }

        // Player Movement - Key down function
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameActive)
            {
                Size windowSize = ClientSize;
                int[] keyCharacters = new int[] { 87, 65, 83, 68 };
                bool keyPressed = false;

                foreach (var key in keyCharacters)
                {
                    if (e.KeyValue == key)
                    {
                        keyPressed = true;
                        break;
                    }
                }

                if (keyPressed)
                {
                    player.playerMovement(e.KeyValue, windowSize);
                }

                for (int i = 0; i < blackBoxes.Count; i++)
                {
                    bool collision = player.box.IntersectsWith(blackBoxesList[i].box);
                    if (collision)
                    {
                        if (blackBoxesList[i].hasKey && e.KeyValue == blackBoxesList[i].keyPosition)
                        {
                            player.hasKey = true;
                            blackBoxesList[i].hasKey = false;
                            blackBoxes[0].BackColor = Color.DarkGray;
                            label1.Visible = true;
                        }

                        while (collision)
                        {
                            player.collisionPrevention(e.KeyValue, windowSize);
                            collision = player.box.IntersectsWith(blackBoxesList[i].box);  // Update collision bool to try and break out of the while loop
                        }
                    }
                }

                if (keyPressed)
                {
                    playerBox.Location = player.box.Location;
                    Invalidate();
                }

                if (player.box.IntersectsWith(goal.center) && player.hasKey)
                {
                    stopwatch.Stop();
                    player.hasKey = false;
                    label1.Visible = false;

                    TimeSpan ts = stopwatch.Elapsed;

                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    label4.Text = "Found the key and entered the goal at time: " + elapsedTime;
                    label4.Visible = true;

                    DatabaseOperations.updateLeaderboard(textBox1.Text, elapsedTime);

                    button4.Visible = true;
                    label2.Visible = true;
                    textBox1.Visible = true;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }
    }
}
