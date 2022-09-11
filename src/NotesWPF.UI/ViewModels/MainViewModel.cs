using System.Windows;
using NotesWPF.UI.Constants;
using NotesWPF.UI.Events;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NotesWPF.UI.ViewModels;

public class MainViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;

    public MainViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
        _regionManager.RegisterViewWithRegion(Regions.MainRegion, Pages.NoteListPage);

        _eventAggregator.GetEvent<ProgressBarEvent>().Subscribe(ProgressBarEventHandler);
    }

    public Visibility ProgressBarVisibility { get; set; } = Visibility.Hidden;

    private void ProgressBarEventHandler(bool isEnable)
    {
        ProgressBarVisibility = isEnable ? Visibility.Visible : Visibility.Hidden;
    }
}