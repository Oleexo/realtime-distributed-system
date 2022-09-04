using System.Diagnostics.CodeAnalysis;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Oleexo.RealtimeDistributedSystem.Common.Extensions;

namespace Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;

[ExcludeFromCodeCoverage]
public abstract class DynamoDbStorage {
    private readonly ILogger _logger;
    private readonly string  _tableName;

    protected DynamoDbStorage(string  tableName,
                              ILogger logger) {
        _tableName = tableName;
        _logger    = logger;
    }

    protected abstract AmazonDynamoDBClient DbClient { get; }

    /// <summary>
    ///     Create a DynamoDB table if it doesn't exist
    /// </summary>
    /// <param name="keys">The keys definitions</param>
    /// <param name="attributes">The attributes used on the key definition</param>
    /// <param name="secondaryIndexes">(optional) The secondary index definitions</param>
    /// <param name="ttlAttributeName">
    ///     (optional) The name of the item attribute that indicates the item TTL (if null, ttl
    ///     won't be enabled)
    /// </param>
    /// <returns></returns>
    public async Task InitializeTable(IEnumerable<KeySchemaElement>      keys,
                                      IEnumerable<AttributeDefinition>   attributes,
                                      IEnumerable<GlobalSecondaryIndex>? secondaryIndexes = null,
                                      string?                            ttlAttributeName = null) {
        try {
            var tableDesc = await GetTableDescription(_tableName);
            if (tableDesc == null) {
                await CreateTable(_tableName, keys.ToList(), attributes.ToList(), secondaryIndexes?.ToList(), ttlAttributeName);
            }
            else {
                await UpdateTable(tableDesc, attributes.ToList(), secondaryIndexes?.ToList(), ttlAttributeName);
            }
        }
        catch (Exception exception) {
            _logger.LogCritical(exception, "Could not initialize connection to storage table {TableName}", _tableName);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string                             tableName,
                                        Dictionary<string, AttributeValue> keys) {
        try {
            var request = new GetItemRequest {
                TableName            = tableName,
                Key                  = keys,
                ConsistentRead       = true,
                ProjectionExpression = string.Join(',', keys.Values)
            };
            var response = await DbClient.GetItemAsync(request);
            return response.IsItemSet;
        }
        catch (Exception) {
            if (_logger.IsEnabled(LogLevel.Debug)) {
                _logger.LogDebug("Unable to find table entry for Keys = {Keys}", keys.ToHumanReadableString());
            }

            throw;
        }
    }

    /// <summary>
    ///     Read an entry from a DynamoDB table
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="tableName">The name of the table to search for the entry</param>
    /// <param name="keys">The table entry keys to search for</param>
    /// <param name="resolver">
    ///     Function that will be called to translate the returned fields into a concrete type. This
    ///     Function is only called if the result is != null
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>The object translated by the resolver function</returns>
    public async Task<TResult?> ReadSingleEntryAsync<TResult>(string                                            tableName,
                                                              Dictionary<string, AttributeValue>                keys,
                                                              Func<Dictionary<string, AttributeValue>, TResult> resolver,
                                                              CancellationToken                                 cancellationToken)
        where TResult : class {
        try {
            var request = new GetItemRequest {
                TableName      = tableName,
                Key            = keys,
                ConsistentRead = true
            };

            var response = await DbClient.GetItemAsync(request, cancellationToken);

            if (response.IsItemSet) {
                return resolver(response.Item);
            }

            return null;
        }
        catch (Exception) {
            if (_logger.IsEnabled(LogLevel.Debug)) {
                _logger.LogDebug("Unable to find table entry for Keys = {Keys}", keys.ToHumanReadableString());
            }

            throw;
        }
    }

    public async Task<TResult?> ReadSingleEntryAsync<TResult>(string                                            hashKey,
                                                              string                                            rangeKey,
                                                              Func<Dictionary<string, AttributeValue>, TResult> resolver,
                                                              CancellationToken                                 cancellationToken = default)
        where TResult : class {
        var keys = new Dictionary<string, AttributeValue> {
            { "PK", new AttributeValue { S = hashKey } },
            { "SK", new AttributeValue { S = rangeKey } }
        };

        try {
            var request = new GetItemRequest {
                TableName      = _tableName,
                Key            = keys,
                ConsistentRead = true
            };

            var response = await DbClient.GetItemAsync(request, cancellationToken);

            if (response.IsItemSet) {
                return resolver(response.Item);
            }

            return null;
        }
        catch (Exception) {
            if (_logger.IsEnabled(LogLevel.Debug)) {
                _logger.LogDebug("Unable to find table entry for Keys = {Keys}", keys.ToHumanReadableString());
            }

            throw;
        }
    }

    public async Task<IReadOnlyCollection<TResult>> ReadEntriesAsync<TResult>(string                                            keyConditionExpression,
                                                                              Dictionary<string, AttributeValue>                expressionAttributes,
                                                                              Func<Dictionary<string, AttributeValue>, TResult> resolver,
                                                                              string?                                           indexName         = null,
                                                                              int?                                              limit             = null,
                                                                              bool                                              scanIndexForward  = false,
                                                                              IReadOnlyCollection<string>?                      projections       = null,
                                                                              CancellationToken                                 cancellationToken = default) {
        try {
            var request = new QueryRequest {
                IndexName                 = indexName,
                TableName                 = _tableName,
                KeyConditionExpression    = keyConditionExpression,
                ScanIndexForward          = scanIndexForward,
                ConsistentRead            = false,
                ExpressionAttributeNames  = expressionAttributes.Keys.ToDictionary(k => $"#{k}", v => v),
                ExpressionAttributeValues = expressionAttributes.ToDictionary(k => $":{k.Key}", v => v.Value)
            };
            if (limit.HasValue) {
                request.Limit = limit.Value;
            }

            if (projections != null &&
                projections.Any()) {
                var sb    = new StringBuilder();
                var count = 1;
                foreach (var projection in projections) {
                    var key = $"#proj{count}";
                    request.ExpressionAttributeNames.Add(key, projection);
                    if (count > 1) {
                        sb.Append(',');
                    }

                    sb.Append(key);
                    count += 1;
                }

                request.ProjectionExpression = sb.ToString();
            }

            var response = await DbClient.QueryAsync(request, cancellationToken);
            return response.Items.Select(resolver)
                           .ToArray();
        }
        catch (Exception) {
            if (_logger.IsEnabled(LogLevel.Debug)) {
                _logger.LogDebug("Unable to find table entry for Keys = {ExpressionAttributes}", expressionAttributes.ToHumanReadableString());
            }

            throw;
        }
    }

    /// <summary>
    ///     Create or Replace an entry in a DynamoDB Table
    /// </summary>
    /// <param name="fields">The fields/attributes to add or replace in the table</param>
    /// <param name="conditionExpression">Optional conditional expression</param>
    /// <param name="conditionValues">Optional field/attribute values used in the conditional expression</param>
    /// <param name="cancellationToken"></param>
    public async Task PutEntryAsync(Dictionary<string, AttributeValue>  fields,
                                    string                              conditionExpression = "",
                                    Dictionary<string, AttributeValue>? conditionValues     = null,
                                    CancellationToken                   cancellationToken   = default) {
        if (_logger.IsEnabled(LogLevel.Trace)) {
            _logger.LogTrace("Creating {TableName} table entry: {Fields}", _tableName, fields.ToHumanReadableString());
        }

        try {
            var request = new PutItemRequest(_tableName, fields, ReturnValue.NONE);
            if (!string.IsNullOrWhiteSpace(conditionExpression)) {
                request.ConditionExpression = conditionExpression;
            }

            if (conditionValues            != null &&
                conditionValues.Keys.Count > 0) {
                request.ExpressionAttributeValues = conditionValues;
            }

            var r = await DbClient.PutItemAsync(request, cancellationToken);
        }
        catch (Exception exception) {
            _logger.LogError(exception, "Unable to create item to table '{TableName}'", _tableName);
            throw;
        }
    }

    public async Task PutEntriesAsync<T>(string                 tableName,
                                         IReadOnlyCollection<T> addedItems,
                                         IReadOnlyCollection<T> removedItems,
                                         Func<T, Document>      resolver,
                                         CancellationToken      cancellationToken = default) {
        var table      = Table.LoadTable(DbClient, tableName);
        var batchWrite = table.CreateBatchWrite();

        foreach (var item in addedItems) {
            var document = resolver(item);
            batchWrite.AddDocumentToPut(document);
        }

        foreach (var item in removedItems) {
            var document = resolver(item);
            batchWrite.AddItemToDelete(document);
        }

        await batchWrite.ExecuteAsync(cancellationToken);
    }

    /// <summary>
    ///     Remove a column (attribute) on item in DynamoDb Table
    /// </summary>
    /// <param name="tableName">The name of the table to remove a column</param>
    /// <param name="keys">The table entry keys to search for</param>
    /// <param name="attributeName">The attribute name to be removed</param>
    public async Task DeleteAttributeAsync(string                             tableName,
                                           Dictionary<string, AttributeValue> keys,
                                           string                             attributeName) {
        try {
            var request = new UpdateItemRequest(tableName,
                                                keys,
                                                new Dictionary<string, AttributeValueUpdate> {
                                                    { attributeName, new AttributeValueUpdate { Action = AttributeAction.DELETE } }
                                                },
                                                ReturnValue.NONE);
            await DbClient.UpdateItemAsync(request);
        }
        catch (Exception exception) {
            _logger.LogError(exception, "Unable to remove attribute {AttributeName} to table {TableName}", attributeName, tableName);
            throw;
        }
    }

    /// <summary>
    ///     Delete an entry from a DynamoDB table
    /// </summary>
    /// <param name="keys">The table entry keys for the entry to be deleted</param>
    /// <param name="conditionExpression">Optional conditional expression</param>
    /// <param name="conditionValues">Optional field/attribute values used in the conditional expression</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task DeleteEntryAsync(Dictionary<string, AttributeValue>  keys,
                                 string                              conditionExpression = "",
                                 Dictionary<string, AttributeValue>? conditionValues     = null,
                                 CancellationToken                   cancellationToken   = default) {
        if (_logger.IsEnabled(LogLevel.Trace)) {
            _logger.LogTrace("Deleting table {TableName}  entry with key(s) {Keys}", _tableName, keys.ToHumanReadableString());
        }

        try {
            var request = new DeleteItemRequest {
                TableName = _tableName,
                Key       = keys
            };

            if (!string.IsNullOrWhiteSpace(conditionExpression)) {
                request.ConditionExpression = conditionExpression;
            }

            if (conditionValues            != null &&
                conditionValues.Keys.Count > 0) {
                request.ExpressionAttributeValues = conditionValues;
            }

            return DbClient.DeleteItemAsync(request, cancellationToken);
        }
        catch (Exception exc) {
            _logger.LogWarning(exc, "Intermediate error deleting entry from the table {TableName}", _tableName);
            throw;
        }
    }

