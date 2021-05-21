using System;       
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CricketTournament
{
    public partial class Registration : Form
    {
        SqlConnection connection = new SqlConnection(@"Server=DESKTOP-C0MLP28\SQLEXPRESS;Database=CricketTeamManagement;Trusted_Connection=true;");
        SqlCommand command;
        SqlDataReader reader;
        public Registration()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Enter a name");
            }
            else
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "TeamSave";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("Name", txtName.Text);
                command.ExecuteNonQuery();
                lsbPlayers.Items.Clear();
                ReadValues(1);
                connection.Close();
                txtName.Clear();
                MessageBox.Show("Sucessfully Added");
            }
          
        }

        public void ReadValues(int id)
        {
            connection = new SqlConnection(@"Server=DESKTOP-C0MLP28\SQLEXPRESS;Database=CricketTeamManagement;Trusted_Connection=true;");
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = "TeamList";
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string teamId = reader["Id"].ToString();
                string teamName= reader["Name"].ToString();
                lsbPlayers.Items.Add(id+"."+teamName);
                id++;
            }
            connection.Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lsbPlayers.SelectedIndex==-1)
            {
                MessageBox.Show( "Please select an item");
                
                return;
            }
            else
            {
               for (int i = 0; i <= lsbPlayers.Items.Count ; i++)
                {
                    string[] listPlayers = lsbPlayers.SelectedItems[i].ToString().Split('.');
                   connection = new SqlConnection(@"Server=DESKTOP-C0MLP28\SQLEXPRESS;Database=CricketTeamManagement;Trusted_Connection=true;");
                    connection.Open();
                    command = connection.CreateCommand();
                    command.CommandText = "TeamList";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string teamId = reader["Id"].ToString();
                        string teamName = reader["Name"].ToString();
                      //  connection.Close();
                        if (listPlayers[1] ==teamName)
                        {
                             connection = new SqlConnection(@"Server=DESKTOP-C0MLP28\SQLEXPRESS;Database=CricketTeamManagement;Trusted_Connection=true;");
                            connection.Open();

                            SqlCommand command = connection.CreateCommand();
                            command.CommandText = "TeamDelete";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("Id", teamId);
                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                lsbPlayers.Items.Remove(listPlayers[1]);
                                lsbPlayers.Items.Clear();
                                ReadValues(1);
                                MessageBox.Show("deleted successfully");
                               
                            }
                        }
                    }
                    break;
                }
            }
            connection.Close();
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            ReadValues(1);
        }
    }
}
