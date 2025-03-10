﻿// TCPKeyb | <https://tcpkeyb.pixelra.in>
// Copyright (c) 2021 Pixel Rain
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY - without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.

using System.Net;
using System.Threading;
using System.Drawing;
using Console = Colorful.Console;
using System;

namespace TCPKeyb
{
    class Client
    {
        /// <summary>
        /// Setup the client information
        /// </summary>
        public void StartClientSetup()
        {
            Header.Draw();

            // User entered IP
            string ip = SetupIP();
            Header.Draw();

            // User entered Port
            int port = SetupPort(ip);

            ConfirmResponses(ip, port);
        }


        /// <summary>
        /// Confirm user entries
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void ConfirmResponses(string ip, int port)
        {
            // Reset window
            Header.Draw();

            Console.Write("\tConnection details are: ");
            Console.Write($"{ip}:{port}", Color.Aquamarine);
            
            Console.WriteLine("");
            Console.WriteLine("");

            Console.Write("\tIs this correct? ");
            Console.WriteLine("[y/n]", Color.DarkOrange);
            Console.Write("\n\t");

            ConsoleKey response = Console.ReadKey().Key;

            // Ask if sure
            if (response == ConsoleKey.N)
                StartClientSetup();
            if (response == ConsoleKey.Y)
                StartKeyboardListener(ip, port);
            else
                ConfirmResponses(ip, port);
        }


        /// <summary>
        /// Starts a thread with the keyboard hook and client connection
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void StartKeyboardListener(string ip, int port)
        {
            Thread keyHook = new Thread(() =>
            ClientConnection.InitKeyboardHookClient(ip, port));
            keyHook.Start();

            while (keyHook.IsAlive)
            {
                Console.CursorVisible = false;
                Console.ReadKey(true);
            }
        }


        /// <summary>
        /// Asks the user for the port number
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int SetupPort(string ip)
        {
            int port = 0;
            bool portOK = false;

            string portPrompt = $"\tWhich port number for {ip}?";
            Console.WriteLine(portPrompt);
            Console.Write("\n\t");


            while (!portOK)
            {

                string response = Console.ReadLine();

                if (!int.TryParse(response, out port)
                    || (port <= 0 || port > 65535))
                {
                    Header.Draw();
                    Console.WriteLine(portPrompt);
                    Console.Write("\n\t");
                }
                else if (port >= 0 && port <= 65535)
                {
                    portOK = true;
                }
            }

            return port;
        }


        /// <summary>
        /// Asks the user for the IP Address
        /// </summary>
        /// <returns>The IP address</returns>
        public string SetupIP()
        {
            IPAddress ip = null;

            string ipPrompt = "\tWhat's the IP address of the server you are connecting to?";
            Console.WriteLine(ipPrompt);
            Console.Write("\n\t");

            while (ip == null)
            {
                if (!IPAddress.TryParse(Console.ReadLine(), out ip))
                {
                    Header.Draw();
                    Console.WriteLine(ipPrompt);
                    Console.Write("\n\t");
                }
            }

            return ip.ToString();
        }
    }
}
