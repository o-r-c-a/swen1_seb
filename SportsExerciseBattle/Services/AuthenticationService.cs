using Microsoft.AspNetCore.Identity;
using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static AuthenticationService? _instance;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }
        public static AuthenticationService GetInstance(IUserRepository userRepository)
        {
            if (_instance == null)
            {
                _instance = new AuthenticationService(userRepository);
            }
            return _instance;
        }
        public static void ResetInstance() => _instance = null;
        public void Register(string givenUsername, string givenPassword)
        {
            //Console.WriteLine($"REGISTERED pw: {HashedPassword}");
            if (_userRepository.UserExists(givenUsername))
            {
                throw new UserAlreadyExistsException(); // 409: user already exists
            }
            var user = new User(givenUsername, givenPassword);
            string HashedPassword = _passwordHasher.HashPassword(user, givenPassword);
            
            user.Password = HashedPassword;

            _userRepository.AddUser(user); // 201: success
        }

        public void Login(string givenUsername, string givenPassword)
        {
            var user = _userRepository.GetUserByUsername(givenUsername) ?? throw new UserNotFoundException();
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, givenPassword);
            //
            //string HashedPassword = _passwordHasher.HashPassword(user, givenPassword);
            //Console.WriteLine($"LOGIN pw; user: {user.Password}, given: {HashedPassword}");
            ////
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedException("Wrong Password!"); // 401: unauthorized
            }

            // Generate a new token for the user
            var token = GenerateToken(user);
            _userRepository.UpdateUserToken(user.Username, token); // Update the token in the database
        }
        public void AuthenticateAction(string token, string? username = null)
        {
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Authentication-Token invalid!");

            if(username == null)
            {
                // If no username is provided, check if the token is valid
                if(_userRepository.TokenExists(token))
                    return;
                throw new UnauthorizedException("Authentication-Token invalid!");
            }

            // Check if the token is valid
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
                throw new UserNotFoundException();
            if (user.AuthToken != token)
                throw new UnauthorizedAccessException("Authentification-Token mismatch!");
        }

        public string GenerateToken(User user)
        {
            if (user == null)
                throw new Exception("Failed generating Authentification-Token!");

            // Simple token format: username-sebToken
            return $"{user.Username}-sebToken";
        }

        public string? GetUserToken(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
            return user == null ? throw new UserNotFoundException() : user.AuthToken;
        }
    }
}
