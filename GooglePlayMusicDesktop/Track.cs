namespace GooglePlayMusicDesktop
{
    public class Track
    {
        public string Title { get; internal set; }
        public string Artist { get; internal set; }
        public string Album { get; internal set; }
        public string AlbumArtUrl { get; internal set; }
        public int Duration { get; internal set; }
        public int CurrentTime { get; internal set; }
        public Rating Rating { get; internal set; }
        public string Lyrics { get; internal set; }
    }
}
