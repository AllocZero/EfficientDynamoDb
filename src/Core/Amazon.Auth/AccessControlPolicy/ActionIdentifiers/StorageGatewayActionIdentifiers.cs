/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Auth.AccessControlPolicy.ActionIdentifiers
{
    /// <summary>
    /// The available AWS access control policy actions for Amazon Storage Gateway.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class StorageGatewayActionIdentifiers
    {
        public static readonly ActionIdentifier AllStorageGatewayActions = new ActionIdentifier("storagegateway:*");

        public static readonly ActionIdentifier ActivateGateway = new ActionIdentifier("storagegateway:ActivateGateway");
        public static readonly ActionIdentifier AddCache = new ActionIdentifier("storagegateway:AddCache");
        public static readonly ActionIdentifier AddUploadBuffer = new ActionIdentifier("storagegateway:AddUploadBuffer");
        public static readonly ActionIdentifier AddWorkingStorage = new ActionIdentifier("storagegateway:AddWorkingStorage");
        public static readonly ActionIdentifier CancelArchival = new ActionIdentifier("storagegateway:CancelArchival");
        public static readonly ActionIdentifier CancelRetrieval = new ActionIdentifier("storagegateway:CancelRetrieval");
        public static readonly ActionIdentifier CreateCachediSCSIVolume = new ActionIdentifier("storagegateway:CreateCachediSCSIVolume");
        public static readonly ActionIdentifier CreateSnapshot = new ActionIdentifier("storagegateway:CreateSnapshot");
        public static readonly ActionIdentifier CreateSnapshotFromVolumeRecoveryPoint = new ActionIdentifier("storagegateway:CreateSnapshotFromVolumeRecoveryPoint");
        public static readonly ActionIdentifier CreateStorediSCSIVolume = new ActionIdentifier("storagegateway:CreateStorediSCSIVolume");
        public static readonly ActionIdentifier CreateTapes = new ActionIdentifier("storagegateway:CreateTapes");
        public static readonly ActionIdentifier DeleteBandwidthRateLimit = new ActionIdentifier("storagegateway:DeleteBandwidthRateLimit");
        public static readonly ActionIdentifier DeleteChapCredentials = new ActionIdentifier("storagegateway:DeleteChapCredentials");
        public static readonly ActionIdentifier DeleteGateway = new ActionIdentifier("storagegateway:DeleteGateway");
        public static readonly ActionIdentifier DeleteSnapshotSchedule = new ActionIdentifier("storagegateway:DeleteSnapshotSchedule");
        public static readonly ActionIdentifier DeleteTape = new ActionIdentifier("storagegateway:DeleteTape");
        public static readonly ActionIdentifier DeleteTapeArchive = new ActionIdentifier("storagegateway:DeleteTapeArchive");
        public static readonly ActionIdentifier DeleteVolume = new ActionIdentifier("storagegateway:DeleteVolume");
        public static readonly ActionIdentifier DescribeBandwidthRateLimit = new ActionIdentifier("storagegateway:DescribeBandwidthRateLimit");
        public static readonly ActionIdentifier DescribeCache = new ActionIdentifier("storagegateway:DescribeCache");
        public static readonly ActionIdentifier DescribeCachediSCSIVolumes = new ActionIdentifier("storagegateway:DescribeCachediSCSIVolumes");
        public static readonly ActionIdentifier DescribeChapCredentials = new ActionIdentifier("storagegateway:DescribeChapCredentials");
        public static readonly ActionIdentifier DescribeGatewayInformation = new ActionIdentifier("storagegateway:DescribeGatewayInformation");
        public static readonly ActionIdentifier DescribeMaintenanceStartTime = new ActionIdentifier("storagegateway:DescribeMaintenanceStartTime");
        public static readonly ActionIdentifier DescribeSnapshotSchedule = new ActionIdentifier("storagegateway:DescribeSnapshotSchedule");
        public static readonly ActionIdentifier DescribeStorediSCSIVolumes = new ActionIdentifier("storagegateway:DescribeStorediSCSIVolumes");
        public static readonly ActionIdentifier DescribeTapeArchives = new ActionIdentifier("storagegateway:DescribeTapeArchives");
        public static readonly ActionIdentifier DescribeTapeRecoveryPoints = new ActionIdentifier("storagegateway:DescribeTapeRecoveryPoints");
        public static readonly ActionIdentifier DescribeTapes = new ActionIdentifier("storagegateway:DescribeTapes");
        public static readonly ActionIdentifier DescribeUploadBuffer = new ActionIdentifier("storagegateway:DescribeUploadBuffer");
        public static readonly ActionIdentifier DescribeVTLDevices = new ActionIdentifier("storagegateway:DescribeVTLDevices");
        public static readonly ActionIdentifier DescribeWorkingStorage = new ActionIdentifier("storagegateway:DescribeWorkingStorage");
        public static readonly ActionIdentifier DisableGateway = new ActionIdentifier("storagegateway:DisableGateway");
        public static readonly ActionIdentifier ListGateways = new ActionIdentifier("storagegateway:ListGateways");
        public static readonly ActionIdentifier ListLocalDisks = new ActionIdentifier("storagegateway:ListLocalDisks");
        public static readonly ActionIdentifier ListVolumeRecoveryPoints = new ActionIdentifier("storagegateway:ListVolumeRecoveryPoints");
        public static readonly ActionIdentifier ListVolumes = new ActionIdentifier("storagegateway:ListVolumes");
        public static readonly ActionIdentifier RetrieveTapeArchive = new ActionIdentifier("storagegateway:RetrieveTapeArchive");
        public static readonly ActionIdentifier RetrieveTapeRecoveryPoint = new ActionIdentifier("storagegateway:RetrieveTapeRecoveryPoint");
        public static readonly ActionIdentifier ShutdownGateway = new ActionIdentifier("storagegateway:ShutdownGateway");
        public static readonly ActionIdentifier StartGateway = new ActionIdentifier("storagegateway:StartGateway");
        public static readonly ActionIdentifier UpdateBandwidthRateLimit = new ActionIdentifier("storagegateway:UpdateBandwidthRateLimit");
        public static readonly ActionIdentifier UpdateChapCredentials = new ActionIdentifier("storagegateway:UpdateChapCredentials");
        public static readonly ActionIdentifier UpdateGatewayInformation = new ActionIdentifier("storagegateway:UpdateGatewayInformation");
        public static readonly ActionIdentifier UpdateGatewaySoftwareNow = new ActionIdentifier("storagegateway:UpdateGatewaySoftwareNow");
        public static readonly ActionIdentifier UpdateMaintenanceStartTime = new ActionIdentifier("storagegateway:UpdateMaintenanceStartTime");
        public static readonly ActionIdentifier UpdateSnapshotSchedule = new ActionIdentifier("storagegateway:UpdateSnapshotSchedule");
    }
}

