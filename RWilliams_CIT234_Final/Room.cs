using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    public abstract class Room : IComparable
    {
        public string RoomNumber;        
        public bool Balcony;
        public bool DownForRepair;
        public int NumberOfBeds;        
        public bool Vacant = true;
        public Vacancy vacancy;               

        public abstract void makeVacant();

        public int CompareTo(object compareRoom)
        {
            return string.Compare(RoomNumber, ((Room)compareRoom).RoomNumber);
        }

        public override string ToString()
        {
            string props = RoomNumber + "/" + Balcony + "/" + DownForRepair + "/" + 
                NumberOfBeds + "/" + Vacant;
            return props;
        }
        
    }
}