    #region private methods

    private async Task<TableDescription?> GetTableDescription(string tableName) {
        try {
            var description = await DbClient.DescribeTableAsync(tableName);
            if (description.Table != null) {
                return description.Table;
            }
        }
        catch (ResourceNotFoundException) {
            return null;
        }

        return null;
    }

    private async Task CreateTable(string                      tableName,
                                   List<KeySchemaElement>      keys,
                                   List<AttributeDefinition>   attributes,
                                   List<GlobalSecondaryIndex>? secondaryIndexes = null,
                                   string?                     ttlAttributeName = null) {
        var request = new CreateTableRequest {
            TableName             = tableName,
            AttributeDefinitions  = attributes,
            KeySchema             = keys,
            BillingMode           = BillingMode.PAY_PER_REQUEST,
            ProvisionedThroughput = null
        };

        if (secondaryIndexes != null &&
            secondaryIndexes.Any()) {
            secondaryIndexes.ForEach(i => {
                i.Projection = new Projection {
                    ProjectionType = ProjectionType.ALL
                };
                foreach (var element in i.KeySchema) {
                    if (request.AttributeDefinitions.All(p => p.AttributeName != element.AttributeName)) {
                        request.AttributeDefinitions.Add(new AttributeDefinition(element.AttributeName, ScalarAttributeType.S));
                    }
                }
            });
            request.GlobalSecondaryIndexes = secondaryIndexes;
        }

        try {
            var               response    = await DbClient.CreateTableAsync(request);
            TableDescription? description = null;
            do {
                description = await GetTableDescription(tableName);

                await Task.Delay(2000);
            } while (description             == null ||
                     description.TableStatus == TableStatus.CREATING);

            if (!string.IsNullOrEmpty(ttlAttributeName)) {
                await DbClient.UpdateTimeToLiveAsync(new UpdateTimeToLiveRequest {
                    TableName               = tableName,
                    TimeToLiveSpecification = new TimeToLiveSpecification { AttributeName = ttlAttributeName, Enabled = true }
                });
            }

            if (description.TableStatus != TableStatus.ACTIVE) {
                throw new InvalidOperationException($"Failure creating table {tableName}");
            }
        }
        catch (Exception exception) {
            _logger.LogCritical(exception, "Could not create table {TableName}", tableName);
            throw;
        }
    }

