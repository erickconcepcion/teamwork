﻿using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
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
        public AzureDevopsClient(Configuration config)
        {
            _config = config;
            _orgUrl = new Uri(config.ServerUrl);
            SetConnection();
        }
        private void SetConnection()=> _connection = new VssConnection(_orgUrl, 
            _config.IsDomainCreds
            ? new VssClientCredentials(new WindowsCredential(new NetworkCredential(_config.User,
                Utilities.GetHiddenConsoleInput("Azure devops Password (hidden field)"), _config.Domain)))
            : new VssBasicCredential(string.Empty, _config.PersonalToken)
            );
        private void SetWorkItemClients() => _workItemClient = _connection.GetClient<WorkItemTrackingHttpClient>();
        private void SetProjectClient() => _projectHttpClient = _connection.GetClient<ProjectHttpClient>();
        private Task<WorkItem> GetWorkItem(int workItemId)
            => _workItemClient.GetWorkItemAsync(workItemId);
        private Task<IPagedList<TeamProjectReference>> GetProjects() => _projectHttpClient.GetProjects();
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