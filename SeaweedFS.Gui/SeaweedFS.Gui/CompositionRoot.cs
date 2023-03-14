using System;
using System.Net.Http;
using Avalonia.Platform.Storage;
using SeaweedFS.Gui.Features.Shell;
using Zafiro.Avalonia;

namespace SeaweedFS.Gui;

public static class CompositionRoot
{
    public static ShellViewModel Create(IStorageProvider storageProvider)
    {
        //var httpClient = new HttpClient(new CustomDelegatingHandler());
        var httpClient = new HttpClient() { Timeout = TimeSpan.FromDays(1) };
        var uriString = "http://192.168.1.31:8888";
        var uri = new Uri(uriString);
        httpClient.BaseAddress = uri;

        return new ShellViewModel(new AvaloniaStorage(storageProvider));
    }
}