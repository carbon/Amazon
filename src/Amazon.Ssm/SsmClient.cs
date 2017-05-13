using System;
using System.Net.Http;
using System.Threading.Tasks;
using Carbon.Json;

namespace Amazon.Ssm
{
    // http://docs.aws.amazon.com/systems-manager/latest/APIReference/API_SendCommand.html

    public class SsmClient : AwsClient
    {
        public static string Version = "2014-11-06";

        public SsmClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.Ssm, region, credential)
        { }


        public Task<AddTagsToResourceResponse> AddTagsToResourceAsync(AddTagsToResourceRequest request)
        {
            return SendAsync<AddTagsToResourceResponse>(request);
        }

        public Task<CancelCommandResponse> CancelCommandAsync(CancelCommandRequest request)
        {
            return SendAsync<CancelCommandResponse>(request);
        }

        public Task<CreateActivationResponse> CreateActivationAsync(CreateActivationRequest request)
        {
            return SendAsync<CreateActivationResponse>(request);
        }

        public Task<CreateAssociationResponse> CreateAssociationAsync(CreateAssociationRequest request)
        {
            return SendAsync<CreateAssociationResponse>(request);
        }

        public Task<CreateAssociationBatchResponse> CreateAssociationBatchAsync(CreateAssociationBatchRequest request)
        {
            return SendAsync<CreateAssociationBatchResponse>(request);
        }

        public Task<CreateDocumentResponse> CreateDocumentAsync(CreateDocumentRequest request)
        {
            return SendAsync<CreateDocumentResponse>(request);
        }

        public Task<CreateMaintenanceWindowResponse> CreateMaintenanceWindowAsync(CreateMaintenanceWindowRequest request)
        {
            return SendAsync<CreateMaintenanceWindowResponse>(request);
        }

        public Task<CreatePatchBaselineResponse> CreatePatchBaseline(CreatePatchBaselineRequest request)
        {
            return SendAsync<CreatePatchBaselineResponse>(request);
        }

        public Task<DeleteActivationResponse> DeleteActivationAsync(DeleteActivationRequest request)
        {
            return SendAsync<DeleteActivationResponse>(request);
        }

        public Task<DeleteAssociationResponse> DeleteAssociationAsync(DeleteAssociationRequest request)
        {
            return SendAsync<DeleteAssociationResponse>(request);
        }

        public Task<DeleteDocumentResponse> DeleteDocumentAsync(DeleteDocumentRequest request)
        {
            return SendAsync<DeleteDocumentResponse>(request);
        }

        public Task<DeleteMaintenanceWindowResponse> DeleteMaintenanceWindowAsync(DeleteMaintenanceWindowRequest request)
        {
            return SendAsync<DeleteMaintenanceWindowResponse>(request);
        }

        public Task<DeleteParameterResponse> DeleteParameterAsync(DeleteParameterRequest request)
        {
            return SendAsync<DeleteParameterResponse>(request);
        }

        public Task<DeletePatchBaselineResponse> DeletePatchBaselineAsync(DeletePatchBaselineRequest request)
        {
            return SendAsync<DeletePatchBaselineResponse>(request);
        }

        public Task<DeregisterManagedInstanceResponse> DeregisterManagedInstanceAsync(DeregisterManagedInstanceRequest request)
        {
            return SendAsync<DeregisterManagedInstanceResponse>(request);
        }

        public Task<DeregisterPatchBaselineForPatchGroupResponse> DeregisterPatchBaselineForPatchGroupAsync(DeregisterPatchBaselineForPatchGroupRequest request)
        {
            return SendAsync<DeregisterPatchBaselineForPatchGroupResponse>(request);
        }

        public Task<DeregisterTargetFromMaintenanceWindowResponse> DeregisterTargetFromMaintenanceWindowAsync(DeregisterTargetFromMaintenanceWindowRequest request)
        {
            return SendAsync<DeregisterTargetFromMaintenanceWindowResponse>(request);
        }

        public Task<DeregisterTaskFromMaintenanceWindowResponse> DeregisterTaskFromMaintenanceWindowAsync(DeregisterTaskFromMaintenanceWindowRequest request)
        {
            return SendAsync<DeregisterTaskFromMaintenanceWindowResponse>(request);
        }

