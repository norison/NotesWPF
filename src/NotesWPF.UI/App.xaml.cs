using System.IO;
using System.Windows;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using NotesWPF.DataAccess;
using NotesWPF.DataAccess.Models;
using NotesWPF.DataAccess.Services.File;
using NotesWPF.DataAccess.Services.NotesLoader;
using NotesWPF.DataAccess.Services.SemaphoreSlim;
using NotesWPF.DataAccess.Services.Serialization;
using NotesWPF.DataAccess.Settings;
using NotesWPF.Domain.Services.Notes;
using NotesWPF.Domain.Validators;
using NotesWPF.UI.Models;
using NotesWPF.UI.Views;
using Prism.Ioc;

namespace NotesWPF.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ConfigureServices(containerRegistry);
            ConfigureSettings(containerRegistry);
            ConfigureMappings(containerRegistry);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        private static void ConfigureServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileService, FileService>();
            containerRegistry.RegisterSingleton<ISemaphoreSlimFactory, SemaphoreSlimFactory>();
            containerRegistry.RegisterSingleton<ISerializationService, SerializationService>();
            containerRegistry.RegisterSingleton<INotesLoader, NotesLoader>();
            containerRegistry.RegisterSingleton<INotesService, NotesService>();
            containerRegistry.RegisterSingleton<IValidator<Note>, NoteValidator>();
            containerRegistry.RegisterSingleton<INotesRepository, NotesRepository>();
        }

        private static void ConfigureSettings(IContainerRegistry containerRegistry)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            containerRegistry.RegisterSingleton<RepositorySettings>(() =>
                configuration.GetSection("RepositorySettings").Get<RepositorySettings>());
        }

        private static void ConfigureMappings(IContainerRegistry containerRegistry)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.CreateMap<NoteModel, Note>();
                config.CreateMap<Note, NoteModel>();
            });

            containerRegistry.RegisterSingleton<IMapper>(() => configuration.CreateMapper());
        }
    }
}