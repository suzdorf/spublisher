using System;
using System.Linq;
using SPublisher.Configuration.Exceptions;
using SPublisher.Core;
using SPublisher.Core.Enums;
using SPublisher.Core.Exceptions;

namespace SPublisher
{
    public class SPublisherRunner
    {
        private readonly IRunOptionsFactory _runOptionsFactory;
        private readonly ILogger _logger;
        private readonly IStorageAccessor _storageAccessor;
        private readonly IConfigurationFactory _configurationFactory;
        private readonly IBuildExecutor _buildExecutor;

        public SPublisherRunner(
            IRunOptionsFactory runOptionsFactory,
            ILogger logger,
            IStorageAccessor storageAccessor,
            IBuildExecutor buildExecutor,
            IConfigurationFactory configurationFactory)
        {
            _runOptionsFactory = runOptionsFactory;
            _logger = logger;
            _storageAccessor = storageAccessor;
            _buildExecutor = buildExecutor;
            _configurationFactory = configurationFactory;
        }

        public void Run(string[] args)
        {
            try
            {
                _logger.LogEvent(SPublisherEvent.SPublisherStarted);

                var options = _runOptionsFactory.Get(args);
                var json = _storageAccessor.ReadAllText(options.ConfigurationFileName);
                var model = _configurationFactory.Get(json);

                if (model.BuildSteps.Any())
                {
                    _logger.LogEvent(SPublisherEvent.BuildExecutionStarted);
                    _buildExecutor.Execute(model.BuildSteps);
                    _logger.LogEvent(SPublisherEvent.BuildExecutionCompleted);
                }

                _logger.LogEvent(SPublisherEvent.SPublisherCompleted);
            }
            catch (SPublisherException ex)
            {
                switch (ex)
                {
                    case ValidationException validationException:
                        _logger.LogValidationError(validationException.ValidationInfo);
                        break;
                    default:
                        if (ex is ILogMessage message)
                        {
                            _logger.LogError(ex.SPublisherEvent, message);
                        }
                        else
                        {
                            _logger.LogError(ex.SPublisherEvent);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(SPublisherEvent.UnknownError);
                _logger.LogError(ex);
            }
        }
    }
}