using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWilliams_CIT234_Final
{
    public class BasicRoom : Room
    {
        public bool Smoking;        
        public BasicRoom() { }
        public BasicRoom(string roomNumber, bool balcony, bool repairs, int beds, bool smoking)
        {
            RoomNumber = roomNumber;
            Balcony = balcony;
            DownForRepair = repairs;
            NumberOfBeds = beds;
            Smoking = smoking;
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
            return base.ToString() + "/" + Smoking + "/" + vacancy;
        }
    }
}
