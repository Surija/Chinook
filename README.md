# Chinook

This application is unfinished. Please complete below tasks. Spend max 3 hours. We would like to have a short written explanation of the changes you made.

1. Move data retrieval methods to separate class / classes (use dependency injection)

* Added 4 Interfaces Under Database -> Persistence and added data retrieval methods under the respective areas (Artist, Playlist, Track & UserPlaylist)
* Updated pages to call newly defined methods.
* Registered depency injection in the Program.cs file.
* Renamed ClientModel Playlist to PlaylistDto to avoid confusion

2. Favorite / unfavorite tracks. An automatic playlist should be created named "My favorite tracks"

* Added a Service called TrackService and registered it in Program.cs file.
* TrackService contains common methods shared by both the ArtistPage and PlaylistPage.
* Implemented methods to mark a track as favorite and unfavorite.
* If user has no Favorite playlist, one is created.

4. The user's playlists should be listed in the left navbar. If a playlist is added (or modified), this should reflect in the left navbar. There is already one playlist link in the Nav Menu as an example.

* Implemented method to get a list of all the user's playlists.
* Removed example and updated NavMenu.razor to loop over actual data.

3. Add tracks to a playlist (existing or new one). The dialog is already created but not yet finished.

* Removed default list of playlists and replaced with actual data
* Implemented methods to get a list of playlists to display in the modal
* Implemented method to add a track to a playlist
* Implemented a method to add a new playlist by binding name entered in the modal
* Implemented method to check if a user has a particular playlist 
* Implemented method to associate a playlist with the login user

5. The user should be able to remove tracks from the playlist.

* Implemented a method to remove a particular track from the selected playlist.

6. User should be able to rename the playlist
8. User should be able to remove the whole playlist

* Updated NavMenu.razor to include a delete icon next to each playlist.
* Implemented method to delete a playlist

7. Search for artist name

* Installed the Nuget Package Syncfusion.Blazor and registered it in Program.cs
* Replaced the table in Index.razor with SfGrid (this made it easy to filter the artist)
* Filtering or searching occurs immediately on key down 


When creating a user account, you will see this:
"This app does not currently have a real email sender registered, see these docs for how to configure a real email sender. Normally this would be emailed: Click here to confirm your account."
After you click 'Click here to confirm your account' you should be able to login.
