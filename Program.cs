using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Threading;

namespace St10443034ChatbotP2
{
    internal class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private static string userName = "";
        private static string favouriteTopic = "";
        private static bool useVoice = true;
        private static bool useSoundEffects = true;
        private static string lastTopic = "";

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
            if (useSoundEffects) SystemSounds.Beep.Play();
            if (useVoice) synth.Speak("Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.");
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

            if (useVoice) synth.Speak($"Hello {userName}, let's talk about cybersecurity.");
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

                var input = Console.ReadLine()?.ToLower().Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter something.");
                    Console.ResetColor();
                    continue;
                }

                if (input == "exit") break;
                if (input == "help")
                {
                    DisplayHelp();
                    continue;
                }

                string response = chatbot.GetResponse(input, out string topicDetected, out string sentiment);

                // Memory
                if (!string.IsNullOrEmpty(topicDetected))
                {
                    lastTopic = topicDetected;

                    if (string.IsNullOrEmpty(favouriteTopic))
                    {
                        favouriteTopic = topicDetected;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[Memory] Noted that you're interested in {favouriteTopic}.");
                        Console.ResetColor();
                    }
                }

                // Sentiment detection
                if (!string.IsNullOrEmpty(sentiment))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"[Mood Detected: {sentiment}]");
                    Console.ResetColor();
                }

                // Personalized tip
                if (!string.IsNullOrEmpty(favouriteTopic) && input != "help" && !input.Contains(favouriteTopic))
                {
                    response += $"\nAs someone interested in {favouriteTopic}, you should explore this further!";
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
        private readonly Dictionary<string, List<string>> keywordResponses;
        private readonly Dictionary<string, string> sentiments;

        public CyberSecurityChatbot()
        {
            keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = new List<string>
                {
                    "Use strong and unique passwords with letters, numbers, and symbols.",
                    "Avoid using personal info in your passwords and update them regularly.",
                    "Consider using a password manager to keep your credentials safe."
                },
                ["phishing"] = new List<string>
                {
                    "Phishing emails often pretend to be from banks—double-check the sender!",
                    "Never click suspicious links. Always verify before taking action.",
                    "Use multi-factor authentication to reduce phishing risks."
                },
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings on all your online accounts regularly.",
                    "Be cautious of the personal information you share online.",
                    "Use encrypted communication tools for more secure chats."
                },
                ["scam"] = new List<string>
                {
                    "Watch out for messages asking for urgent payments or gift cards.",
                    "Always verify unknown callers and emails before responding.",
                    "Scams often involve emotional manipulation—stay calm and verify."
                }
            };

            sentiments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["worried"] = "It's okay to feel that way. Cybersecurity can seem overwhelming, but I'm here to guide you.",
                ["curious"] = "That’s great! Curiosity is key to learning more about online safety.",
                ["frustrated"] = "Don't worry, you're not alone. Many people feel this way when starting to learn cybersecurity."
            };
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
            foreach (var entry in keywordResponses)
            {
                if (userInput.Contains(entry.Key))
                {
                    topicDetected = entry.Key;
                    var responses = entry.Value;
                    return responses[new Random().Next(responses.Count)];
                }
            }

            return "I'm not sure I understand. Could you try rephrasing or type 'help' for suggestions?";
        }
    }
}
