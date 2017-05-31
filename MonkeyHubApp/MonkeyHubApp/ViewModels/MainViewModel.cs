using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using MonkeyHubApp.Models;
using System;
using MonkeyHubApp.Services;

namespace MonkeyHubApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {   
        private string _searchTerm;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (SetProperty(ref _searchTerm, value));
                    SearchCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<Tag> Resultados { get; }

        public Command SearchCommand { get; }

        public Command AboutCommand { get; }

        public Command<Tag> ShowCategoriaCommand { get; }

        private readonly IMonkeyHubApiService _monkeyHubApiService;

        public MainViewModel(IMonkeyHubApiService monkeyHubApiService)
        {
            _monkeyHubApiService = monkeyHubApiService;

            SearchCommand = new Command(ExecuteSearchCommand, CanExecuteSearchCommand);

            AboutCommand = new Command(ExecuteAboutCommand);

            ShowCategoriaCommand = new Command<Tag>(ExecuteShowCategoriaCommand);

            Resultados = new ObservableCollection<Tag>();
        }

        private async void ExecuteShowCategoriaCommand(Tag tag)
        {
            await PushAsync<CategoriaViewModel>(_monkeyHubApiService, tag);
        }

        async void ExecuteAboutCommand()
        {
            await PushAsync<AboutViewModel>();
        }

        async void ExecuteSearchCommand()
        {
            //await Task.Delay(2000);

            bool resposta = await App.Current.MainPage.DisplayAlert("MonkeyHubApp", $"Você pesquisou por '{SearchTerm}'?", "Sim", "Não");

            if (resposta)
            {
                await App.Current.MainPage.DisplayAlert("MonkeyHubApp", "Obrigado!", "OK");     

                var tagsRetornadasDoServico = await _monkeyHubApiService.GetTagAsync();

                Resultados.Clear();

                if (tagsRetornadasDoServico != null)
                {
                    foreach (var tag in tagsRetornadasDoServico)
                    {
                        Resultados.Add(tag);
                    }
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("MonkeyHubApp", "De Nada!", "OK");
                Resultados.Clear();
            }
        }

        bool CanExecuteSearchCommand()
        {
            return string.IsNullOrWhiteSpace(SearchTerm) == false;
        }
    }
}
