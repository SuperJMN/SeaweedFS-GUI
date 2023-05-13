using System;
using System.Net.Http;
using Avalonia.Controls;
using SeaweedFS.Gui.Features.Shell;
using Zafiro.Avalonia;

namespace SeaweedFS.Gui;

public static class CompositionRoot
{
    public static ShellViewModel Create(TopLevel topLevel)
    {
        var httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };
        var uriString = "http://192.168.1.31:8888";
        var uri = new Uri(uriString);
        httpClient.BaseAddress = uri;

        return new ShellViewModel(new AvaloniaStorage(topLevel.StorageProvider), topLevel);
    }
}