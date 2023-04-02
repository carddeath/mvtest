using FluentValidation;
using Microsoft.Extensions.Logging;
using MusicVineTest.Models;
using MusicVineTest.Services;
using Serilog;

namespace MusicVineTest
{
    public static class MusicEndoints
    {
        public static RouteGroupBuilder MapMusicApi(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetSongs);

            group.MapPost("/", AddSong)
                .AddEndpointFilter(async (context, next) =>
                {
                    var validator = context.HttpContext.RequestServices.GetService<IValidator<MusicEntry>>();
                    var entry = context.GetArgument<MusicEntry>(0);

                    if (validator is not null && entry is not null)
                    {
                        var validation = await validator.ValidateAsync(entry);
                        if (validation.IsValid)
                        {
                            return await next(context);
                        }
                        return Results.ValidationProblem(validation.ToDictionary());
                    }
                    return await next(context);

                });

            group.MapPut("/{id}", UpdateSong).AddEndpointFilter(async (context, next) =>
            {
                var validator = context.HttpContext.RequestServices.GetService<IValidator<MusicEntry>>();
                var entry = context.GetArgument<MusicEntry>(1);

                if (validator is not null && entry is not null)
                {
                    var validation = await validator.ValidateAsync(entry);
                    if (validation.IsValid)
                    {
                        return await next(context);
                    }
                    return Results.ValidationProblem(validation.ToDictionary());
                }
                return await next(context);
            });
            group.MapDelete("/{id}", DeleteSong);

            return group;
        }

        public static async Task<IResult> GetSongs(IMusicService musicService) 
        {

            var songs = await musicService.GetSongsAsync();

            if (songs is not null && songs.Count > 0) 
            {
                Log.Information($"Returning {songs.Count} songs");
                return TypedResults.Ok(songs);
            }

            Log.Information("No songs to return");
            return TypedResults.NoContent();
        }

        public static async Task<IResult> AddSong(MusicEntry entry, IMusicService musicService) 
        {
            Log.Information($"Adding song {entry.Name}");
            await musicService.AddAsync(entry);

            return TypedResults.Created("New song added: ", entry);
        }

        //TODO: This needs a EndpointFilter to check that the entry is valid 
        public static async Task<IResult> UpdateSong(int id, MusicEntry newEntry, IMusicService musicService) 
        {
            var existingSong = await musicService.FindMusicEntryAsync(id);

            if (existingSong is not null) 
            {
                Log.Information($"Updating song at id: {id} with new name {newEntry.Name}");
                existingSong.Name = newEntry.Name;
                existingSong.DurationSeconds = newEntry.DurationSeconds;

                await musicService.UpdateAsync(existingSong);

                Log.Information($"Song Updated");
                return TypedResults.Created("Modified song: ", existingSong);
            }
            Log.Information($"Updating song at id: {id} with new name {newEntry.Name}");
            return TypedResults.NotFound();
        }

        public static async Task<IResult> DeleteSong(int id, IMusicService musicService)
        { 
            var song = await musicService.FindMusicEntryAsync(id);
            Log.Information($"Found song at id: {id}");

            if(song is not null) 
            {
                await musicService.DeleteAsync(song);
                Log.Information($"Deleted song: {song.Name}");
                return TypedResults.NoContent();
            }
            Log.Information($"No song to delete at id: {id}");
            return TypedResults.NotFound();
        }
    }
}
