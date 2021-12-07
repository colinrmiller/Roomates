using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true; TrustServerCertificate=True";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);

            Roommate newRoommate = new Roommate("Karl", "Flannagin", 4, new DateTime(1987, 5, 4), 4);
            Roommate otherNewRoommate = new Roommate()
            {
                FirstName = "Benjamin",
                LastName = "Jamin",
                RentPortion = 100,
                MovedInDate = new DateTime(2020, 2, 2),
                Room = roomRepo.GetById(1)
                // TODO Add Room
            };


            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> getRoomOptions = roomRepo.GetAll();
                        foreach (Room r in getRoomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to delete? ");
                        int deleteRoomId = int.Parse(Console.ReadLine());

                        roomRepo.Delete
                            (deleteRoomId);

                        Console.WriteLine("Room has been successfully been deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName,
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("View unassaigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore uc in unassignedChores)
                        {
                            Console.WriteLine($"  {uc.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();

                        break;

                    case ("Assign a chore"):
                        List<Chore> unassignedChoresList = choreRepo.GetUnassignedChores();
                        List<Roommate> roommatesList = roommateRepo.GetAll();

                        Console.WriteLine("Unassigned Chores:");
                        foreach (Chore uc in unassignedChoresList)
                        {
                            Console.WriteLine($"{uc.Id})  {uc.Name}");
                        }
                        Console.Write("Chore Id: ");
                        int assignChoreId = int.Parse(Console.ReadLine());

                        Console.WriteLine("Roommates:");
                        foreach (Roommate rm in roommatesList)
                        {
                            Console.WriteLine($"{rm.Id})  {rm.Details}");
                        }
                        Console.Write("Roommate Id: ");
                        int assignRoommateId = int.Parse(Console.ReadLine());

                        choreRepo.AssignChore(assignRoommateId, assignChoreId);

                        Console.Write("Press any key to continue");
                        Console.ReadKey();

                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore r in choreOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(r => r.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();
                        selectedChore.Id = selectedChoreId;

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        List<Chore> getChoreOptions = choreRepo.GetAll();
                        foreach (Chore r in getChoreOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }

                        Console.Write("Which chore would you like to delete? ");
                        int deleteChoreId = int.Parse(Console.ReadLine());

                        choreRepo.Delete(deleteChoreId);

                        Console.WriteLine("Chore has been successfully been deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Get chore counts"):
                        Dictionary<Roommate, int> roommateObjs = choreRepo.GetChoreCounts();
                        foreach (var rm in roommateObjs)
                        {
                            Console.WriteLine(@$"{rm.Key.FirstName} {rm.Value}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Reassign Chore"):
                        Dictionary<Chore, Roommate> assignedChores = choreRepo.GetAssignedChores();
                        List<Roommate> roommatesList1 = roommateRepo.GetAll();
                        // Write GetAssigned Chores
                        // Display Chores and roommate assigned
                        // prompt for choreId to reassign
                        // reassignChore(choreId, roommateId)
                        foreach (var ac in assignedChores)
                        {
                            Console.WriteLine(@$"{ac.Key.Id}) {ac.Key.Name}:  {ac.Value.FirstName}");
                        }
                        Console.Write("Which chore would you like to reassign: ");
                        int choreIdToReassign = int.Parse(Console.ReadLine());

                        Console.WriteLine();
                        foreach (var rm in roommatesList1)
                        {
                            Console.WriteLine(@$"{rm.Id}) {rm.FirstName}");
                        }
                        Console.Write("Who would you like to reassign it too: ");
                        int roommateIdToReassign = int.Parse(Console.ReadLine());

                        choreRepo.ReassignChore(choreIdToReassign, roommateIdToReassign);
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all roommates"):
                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate rm in roommates)
                        {
                            Console.WriteLine(@$"{rm.FirstName} {rm.LastName} has an Id of {rm.Id}, 
Pays {rm.RentPortion }, 
Moved In{rm.MovedInDate}, 
Lives in Room : {rm.Room.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a roommate"):
                        Console.Write("First name: ");
                        string roommateFirstName = Console.ReadLine();

                        Console.Write("Last name: ");
                        string roommateLastName = Console.ReadLine();

                        Console.Write("Rent portion: ");
                        int roommateRentPortion = int.Parse(Console.ReadLine());

                        Console.Write("Move In Year: ");
                        DateTime roommateMoveInDate = new DateTime(int.Parse(Console.ReadLine()));

                        Console.Write("Room Id: ");
                        int roomId = int.Parse(Console.ReadLine());

                        Roommate roommateToAdd = new Roommate()
                        {
                            FirstName = roommateFirstName,
                            LastName = roommateLastName,
                            RentPortion = roommateRentPortion,
                            MovedInDate = roommateMoveInDate,
                        };

                        roommateRepo.Insert(roommateToAdd);

                        Console.WriteLine($"{roommateToAdd.FirstName} {roommateToAdd.LastName}has been added and assigned an Id of {roommateToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for roommate"):
                        Console.Write("Chore Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        Console.WriteLine($"{roommate.Id} - {roommate.FirstName} {roommate.LastName}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }
        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Search for chore",
                "View unassaigned chores",
                "Assign a chore",
                "Add a chore",
                "Update a chore",
                "Delete a chore",
                "Get chore counts",
                "Reassign Chore",
                "Show all roommates",
                "Add a roommate",
                "Search for roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
