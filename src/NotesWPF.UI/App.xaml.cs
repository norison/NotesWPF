using System.IO;
using System.Windows;
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
            containerRegistry.Register<IFileService, FileService>();
            containerRegistry.Register<ISemaphoreSlimFactory, SemaphoreSlimFactory>();
            containerRegistry.Register<ISerializationService, SerializationService>();
            containerRegistry.Register<INotesLoader, NotesLoader>();
            containerRegistry.Register<INotesService, NotesService>();
            containerRegistry.Register<IValidator<Note>, NoteValidator>();

            containerRegistry.RegisterSingleton<INotesRepository, NotesRepository>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            containerRegistry.RegisterSingleton<RepositorySettings>(() =>
                configuration.GetSection("RepositorySettings").Get<RepositorySettings>());
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
    }
}