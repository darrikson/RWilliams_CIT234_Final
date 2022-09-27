using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    public class Vacancy
    {
        public string ClientID { get; set; }
        public string RoomNumber { get; set; }
        
        public Client clientInfo;        

        public static Client VacantClient = new Client("Vacant", "Vacant", "Vacant", "Vacant", 0000000000);
        
        public Vacancy(string room)
        {            
            RoomNumber = room;
            ClientID = VacantClient.ID;
        }

        public void setClient(Client client)
        {
            ClientID = client.ID;
            clientInfo = client;
        }

        public override string ToString()
        {
            string vacancyString = $"{RoomNumber}/{ClientID}/{clientInfo}";
            return vacancyString;
        }
    }
}
