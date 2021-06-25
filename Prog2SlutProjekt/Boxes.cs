using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog2SlutProjekt03
{
    public abstract class Boxes
    {
        public Rectangle box;
        public bool hasKey;
    }

    public class BlackBox : Boxes
    {
        public int keyPosition;  // The direction (key value) the player must be going when collisioning with the box to get the key
        public Rectangle keySpacing;

        public BlackBox(Rectangle box)
        {
            this.box.Location = box.Location;
            this.box.Size = box.Size;
        }

        public void checkKeySpacing()
        {
            if (keyPosition == 87)  // w
            {
                keySpacing = new Rectangle(box.Location, new Size(box.Width, box.Height * 2));
            } else if (keyPosition == 65)  // a
            {
                keySpacing = new Rectangle(box.Location, new Size(box.Width * 2, box.Height));
            } else if (keyPosition == 83)  // s
            {
                keySpacing = new Rectangle(new Point(box.X, box.Y - box.Width), new Size(box.Width, box.Height * 2));
            } else if (keyPosition == 68)  // d
            {
                keySpacing = new Rectangle(new Point(box.X - box.Height, box.Y), new Size(box.Width * 2, box.Height));
            }
        }
    }

    public class PlayerBox : Boxes
    {
        public PlayerBox(Rectangle box)
        { 
            this.box.Location = box.Location;
            this.box.Size = box.Size;
        }

        public void playerMovement(int key, Size windowSize)
        {
            if (key == 87 && box.Y > 0)  // w
            {
                box.Y += -4;
            } else if (key == 65 && box.X > 0)  // a
            {
                box.X += -4;
            } else if (key == 83 && box.Y < windowSize.Height - box.Height)  // s
            {
                box.Y += 4;
            } else if (key == 68 && box.X < windowSize.Width - box.Width)  // d
            {
                box.X += 4;
            }
        }

        // Moves one pixel back for smoothly preventing collission, as it is called every time a collision is detected
        public void collisionPrevention(int key, Size windowSize)
        {
            if (key == 87 && box.Y > 0)  // w
            {
                box.Y += 1;
            } else if (key == 65 && box.X > 0)  // a
            {
                box.X += 1;
            } else if (key == 83 && box.Y < windowSize.Height)  // s
            {
                box.Y += -1;
            } else if (key == 68 && box.X < windowSize.Width)  // d
            {
                box.X += -1;
            }
        }
    }

    public class Goal : Boxes
    {
        public Rectangle center;
        public Goal(Rectangle box)
        {
            this.box.Location = box.Location;
            this.box.Size = box.Size;
            center = new Rectangle(new Point(box.X + (box.Width / 2), box.Y + (box.Height / 2)), new Size(1, 1));
        }
    }
}
