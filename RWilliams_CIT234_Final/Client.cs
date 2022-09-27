using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    public class Client : IComparable
    {
        public string ID;        
        public string FirstName;        
        public string LastName;        
        public string Address;        
        public long PhoneNumber;        
        public Stack<string> guestPrizes;

        public Client(string id, string fname, string lname, string addy, long phone)
        {
            ID = id;
            FirstName = fname;
            LastName = lname;
            Address = addy;
            PhoneNumber = phone;
            guestPrizes = new Stack<string>();
        }

        public string getDoorPrizes()
        {            
            string prizes = String.Empty;
            if (guestPrizes.Count == 0)
            {
                prizes = "No Door Prizes";
                return prizes;
            }
            else
            {
                foreach (var item in guestPrizes)
                {
                    if (guestPrizes.Count > 1)
                    {
                        prizes += item + "/";
                    }
                    else
                    {
                        prizes = item;
                    }
                }
                return prizes;
            }
            
            
            
        }
        public void addDoorPrize(string prize)
        {
            guestPrizes.Push(prize);
        }

        public override string ToString()
        {
            string client = $"{ID}/{FirstName}/{LastName}/{Address}/{PhoneNumber}/{getDoorPrizes()}";
            return client;            
        }

        public int CompareTo(object compareClient)
        {
            return string.Compare(ID, ((Client)compareClient).ID);
        }
    }
}
