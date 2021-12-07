using System;
using Roommates.Repositories;
namespace Roommates.Models
{
    // C# representation of the Roommate table
    public class Roommate
    {
        public  Roommate(string firstName, string lastName, int rentPortion, DateTime movedInDate, int roomId)
        {
        string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true; TrustServerCertificate=True";

        RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

        FirstName = firstName;
            LastName = lastName;
            RentPortion = rentPortion;
            MovedInDate = movedInDate;
            Room = roomRepo.GetById(roomId);
        }
        public Roommate()
        {
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RentPortion { get; set; }
        public DateTime MovedInDate { get; set; }
        public Room Room { get; set; }
        public string Details { 
            get
            {
                return $"{FirstName} {LastName} > Rent Portion: {RentPortion}; movedInDate: {MovedInDate}";
            }
        }
    }
}
