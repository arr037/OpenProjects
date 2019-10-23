using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.Win32;
using OpenProjects.Model;

namespace OpenProjects.ViewModels
{
    public class EditViewModel : BaseVm
    {
        private Item _selectItem;

        public Item SelectedItem
        {
            get => _selectItem;
            set
            {
                _selectItem = value;
                RaisePropertyChanged("Changed");
            }
        }

        public ICommand OpenFileDialog
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true) SelectedItem.FilePath = openFileDialog.FileName;
                });
            }
        }

        public ICommand SaveInfo
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Messenger.Default.Send(new Message(0, MessageType.Changed, selectItem: SelectedItem));
                });
            }
        }
    }
}