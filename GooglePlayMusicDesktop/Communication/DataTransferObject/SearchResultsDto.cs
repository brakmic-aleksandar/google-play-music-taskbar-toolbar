using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    class SearchResultsDto
    {
        [JsonProperty("searchText")]
        public string SearchText { get; set; }

        [JsonProperty("albums")]
        public AlbumDto[] AlbumsDto { get; set; }

        [JsonProperty("artists")]
        public ArtistDto[] ArtistsDto { get; set; }

        [JsonProperty("tracks")]
        public PlaylistTrackDto[] TracksDto { get; set; }
    }
}
