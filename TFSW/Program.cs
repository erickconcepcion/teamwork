using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Text;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Newtonsoft.Json.Linq;

Uri orgUrl = new Uri("org url");           
string personalAccessToken = "personal token";
int workItemId = 23;   

// Create a connection
var credentials = new VssBasicCredential(string.Empty, personalAccessToken);
//var credentials = new VssClientCredentials(new WindowsCredential(new NetworkCredential("user", "pass.", "domain")));
VssConnection connection = new VssConnection(orgUrl, credentials);

// Show details a work item
WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

try
{
    // Get the specified work item
    WorkItem workitem = await witClient.GetWorkItemAsync(workItemId);

    // Output the work item's field values
    foreach (var field in workitem.Fields)
    {
        Console.WriteLine("  {0}: {1}", field.Key, field.Value);
    }
    var obj = new JObject();
    obj["rel"] = "System.LinkTypes.Hierarchy-Reverse";
    obj["url"] = string.Format("{0}/_apis/wit/workItems/{1}", orgUrl.AbsoluteUri, workItemId);

    var document = new JsonPatchDocument();
    document.Add(new JsonPatchOperation()
    {
        Operation = Operation.Add,
        Path = "/fields/System.Title",
        Value = "New Task CLI"
    });
    document.Add(new JsonPatchOperation()
    {
        Operation = Operation.Add,
        Path = "/relations/-",
        Value = obj
    });
    document.Add(new JsonPatchOperation()
    {
        Operation = Operation.Add,
        Path = "/fields/System.IterationPath",
        Value = workitem.Fields["System.IterationPath"]
    });
    document.Add(new JsonPatchOperation()
    {
        Operation = Operation.Add,
        Path = "/fields/System.AreaPath",
        Value = workitem.Fields["System.AreaPath"]
    });
    document.Add(new JsonPatchOperation()
    {
        Operation = Operation.Add,
        Path = "/fields/System.AssignedTo",
        Value = workitem.Fields["System.AssignedTo"]
    });

    var wi = await witClient.CreateWorkItemAsync(document,
        new Guid("00000000-0000-0000-0000-000000000000"),
        "Task"
        );
    foreach (var field in wi.Fields)
    {
        Console.WriteLine("  {0}: {1}", field.Key, field.Value);
    }
}
catch (AggregateException aex)
{
    VssServiceException vssex = aex.InnerException as VssServiceException;
    if (vssex != null)
    {
        Console.WriteLine(vssex.Message);
    }
}




var rel = witClient.GetRelationTypesAsync().Result;
foreach (var r in rel)
{
    Console.WriteLine("  {0}: {1}", r.Name, r.ReferenceName);
}


static string GetHiddenConsoleInput()
{
    StringBuilder input = new StringBuilder();
    while (true)
    {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Enter) break;
        if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
        else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
    }
    return input.ToString();
}