        public Task<DescribeActivationsResponse> DescribeActivationsAsync(DescribeActivationsRequest request)
        {
            return SendAsync<DescribeActivationsResponse>(request);
        }

        public Task<DescribeAssociationResponse> DescribeAssociationAsync(DescribeAssociationRequest request)
        {
            return SendAsync<DescribeAssociationResponse>(request);
        }

        public Task<DescribeAutomationExecutionsResponse> DescribeAutomationExecutionsAsync(DescribeAutomationExecutionsRequest request)
        {
            return SendAsync<DescribeAutomationExecutionsResponse>(request);
        }

        public Task<DescribeAvailablePatchesResponse> DescribeAvailablePatchesAsync(DescribeAvailablePatchesRequest request)
        {
            return SendAsync<DescribeAvailablePatchesResponse>(request);
        }

        public Task<DescribeDocumentResponse> DescribeDocumentAsync(DescribeDocumentRequest request)
        {
            return SendAsync<DescribeDocumentResponse>(request);
        }

        public Task<DescribeDocumentPermissionResponse> DescribeDocumentPermissionAsync(DescribeDocumentPermissionRequest request)
        {
            return SendAsync<DescribeDocumentPermissionResponse>(request);
        }

        public Task<DescribeEffectiveInstanceAssociationsResponse> DescribeEffectiveInstanceAssociationsAsync(DescribeEffectiveInstanceAssociationsRequest request)
        {
            return SendAsync<DescribeEffectiveInstanceAssociationsResponse>(request);
        }

        public Task<DescribeEffectivePatchesForPatchBaselineResponse> DescribeEffectivePatchesForPatchBaselineAsync(DescribeEffectivePatchesForPatchBaselineRequest request)
        {
            return SendAsync<DescribeEffectivePatchesForPatchBaselineResponse>(request);
        }

        public Task<DescribeInstanceAssociationsStatusResponse> DescribeInstanceAssociationsStatusAsync(DescribeInstanceAssociationsStatusRequest request)
        {
            return SendAsync<DescribeInstanceAssociationsStatusResponse>(request);
        }

        public Task<DescribeInstanceInformationResponse> DescribeInstanceInformationAsync(DescribeInstanceInformationRequest request)
        {
            return SendAsync<DescribeInstanceInformationResponse>(request);
        }

        public Task<DescribeInstancePatchesResponse> DescribeInstancePatchesAsync(DescribeInstancePatchesRequest request)
        {
            return SendAsync<DescribeInstancePatchesResponse>(request);
        }

        public Task<DescribeInstancePatchStatesResponse> DescribeInstancePatchStatesAsync(DescribeInstancePatchStatesRequest request)
        {
            return SendAsync<DescribeInstancePatchStatesResponse>(request);
        }

        public Task<DescribeInstancePatchStatesForPatchGroupResponse> DescribeInstancePatchStatesForPatchGroupAsync(DescribeInstancePatchStatesForPatchGroupRequest request)
        {
            return SendAsync<DescribeInstancePatchStatesForPatchGroupResponse>(request);
        }
        public Task<DescribeMaintenanceWindowExecutionsResponse> DescribeMaintenanceWindowExecutionsAsync(DescribeMaintenanceWindowExecutionsRequest request)
        {
            return SendAsync<DescribeMaintenanceWindowExecutionsResponse>(request);
        }

        public Task<DescribeMaintenanceWindowExecutionTaskInvocationsResponse> DescribeMaintenanceWindowExecutionTaskInvocationsAsync(DescribeMaintenanceWindowExecutionTaskInvocationsRequest request)
        {
            return SendAsync<DescribeMaintenanceWindowExecutionTaskInvocationsResponse>(request);
        }

        public Task<DescribeMaintenanceWindowExecutionTasksResponse> DescribeMaintenanceWindowExecutionTasks(DescribeMaintenanceWindowExecutionTasksRequest request)
        {
            return SendAsync<DescribeMaintenanceWindowExecutionTasksResponse>(request);
        }

        public Task<DescribeMaintenanceWindowsResponse> DescribeMaintenanceWindows(DescribeMaintenanceWindowsRequest request)
        {
            return SendAsync<DescribeMaintenanceWindowsResponse>(request);
        }

