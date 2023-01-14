using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Database.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public interface ITrackService
    {
        Task<string> Unfavorite(string userId, long trackId, PlaylistTrack domainTrack);
        Task<string> Favorite(string userId, long trackId, PlaylistTrack domainTrack);

    }

    public class TrackService : ITrackService
    {
        private readonly IUserPlaylistRepository _userPlaylist;
        private readonly ITrackRepository _track;
        private readonly IPlaylistRepository _playlist;
        public TrackService(IUserPlaylistRepository userPlaylist, ITrackRepository track, IPlaylistRepository playlist) 
        {
            _userPlaylist = userPlaylist;
            _track = track;
            _playlist = playlist;
        }


        public async Task<string> Unfavorite(string userId, long trackId, PlaylistTrack domainTrack)
        {
            var InfoMessage = "";
            var userPlaylist = await _userPlaylist.GetFavoritePlaylist(userId);
            if (userPlaylist != null)
            {
                var status = await _track.RemoveTrack(userPlaylist.Playlist.PlaylistId, trackId);
                if (status)
                {
                    InfoMessage = $"Track {domainTrack.ArtistName} - {domainTrack.AlbumTitle} - {domainTrack.TrackName} removed from playlist 'My favorite tracks'.";
                }
                else
                {
                    InfoMessage = $"Track {domainTrack.ArtistName} - {domainTrack.AlbumTitle} - {domainTrack.TrackName} does not exist.";
                }
            }
            else
            {
                InfoMessage = $"User has no Favorite playlist.";

            }

            return InfoMessage;
        }

        public async Task<string> Favorite(string userId, long trackId, PlaylistTrack domainTrack)
        {
            var InfoMessage = "";
            long SelectedPlaylistId;
            var userPlaylist = await _userPlaylist.GetFavoritePlaylist(userId);
            if (userPlaylist == null)
            {
                var favoritePlaylist = await _playlist.AddPlaylist("My favorite tracks");
                userPlaylist = await _userPlaylist.AddPlaylist(new UserPlaylist { UserId = userId, PlaylistId = favoritePlaylist.PlaylistId });
                SelectedPlaylistId = favoritePlaylist.PlaylistId;
            }
            else
            {
                SelectedPlaylistId = userPlaylist.Playlist.PlaylistId;
            }

            var status = await _track.AddTrack(SelectedPlaylistId, trackId);
            if (status)
            {
                InfoMessage = $"Track {domainTrack.ArtistName} - {domainTrack.AlbumTitle} - {domainTrack.TrackName} added to playlist 'My favorite tracks'.";
            }
            else
            {
                InfoMessage = $"Track {domainTrack.ArtistName} - {domainTrack.AlbumTitle} - {domainTrack.TrackName} does not exist.";
            }

            return InfoMessage;
        }
    }
}