    private async Task UpdateTable(TableDescription            tableDesc,
                                   List<AttributeDefinition>   attributes,
                                   List<GlobalSecondaryIndex>? secondaryIndexes = null,
                                   string?                     ttlAttributeName = null) {
        var existingSecondaryIndexes = tableDesc.GlobalSecondaryIndexes?.Select(i => i.IndexName) ?? new List<string>();
        var secondaryIndexesToAdd = secondaryIndexes?.Where(i => !existingSecondaryIndexes.Contains(i.IndexName))
                                                    ?.ToList() ??
                                    new List<GlobalSecondaryIndex>();

        if (!secondaryIndexesToAdd.Any()) {
            return;
        }

        var request = new UpdateTableRequest(tableDesc.TableName, null);
        request.AttributeDefinitions = attributes;
        foreach (var newGsi in secondaryIndexesToAdd) {
            request.GlobalSecondaryIndexUpdates.Add(new GlobalSecondaryIndexUpdate {
                Create = new CreateGlobalSecondaryIndexAction {
                    IndexName = newGsi.IndexName,
                    KeySchema = newGsi.KeySchema,
                    Projection = new Projection {
                        ProjectionType = "ALL"
                    }
                }
            });
        }

        try {
            var response = await DbClient.UpdateTableAsync(request);

            if (!string.IsNullOrEmpty(ttlAttributeName)) {
                await DbClient.UpdateTimeToLiveAsync(new UpdateTimeToLiveRequest {
                    TableName               = tableDesc.TableName,
                    TimeToLiveSpecification = new TimeToLiveSpecification { AttributeName = ttlAttributeName, Enabled = true }
                });
            }
        }
        catch (Exception exception) {
            _logger.LogCritical(exception, "Could not update table {TableName}", tableDesc.TableName);
            throw;
        }
    }

    #endregion
}
