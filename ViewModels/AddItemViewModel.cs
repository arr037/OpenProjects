﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.Win32;
using Newtonsoft.Json;
using OpenProjects.Crypt;
using OpenProjects.Model;

namespace OpenProjects.ViewModels
{
    internal class AddItemViewModel : BaseVm
    {
        private string _author = "Автор";
        private string _deksription = "Описание";
        private string _filepath = "Выберите путь ...";
        private bool _isEnabled = true;
        private string _miniDesk = "Краткое описание";
        private string _title = "Новый заголовок";
        private ObservableCollection<Item> Items;
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        public AddItemViewModel()
        {
            Items = File.Exists("Data/Items.json")
                ? JsonConvert.DeserializeObject<ObservableCollection<Item>>(
                    Crypter.Decrypt(File.ReadAllText("Data/Items.json"), true))
                : new ObservableCollection<Item>();
        }

        public string FilePath
        {
            get => _filepath;
            set
            {
                _filepath = value;
                RaisePropertyChanged("Изменения");
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("Изменения isenabled");
            }
        }

        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                RaisePropertyChanged("Изменения автора");
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Изменения МиниОписание");
            }
        }

        public string Deskription
        {
            get => _deksription;
            set
            {
                _deksription = value;
                RaisePropertyChanged("Изменения заголовок");
            }
        }

        public string MiniDesk
        {
            get => _miniDesk;
            set
            {
                _miniDesk = value;
                RaisePropertyChanged("Изменения МиниЗаголовок");
            }
        }

        public ICommand OpenFileDialog
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    string currectDirectory = Environment.CurrentDirectory;
                    openFileDialog.InitialDirectory = currectDirectory;
                    if (openFileDialog.ShowDialog() == true && openFileDialog.FileName.Contains(currectDirectory))
                    {
                        string currectFile = openFileDialog.FileName;
                        FilePath = currectFile.Remove(0, currectDirectory.Length);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Ошибка\nВозможно вы пытаетесь открыть файл который не находится в папке с проектом!",
                            "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                });
            }
        }

        public ICommand SaveInfo
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Messenger.Default.Send(
                        new Message(0, MessageType.Added, Title, MiniDesk, Deskription, FilePath, author: Author,
                            _isEdit: IsEnabled));
                });
            }
        }
    }
}