using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog2SlutProjekt03
{
    public class Player
    {
        string name, time;
        public Player(string name, string time)
        {
            this.name = name;

            this.time = time;
        }

        public string getName()
        {
            return (name);
        }

        public string getTime()
        {
            return (time);
        }
    }
}
