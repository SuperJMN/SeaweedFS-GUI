﻿using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace SeaweedFS.Gui.SeaweedFS;

[Headers("Accept: application/json")]
public interface ISeaweedApi
{
    [Get("/{directoryPath}?pretty=y")]
    Task<Folder> GetContents(string directoryPath);

    [Multipart]
    [Post("/{path}")]
    Task Upload(string path, StreamPart stream, CancellationToken cancellationToken);

    [Post("/{directoryPath}")]
    Task CreateFolder(string directoryPath);
}