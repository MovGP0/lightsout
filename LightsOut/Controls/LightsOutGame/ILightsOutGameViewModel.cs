using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LightsOut
{
    public interface ILightsOutGameViewModel : INotifyPropertyChanged
    {
        void NextLevel();
        void ResetLevel();
        void ResetGame();
        void Setup();
        void SetLevel(int level);
        int Rows { get; }
        int Columns { get; }
        bool IsGameWon { get; }
        ObservableCollection<ISwitchViewModel> SwitchViewModels { get; }
    }
}