        public Task<DescribeMaintenanceWindowTargetsResponse> DescribeMaintenanceWindowTargets(DescribeMaintenanceWindowTargetsRequest request)
        {
            return SendAsync<DescribeMaintenanceWindowTargetsResponse>(request);
        }

        public Task<DescribeMaintenanceWindowTasksResponse> DescribeMaintenanceWindowTasks(DescribeMaintenanceWindowTasksRequest request)
        {
            return SendAsync<DescribeMaintenanceWindowTasksResponse>(request);
        }

        public Task<DescribeParametersResponse> DescribeParameters(DescribeParametersRequest request)
        {
            return SendAsync<DescribeParametersResponse>(request);
        }

        public Task<DescribePatchBaselinesResponse> DescribePatchBaselines(DescribePatchBaselinesRequest request)
        {
            return SendAsync<DescribePatchBaselinesResponse>(request);
        }

        public Task<DescribePatchGroupsResponse> DescribePatchGroupsAsync(DescribePatchGroupsRequest request)
        {
            return SendAsync<DescribePatchGroupsResponse>(request);
        }

        public Task<DescribePatchGroupStateResponse> DescribePatchGroupStateAsync(DescribePatchGroupStateRequest request)
        {
            return SendAsync<DescribePatchGroupStateResponse>(request);
        }

        public Task<GetAutomationExecutionResponse> GetAutomationExecutionAsync(GetAutomationExecutionRequest request)
        {
            return SendAsync<GetAutomationExecutionResponse>(request);
        }

        public Task<GetCommandInvocationResponse> GetCommandInvocationAsync(GetCommandInvocationRequest request)
        {
            return SendAsync<GetCommandInvocationResponse>(request);
        }

        public Task<GetDefaultPatchBaselineResponse> GetDefaultPatchBaselineAsync(GetDefaultPatchBaselineRequest request)
        {
            return SendAsync<GetDefaultPatchBaselineResponse>(request);
        }

        public Task<GetDeployablePatchSnapshotForInstanceResponse> GetDeployablePatchSnapshotForInstanceAsync(GetDeployablePatchSnapshotForInstanceRequest request)
        {
            return SendAsync<GetDeployablePatchSnapshotForInstanceResponse>(request);
        }

        public Task<GetDocumentResponse> GetDocumentAsync(GetDocumentRequest request)
        {
            return SendAsync<GetDocumentResponse>(request);
        }

        public Task<GetInventoryResponse> GetInventoryAsync(GetInventoryRequest request)
        {
            return SendAsync<GetInventoryResponse>(request);
        }

        public Task<GetInventorySchemaResponse> GetInventorySchemaAsync(GetInventorySchemaRequest request)
        {
            return SendAsync<GetInventorySchemaResponse>(request);
        }

        public Task<GetMaintenanceWindowResponse> GetMaintenanceWindowAsync(GetMaintenanceWindowRequest request)
        {
            return SendAsync<GetMaintenanceWindowResponse>(request);
        }

        public Task<GetMaintenanceWindowExecutionResponse> GetMaintenanceWindowExecutionAsync(GetMaintenanceWindowExecutionRequest request)
        {
            return SendAsync<GetMaintenanceWindowExecutionResponse>(request);
        }

        public Task<GetMaintenanceWindowExecutionTaskResponse> GetMaintenanceWindowExecutionTaskAsync(GetMaintenanceWindowExecutionTaskRequest request)
        {
            return SendAsync<GetMaintenanceWindowExecutionTaskResponse>(request);
        }

        public Task<GetParameterHistoryResponse> GetParameterHistoryAsync(GetParameterHistoryRequest request)
        {
            return SendAsync<GetParameterHistoryResponse>(request);
        }

        public Task<GetParametersResponse> GetParametersAsync(GetParametersRequest request)
        {
            return SendAsync<GetParametersResponse>(request);
        }

        public Task<GetPatchBaselineResponse> GetPatchBaselineAsync(GetPatchBaselineRequest request)
        {
            return SendAsync<GetPatchBaselineResponse>(request);
        }

