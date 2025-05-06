using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models.Messages;
using SportsExerciseBattle.Utilities;
using SportsExerciseBattle.API.Endpoints.Initializer;

namespace SportsExerciseBattle.API
{
    public class ServerController
    {
        private static readonly object WriterLock = new();
        private readonly IServiceProvider _serviceProvider; // DI-Container
        private readonly HttpRequestHandler _requestHandler;
        private readonly HttpResponseHandler _responseHandler;
        private readonly HttpParser _httpParser;

        public ServerController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _requestHandler = new HttpRequestHandler();
            _httpParser = new HttpParser();
            _responseHandler = new HttpResponseHandler();

            EndpointInitializer.InitializeEndpoints(_serviceProvider, _requestHandler);
        }

        /*
        Multithreaded listening to incoming client requests.
        For every request a thread is created (and joined after running)
        */
        public void ListenAsync()
        {
            Console.WriteLine($"[Server] Listening on http://localhost:{ConstantsSettings.ServerPort}/");
            var server = new TcpListener(IPAddress.Any, ConstantsSettings.ServerPort);
            server.Start();

            while (true)
            {
                var client = server.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        /*
        Client requests are handled in parallel by using Tasks, see HandleRequestAsync
        */
        private void HandleClient(TcpClient client)
        {
            Console.WriteLine("[Server] Executing request");

            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(client.GetStream());

            try
            {
                var request = _httpParser.Parse(reader);

                var response = _requestHandler.HandleRequest(request);

                lock (WriterLock)
                    _responseHandler.SendResponse(writer, response);
                //_responseHandler.SendResponse(writer, response);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server] Error while trying to handle client requests: {ex.Message}");
                _responseHandler.SendResponse(writer, new Response(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
