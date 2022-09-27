using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    public class Suite : Room
    {
        public int NumberOfRooms;        

        public Suite() { }
        public Suite(string roomNumber, bool balcony, bool repairs, int beds, int rooms)
        {
            RoomNumber = roomNumber;
            Balcony = balcony;
            DownForRepair = repairs;
            NumberOfBeds = beds;
            NumberOfRooms = rooms;
            vacancy = new Vacancy(roomNumber);
            makeVacant();
        }

        public override void makeVacant()
        {
            if (Vacant)
            {
                vacancy.setClient(Vacancy.VacantClient);
            }
        }

        public override string ToString()
        {
            return base.ToString() + "/" + NumberOfRooms + "/" + vacancy;
        }
    }
}
