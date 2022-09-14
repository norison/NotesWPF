using System.Windows;
using NotesWPF.UI.Constants;
using NotesWPF.UI.Events;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NotesWPF.UI.ViewModels;

public class MainViewModel : BindableBase
{
    public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        regionManager.RegisterViewWithRegion(Regions.MainRegion, Pages.NoteListPage);
        eventAggregator.GetEvent<ProgressBarEvent>().Subscribe(ProgressBarEventHandler);
    }

    public Visibility ProgressBarVisibility { get; set; } = Visibility.Hidden;

    private void ProgressBarEventHandler(bool isEnable)
    {
        ProgressBarVisibility = isEnable ? Visibility.Visible : Visibility.Hidden;
    }
}