        public Task<GetPatchBaselineForPatchGroupResponse> GetPatchBaselineForPatchGroupAsync(GetPatchBaselineForPatchGroupRequest request)
        {
            return SendAsync<GetPatchBaselineForPatchGroupResponse>(request);
        }

        public Task<ListAssociationsResponse> ListAssociationsAsync(ListAssociationsRequest request)
        {
            return SendAsync<ListAssociationsResponse>(request);
        }

        public Task<ListCommandInvocationsResponse> ListCommandInvocationsAsync(ListCommandInvocationsRequest request)
        {
            return SendAsync<ListCommandInvocationsResponse>(request);
        }

        public Task<ListCommandsResponse> ListCommands(ListCommandsRequest request)
        {
            return SendAsync<ListCommandsResponse>(request);
        }

        public Task<ListDocumentsResponse> ListDocumentsAsync(ListDocumentsRequest request)
        {
            return SendAsync<ListDocumentsResponse>(request);
        }

        public Task<ListDocumentVersionsResponse> ListDocumentVersionsAsync(ListDocumentVersionsRequest request)
        {
            return SendAsync<ListDocumentVersionsResponse>(request);
        }
        public Task<ListInventoryEntriesResponse> ListInventoryEntriesAsync(ListInventoryEntriesRequest request)
        {
            return SendAsync<ListInventoryEntriesResponse>(request);
        }

        public Task<ListTagsForResourceResponse> ListTagsForResourceAsync(ListTagsForResourceRequest request)
        {
            return SendAsync<ListTagsForResourceResponse>(request);
        }

        public Task<ModifyDocumentPermissionResponse> ModifyDocumentPermissionAsync(ModifyDocumentPermissionRequest request)
        {
            return SendAsync<ModifyDocumentPermissionResponse>(request);
        }

        public Task<PutInventoryResponse> PutInventoryAsync(PutInventoryRequest request)
        {
            return SendAsync<PutInventoryResponse>(request);
        }

        public Task<PutParameterResponse> PutParameterAsync(PutParameterRequest request)
        {
            return SendAsync<PutParameterResponse>(request);
        }

        public Task<RegisterDefaultPatchBaselineResponse> RegisterDefaultPatchBaselineAsync(RegisterDefaultPatchBaselineRequest request)
        {
            return SendAsync<RegisterDefaultPatchBaselineResponse>(request);
        }

        public Task<RegisterPatchBaselineForPatchGroupResponse> RegisterPatchBaselineForPatchGroupAsync(RegisterPatchBaselineForPatchGroupRequest request)
        {
            return SendAsync<RegisterPatchBaselineForPatchGroupResponse>(request);
        }

        public Task<RegisterTargetWithMaintenanceWindowResponse> RegisterTargetWithMaintenanceWindowAsync(RegisterTargetWithMaintenanceWindowRequest request)
        {
            return SendAsync<RegisterTargetWithMaintenanceWindowResponse>(request);
        }

        public Task<RegisterTaskWithMaintenanceWindowResponse> RegisterTaskWithMaintenanceWindowAsync(RegisterTaskWithMaintenanceWindowRequest request)
        {
            return SendAsync<RegisterTaskWithMaintenanceWindowResponse>(request);
        }

        public Task<RemoveTagsFromResourceResponse> RemoveTagsFromResourceAsync(RemoveTagsFromResourceRequest request)
        {
            return SendAsync<RemoveTagsFromResourceResponse>(request);
        }
        public Task<SendCommandResponse> SendCommand(SendCommandRequest request)
        {
            return SendAsync<SendCommandResponse>(request);
        }

        public Task<StartAutomationExecutionResponse> StartAutomationExecutionAsync(StartAutomationExecutionRequest request)
        {
            return SendAsync<StartAutomationExecutionResponse>(request);
        }

        public Task<StopAutomationExecutionResponse> StopAutomationExecutionAsync(StopAutomationExecutionRequest request)
        {
            return SendAsync<StopAutomationExecutionResponse>(request);
        }

        public Task<UpdateAssociationResponse> UpdateAssociationAsync(UpdateAssociationRequest request)
        {
            return SendAsync<UpdateAssociationResponse>(request);
        }

        public Task<UpdateAssociationStatusResponse> UpdateAssociationStatusAsync(UpdateAssociationStatusRequest request)
        {
            return SendAsync<UpdateAssociationStatusResponse>(request);
        }

