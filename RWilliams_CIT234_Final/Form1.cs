using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    public partial class Form1 : Form
    {
        public Room holder;
        public ArrayList rooms = new ArrayList();
        public ArrayList clients = new ArrayList();
        public static char separators = '/' ;
        public static int roomsIndex = 0;
        public static int clientIndex = 0;
        public static Stack<Room> booked = new Stack<Room>();
        public static Queue<Room> vacant = new Queue<Room>();
        public static Random rand = new Random();
        public static int bookings = 0;
        public static bool prizeHit = false;        
        public static Stack<string> DoorPrizes = new Stack<string>();
        public static int prizeIndex = 0;
        private static StreamReader fileReader;
        private string roomFile = "HotelManager.txt";
        private string clientFile = "HotelClients.txt";
        private string prizeFile = "HotelPrizes.txt";

        public Form1()
        {
            InitializeComponent();
            InitializeTabRoom();
            InitializeTabClient();
            InitializeTabStatus();
            InitializeTabPrize();
            LoadFile();
            SortRooms();
        }

        //
        //tabRoom
        //

        private void tabRooms_Enter(object sender, EventArgs e)
        {
            InitializeTabRoom();
        }
        private void InitializeTabRoom()
        {
            txtNumRooms.Enabled = false;
            btnRmSave.Enabled = false;
            btnRmNext.Enabled = false;
            btnRmPrev.Enabled = false;
            btnRmRmve.Enabled = false;
            btnSaveEdit.Enabled = false;
            chkVacant.Enabled = false;
            txtBeds.Text = "";
            txtRoom.Text = "";
            txtNumRooms.Text = "";
            chkBalcony.Checked = false;
            chkRepairs.Checked = false;
            chkSmoking.Checked = false;
            chkVacant.Checked = true;
            chkSuite.Checked = true;
            chkSuite.Checked = false;
            roomsIndex = 0;
        }

        private void btnRmNext_Click(object sender, EventArgs e)
        {            
            if (roomsIndex + 1 < rooms.Count)
            {
                roomsIndex++;
                DisplayRoom();                
            }
            else
            {
                MessageBox.Show("No More Rooms to Show");
            }           
        }

        private void chkSuite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSuite.Checked)
            {
                txtNumRooms.Enabled = true;
                chkSmoking.Enabled = false;
            }
            if (!chkSuite.Checked)
            {
                txtNumRooms.Text = "1";
                txtNumRooms.Enabled = false;
                chkSmoking.Enabled = true;
            }
        }

        private void btnRmEdit_Click(object sender, EventArgs e)
        {
            try
            {                
                btnRmSave.Enabled = false;
                btnRmRmve.Enabled = true;
                btnRmPrev.Enabled = true;
                btnRmNext.Enabled = true;
                btnSaveEdit.Enabled = true;
                roomsIndex = 0;
                DisplayRoom();
            }
            catch (Exception)
            {                
                MessageBox.Show("No Rooms to Show\nPlease add some available rooms", "No Rooms to Show!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        private void btnRmPrev_Click(object sender, EventArgs e)
        {
            if (roomsIndex - 1 >= 0)
            {
                roomsIndex--;
                DisplayRoom();                
            }
            else
            {
                MessageBox.Show("No Other Rooms to Show");
            }
        }

        public void DisplayRoom()
        {
            object[] temp = new object[rooms.Count];
            rooms.CopyTo(temp);
            ArrayList holder = new ArrayList();
            foreach (object item in temp)
            {
                holder.Add(item);
            }
            holder.Sort();
            var dump = holder[roomsIndex].ToString();
            string[] dumped = dump.Split(separators);            
            txtRoom.Text = dumped[0];
            bool convert = bool.Parse(dumped[1]);
            chkBalcony.Checked = convert;
            convert = bool.Parse(dumped[2]);
            chkRepairs.Checked = convert;
            txtBeds.Text = dumped[3];
            bool outBool;
            convert = bool.Parse(dumped[4]);
            chkVacant.Checked = convert;
            if (bool.TryParse(dumped[5], out outBool))
            {
                convert = bool.Parse(dumped[5]);
                chkSmoking.Checked = convert;
                txtNumRooms.Text = "1";
                chkSuite.Checked = false;
            }
            else
            {
                txtNumRooms.Text = dumped[5];
                chkSmoking.Checked = false;
                chkSuite.Checked = true;
            }
            
        }

        private void btnRmRmve_Click(object sender, EventArgs e)
        {
            if (rooms.Count == 0)
            {
                MessageBox.Show("No Rooms to Remove");
                btnRmRmve.Enabled = false;
            }
            else
            {
                if (!chkVacant.Checked)
                {
                    MessageBox.Show("Occupied Rooms Cannot Be Removed! " +
                        "\nVacate the Room First and Try Again", "Occupied Room",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var confirm = MessageBox.Show("Are you sure you want to remove this room?",
                        "Alert! Deleteing Room!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        if (MatchRoomNumber(txtRoom.Text) != -1)
                        {
                            rooms.RemoveAt(MatchRoomNumber(txtRoom.Text));
                            rooms.TrimToSize();
                            MessageBox.Show("Entry Removed");
                        }
                        else
                        {
                            MessageBox.Show("Room Not Found");
                        }                      
                        InitializeTabRoom();
                        SortRooms();
                        foreach (Control control in tabRooms.Controls)
                        {
                            (control as TextBox)?.Clear();
                            if (control is CheckBox)
                            {
                                ((CheckBox)control).Checked = false;
                            }
                        }
                    }
                }
            }
        }
        

        private void btnRmNew_Click(object sender, EventArgs e)
        {
            foreach (Control control in tabRooms.Controls)
            {
                (control as TextBox)?.Clear();
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = false;
                }
                if (control is TextBox)
                {
                    ((TextBox)control).Enabled = true;
                }
                if (control is CheckBox)
                {
                    ((CheckBox)control).Enabled = true;
                }
                btnRmPrev.Enabled = false;
                btnRmNext.Enabled = false;
                btnRmRmve.Enabled = false;
                btnRmSave.Enabled = true;
                btnSaveEdit.Enabled = false;
                chkSuite.Checked = true;
                chkSuite.Checked = false;
                chkVacant.Checked = true;
                chkVacant.Enabled = false;
            }
        }

        private void btnRmSave_Click(object sender, EventArgs e)
        {            
            try
            {
                string roomNumber = CheckEmpty(txtRoom.Text, lblRmNum.Text);
                string testBeds = CheckEmpty(txtBeds.Text, lblRmBeds.Text);
                int numBeds = 0;              
                var validBeds = Int32.TryParse(txtBeds.Text, out numBeds);
                if (!validBeds)
                {
                    throw new NotAValidNumber($"{testBeds} Is Not A Valid Number");
                }
                else
                {
                    if (numBeds < 0)
                    {
                        throw new NoNegatives("No Negatives Allowed\nPlease use a valid number");
                    }
                }                               
                var balcony = chkBalcony.Checked;
                var repairs = chkRepairs.Checked;
                var smoking = chkSmoking.Checked;
                var suite = chkSuite.Checked;
                string testRooms = CheckEmpty(txtNumRooms.Text, lblRmRooms.Text);
                int numRooms = 0;                
                var validRooms = Int32.TryParse(txtNumRooms.Text, out numRooms);
                if (!validRooms)
                {
                    throw new NotAValidNumber($"{testRooms} Is Not A Valid Number");
                }
                else
                {
                    if (numRooms < 0)
                    {
                        throw new NoNegatives("No Negatives Allowed\nPlease use a valid number");
                    }
                }               
                if (!suite)
                {
                    holder = new BasicRoom(roomNumber, balcony, repairs, numBeds, smoking);
                }
                else
                {
                    holder = new Suite(roomNumber, balcony, repairs, numBeds, numRooms);
                }
                rooms.Add(holder);                
                SortRooms();
                foreach (Control control in tabRooms.Controls)
                {
                    (control as TextBox)?.Clear();
                    if (control is CheckBox)
                    {
                        ((CheckBox)control).Checked = false;
                    }
                }
                chkVacant.Checked = true;
                chkSuite.Checked = true;
                chkSuite.Checked = false;
                MessageBox.Show("Room Added Succesfully");
            }catch(Exception){ };
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if(!chkVacant.Checked && chkRepairs.Checked)
            {
                try
                {
                    throw new OccupiedRepair("Occupied rooms cannot be marked in-repair\n" +
                        "Please vacate the room, then re-edit");
                }catch (Exception) { InitializeTabRoom(); };
            }
            else
            {
                try
                {
                    string roomNumber = CheckEmpty(txtRoom.Text, lblRmNum.Text);
                    string testBeds = CheckEmpty(txtBeds.Text, lblRmBeds.Text);
                    int numBeds = 0;
                    var validBeds = Int32.TryParse(txtBeds.Text, out numBeds);
                    if (!validBeds)
                    {
                        throw new NotAValidNumber($"{testBeds} Is Not A Valid Number");
                    }
                    else
                    {
                        if (numBeds < 0)
                        {
                            throw new NoNegatives("No Negatives Allowed\nPlease use a valid number");
                        }
                    }
                    var balcony = chkBalcony.Checked;
                    var repairs = chkRepairs.Checked;
                    var smoking = chkSmoking.Checked;
                    var suite = chkSuite.Checked;
                    string testRooms = CheckEmpty(txtNumRooms.Text, lblRmRooms.Text);
                    int numRooms = 0;
                    var validRooms = Int32.TryParse(txtNumRooms.Text, out numRooms);
                    if (!validRooms)
                    {
                        throw new NotAValidNumber($"{testRooms} Is Not A Valid Number");
                    }
                    else
                    {
                        if (numRooms < 0)
                        {
                            throw new NoNegatives("No Negatives Allowed\nPlease use a valid number");
                        }
                    }
                    if (!suite)
                    {
                        holder = new BasicRoom(roomNumber, balcony, repairs, numBeds, smoking);
                    }
                    else
                    {
                        holder = new Suite(roomNumber, balcony, repairs, numBeds, numRooms);
                    }
                    rooms.RemoveAt(MatchRoomNumber(txtRoom.Text));
                    rooms.Add(holder);                   
                    SortRooms();
                    roomsIndex = 0;
                    DisplayRoom();
                    MessageBox.Show("Edit Saved Succesfully");
                }
                catch (Exception) { };
            }
        }

        //
        // tabClients
        //

        private void tabClients_Enter(object sender, EventArgs e)
        {
            InitializeTabClient();
        }

        private void InitializeTabClient()
        {
            lblDisplayClientID.Text = "";
            lblDisplayFirstName.Text = "";
            lblDisplayLastName.Text = "";
            lblDisplayAddress.Text = "";
            lblDisplayPhone.Text = "";
            lblDisplayPrizes.Text = "";
            lblDisplayRooms.Text = "";
            btnClientNext.Enabled = false;
            btnClientPrev.Enabled = false;
            btnClientRmove.Enabled = false;
            btnClientSave.Enabled = false;
            txtClientID.Enabled = false;
            txtClientFName.Enabled = false;
            txtClientLName.Enabled = false;
            txtClientAddy.Enabled = false;
            txtClientPhone.Enabled = false;
            clientIndex = 0;
        }

        private void btnClientNew_Click(object sender, EventArgs e)
        {
            btnClientNext.Enabled = false;
            btnClientPrev.Enabled = false;
            btnClientRmove.Enabled = false;
            btnClientSave.Enabled = true;
            lblDisplayAddress.Text = "";
            lblDisplayClientID.Text = "";
            lblDisplayFirstName.Text = "";
            lblDisplayLastName.Text = "";
            lblDisplayPhone.Text = "";
            lblDisplayPrizes.Text = "";
            foreach (Control ctrl in tabClients.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Enabled = true;
                }
            }

        }

        private void btnClientSave_Click(object sender, EventArgs e)
        {
            try
            {
                var id = CheckEmpty(txtClientID.Text, lblClientID.Text);
                var fName = CheckEmpty(txtClientFName.Text, lblClientFName.Text);
                var lName = CheckEmpty(txtClientLName.Text, lblClientLName.Text);
                var address = CheckEmpty(txtClientAddy.Text, lblClientAddy.Text);
                var validPhone = CheckEmpty(txtClientPhone.Text, lblClientPhone.Text);
                if (validPhone.Length < 10 | validPhone.Length > 10)
                {
                    throw new LessThanTen("Phone Number Must be 10 digits");
                }
                long phone = 0;
                bool validPhoneNumber = long.TryParse(validPhone, out phone);
                if (!validPhoneNumber)
                {
                    throw new NotAValidNumber("Do not include any letters or symbols in phone number");
                }                             
                Client holder = new Client(id, fName, lName, address, phone);
                clients.Add(holder);
                clients.Sort();
                foreach (Control ctrl in tabClients.Controls)
                {
                    (ctrl as TextBox)?.Clear();
                }
                MessageBox.Show("Client saved successfully");
            }
            catch (Exception) { }
        }

        private void btnClientLoad_Click(object sender, EventArgs e)
        {
            try
            {
                btnClientSave.Enabled = false;
                btnClientRmove.Enabled = true;
                btnClientNext.Enabled = true;
                btnClientPrev.Enabled = true;
                lblDisplayPrizes.Text = "";
                clientIndex = 0;
                foreach (Control ctrl in tabClients.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        ((TextBox)ctrl).Enabled = false;
                    }
                }
                DisplayClient();
            }
            catch (Exception)
            {
                MessageBox.Show("No Clients to Show\nPlease add some new clients", 
                    "No Clients to Show!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        public void DisplayClient()
        {
            var dump = clients[clientIndex].ToString();
            string[] dumped = dump.Split(separators);
            lblDisplayClientID.Text = dumped[0];
            lblDisplayFirstName.Text = dumped[1];
            lblDisplayLastName.Text = dumped[2];
            lblDisplayAddress.Text = dumped[3];
            lblDisplayPhone.Text = dumped[4];
            lblDisplayPrizes.Text = "";
            for (int i = 5; i < dumped.Length; i++)
            {
                lblDisplayPrizes.Text += dumped[i] + " ";
            }            
            string bookedRooms = "";
            if (CheckOccoupancy(dumped[0]))
            {                
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (((Room)rooms[i]).vacancy.ClientID == dumped[0])
                    {
                        bookedRooms += "Room " + ((Room)rooms[i]).vacancy.RoomNumber + " ";
                    }
                }
            }
            else
            {
                bookedRooms = "Not a Current Guest";
            }
            lblDisplayRooms.Text = bookedRooms;
        }


        private void btnClientPrev_Click(object sender, EventArgs e)
        {
            if (clientIndex - 1 >= 0)
            {
                clientIndex--;
                DisplayClient();                
            }
            else
            {
                MessageBox.Show("No Other Clients to Show");
            }
        }

        private void btnClientNext_Click(object sender, EventArgs e)
        {
            if (clientIndex + 1 < clients.Count)
            {
                clientIndex++;
                DisplayClient();                
            }
            else
            {
                MessageBox.Show("No More Clients to Show");
            }
        }

        private void btnClientRmove_Click(object sender, EventArgs e)
        {
            if (clients.Count == 0)
            {
                MessageBox.Show("No Clients to Remove");
                btnClientRmove.Enabled = false;
            }
            else
            {
                var clientID = lblDisplayClientID.Text;
                if (CheckOccoupancy(clientID))
                {
                    MessageBox.Show("Current guests cannot be deleted");
                }
                else
                {
                    var confirm = MessageBox.Show("Are you sure you want to remove this client?",
                        "Alert! Deleteing Client!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        clients.RemoveAt(clientIndex);
                        clients.Sort();
                        clients.TrimToSize();
                        MessageBox.Show("Entry Removed");
                        clientIndex = 0;
                        InitializeTabClient();
                    }
                }
            }
        }        

        public bool CheckOccoupancy(string clientID)
        {
            
            bool occupancy = false;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (((Room)rooms[i]).vacancy.ClientID == clientID)
                {
                    occupancy = true;
                    return occupancy;
                }
            }
            return occupancy;
        }

        //
        // tabStatus
        //

        private void tabVacancies_Enter(object sender, EventArgs e)
        {
            InitializeTabStatus();
        }

        private void InitializeTabStatus()
        {
            chkStatusDisplayBalcony.Enabled = false;
            chkStatusDisplayRepair.Enabled = false;
            chkStatusDisplaySmoking.Enabled = false;
            chkStatusDisplaySuite.Enabled = false;
            chkStatusDisplayVacant.Enabled = false;
            lblStatusDisplayCAddy.Text = "";
            lblStatusDisplayCFName.Text = "";
            lblStatusDisplayCID.Text = "";
            lblStatusDisplayCLName.Text = "";
            lblStatusDisplayCPhone.Text = "";
            lblStatusDisplayNumRooms.Text = "";
            lblStatusDisplayRmBeds.Text = "";
            lblStatusDisplayRmNum.Text = "";            
            btnStatusBook.Enabled = false;
            btnStsClPrev.Enabled = false;
            btnStsClNext.Enabled = false;
            btnStsRmNext.Enabled = false;
            btnStsRmPrev.Enabled = false;
            roomsIndex = 0;
            clientIndex = 0;
            DisplayNextAvailable();
        }

        public void StatusDisplayRoom()
        {
            object[] temp = new object[rooms.Count];
            rooms.CopyTo(temp);
            ArrayList holder = new ArrayList();
            foreach (object item in temp)
            {
                holder.Add(item);
            }
            holder.Sort();
            var dump = holder[roomsIndex].ToString();            
            string[] dumped = dump.Split(separators);
            lblStatusDisplayRmNum.Text = dumped[0];
            bool convert = bool.Parse(dumped[1]);
            chkStatusDisplayBalcony.Checked = convert;
            convert = bool.Parse(dumped[2]);
            chkStatusDisplayRepair.Checked = convert;
            lblStatusDisplayRmBeds.Text = dumped[3];
            convert = bool.Parse(dumped[4]);
            chkStatusDisplayVacant.Checked = convert;
            bool outBool;
            if (bool.TryParse(dumped[5], out outBool))
            {
                convert = bool.Parse(dumped[5]);
                chkStatusDisplaySmoking.Checked = convert;
                lblStatusDisplayNumRooms.Text = "1";
                chkStatusDisplaySuite.Checked = false;
            }
            else
            {
                lblStatusDisplayNumRooms.Text = dumped[5];
                chkStatusDisplaySmoking.Checked = false;
                chkStatusDisplaySuite.Checked = true;
            }          
            lblStatusDisplayCID.Text = dumped[8];
            lblStatusDisplayCFName.Text = dumped[9];
            lblStatusDisplayCLName.Text = dumped[10];
            lblStatusDisplayCAddy.Text = dumped[11];
            lblStatusDisplayCPhone.Text = dumped[12];
        }

        public void DisplayNextAvailable()
        {
            try
            {
                if (vacant.Count > 0)
                {
                    lblStatusDisplayNextAvail.Text = "Room: " +((Room)vacant.Peek()).RoomNumber;
                }
                else
                {
                    lblStatusDisplayNextAvail.Text = "None/Full";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void StatusDisplayClient()
        {
            var dump = clients[clientIndex].ToString();
            string[] dumped = dump.Split(separators);
            lblStatusDisplayCID.Text = dumped[0];
            lblStatusDisplayCFName.Text = dumped[1];
            lblStatusDisplayCLName.Text = dumped[2];
            lblStatusDisplayCAddy.Text = dumped[3];
            lblStatusDisplayCPhone.Text = dumped[4];            
        }

        private void btnStatusLoadRooms_Click(object sender, EventArgs e)
        {            
            try
            {
                roomsIndex = 0;
                lblStatusDisplayRmNum.Text = "";
                lblStatusDisplayRmBeds.Text = "";
                lblStatusDisplayRmNum.Text = "";
                chkStatusDisplayBalcony.Checked = false;
                chkStatusDisplayRepair.Checked = false;
                chkStatusDisplaySmoking.Checked = false;
                chkStatusDisplaySuite.Checked = false;
                chkStatusDisplayVacant.Checked = false;
                btnStatusBook.Enabled = false;
                btnStsRmNext.Enabled = true;
                btnStsRmPrev.Enabled = true;
                StatusDisplayRoom();
            }
            catch (Exception)
            {
                MessageBox.Show("No Rooms to Show\nPlease add some available rooms", "No Rooms to Show!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void btnStsRmNext_Click(object sender, EventArgs e)
        {
            if (roomsIndex + 1 < rooms.Count)
            {
                roomsIndex++;
                StatusDisplayRoom();                
            }
            else
            {
                MessageBox.Show("No More Rooms to Show");
            }
        }

        private void btnStsRmPrev_Click(object sender, EventArgs e)
        {
            if (roomsIndex - 1 >= 0)
            {
                roomsIndex--;
                StatusDisplayRoom();                
            }
            else
            {
                MessageBox.Show("No Other Rooms to Show");
            }
        }

        private void btnStatusLoadClients_Click(object sender, EventArgs e)
        {            
            try
            {
                clientIndex = 0;
                btnStatusBook.Enabled = true;
                btnStsClNext.Enabled = true;
                btnStsClPrev.Enabled = true;
                StatusDisplayClient();
            }
            catch (Exception)
            {
                MessageBox.Show("No Clients to Show\nPlease add some new clients", 
                    "No Clients to Show!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStsClNext_Click(object sender, EventArgs e)
        {
            if (clientIndex + 1 < clients.Count)
            {
                clientIndex++;
                StatusDisplayClient();
            }
            else
            {
                MessageBox.Show("No More Clients to Show");
            }
        }

        private void btnStsClPrev_Click(object sender, EventArgs e)
        {
            if (clientIndex - 1 >= 0)
            {
                clientIndex--;
                StatusDisplayClient();
            }
            else
            {
                MessageBox.Show("No Other Clients to Show");
            }
        }

        private void btnStatusBook_Click(object sender, EventArgs e)
        {
            try
            {
                Room holder = vacant.Dequeue();            
                holder.Vacant = false;
                (holder).vacancy.setClient((Client)clients[clientIndex]);
                booked.Push(holder);
                MessageBox.Show($"{((Client)clients[clientIndex]).FirstName} " +
                    $"{((Client)clients[clientIndex]).LastName}" +
                    " was booked into Room: " + holder.RoomNumber);
                bookings++;
                bool winPrize = WinPrize();
                if (winPrize)
                {
                    AddPrizeToClient();
                }
                ClearDisplayClient();
                ClearDisplayRoom();
                DisplayNextAvailable();
                btnStatusBook.Enabled = false;
                btnStsClNext.Enabled = false;
                btnStsClPrev.Enabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("All Rooms Are Full\nNo Available Rooms");                
            }
        }

        private void ClearDisplayRoom()
        {
            lblStatusDisplayRmNum.Text = "";
            lblStatusDisplayRmBeds.Text = "";
            lblStatusDisplayNumRooms.Text = "";
        }

        private void ClearDisplayClient()
        {
            lblStatusDisplayCID.Text = "";
            lblStatusDisplayCFName.Text = "";
            lblStatusDisplayCLName.Text = "";
            lblStatusDisplayCAddy.Text = "";
            lblStatusDisplayCPhone.Text = "";
        }

        private void btnStatusVacate_Click(object sender, EventArgs e)
        {
            try
            {
                Room holder = booked.Pop();
                MessageBox.Show($"{holder.vacancy.clientInfo.FirstName} " +
                    $"{holder.vacancy.clientInfo.LastName} has checked out of Room: " +
                    $"{holder.vacancy.RoomNumber}");
                holder.Vacant = true;
                holder.makeVacant();
                vacant.Enqueue(holder);                
                ClearDisplayClient();
                ClearDisplayRoom();
                DisplayNextAvailable();
            }
            catch (Exception)
            {
                MessageBox.Show("All Rooms Are Empty");
            }
            
        }

        //
        // tabPrizes
        //

        private void tabPrizes_Enter(object sender, EventArgs e)
        {
            InitializeTabPrize();
        }

        private void InitializeTabPrize()
        {
            txtPrzEntry.Visible = false;
            lblPrizeDisplay.Text = "";
            btnPrzEnter.Visible = false;
            btnPrzNext.Visible = false;
            btnPrzPrev.Visible = false;
            btnPrzRemv.Visible = false;
        }

        private void btnNewPrize_Click(object sender, EventArgs e)
        {
            txtPrzEntry.Visible = true;
            btnPrzEnter.Visible = true;
            btnPrzNext.Visible = false;
            btnPrzPrev.Visible = false;
            btnPrzRemv.Visible = false;
        }

        private void btnViewPrizes_Click(object sender, EventArgs e)
        {
            try
            {
                prizeIndex = 0;
                txtPrzEntry.Visible = false;
                btnPrzEnter.Visible = false;
                btnPrzNext.Visible = true;
                btnPrzPrev.Visible = true;
                btnPrzRemv.Visible = true;
                DisplayPrize();
            }
            catch (Exception)
            { 
                MessageBox.Show("No Prizes to Remove\nPlease Add New Prizes", "No Prizes", 
                    MessageBoxButtons.OK);
            }
            
        }

        private void btnPrzEnter_Click(object sender, EventArgs e)
        {            
            try
            {
                string prize = CheckEmpty(txtPrzEntry.Text, lblPrize.Text);                
                DoorPrizes.Push(prize);
                MessageBox.Show($"Added {prize} to the possible prizes");                
            }
            catch (Exception){ }
            txtPrzEntry.Clear();

        }

        private void btnPrzRemv_Click(object sender, EventArgs e)
        {
            try
            {
                string removed = DoorPrizes.Pop();
                MessageBox.Show($"{removed} was removed from the possible prizes");
                prizeIndex = 0;
                DisplayPrize();
            }
            catch (Exception)
            {
                MessageBox.Show("No Prizes to Remove\nPlease Add New Prizes", 
                    "No Prizes", MessageBoxButtons.OK);
                lblPrizeDisplay.Text = "";
            }
            
        }

        private void btnPrzPrev_Click(object sender, EventArgs e)
        {
            if (prizeIndex - 1 >= 0)
            {
                prizeIndex--;
                DisplayPrize();
            }
            else
            {
                MessageBox.Show("No other prizes to show");
            }
        }

        private void btnPrzNext_Click(object sender, EventArgs e)
        {
            if (prizeIndex + 1 < DoorPrizes.Count)
            {
                prizeIndex++;
                DisplayPrize();
            }
            else
            {
                MessageBox.Show("No more prizes to show");
            }
        }

        public void DisplayPrize()
        {
            string prize = DoorPrizes.ElementAt<string>(prizeIndex);
            lblPrizeDisplay.Text = prize;
        }

        public bool WinPrize()
        {
            var x = rand.Next();
            if (bookings % 3 == 0)
            {
                if (x % 2 == 0)
                {
                    prizeHit = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (bookings % 4 == 0 && !prizeHit)
            {
                prizeHit = false;
                return true;
            }
            return false;
        }

        public void AddPrizeToClient()
        {
            try
            {
                string prize = DoorPrizes.Pop();
                ((Client)clients[clientIndex]).addDoorPrize(prize);
                MessageBox.Show($"{((Client)clients[clientIndex]).FirstName} " +
                    $"{((Client)clients[clientIndex]).LastName} won a {prize}!");
            }
            catch (Exception)
            {
                MessageBox.Show("This customer missed out on a door prize " +
                    "\nPlease refill the prizes", "Please Refill DoorPrizes",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //
        // Validation
        //

        public string CheckEmpty(string value, string field)
        {
            string valid = value;
            if (!string.IsNullOrEmpty(valid))
            {
                return value;
            }
            else
            {
                throw new EmptyBox($"{field} Cannot Be Empty");
            }
        }

        public bool RoomAvailable(Room room)
        {
            if (room.Vacant && room.DownForRepair)
            {
                return false;
            }
            if (!room.Vacant)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SortRooms()
        {            
            booked.Clear();
            vacant.Clear();
            foreach (Room room in rooms)
            {
                if (RoomAvailable(room))
                {
                    vacant.Enqueue(room);
                }
                if (!room.Vacant)
                {
                    booked.Push(room);
                }
            }
        }

        private int MatchRoomNumber(string roomToMatch)
        {            
            string[] roomNumbers = new string[rooms.Count];
            for (int i = 0; i < rooms.Count; i++)
            {
                roomNumbers[i] = ((Room)rooms[i]).RoomNumber;                
            }
            int index = -1;
            for (var x = 0; x < roomNumbers.Length; x++)
            {                
                if (roomNumbers[x] == roomToMatch)
                {
                    index = x;
                    break;
                }
            }
            return index;

        }

        //
        // streams
        //

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rooms.Clear();
            clients.Clear();
            DoorPrizes.Clear();
            booked.Clear();
            vacant.Clear();
            LoadFile();
            SortRooms();            
            InitializeTabRoom();
            InitializeTabClient();
            InitializeTabStatus();
            InitializeTabPrize();
        }

        private void LoadFile()
        {
            try
            {
                FileStream input = new FileStream(roomFile, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(input);
                var inputRecord = fileReader.ReadToEnd();
                string[] dumpedRooms = inputRecord.Split(',');
                foreach (string room in dumpedRooms)
                {
                    if (!string.IsNullOrWhiteSpace(room))
                    {
                        Room holder;
                        string[] dumped = room.Split(separators);
                        var roomNum = dumped[0].Trim();
                        bool convert = bool.Parse(dumped[1]);
                        var balcony = convert;
                        convert = bool.Parse(dumped[2]);
                        var repairs = convert;
                        var beds = int.Parse(dumped[3]);
                        bool outBool;
                        if (bool.TryParse(dumped[5], out outBool))
                        {
                            convert = bool.Parse(dumped[5]);
                            var smoking = convert;
                            holder = new BasicRoom(roomNum, balcony, repairs, beds, smoking);
                            convert = bool.Parse(dumped[4]);
                            holder.Vacant = convert;
                            if (!convert)
                            {
                                Client holderClient;
                                var id = dumped[8];
                                var fName = dumped[9];
                                var lName = dumped[10];
                                var address = dumped[11];
                                var phone = long.Parse(dumped[12]);
                                holderClient = new Client(id, fName, lName, address, phone);                                
                                for (int i = 13; i < dumped.Length; i++)
                                {
                                    holderClient.addDoorPrize(dumped[i]);
                                }
                                holder.vacancy.setClient(holderClient);                                
                            }                            
                            rooms.Add(holder);                            
                        }
                        else
                        {
                            var numRooms = int.Parse(dumped[5]);
                            holder = new Suite(roomNum, balcony, repairs, beds, numRooms);
                            convert = bool.Parse(dumped[4]);
                            holder.Vacant = convert;
                            if (!convert)
                            {
                                Client holderClient;
                                var id = dumped[8];
                                var fName = dumped[9];
                                var lName = dumped[10];
                                var address = dumped[11];
                                var phone = long.Parse(dumped[12]);
                                holderClient = new Client(id, fName, lName, address, phone);
                                for (int i = 13; i < dumped.Length; i++)
                                {
                                    if (dumped[13] != "No Door Prizes")
                                    {
                                        holderClient.addDoorPrize(dumped[i]);
                                    }                                    
                                }
                                holder.vacancy.setClient(holderClient);
                                holder.Vacant = false;
                            }                            
                            rooms.Add(holder);                            
                        }
                    }
                }
                fileReader.Close();

                input = new FileStream(clientFile, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(input);
                inputRecord = fileReader.ReadToEnd();
                string[] dumpedClients = inputRecord.Split(',');
                foreach (string client in dumpedClients)
                {
                    if (!string.IsNullOrWhiteSpace(client))
                    {
                        Client holderClient;
                        string[] dumpedCL = client.Split(separators);
                        var clientID = dumpedCL[0].Trim();
                        var firstName = dumpedCL[1];
                        var lastName = dumpedCL[2];
                        var address = dumpedCL[3];
                        var phone = long.Parse(dumpedCL[4]);
                        holderClient = new Client(clientID, firstName, lastName, address, phone);
                        for (int i = 5; i < dumpedCL.Length; i++)
                        {
                            if (dumpedCL[5] != "No Door Prizes")
                            {
                                holderClient.addDoorPrize(dumpedCL[i]);
                            }
                        }
                        clients.Add(holderClient);
                        clients.Sort();
                    }
                }
                fileReader.Close();

                input = new FileStream(prizeFile, FileMode.Open, FileAccess.Read);
                fileReader = new StreamReader(input);
                inputRecord = fileReader.ReadToEnd();
                string[] dumpedPrizes = inputRecord.Split(',');
                for (int i = dumpedPrizes.Length -1; i >= 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(dumpedPrizes[i]))
                    {
                        DoorPrizes.Push(dumpedPrizes[i].Trim());
                    }                    
                }
                fileReader.Close();                
                MessageBox.Show("Loading Complete");
            }
            catch (Exception ex) 
            {
                fileReader.Close();
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }        

        private void SaveFile()
        {
            var confirm = MessageBox.Show("Save All Changes?", "Save", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {                    
                    var output = new FileStream(roomFile, FileMode.Truncate, FileAccess.Write);

                    using (StreamWriter writer = new StreamWriter(output))
                    {
                        foreach (Room room in rooms)
                        {
                            writer.Write(((Room)room).ToString() + ',' + Environment.NewLine);
                        }
                    }
                    output.Close();

                    output = new FileStream(clientFile, FileMode.Truncate, FileAccess.Write);

                    using (StreamWriter writer = new StreamWriter(output))
                    {
                        foreach (Client client in clients)
                        {
                            writer.Write(client.ToString() + ',' + Environment.NewLine);
                        }
                    }
                    output.Close();

                    output = new FileStream(prizeFile, FileMode.Truncate, FileAccess.Write);

                    using (StreamWriter writer = new StreamWriter(output))
                    {
                        for (int i = DoorPrizes.Count; i > 0; i--)
                        {
                            writer.Write(DoorPrizes.Pop() + ',' + Environment.NewLine);
                        }
                    }
                    output.Close();
                    MessageBox.Show("Saved Successfully");
                }catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFile();
            this.Dispose();
        }
    }
}
