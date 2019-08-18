using System.Collections.Generic;
using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    class LibraryDto
    {
        [JsonProperty("albums")]
        public List<LibraryAlbumDto> Albums { get; set; }
    }

    class LibraryAlbumDto : AlbumDto
    {
        [JsonProperty("isAlbumArtist")]
        public bool IsAlbumArtist { get; set; }

        [JsonProperty("tracks")]
        public PlaylistTrackDto[] TracksDto { get; set; }
    }
}