        public Task<UpdateDocumentResponse> UpdateDocumentAsync(UpdateDocumentRequest request)
        {
            return SendAsync<UpdateDocumentResponse>(request);
        }

        public Task<UpdateDocumentDefaultVersionResponse> UpdateDocumentDefaultVersionAsync(UpdateDocumentDefaultVersionRequest request)
        {
            return SendAsync<UpdateDocumentDefaultVersionResponse>(request);
        }

        public Task<UpdateMaintenanceWindowResponse> UpdateMaintenanceWindowAsync(UpdateMaintenanceWindowRequest request)
        {
            return SendAsync<UpdateMaintenanceWindowResponse>(request);
        }

        public Task<UpdateManagedInstanceRoleResponse> UpdateManagedInstanceRoleAsync(UpdateManagedInstanceRoleRequest request)
        {
            return SendAsync<UpdateManagedInstanceRoleResponse>(request);
        }

        public Task<UpdatePatchBaselineResponse> UpdatePatchBaselineAsync(UpdatePatchBaselineRequest request)
        {
            return SendAsync<UpdatePatchBaselineResponse>(request);

        }

        #region Helpers

        private async Task<T> SendAsync<T>(object request)
            where T : new()
        {
            var message = new HttpRequestMessage();

            var responseText = await SendAsync(message);

            return JsonObject.Parse(responseText).As<T>();
        }


        private FormUrlEncodedContent GetPostContent(AwsRequest request)
        {
            request.Add("Version", Version);

            return new FormUrlEncodedContent(request.Parameters);
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync();

            throw new Exception(response.StatusCode + "/" + responseText);
        }

        #endregion
    }
}

/*
AddTagsToResource
CancelCommand
CreateActivation
CreateAssociation
CreateAssociationBatch
CreateDocument
CreateMaintenanceWindow
CreatePatchBaseline
DeleteActivation
DeleteAssociation
DeleteDocument
DeleteMaintenanceWindow
DeleteParameter
DeletePatchBaseline
DeregisterManagedInstance
DeregisterPatchBaselineForPatchGroup
DeregisterTargetFromMaintenanceWindow
DeregisterTaskFromMaintenanceWindow
DescribeActivations
DescribeAssociation
DescribeAutomationExecutions
DescribeAvailablePatches
DescribeDocument
DescribeDocumentPermission
DescribeEffectiveInstanceAssociations
DescribeEffectivePatchesForPatchBaseline
DescribeInstanceAssociationsStatus
DescribeInstanceInformation
DescribeInstancePatches
DescribeInstancePatchStates
DescribeInstancePatchStatesForPatchGroup
DescribeMaintenanceWindowExecutions
DescribeMaintenanceWindowExecutionTaskInvocations
DescribeMaintenanceWindowExecutionTasks
DescribeMaintenanceWindows
DescribeMaintenanceWindowTargets
DescribeMaintenanceWindowTasks
DescribeParameters
DescribePatchBaselines
DescribePatchGroups
DescribePatchGroupState
GetAutomationExecution
GetCommandInvocation
GetDefaultPatchBaseline
GetDeployablePatchSnapshotForInstance
GetDocument
GetInventory
GetInventorySchema
GetMaintenanceWindow
GetMaintenanceWindowExecution
GetMaintenanceWindowExecutionTask
GetParameterHistory
GetParameters
GetPatchBaseline
GetPatchBaselineForPatchGroup
ListAssociations
ListCommandInvocations
ListCommands
ListDocuments
ListDocumentVersions
ListInventoryEntries
ListTagsForResource
ModifyDocumentPermission
PutInventory
PutParameter
RegisterDefaultPatchBaseline
RegisterPatchBaselineForPatchGroup
RegisterTargetWithMaintenanceWindow
RegisterTaskWithMaintenanceWindow
RemoveTagsFromResource
SendCommand
StartAutomationExecution
StopAutomationExecution
UpdateAssociation
UpdateAssociationStatus
UpdateDocument
UpdateDocumentDefaultVersion
UpdateMaintenanceWindow
UpdateManagedInstanceRole
UpdatePatchBaseline
*/
