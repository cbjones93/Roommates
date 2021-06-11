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
                    cmd.CommandText = "SELECT (room.name) as RoomName, Roommate.FirstName,Roommate.RentPortion from Roommate join Room on Roommate.RoomId = Room.id where Room.id=@id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    Room room = null;

                    if (reader.Read())
                    {
                       
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = room
      
                        };
                        room = new Room
                        { 
                           Name=reader.GetString(reader.GetOrdinal("RoomName"))
                        };


                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
    }
}
