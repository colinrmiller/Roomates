using System;
using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    /// <summary>
    ///  This class is responsible for interacting with Roommate data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    public class RoommateRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRepository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoommateRepository(string connectionString) : base(connectionString) { }

        // ...We'll add some methods shortly...
        /// <summary>
        ///  Get a list of all Roommates in the database
        /// </summary>
        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Roommate.Id, FirstName, LastName, MoveInDate, RentPortion, r.Name as RoomName FROM Roommate JOIN Room as r ON Roommate.RoomId = r.Id"; 

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommatess = new List<Roommate>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                            string getFirstName = reader.GetString(firstNameColumnPosition);

                            int lastNameColumnPosition = reader.GetOrdinal("LastName");
                            string getLastName = reader.GetString(lastNameColumnPosition);

                            int rentPortionColumPosition = reader.GetOrdinal("RentPortion");
                            int getRentPortion = reader.GetInt32(rentPortionColumPosition);

                            int movedInColumnPosition = reader.GetOrdinal("MoveInDate");
                            System.DateTime getMovedInDate = reader.GetDateTime(movedInColumnPosition);

                            int roomIdColumnPosition = reader.GetOrdinal("RoomName");
                            string getRoomName = reader.GetString(roomIdColumnPosition);

                            // Now let's create a new roommate object using the data from the database.
                            Roommate roommate = new Roommate
                            {
                                Id = idValue,
                                FirstName = getFirstName,
                                LastName = getLastName,
                                RentPortion = getRentPortion,
                                MovedInDate = getMovedInDate,
                                Room = new Room
                                {
                                    Name = getRoomName
                                }
                            };
                            roommatess.Add(roommate);
                        }

                        return roommatess;
                    }
                }
            }
        }
        /// <summary>
        ///  Returns a single roommate with the given id.
        /// </summary>
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Roommate.Id, FirstName, LastName, MoveInDate, RentPortion, r.Name as RoomName FROM Roommate JOIN Room as r ON Roommate.RoomId = r.Id WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room
                                {
                                    Name = reader.GetString(reader.GetOrdinal("RoomName")),
                                }

                            };
                        }
                        return roommate;
                    }

                }
            }
        }
        /// <summary>
        ///  Add a new roommate to the database
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>
        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Roommate (FirstName, LastName, RentPortion, MoveInDate) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@firstName, @lastName, @rentPortion, @moveInDate)";
                    cmd.Parameters.AddWithValue("@firstName", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", roommate.LastName);
                    cmd.Parameters.AddWithValue("@rentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@moveInDate", roommate.MovedInDate);
                    int id = (int)cmd.ExecuteScalar();

                    roommate.Id = id;
                }
            }

            //    // when this method is finished we can look in the database and see the new roommate.
        }

    }
}
