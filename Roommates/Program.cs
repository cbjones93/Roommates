﻿using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);


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

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}.");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateid = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.getById(roommateid);
                        Console.WriteLine($"{roommate.FirstName} lives in {roommate.Room.Name} and their rent portion is {roommate.RentPortion}.");

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores unassigned to roommates"):
                        List<Chore> unassignedchores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedchores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}.");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Assign a chore"):
                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate mate in roommates)
                        {
                            Console.WriteLine($"{mate.Id}) {mate.FirstName}");
                        }
                        Console.Write("Search Roommate by Id: ");
                        int roomie = int.Parse(Console.ReadLine());
                        List<Chore> choreList = choreRepo.GetAll();
                        foreach (Chore chore in choreList)
                        {
                            Console.WriteLine($"{chore.Id}) {chore.Name}");
                        }
                        Console.Write("Assign by ChoreId: ");
                        int assignChore = int.Parse(Console.ReadLine());
                        choreRepo.AssignChore(assignChore, roomie);

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
                        Console.Write("Press any key to continue.");
                        Console.ReadKey();

                        break;

                    case ("Delete a room"):
                        List<Room> roomDeleteOptions = roomRepo.GetAll();
                        foreach (Room r in roomDeleteOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.Write("Which room would you like to delete? ");
                        int selectedRoomIdToDelete = int.Parse(Console.ReadLine());
                        Room selectedRoomToDelete = roomDeleteOptions.FirstOrDefault(r => r.Id == selectedRoomIdToDelete);


                        roomRepo.Delete(selectedRoomIdToDelete);
                        Console.WriteLine("Room has been successfully deleted.");
                        Console.Write("Press any key to continue.");
                        Console.ReadKey();

                        break;

                    case ("Update a chore"):
                        List<Chore> choresOptions = choreRepo.GetAll();
                        foreach (Chore c in choresOptions)
                        {
                            Console.WriteLine($"{c.Id}) {c.Name}");
                        }
                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choresOptions.FirstOrDefault(c => c.Id == selectedChoreId);
                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();
                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue.");
                        Console.ReadKey();

                        break;

                    case ("Delete a chore"):
                        List<Chore> choreDeleteOptions = choreRepo.GetAll();
                        foreach (Chore c in choreDeleteOptions)
                        {
                            Console.WriteLine($"{c.Id}) {c.Name}");
                        }
                        Console.Write("Which chore would you like to delete? ");
                        int selectedChoreIdToDelete = int.Parse(Console.ReadLine());
                        Chore selectedChoreToDelete = choreDeleteOptions.FirstOrDefault(c => c.Id == selectedChoreIdToDelete);

                        try
                        {
                            choreRepo.Delete(selectedChoreIdToDelete);
                            Console.WriteLine("Chore has been successfully deleted.");
                            Console.Write("Press any key to continue.");
                            Console.ReadKey();
                        }

                        catch
                        {
                            Console.WriteLine("You cannot delete this chore as its been assigned to a roommate");
                            Console.Write("Press any key to continue.");
                            Console.ReadKey();
                        }
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
                "Show all chores",
                "Search for roommate",
                "Show all chores unassigned to roommates",
                "Assign a chore",
                "Update a room",
                "Delete a room",
                "Update a chore",
                "Delete a chore",
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

