﻿namespace SPublisher.Core
{
    public interface IFile : IScriptHashInfo
    {
        string Path { get; }
        string Content { get; }
    }
}