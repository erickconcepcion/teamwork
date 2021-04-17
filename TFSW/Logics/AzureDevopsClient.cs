using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Text;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.Services.Client;
using System.Net;
using TFSW.Data;
using TFSW.Utils;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Core.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace TFSW.Logics
{
    public class AzureDevopsClient: IDisposable
    {
        private readonly Configuration _config;
        private readonly Uri _orgUrl;
        private VssConnection _connection;
        private WorkItemTrackingHttpClient _workItemClient;
        private ProjectHttpClient _projectHttpClient;
        private VssCredentials _credentials;
        public AzureDevopsClient(Configuration config, bool setWorkItemClient = false, bool setProjectClient = false)
        {
            _config = config;
            _orgUrl = new Uri(config.ServerUrl);
            SetConnection();
            if (setWorkItemClient) SetWorkItemClient();
            if (setProjectClient) SetProjectClient();
        }
        private void SetConnection()=> _connection = new VssConnection(_orgUrl, 
            _config.IsDomainCreds
            ? new VssClientCredentials(new WindowsCredential(new NetworkCredential(_config.User,
                Utilities.GetHiddenConsoleInput("Azure devops Domain Password (hidden field)"), _config.Domain)))
            : new VssBasicCredential(string.Empty, _config.PersonalToken)
            );
        private void SetWorkItemClient() => _workItemClient = _connection.GetClient<WorkItemTrackingHttpClient>();
        private void SetProjectClient() => _projectHttpClient = _connection.GetClient<ProjectHttpClient>();
        public Task<WorkItem> GetWorkItem(int workItemId)
            => _workItemClient.GetWorkItemAsync(workItemId);
        public async Task<IEnumerable<TeamProjectReference>> GetProjects() => await _projectHttpClient.GetProjects();
        public async Task<IEnumerable<WorkItemRelationType>> GetWorkItemRelationTypes()
            => await _workItemClient.GetRelationTypesAsync();
        public async Task<IEnumerable<WorkItemType>> GetWorkItemTypes()
            => await _workItemClient.GetWorkItemTypesAsync(_config.Project);
        public async Task<IEnumerable<WorkReference>> GetWorkItemRelationTypesReference()
            => (await _workItemClient.GetRelationTypesAsync()).Select(t => new WorkReference
            {
                Name = t.Name,
                ReferenceName = t.ReferenceName
            });
        public async Task<IEnumerable<WorkReference>> GetWorkItemTypesReference()
            => (await _workItemClient.GetWorkItemTypesAsync(_config.Project)).Select(t=> new WorkReference { 
                Name = t.Name, ReferenceName = t.ReferenceName
            });
        public async Task<IEnumerable<WorkItem>> NestedWorkItem(WorkItem workItem, IEnumerable<WorkItemHierarchy> hierarchy)
        => await Task.WhenAll(hierarchy.Select(h => _workItemClient.CreateWorkItemAsync(GetPatchBased(workItem, h.Title, h.WorkRelationshipType),
            _config.Project,
            h.WorkItemType
            )));
        private JsonPatchDocument GetPatchBased(WorkItem workItem, string title, string relationReferenceName)
        {
            var obj = new JObject();
            obj["rel"] = relationReferenceName;
            obj["url"] = string.Format("{0}/_apis/wit/workItems/{1}", _orgUrl.AbsoluteUri, workItem.Id);
            return new JsonPatchDocument()
            {
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Title",
                    Value = string.Format(title, workItem.Id, workItem.Fields["System.Title"])
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/relations/-",
                    Value = obj
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.IterationPath",
                    Value = workItem.Fields["System.IterationPath"]
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.AreaPath",
                    Value = workItem.Fields["System.AreaPath"]
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.AssignedTo",
                    Value = workItem.Fields["System.AssignedTo"]
                }
            };
        }
        public void Dispose()
        {
            if (_workItemClient is not null)
            {
                _workItemClient.Dispose();
            }
            if (_projectHttpClient is not null)
            {
                _projectHttpClient.Dispose();
            }
        }
    }

}
