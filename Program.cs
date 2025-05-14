using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Threading;

namespace St10443034Part2
{
    internal class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private static string userName = "";
        private static string favouriteTopic = "";
        private static bool useVoice = true;
        private static bool useSoundEffects = true;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;

            PlayVoiceGreeting();
            DisplayWelcomeScreen();
            GetUserName();
            RunChatbot();

            Console.ResetColor();
            Console.WriteLine("\nThank you for using the Cybersecurity Awareness Chatbot. Stay safe online!");
            if (useVoice) synth.Speak("Thank you for using the Cybersecurity Awareness Chatbot. Stay safe online!");
        }

        static void PlayVoiceGreeting()
        {
            try
            {
                if (useSoundEffects) SystemSounds.Beep.Play();

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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"   ____        _   _           _           _            ");
            Console.WriteLine(@"  / ___| _   _| |_| |__   ___ | |__   ___ | |_ ___ _ __ ");
            Console.WriteLine(@"  \___ \| | | | __| '_ \ / _ \| '_ \ / _ \| __/ _ \ '__|");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"   ___) | |_| | |_| | | | (_) | |_) | (_) | ||  __/ |   ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"  |____/ \__, |\__|_| |_|\___/|_.__/ \___/ \__\___|_|   ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"         |___/                                          ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n==================================================================");
            Console.WriteLine("|              CYBERSECURITY AWARENESS CHATBOT                   |");
            Console.WriteLine("|         Now with memory, mood detection, and more!             |");
            Console.WriteLine("==================================================================");
            Console.ResetColor();
        }

        private static void GetUserName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nWhat's your name? ");
            Console.ResetColor();

            userName = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Please enter a valid name: ");
                Console.ResetColor();
                userName = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nHello {userName}! Let's talk about cybersecurity.");
            Console.ResetColor();

            if (useVoice)
            {
                synth.Speak($"Hello {userName}, let's talk about cybersecurity.");
            }
        }

        private static void RunChatbot()
        {
            var chatbot = new CyberSecurityChatbot();

            DisplayHelp();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\n{userName}: ");
                Console.ResetColor();

                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter something.");
                    Console.ResetColor();
                    continue;
                }

                input = input.ToLower();

                if (input == "exit") break;

                if (input == "help")
                {
                    DisplayHelp();
                    continue;
                }

                string response = chatbot.GetResponse(input, out string topicDetected, out string sentiment);

                // Memory: Save favourite topic if detected
                if (!string.IsNullOrEmpty(topicDetected) && string.IsNullOrEmpty(favouriteTopic))
                {
                    favouriteTopic = topicDetected;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[Memory] Noted that you're interested in {favouriteTopic}.");
                    Console.ResetColor();
                }

                // Sentiment detection handling
                if (!string.IsNullOrEmpty(sentiment))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"[Mood Detected: {sentiment}]");
                    Console.ResetColor();
                }

 

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Bot: ");
                Console.ResetColor();

                TypeWriterEffect(response);
                if (useVoice) synth.Speak(response);
            }
        }

        private static void DisplayHelp()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nI can help with these cybersecurity topics:");
            Console.WriteLine("- Password safety");
            Console.WriteLine("- Phishing scams");
            Console.WriteLine("- Privacy and data protection");
            Console.WriteLine("- Safe browsing");
            Console.WriteLine("- Social media security");
            Console.WriteLine("- SIM swap fraud");
            Console.WriteLine("Type 'help' to see this list again or 'exit' to quit.");
            Console.ResetColor();
        }

        private static void TypeWriterEffect(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(20);
            }
            Console.WriteLine();
        }
    }

    public class CyberSecurityChatbot
    {
        private Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>();
        private Dictionary<string, string> sentiments = new Dictionary<string, string>();

        public CyberSecurityChatbot()
        {
            keywordResponses["password"] = new List<string>
            {
                "Use strong and unique passwords with letters, numbers, and symbols.",
                "Avoid using personal info in your passwords and update them regularly.",
                "Consider a password manager to keep track of different passwords securely."
            };

            keywordResponses["phishing"] = new List<string>
            {
                "Phishing emails often pretend to be from banks—double-check the sender!",
                "Never click suspicious links. Always verify before taking action.",
                "Use multi-factor authentication to reduce damage from phishing attacks."
            };

            keywordResponses["privacy"] = new List<string>
            {
                "Review your privacy settings on all social media platforms.",
                "Be mindful of what you post publicly online.",
                "Use encrypted communication apps to enhance your privacy."
            };

            keywordResponses["scam"] = new List<string>
            {
                "Scams can be very convincing. Always verify requests before responding.",
                "If it sounds too good to be true, it probably is.",
                "Block and report scam messages immediately."
            };

            sentiments["worried"] = "It's completely understandable to feel that way. Let's look at some tips to ease your concerns.";
            sentiments["frustrated"] = "Sorry to hear that. Cybersecurity can be confusing, but I'm here to help.";
            sentiments["curious"] = "That's great! Curiosity is the first step to learning how to stay safe online.";
        }

        public string GetResponse(string userInput, out string topicDetected, out string sentimentDetected)
        {
            topicDetected = "";
            sentimentDetected = "";

            // Sentiment Detection
            foreach (var mood in sentiments)
            {
                if (userInput.Contains(mood.Key))
                {
                    sentimentDetected = mood.Key;
                    return sentiments[mood.Key];
                }
            }

            // Keyword Recognition
            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInput.Contains(keyword))
                {
                    topicDetected = keyword;
                    var responses = keywordResponses[keyword];
                    var random = new Random();
                    return responses[random.Next(responses.Count)];
                }
            }

            return "I'm not sure I understand. Can you try rephrasing or type 'help' to see the list of topics I cover?";
        }
    }
}
