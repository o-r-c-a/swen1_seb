// See https://aka.ms/new-console-template for more information
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Utilities;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;
using System.Text.Json;
using SportsExerciseBattle.Infrastructure;
using SportsExerciseBattle.Repositories;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Utilities.Exceptions;
using SportsExerciseBattle.API;

namespace SportsExerciseBattle;

class Program
{
    static void Main(string[] args)
    {
        DatabaseInitializer.InitializeDatabase();
        var serviceProvider = DIConfig.ConfigureServices();
        var serverController = new ServerController(serviceProvider);
        serverController.ListenAsync();
    }
}