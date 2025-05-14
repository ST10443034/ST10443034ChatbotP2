using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Threading;

namespace St10443034Part1
{
    internal class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private static string userName = "";
        private static bool useVoice = true;
        private static bool useSoundEffects = true;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;

            // Play welcome sound and voice greeting
            PlayVoiceGreeting();

            // Display welcome screen with ASCII art
            DisplayWelcomeScreen();

            // Get user's name
            GetUserName();

            // Main interaction loop
            RunChatbot();

            Console.ResetColor();
            Console.WriteLine("\nThank you for using the Cybersecurity Awareness Chatbot. Stay safe online!");
            if (useVoice) synth.Speak("Thank you for using the Cybersecurity Awareness Chatbot. Stay safe online!");
        }

        static void PlayVoiceGreeting()
        {
            try
            {
                // Play system sound
                if (useSoundEffects) SystemSounds.Beep.Play();

                // Text-to-speech greeting
                if (useVoice)
                {
                    synth.Speak("Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing voice greeting: " + ex.Message);
            }
        }

        private static void DisplayWelcomeScreen()
        {
            Console.Clear();

            // ASCII Art Banner - JeffBot themed
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"      _____ ______  _____  _______ _           _   ");
            Console.WriteLine(@"     |  ___|  _  \|  ___||__   __| |         | |  ");
            Console.WriteLine(@"     | |__ | | | || |__     | |  | |__   ___ | |_ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"     |  __|| | | ||  __|    | |  | '_ \ / _ \| __|");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"     | |___| |/ / | |___    | |  | | | | (_) | |_ ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"     \____/|___/  \____/    |_|  |_| |_|\___/ \__|");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("==================================================================");
            Console.WriteLine("|                   CYBERSECURITY AWARENESS BOT                  |");
            Console.WriteLine("|               Powered by JeffBot Technology                   |");
            Console.WriteLine("==================================================================");
            Console.ResetColor();
        }



        private static void GetUserName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nBefore we begin, what's your name? ");
            Console.ResetColor();

            userName = Console.ReadLine();

            // Input validation for name
            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid name:");
                Console.ResetColor();
                userName = Console.ReadLine();
            }

            // Personalize the greeting
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nHello {userName}! Let's talk about cybersecurit" +
                $"y.");

            Console.ResetColor();

            if (useVoice)
            {
                synth.Speak($"Hello {userName}! Let's talk about cyber security.");
            }
        }

        private static void RunChatbot()
        {
            var chatbot = new CyberSecurityChatbot();

            // Display help message
            DisplayHelp();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\n{userName}: ");
                Console.ResetColor();

                var input = Console.ReadLine();

                // Input validation
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid question or topic.");
                    Console.ResetColor();
                    continue;
                }

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    DisplayHelp();
                    continue;
                }

                // Get response with typing effect
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Bot: ");
                Console.ResetColor();

                var response = chatbot.GetResponse(input);
                TypeWriterEffect(response);

                // Voice response
                if (useVoice)
                {
                    synth.Speak(response);
                }
            }
        }

        private static void DisplayHelp()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nI can help with these cybersecurity topics:");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("- Password safety");
            Console.WriteLine("- Phishing scams");
            Console.WriteLine("- Safe browsing");
            Console.WriteLine("- Social media security");
            Console.WriteLine("- Banking fraud protection");
            Console.WriteLine("- SIM swap fraud prevention");
            Console.WriteLine("\nType 'help' to see this again or 'exit' to quit");
            Console.ResetColor();
        }

        private static void TypeWriterEffect(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(20); // Adjust speed as needed
            }
            Console.WriteLine();
        }
    }

    public class CyberSecurityChatbot
    {
        public List<CyberSecurityTopic> Topics { get; set; }

        public CyberSecurityChatbot()
        {
            // Initialize with cybersecurity topics
            Topics = new List<CyberSecurityTopic>
            {
                // Greetings
                new CyberSecurityTopic
                {
                    Keywords = new[] {"hello", "hi", "greetings"},
                    Response = "Hello! I'm here to help with cybersecurity awareness. What would you like to know about?"
                },
                new CyberSecurityTopic
                {
                    Keywords = new[] {"how are you", "how's it going"},
                    Response = "I'm just a chatbot, but I'm functioning well! How can I help you with cybersecurity today?"
                },
                
                // Purpose
                new CyberSecurityTopic
                {
                    Keywords = new[] {"purpose", "what do you do", "why are you here"},
                    Response = "My purpose is to educate South African citizens about cybersecurity risks and how to stay safe online."
                },
                
                // Password safety
                new CyberSecurityTopic
                {
                    Keywords = new[] {"password", "secure password", "password safety"},
                    Response = "For strong passwords: 1) Use at least 12 characters, 2) Combine letters, numbers and symbols, " +
                              "3) Don't reuse passwords, 4) Consider a password manager, 5) Enable two-factor authentication."
                },
                
                // Phishing
                new CyberSecurityTopic
                {
                    Keywords = new[] {"phishing", "email scam", "banking fraud"},
                    Response = "Phishing is a major threat in South Africa. Never click links in unsolicited emails. " +
                              "Legitimate banks will never ask for your PIN or password via email. Check sender addresses carefully."
                },
                
                // Safe browsing
                new CyberSecurityTopic
                {
                    Keywords = new[] {"safe browsing", "internet safety", "secure browsing"},
                    Response = "For safe browsing: 1) Look for HTTPS in URLs, 2) Keep browser updated, " +
                              "3) Use ad-blockers, 4) Avoid public WiFi for sensitive transactions, " +
                              "5) Be cautious of 'too good to be true' offers."
                },
                
                // Social media
                new CyberSecurityTopic
                {
                    Keywords = new[] {"social media", "facebook", "whatsapp", "instagram"},
                    Response = "Social media safety tips: 1) Review privacy settings, 2) Be careful what you share, " +
                              "3) Watch for impersonation scams, 4) Don't trust unsolicited messages, " +
                              "5) The 'Hi Mum' scam is common in SA - verify unusual requests."
                },
                
                // SIM swap
                new CyberSecurityTopic
                {
                    Keywords = new[] {"sim swap", "mobile fraud", "cellphone scam"},
                    Response = "SIM swap fraud prevention: 1) Set up SIM swap alerts with your provider, " +
                              "2) Use banking apps instead of SMS OTPs, 3) Report lost signal immediately, " +
                              "4) Be wary of phishing attempts for personal info."
                }
            };
        }

        public string GetResponse(string userInput)
        {
            var input = userInput.ToLower();

            // Check for exact matches first
            foreach (var topic in Topics)
            {
                if (topic.Keywords.Any(k => input.Equals(k, StringComparison.OrdinalIgnoreCase)))
                {
                    return topic.Response;
                }
            }

            // Check for partial matches
            foreach (var topic in Topics)
            {
                if (topic.Keywords.Any(k => input.Contains(k)))
                {
                    return topic.Response;
                }
            }

            // Default response
            return "I'm not sure I understand. Could you try asking about: password safety, phishing scams, or safe browsing? Type 'help' for more options.";
        }
    }

    public class CyberSecurityTopic
    {
        public string[] Keywords { get; set; }
        public string Response { get; set; }
    }
}