using System;
using System.Net.Http;
using Avalonia.Controls;
using SeaweedFS.Gui.Features.Main;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.Avalonia;

namespace SeaweedFS.Gui;

public static class CompositionRoot
{
    public static MainViewModel Create(TopLevel topLevel)
    {
        //var httpClient = new HttpClient(new CustomDelegatingHandler());
        var httpClient = new HttpClient() { Timeout = TimeSpan.FromDays(1) };
        var uriString = "http://192.168.1.31:8888";
        var uri = new Uri(uriString);
        httpClient.BaseAddress = uri;
        var client = Client(httpClient);

        return new MainViewModel(client, new AvaloniaStorage(topLevel.StorageProvider));
    }

    private static ISeaweedFS Client(HttpClient httpClient)
    {
        return new SeaweedFSClient(httpClient);
    }
}