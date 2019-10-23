using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using OpenProjects.Crypt;
using OpenProjects.Model;
using OpenProjects.View;

namespace OpenProjects.ViewModels
{
    public class MainViewModel : BaseVm
    {
        private View.About about;
        private Item _selectItem;
        private AddNewItem w;
        private EditItem w1;

        public MainViewModel()
        {
            OverlayService.GetInstance().Show = delegate(string str, string str1)
            {
                OverlayService.GetInstance().Text = str;
                OverlayService.GetInstance().Description = str1;
            };

            Items = File.Exists("Data/Items.json")
                ? JsonConvert.DeserializeObject<ObservableCollection<Item>>(
                    Crypter.Decrypt(File.ReadAllText("Data/Items.json"), true))
                : new ObservableCollection<Item>();
            Items.CollectionChanged += (s, e) =>
            {
                if (!File.Exists("Data")) Directory.CreateDirectory("Data");
                File.WriteAllText("Data/Items.json", Crypter.Encrypt(JsonConvert.SerializeObject(Items), true));
            };
            Messenger.Default.Register<Message>(this, OnMessage);
        }

        public ObservableCollection<Item> Items { get; set; }

        public Item SelectedItem
        {
            get => _selectItem;
            set
            {
                _selectItem = value;
                RaisePropertyChanged("Changed");
            }
        }

        public ICommand AddItem
        {
            get
            {
                return new DelegateCommand<Item>(async item =>
                {
                    SelectedItem = item;
                    await Task.Factory.StartNew(() =>
                    {
                        OverlayService.GetInstance().SelItem = SelectedItem;
                        OverlayService.GetInstance().Show(item.Title, item.Description);
                    });
                }, item => item != null);
            }
        }

        public ICommand RemoveDialogWindow
        {
            get
            {
                return new DelegateCommand<Item>(item =>
                {
                    if (MessageBox.Show("Вы точно хотите удалить?", "Вопрос", MessageBoxButton.YesNo,
                            MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        Items.Remove(item);
                        OverlayService.GetInstance().Close();
                        SelectedItem = null;
                    }
                }, item => item != null);
            }
        }

        public ICommand OpenFile
        {
            get
            {
                return new DelegateCommand<Item>(item =>
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(item.FilePath));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Ошибка");
                    }
                }, item => !item.FilePath.Contains("Выберите путь"));
            }
        }

        public ICommand CloseDialogWindow
        {
            get { return new DelegateCommand(() => { OverlayService.GetInstance().Close(); }); }
        }

        public ICommand AddNewItem
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    w = new AddNewItem();
                    var vm = new AddItemViewModel();
                    w.DataContext = vm;
                    w.ShowDialog();
                });
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new DelegateCommand<Item>(item =>
                {
                    w1 = new EditItem();
                    var vm = new EditViewModel
                    {
                        SelectedItem = SelectedItem
                    };
                    w1.DataContext = vm;
                    w1.ShowDialog();
                });
            }
        }

        public ICommand OpenUrl
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        about = new View.About();
                        about.DataContext = this;
                        about.ShowDialog();
                        //Process.Start(new ProcessStartInfo("https://vk.com/arr073099"));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Ошибка");
                    }
                });
            }
        }

        private void OnMessage(Message message)
        {
            switch (message.MessageType)
            {
                case MessageType.Added:
                    var check = GetCheckData(message.Title);
                    if (check == false)
                    {
                        Items.Add(new Item
                        {
                            Title = message.Title,
                            FilePath = message.Filepath,
                            Description = message.Description,
                            MiniDescription = message.miniDescription,
                            Author = message.Author,
                            IsEdit = message.IsEdit
                        });

                        w.Close();
                    }
                    else
                    {
                        MessageBox.Show("Такой элемент уже существует", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }

                    break;
                case MessageType.Changed:
                    var oldItem = Items.IndexOf(SelectedItem);
                    Items[oldItem] = message.SelectItem;
                    w1.Close();
                    break;
            }
        }

        public ICommand CloseAboutWindow
        {
            get
            {
                return  new DelegateCommand(() =>
                {
                    about.Close();
                });
            }
        }

        private bool GetCheckData(string text)
        {
            if (Items.Any(p => p.Title == text)) return true;
            return false;
        }
    }
}