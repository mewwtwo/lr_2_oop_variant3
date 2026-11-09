using System.Xml;
using Hurto.Core;

namespace Hurto;

public partial class MainPage : ContentPage
{
    private const string EmptyHtml =
        "<html><head><meta charset=\"utf-8\" /><style>" +
        "html,body{margin:0;background:#FFFFFF;color:#000000}" +
        "@media (prefers-color-scheme: dark){html,body{background:#1E1E1E;color:#EEEEEE}}" +
        "</style></head><body></body></html>";

    private readonly HurtoFacade _facade = new();

    public MainPage()
    {
        InitializeComponent();
        HtmlView.Source = new HtmlWebViewSource { Html = EmptyHtml };
    }

    private async void OnOpenFile(object? sender, EventArgs e)
    {
        var xmlType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.MacCatalyst, new[] { "public.xml" } },
            { DevicePlatform.WinUI, new[] { ".xml" } }
        });

        var file = await FilePicker.Default.PickAsync(
            new PickOptions { PickerTitle = "Оберіть XML-файл", FileTypes = xmlType });
        if (file == null) return;

        try
        {
            _facade.LoadFile(file.FullPath);
            ClearFields();
            FileLabel.Text = file.FileName;
            AttributeList.ItemsSource = _facade.GetAttributeNames().ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private void OnAttributeChanged(object? sender, SelectionChangedEventArgs e)
    {
        ValueList.ItemsSource = AttributeList.SelectedItem is string attr
            ? _facade.GetValues(attr).ToList()
            : null;
    }

    private async void OnSearch(object? sender, EventArgs e)
    {
        if (AttributeList.SelectedItem is not string attr || ValueList.SelectedItem is not string value)
        {
            await DisplayAlert("Увага", "Оберіть файл, атрибут і його значення.", "OK");
            return;
        }
        string method = MethodPanel.Children.OfType<RadioButton>().First(r => r.IsChecked).Content.ToString()!;
        OutputEditor.Text = _facade.Search(method, attr, value);
    }

    private async void OnTransform(object? sender, EventArgs e)
    {
        if (_facade.FilePath == null)
        {
            await DisplayAlert("Увага", "Спочатку оберіть XML-файл.", "OK");
            return;
        }
        string attr = AttributeList.SelectedItem as string ?? "";
        string value = ValueList.SelectedItem as string ?? "";

        using var stream = await FileSystem.OpenAppPackageFileAsync("hostel.xsl");
        using var xsl = XmlReader.Create(stream);
        HtmlView.Source = new HtmlWebViewSource { Html = _facade.ToHtml(xsl, attr, value) };
    }

    private void OnClear(object? sender, EventArgs e) => ClearFields();

    private async void OnExit(object? sender, EventArgs e)
    {
        bool yes = await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
        if (yes) Application.Current?.Quit();
    }

    private void ClearFields()
    {
        OutputEditor.Text = "";
        HtmlView.Source = new HtmlWebViewSource { Html = EmptyHtml };
        AttributeList.SelectedItem = null;
        ValueList.ItemsSource = null;
    }
}
