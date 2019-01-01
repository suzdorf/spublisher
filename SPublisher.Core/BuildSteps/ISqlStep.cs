﻿namespace SPublisher.Core.BuildSteps
{
    public interface ISqlStep : IBuildStep, ISqlConnectionSettings
    {
        IDatabase[] Databases { get; }
    }
}