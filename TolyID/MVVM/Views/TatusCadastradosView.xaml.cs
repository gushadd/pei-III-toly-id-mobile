using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using TolyID.MVVM.Models;
using TolyID.MVVM.ViewModels;

namespace TolyID.MVVM.Views;  

public partial class TatusCadastradosView : ContentPage
{
    private TatusCadastradosViewModel _viewModel = new();
	public TatusCadastradosView()
	{
		InitializeComponent();
        BindingContext = _viewModel;
	}

    private void AdicionarTatu_Clicked(object sender, EventArgs e)
    {
        var popup = new CadastroTatuPopup();
        popup.TatuAdicionado += Popup_TatuAdicionado;
        this.ShowPopup(popup);
    }

    private async void Popup_TatuAdicionado(object sender, EventArgs e)
    {
        await _viewModel.BuscaTatusNoBanco();
    }

    private async void Tatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var tatuSelecionado = e.CurrentSelection[0] as TatuModel;

            if(tatuSelecionado != null)
            {
                Debug.WriteLine($"{tatuSelecionado.IdentificacaoAnimal}");
                await Navigation.PushAsync(new TatuView(tatuSelecionado));
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }

    private async void Atualizar_Clicked(object sender, EventArgs e)
    {
        await _viewModel.BuscaTatusNoBanco();
    }

    protected async override void OnAppearing()
    {
        await _viewModel.BuscaTatusNoBanco();
    }
}