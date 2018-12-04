using NzbDrone.Core.Messaging.Commands;

namespace NzbDrone.Core.MediaCover
{
    public class ResizeTestCommand : Command
    {
        public int MovieId { get; set; }
        
        public string ImagePath { get; set; }
    }
}
