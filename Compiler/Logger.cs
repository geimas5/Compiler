namespace Compiler
{
    using System;
    using System.Collections.Generic;

    using Compiler.SyntaxTree;

    public class Logger
    {
        private readonly List<Message> messages = new List<Message>();

        public int TotalErrors { get; private set; }

        public int TotalWarnings { get; private set; }

        public int TotalInfo { get; private set; }

        public void LogError(Location location, string messageFormat, params object[] parameters)
        {
            var msg = new Message(location, MessageLevel.Error, string.Format(messageFormat, parameters));
            this.messages.Add(msg);
            this.TotalErrors++;
        }

        public void LogError(Location location, string message)
        {
            var msg = new Message(location, MessageLevel.Error, message);
            this.messages.Add(msg);
            this.TotalErrors++;
        }

        public void LogWarning(Location location, string message)
        {
            var msg = new Message(location, MessageLevel.Warning, message);
            this.messages.Add(msg);
            this.TotalWarnings++;
        }

        public void LogInfo(Location location, string message)
        {
            var msg = new Message(location, MessageLevel.Info, message);
            this.messages.Add(msg);
            this.TotalInfo++;
        }

        public void PrintMessages()
        {
            foreach (var message in this.messages)
            {
                this.PrintMessage(message);
            }
        }

        private void PrintMessage(Message message)
        {
            Console.WriteLine(
                "{0} Line {1} Col {2}: {3}",
                message.MessageLevel,
                message.Location.Line,
                message.Location.Column,
                message.MessageBody);
        }

        private class Message
        {
            public Message(Location location, MessageLevel messageLevel, string messageBody)
            {
                this.Location = location;
                this.MessageLevel = messageLevel;
                this.MessageBody = messageBody;
            }

            public Location Location { get; private set; }

            public string MessageBody { get; private set; }

            public MessageLevel MessageLevel { get; private set; }
        }

        private enum MessageLevel
        {
            Error,
            Warning,
            Info
        }
    }
}
