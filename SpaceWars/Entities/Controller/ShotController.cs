using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars.Entities.Controller
{
    public class ShotController
    {
        public List<Shot> ShotsList { get; set; }
        public List<Shot> shotsToRemove { get; set; }
        public ShotController()
        {
            this.ShotsList = new List<Shot>();
            this.shotsToRemove = new List<Shot>();
        }

        public void Add(Shot shot)
        {
            this.ShotsList.Add(shot);
        }

        public void Remove(Shot shot)
        {
            shotsToRemove.Add(shot);
        }

        public void DisposeShots()
        {
            ShotsList.RemoveAll(x => shotsToRemove.Contains(x));
        }
    }
}
