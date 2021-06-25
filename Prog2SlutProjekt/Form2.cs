using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prog2SlutProjekt03
{
    public partial class Form2 : Form
    {
        DatabaseOperations DatabaseOperation = new DatabaseOperations();
        List<Player> playerList = new List<Player>();
        List<Label> labels = new List<Label>();

        public Form2()
        {
            InitializeComponent();
        }

        private string generateLabelText(int i)
        {
            string generatedText = "Name: " + playerList[i].getName() + " \t| \tTime: " + playerList[i].getTime() + "\n";

            return (generatedText);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            playerList = DatabaseOperation.createLeaderboard();

            for (int i = 0; i < playerList.Count; i++)
            {
                labels.Add(new Label());
                
                flowLayoutPanel1.Controls.Add(labels[i]);

                string labelText = generateLabelText(i);
                labels[i].AutoSize = true;
                labels[i].Text = labelText;
                labels[i].Size = new Size(250, 15);
                labels[i].Location = new Point(3, i * labels[i].Size.Height);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
