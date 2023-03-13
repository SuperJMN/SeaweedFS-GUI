using System;
using System.Net.Http;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using SeaweedFS.Gui.SeaweedFS;
using SeaweedFS.Gui.ViewModels;
using Zafiro.Avalonia;

namespace SeaweedFS.Gui.Views;

public static class CompositionRoot
{
    public static MainViewModel Create(TopLevel topLevel)
    {
        //var httpClient = new HttpClient(new CustomDelegatingHandler());
        var httpClient = new HttpClient();
        var uriString = "http://192.168.1.31:8888";
        var uri = new Uri(uriString);
        httpClient.BaseAddress = uri;
        var client = Client(httpClient);

        var filePicker = new OpenFilePicker(topLevel);
        var desktopSaveFilePicker = new SaveFilePicker(topLevel);
        return new MainViewModel(client, new NotificationService(new WindowNotificationManager(topLevel)), new AvaloniaStorage(topLevel.StorageProvider));
    }

    private static ISeaweedFS Client(HttpClient httpClient)
    {
        return new SeaweedFSClient(httpClient);
    }
}