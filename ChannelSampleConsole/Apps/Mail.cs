using System.Collections.Generic;

namespace ChannelSampleConsole.Apps
{
    public class Mail
    {
        public string From { get; set; }
        public string To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public Dictionary<string, object> Metadata { get; set; }

        public List<string> Attachments { get; set; }
    }
}
