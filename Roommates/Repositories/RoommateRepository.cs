using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public Roommate getById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT (room.name) as RoomName, Roommate.FirstName,Roommate.RentPortion from Roommate join Room on Roommate.RoomId = Room.id where Roommate.id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    Room room = null;

                    if (reader.Read())
                    {
                       
                        room = new Room
                        { 
                           Name=reader.GetString(reader.GetOrdinal("RoomName"))
                        };
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = room
      
                        };


                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select * from Roommate";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);
                        int nameColumnPosition = reader.GetOrdinal("FirstName");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = nameValue
                        };
                        roommates.Add(roommate);
                    }
                    reader.Close();
                    return roommates;
                }
            }
        }
    }
}
