using Azure.Storage.Queues.Models;
using Database.Repository.CQC;
using Domain.Logic.Integration.CQC;
using Domain.Logic.Integration.CQC.API.Models;
using Domain.Models.Exceptions;
using Functions.Factories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Functions.Functions;

public class CQCImportFunctions(
    ILoggerFactory loggerFactory,
    ICQCProviderOrchestration cQCProviderOrchestration,
    IInputModelFactory inputModelFactory,
    IRepositoryForCQCProvider repositoryForCQCProvider,
    CQCFunctionConfiguration configuration)
{
    private readonly ILogger logger = loggerFactory.CreateLogger<CQCImportFunctions>();
    private readonly ICQCProviderOrchestration cQCProviderOrchestration = cQCProviderOrchestration;
    private readonly IInputModelFactory inputModelFactory = inputModelFactory;
    private readonly IRepositoryForCQCProvider repositoryForCQCProvider = repositoryForCQCProvider;
    private readonly CQCFunctionConfiguration configuration = configuration;

    [Function(nameof(CQCTriggerStartImportAllCQCProviders))]
    public async Task CQCTriggerStartImportAllCQCProviders(
#pragma warning disable IDE0060 // Remove unused parameter
        [QueueTrigger("cqc-providers-import-all-providers")] QueueMessage message,
#pragma warning restore IDE0060 // Remove unused parameter
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger(nameof(CQCTriggerStartImportAllCQCProviders));

        var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(CQCOrchestrateImportAllCQCProviders));

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);
    }

    [Function(nameof(CQCOrchestrateImportAllCQCProviders))]
    public async Task<string> CQCOrchestrateImportAllCQCProviders(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        try
        {
            var instanceIdAsGuid = Guid.Parse(context.InstanceId);

            await context.CallActivityAsync(
                nameof(CQCCreateInstanceRecord),
                instanceIdAsGuid,
                CreateRetryOptions(configuration));

            var firstBatchOfProvidersTask = context.CallActivityAsync<ProvidersOutputModel>(
                nameof(CQCGetAllProviders),
                inputModelFactory.Create(1, context.InstanceId),
                CreateRetryOptions(configuration));

            var firstBatchOfProviders = await firstBatchOfProvidersTask;

            var totalNumberOfPages = firstBatchOfProviders.totalPages;

            var tasks = new Task<ProvidersOutputModel>[totalNumberOfPages];

            tasks[0] = firstBatchOfProvidersTask;

            /* start on 2nd page */
            for (var i = 1; i < totalNumberOfPages; i++)
            {
                tasks[i] = context.CallActivityAsync<ProvidersOutputModel>(
                    nameof(CQCGetAllProviders),
                    inputModelFactory.Create(i + 1, context.InstanceId),
                    CreateRetryOptions(configuration));
            }

            await Task.WhenAll(tasks);

            var importMessages = new string[totalNumberOfPages];

            for (var i = 1; i <= totalNumberOfPages; i++)
            {
                importMessages[i - 1] = await context.CallSubOrchestratorAsync<string>(
                    nameof(CQCImportProviderForInstance),
                    inputModelFactory.Create(i, context.InstanceId));

                var dueTime = context.CurrentUtcDateTime.AddMinutes(configuration.DelayInMinuets);

                await context.CreateTimer(dueTime, CancellationToken.None);
            }

            await context.CallActivityAsync(
                nameof(CQCDeleteInstanceRecord),
                instanceIdAsGuid,
                CreateRetryOptions(configuration));

            return string.Join(',', importMessages);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Orchestration Event Failure, {funcName}, {InstanceId}", nameof(CQCOrchestrateImportAllCQCProviders), context.InstanceId);
            throw;
        }
    }

    [Function(nameof(CQCTriggerGetAllCQCProvidersThatHaveChanged))]
    [QueueOutput("cqc-providers-import-providers-that-have-changed")]
    [FixedDelayRetry(10, "00:01:00")]
    public async Task<string> CQCTriggerGetAllCQCProvidersThatHaveChanged(
#pragma warning disable IDE0060 // Remove unused parameter
        [TimerTrigger("%TimerTriggerImportCQCProvidersThatHaveChanged%")] TimerInfo myTimer,
#pragma warning restore IDE0060 // Remove unused parameter
        FunctionContext context)
    {
        var logger = context.GetLogger(nameof(CQCTriggerGetAllCQCProvidersThatHaveChanged));

        logger.LogInformation("Start {nameofFunction}", nameof(CQCTriggerGetAllCQCProvidersThatHaveChanged));

        /* Start database */
        var can = await repositoryForCQCProvider.CanConnectAsync();

        if (can)
        {
            logger.LogInformation("Can connect to the database ");
            return DateTime.UtcNow.ToString();
        }

        var exception = new CannotConnectToTheDatabaseException($"Cannot connect to database: {DateTime.UtcNow}");

        logger.LogWarning(exception, "Can't connect to the database");

        throw exception;
    }

    [Function(nameof(CQCStartGetAllCQCProvidersThatHaveChanged))]
    public async Task CQCStartGetAllCQCProvidersThatHaveChanged(
#pragma warning disable IDE0060 // Remove unused parameter
        [QueueTrigger("cqc-providers-import-providers-that-have-changed")] QueueMessage message,
#pragma warning restore IDE0060 // Remove unused parameter
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger(nameof(CQCStartGetAllCQCProvidersThatHaveChanged));

        var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(CQCOrchestrateImportAllCQCProvidersThatHaveChanged));

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);
    }

    [Function(nameof(CQCOrchestrateImportAllCQCProvidersThatHaveChanged))]
    public async Task<string> CQCOrchestrateImportAllCQCProvidersThatHaveChanged(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        try
        {
            var instanceIdAsGuid = Guid.Parse(context.InstanceId);

            await context.CallActivityAsync(
                nameof(CQCCreateInstanceRecord),
                instanceIdAsGuid,
                CreateRetryOptions(configuration));

            var firstBatchOfProvidersTask = context.CallActivityAsync<ProvidersOutputModel>(
                nameof(CQCGetProvidersThatHaveChanged),
                inputModelFactory.Create(1, context.InstanceId),
                CreateRetryOptions(configuration));

            var firstBatchOfProviders = await firstBatchOfProvidersTask;

            var totalNumberOfPages = firstBatchOfProviders.totalPages;

            var tasks = new Task<ProvidersOutputModel>[totalNumberOfPages];

            tasks[0] = firstBatchOfProvidersTask;

            /* start on 2nd page */
            for (var i = 1; i < totalNumberOfPages; i++)
            {
                tasks[i] = context.CallActivityAsync<ProvidersOutputModel>(
                    nameof(CQCGetProvidersThatHaveChanged),
                    inputModelFactory.Create(i + 1, context.InstanceId),
                    CreateRetryOptions(configuration));
            }

            await Task.WhenAll(tasks);

            var importMessages = new string[totalNumberOfPages];

            for (var i = 1; i <= totalNumberOfPages; i++)
            {
                importMessages[i - 1] = await context.CallSubOrchestratorAsync<string>(
                    nameof(CQCImportProviderForInstance),
                    inputModelFactory.Create(i, context.InstanceId));

                var dueTime = context.CurrentUtcDateTime.AddMinutes(configuration.DelayInMinuets);

                await context.CreateTimer(dueTime, CancellationToken.None);
            }

            await context.CallActivityAsync(
                nameof(CQCDeleteInstanceRecord),
                instanceIdAsGuid,
                CreateRetryOptions(configuration));

            return string.Join(",", importMessages);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Orchestration Event Failure, {methodName}, {InstanceId}", nameof(CQCOrchestrateImportAllCQCProvidersThatHaveChanged), context.InstanceId);
            throw;
        }
    }

    [Function(nameof(CQCImportProviderForInstance))]
    public async Task<string> CQCImportProviderForInstance(
      [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var inputModel = context.GetInput<ProvidersInputModel>() ?? throw new Exception($"Unable to obtain parent instance id: {context.InstanceId}");

        var providerIds = await context.CallActivityAsync<string[]>(
            nameof(CQCGetProviderIdsForThisImport),
            inputModel,
            CreateRetryOptions(configuration));

        var tasks = new Task[providerIds.Length];

        for (var i = 0; i < providerIds.Length; i++)
        {
            tasks[i] = context.CallActivityAsync(
                    nameof(CQCImportProvider),
                    input: inputModelFactory.Create(providerIds[i], inputModel.InstanceId.ToString()),
                    options: CreateRetryOptions(configuration));
        }

        await Task.WhenAll(tasks);

        return $"Completed getting all provider ids {providerIds.Length}";
    }

    [Function(nameof(CQCCreateInstanceRecord))]
    public async Task CQCCreateInstanceRecord(
        [ActivityTrigger] Guid guid)
    {
        await cQCProviderOrchestration.CreateInstanceRecord(guid);
    }

    [Function(nameof(CQCDeleteInstanceRecord))]
    public async Task CQCDeleteInstanceRecord(
        [ActivityTrigger] Guid guid)
    {
        await cQCProviderOrchestration.DeleteInstanceRecord(guid);
    }

    [Function(nameof(CQCGetAllProviders))]
    public async Task<ProvidersOutputModel> CQCGetAllProviders(
        [ActivityTrigger] ProvidersInputModel inputModel)
    {
        return await cQCProviderOrchestration.CQCGetAllProviders(inputModel);
    }

    [Function(nameof(CQCGetProvidersThatHaveChanged))]
    public async Task<ProvidersThatHaveChangedOutputModel> CQCGetProvidersThatHaveChanged(
        [ActivityTrigger] ProvidersInputModel inputModel)
    {
        return await cQCProviderOrchestration.GetProvidersThatHaveChanged(inputModel);
    }

    [Function(nameof(CQCGetProviderIdsForThisImport))]
    public async Task<string[]> CQCGetProviderIdsForThisImport(
        [ActivityTrigger] ProvidersInputModel inputModel)
    {
        return await cQCProviderOrchestration.GetProviderIdsForThisImport(inputModel);
    }

    [Function(nameof(CQCImportProvider))]
    public async Task CQCImportProvider(
        [ActivityTrigger] ProviderImportInputModel inputModel)
    {
        await cQCProviderOrchestration.ImportProvider(inputModel);
    }

    [Function(nameof(CQCImportProviderManually))]
    public async Task CQCImportProviderManually(
        [QueueTrigger("cqc-import-provider-manually")] ProviderImportInputModel inputModel, FunctionContext _)
    {
        await cQCProviderOrchestration.ImportProvider(inputModel);
    }

    private static TaskOptions CreateRetryOptions(CQCFunctionConfiguration configuration)
    {
        return TaskOptions.FromRetryPolicy(new RetryPolicy(
                    maxNumberOfAttempts: configuration.RetryPolicyNumberOfAttempts,
                    firstRetryInterval: TimeSpan.FromMinutes(configuration.RetryPolicyFirstRetryInterval),
                    backoffCoefficient: configuration.RetryPolicyBackoffCoefficient));
    }
}
