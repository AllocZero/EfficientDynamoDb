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
    /// The available AWS access control policy actions for Amazon EC2.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class EC2ActionIdentifiers
    {
        public static readonly ActionIdentifier AllEC2Actions = new ActionIdentifier("ec2:*");

        public static readonly ActionIdentifier AcceptVpcPeeringConnection = new ActionIdentifier("ec2:AcceptVpcPeeringConnection");
        public static readonly ActionIdentifier ActivateLicense = new ActionIdentifier("ec2:ActivateLicense");
        public static readonly ActionIdentifier AllocateAddress = new ActionIdentifier("ec2:AllocateAddress");
        public static readonly ActionIdentifier AssignPrivateIpAddresses = new ActionIdentifier("ec2:AssignPrivateIpAddresses");
        public static readonly ActionIdentifier AssociateAddress = new ActionIdentifier("ec2:AssociateAddress");
        public static readonly ActionIdentifier AssociateDhcpOptions = new ActionIdentifier("ec2:AssociateDhcpOptions");
        public static readonly ActionIdentifier AssociateRouteTable = new ActionIdentifier("ec2:AssociateRouteTable");
        public static readonly ActionIdentifier AttachInternetGateway = new ActionIdentifier("ec2:AttachInternetGateway");
        public static readonly ActionIdentifier AttachNetworkInterface = new ActionIdentifier("ec2:AttachNetworkInterface");
        public static readonly ActionIdentifier AttachVolume = new ActionIdentifier("ec2:AttachVolume");
        public static readonly ActionIdentifier AttachVpnGateway = new ActionIdentifier("ec2:AttachVpnGateway");
        public static readonly ActionIdentifier AuthorizeSecurityGroupEgress = new ActionIdentifier("ec2:AuthorizeSecurityGroupEgress");
        public static readonly ActionIdentifier AuthorizeSecurityGroupIngress = new ActionIdentifier("ec2:AuthorizeSecurityGroupIngress");
        public static readonly ActionIdentifier BundleInstance = new ActionIdentifier("ec2:BundleInstance");
        public static readonly ActionIdentifier CancelBundleTask = new ActionIdentifier("ec2:CancelBundleTask");
        public static readonly ActionIdentifier CancelConversionTask = new ActionIdentifier("ec2:CancelConversionTask");
        public static readonly ActionIdentifier CancelExportTask = new ActionIdentifier("ec2:CancelExportTask");
        public static readonly ActionIdentifier CancelReservedInstancesListing = new ActionIdentifier("ec2:CancelReservedInstancesListing");
        public static readonly ActionIdentifier CancelSpotInstanceRequests = new ActionIdentifier("ec2:CancelSpotInstanceRequests");
        public static readonly ActionIdentifier ConfirmProductInstance = new ActionIdentifier("ec2:ConfirmProductInstance");
        public static readonly ActionIdentifier CopyImage = new ActionIdentifier("ec2:CopyImage");
        public static readonly ActionIdentifier CopySnapshot = new ActionIdentifier("ec2:CopySnapshot");
        public static readonly ActionIdentifier CreateCustomerGateway = new ActionIdentifier("ec2:CreateCustomerGateway");
        public static readonly ActionIdentifier CreateDhcpOptions = new ActionIdentifier("ec2:CreateDhcpOptions");
        public static readonly ActionIdentifier CreateImage = new ActionIdentifier("ec2:CreateImage");
        public static readonly ActionIdentifier CreateInstanceExportTask = new ActionIdentifier("ec2:CreateInstanceExportTask");
        public static readonly ActionIdentifier CreateInternetGateway = new ActionIdentifier("ec2:CreateInternetGateway");
        public static readonly ActionIdentifier CreateKeyPair = new ActionIdentifier("ec2:CreateKeyPair");
        public static readonly ActionIdentifier CreateNetworkAcl = new ActionIdentifier("ec2:CreateNetworkAcl");
        public static readonly ActionIdentifier CreateNetworkAclEntry = new ActionIdentifier("ec2:CreateNetworkAclEntry");
        public static readonly ActionIdentifier CreateNetworkInterface = new ActionIdentifier("ec2:CreateNetworkInterface");
        public static readonly ActionIdentifier CreatePlacementGroup = new ActionIdentifier("ec2:CreatePlacementGroup");
        public static readonly ActionIdentifier CreateReservedInstancesListing = new ActionIdentifier("ec2:CreateReservedInstancesListing");
        public static readonly ActionIdentifier CreateRoute = new ActionIdentifier("ec2:CreateRoute");
        public static readonly ActionIdentifier CreateRouteTable = new ActionIdentifier("ec2:CreateRouteTable");
        public static readonly ActionIdentifier CreateSecurityGroup = new ActionIdentifier("ec2:CreateSecurityGroup");
        public static readonly ActionIdentifier CreateSnapshot = new ActionIdentifier("ec2:CreateSnapshot");
        public static readonly ActionIdentifier CreateSpotDatafeedSubscription = new ActionIdentifier("ec2:CreateSpotDatafeedSubscription");
        public static readonly ActionIdentifier CreateSubnet = new ActionIdentifier("ec2:CreateSubnet");
        public static readonly ActionIdentifier CreateTags = new ActionIdentifier("ec2:CreateTags");
        public static readonly ActionIdentifier CreateVolume = new ActionIdentifier("ec2:CreateVolume");
        public static readonly ActionIdentifier CreateVpc = new ActionIdentifier("ec2:CreateVpc");
        public static readonly ActionIdentifier CreateVpcPeeringConnection = new ActionIdentifier("ec2:CreateVpcPeeringConnection");
        public static readonly ActionIdentifier CreateVpnConnection = new ActionIdentifier("ec2:CreateVpnConnection");
        public static readonly ActionIdentifier CreateVpnConnectionRoute = new ActionIdentifier("ec2:CreateVpnConnectionRoute");
        public static readonly ActionIdentifier CreateVpnGateway = new ActionIdentifier("ec2:CreateVpnGateway");
        public static readonly ActionIdentifier DeactivateLicense = new ActionIdentifier("ec2:DeactivateLicense");
        public static readonly ActionIdentifier DeleteCustomerGateway = new ActionIdentifier("ec2:DeleteCustomerGateway");
        public static readonly ActionIdentifier DeleteDhcpOptions = new ActionIdentifier("ec2:DeleteDhcpOptions");
        public static readonly ActionIdentifier DeleteInternetGateway = new ActionIdentifier("ec2:DeleteInternetGateway");
        public static readonly ActionIdentifier DeleteKeyPair = new ActionIdentifier("ec2:DeleteKeyPair");
        public static readonly ActionIdentifier DeleteNetworkAcl = new ActionIdentifier("ec2:DeleteNetworkAcl");
        public static readonly ActionIdentifier DeleteNetworkAclEntry = new ActionIdentifier("ec2:DeleteNetworkAclEntry");
        public static readonly ActionIdentifier DeleteNetworkInterface = new ActionIdentifier("ec2:DeleteNetworkInterface");
        public static readonly ActionIdentifier DeletePlacementGroup = new ActionIdentifier("ec2:DeletePlacementGroup");
        public static readonly ActionIdentifier DeleteRoute = new ActionIdentifier("ec2:DeleteRoute");
        public static readonly ActionIdentifier DeleteRouteTable = new ActionIdentifier("ec2:DeleteRouteTable");
        public static readonly ActionIdentifier DeleteSecurityGroup = new ActionIdentifier("ec2:DeleteSecurityGroup");
        public static readonly ActionIdentifier DeleteSnapshot = new ActionIdentifier("ec2:DeleteSnapshot");
        public static readonly ActionIdentifier DeleteSpotDatafeedSubscription = new ActionIdentifier("ec2:DeleteSpotDatafeedSubscription");
        public static readonly ActionIdentifier DeleteSubnet = new ActionIdentifier("ec2:DeleteSubnet");
        public static readonly ActionIdentifier DeleteTags = new ActionIdentifier("ec2:DeleteTags");
        public static readonly ActionIdentifier DeleteVolume = new ActionIdentifier("ec2:DeleteVolume");
        public static readonly ActionIdentifier DeleteVpc = new ActionIdentifier("ec2:DeleteVpc");
        public static readonly ActionIdentifier DeleteVpcPeeringConnection = new ActionIdentifier("ec2:DeleteVpcPeeringConnection");
        public static readonly ActionIdentifier DeleteVpnConnection = new ActionIdentifier("ec2:DeleteVpnConnection");
        public static readonly ActionIdentifier DeleteVpnConnectionRoute = new ActionIdentifier("ec2:DeleteVpnConnectionRoute");
        public static readonly ActionIdentifier DeleteVpnGateway = new ActionIdentifier("ec2:DeleteVpnGateway");
        public static readonly ActionIdentifier DeregisterImage = new ActionIdentifier("ec2:DeregisterImage");
        public static readonly ActionIdentifier DescribeAccountAttributes = new ActionIdentifier("ec2:DescribeAccountAttributes");
        public static readonly ActionIdentifier DescribeAddresses = new ActionIdentifier("ec2:DescribeAddresses");
        public static readonly ActionIdentifier DescribeAvailabilityZones = new ActionIdentifier("ec2:DescribeAvailabilityZones");
        public static readonly ActionIdentifier DescribeBundleTasks = new ActionIdentifier("ec2:DescribeBundleTasks");
        public static readonly ActionIdentifier DescribeConversionTasks = new ActionIdentifier("ec2:DescribeConversionTasks");
        public static readonly ActionIdentifier DescribeCustomerGateways = new ActionIdentifier("ec2:DescribeCustomerGateways");
        public static readonly ActionIdentifier DescribeDhcpOptions = new ActionIdentifier("ec2:DescribeDhcpOptions");
        public static readonly ActionIdentifier DescribeExportTasks = new ActionIdentifier("ec2:DescribeExportTasks");
        public static readonly ActionIdentifier DescribeImageAttribute = new ActionIdentifier("ec2:DescribeImageAttribute");
        public static readonly ActionIdentifier DescribeImages = new ActionIdentifier("ec2:DescribeImages");
        public static readonly ActionIdentifier DescribeInstanceAttribute = new ActionIdentifier("ec2:DescribeInstanceAttribute");
        public static readonly ActionIdentifier DescribeInstanceStatus = new ActionIdentifier("ec2:DescribeInstanceStatus");
        public static readonly ActionIdentifier DescribeInstances = new ActionIdentifier("ec2:DescribeInstances");
        public static readonly ActionIdentifier DescribeInternetGateways = new ActionIdentifier("ec2:DescribeInternetGateways");
        public static readonly ActionIdentifier DescribeKeyPairs = new ActionIdentifier("ec2:DescribeKeyPairs");
        public static readonly ActionIdentifier DescribeLicenses = new ActionIdentifier("ec2:DescribeLicenses");
        public static readonly ActionIdentifier DescribeNetworkAcls = new ActionIdentifier("ec2:DescribeNetworkAcls");
        public static readonly ActionIdentifier DescribeNetworkInterfaceAttribute = new ActionIdentifier("ec2:DescribeNetworkInterfaceAttribute");
        public static readonly ActionIdentifier DescribeNetworkInterfaces = new ActionIdentifier("ec2:DescribeNetworkInterfaces");
        public static readonly ActionIdentifier DescribePlacementGroups = new ActionIdentifier("ec2:DescribePlacementGroups");
        public static readonly ActionIdentifier DescribeRegions = new ActionIdentifier("ec2:DescribeRegions");
        public static readonly ActionIdentifier DescribeReservedInstances = new ActionIdentifier("ec2:DescribeReservedInstances");
        public static readonly ActionIdentifier DescribeReservedInstancesListings = new ActionIdentifier("ec2:DescribeReservedInstancesListings");
        public static readonly ActionIdentifier DescribeReservedInstancesModifications = new ActionIdentifier("ec2:DescribeReservedInstancesModifications");
        public static readonly ActionIdentifier DescribeReservedInstancesOfferings = new ActionIdentifier("ec2:DescribeReservedInstancesOfferings");
        public static readonly ActionIdentifier DescribeRouteTables = new ActionIdentifier("ec2:DescribeRouteTables");
        public static readonly ActionIdentifier DescribeSecurityGroups = new ActionIdentifier("ec2:DescribeSecurityGroups");
        public static readonly ActionIdentifier DescribeSnapshotAttribute = new ActionIdentifier("ec2:DescribeSnapshotAttribute");
        public static readonly ActionIdentifier DescribeSnapshots = new ActionIdentifier("ec2:DescribeSnapshots");
        public static readonly ActionIdentifier DescribeSpotDatafeedSubscription = new ActionIdentifier("ec2:DescribeSpotDatafeedSubscription");
        public static readonly ActionIdentifier DescribeSpotInstanceRequests = new ActionIdentifier("ec2:DescribeSpotInstanceRequests");
        public static readonly ActionIdentifier DescribeSpotPriceHistory = new ActionIdentifier("ec2:DescribeSpotPriceHistory");
        public static readonly ActionIdentifier DescribeSubnets = new ActionIdentifier("ec2:DescribeSubnets");
        public static readonly ActionIdentifier DescribeTags = new ActionIdentifier("ec2:DescribeTags");
        public static readonly ActionIdentifier DescribeVolumeAttribute = new ActionIdentifier("ec2:DescribeVolumeAttribute");
        public static readonly ActionIdentifier DescribeVolumeStatus = new ActionIdentifier("ec2:DescribeVolumeStatus");
        public static readonly ActionIdentifier DescribeVolumes = new ActionIdentifier("ec2:DescribeVolumes");
        public static readonly ActionIdentifier DescribeVpcAttribute = new ActionIdentifier("ec2:DescribeVpcAttribute");
        public static readonly ActionIdentifier DescribeVpcs = new ActionIdentifier("ec2:DescribeVpcs");
        public static readonly ActionIdentifier DescribeVpcPeeringConnection = new ActionIdentifier("ec2:DescribeVpcPeeringConnection");
        public static readonly ActionIdentifier DescribeVpnConnections = new ActionIdentifier("ec2:DescribeVpnConnections");
        public static readonly ActionIdentifier DescribeVpnGateways = new ActionIdentifier("ec2:DescribeVpnGateways");
        public static readonly ActionIdentifier DetachInternetGateway = new ActionIdentifier("ec2:DetachInternetGateway");
        public static readonly ActionIdentifier DetachNetworkInterface = new ActionIdentifier("ec2:DetachNetworkInterface");
        public static readonly ActionIdentifier DetachVolume = new ActionIdentifier("ec2:DetachVolume");
        public static readonly ActionIdentifier DetachVpnGateway = new ActionIdentifier("ec2:DetachVpnGateway");
        public static readonly ActionIdentifier DisableVgwRoutePropagation = new ActionIdentifier("ec2:DisableVgwRoutePropagation");
        public static readonly ActionIdentifier DisassociateAddress = new ActionIdentifier("ec2:DisassociateAddress");
        public static readonly ActionIdentifier DisassociateRouteTable = new ActionIdentifier("ec2:DisassociateRouteTable");
        public static readonly ActionIdentifier EnableVgwRoutePropagation = new ActionIdentifier("ec2:EnableVgwRoutePropagation");
        public static readonly ActionIdentifier EnableVolumeIO = new ActionIdentifier("ec2:EnableVolumeIO");
        public static readonly ActionIdentifier GetConsoleOutput = new ActionIdentifier("ec2:GetConsoleOutput");
        public static readonly ActionIdentifier GetPasswordData = new ActionIdentifier("ec2:GetPasswordData");
        public static readonly ActionIdentifier ImportInstance = new ActionIdentifier("ec2:ImportInstance");
        public static readonly ActionIdentifier ImportKeyPair = new ActionIdentifier("ec2:ImportKeyPair");
        public static readonly ActionIdentifier ImportVolume = new ActionIdentifier("ec2:ImportVolume");
        public static readonly ActionIdentifier ModifyImageAttribute = new ActionIdentifier("ec2:ModifyImageAttribute");
        public static readonly ActionIdentifier ModifyInstanceAttribute = new ActionIdentifier("ec2:ModifyInstanceAttribute");
        public static readonly ActionIdentifier ModifyNetworkInterfaceAttribute = new ActionIdentifier("ec2:ModifyNetworkInterfaceAttribute");
        public static readonly ActionIdentifier ModifyReservedInstances = new ActionIdentifier("ec2:ModifyReservedInstances");
        public static readonly ActionIdentifier ModifySnapshotAttribute = new ActionIdentifier("ec2:ModifySnapshotAttribute");
        public static readonly ActionIdentifier ModifyVolumeAttribute = new ActionIdentifier("ec2:ModifyVolumeAttribute");
        public static readonly ActionIdentifier ModifyVpcAttribute = new ActionIdentifier("ec2:ModifyVpcAttribute");
        public static readonly ActionIdentifier MonitorInstances = new ActionIdentifier("ec2:MonitorInstances");
        public static readonly ActionIdentifier PurchaseReservedInstancesOffering = new ActionIdentifier("ec2:PurchaseReservedInstancesOffering");
        public static readonly ActionIdentifier RebootInstances = new ActionIdentifier("ec2:RebootInstances");
        public static readonly ActionIdentifier RegisterImage = new ActionIdentifier("ec2:RegisterImage");
        public static readonly ActionIdentifier RejectVpcPeeringConnection = new ActionIdentifier("ec2:RejectVpcPeeringConnection");
        public static readonly ActionIdentifier ReleaseAddress = new ActionIdentifier("ec2:ReleaseAddress");
        public static readonly ActionIdentifier ReplaceNetworkAclAssociation = new ActionIdentifier("ec2:ReplaceNetworkAclAssociation");
        public static readonly ActionIdentifier ReplaceNetworkAclEntry = new ActionIdentifier("ec2:ReplaceNetworkAclEntry");
        public static readonly ActionIdentifier ReplaceRoute = new ActionIdentifier("ec2:ReplaceRoute");
        public static readonly ActionIdentifier ReplaceRouteTableAssociation = new ActionIdentifier("ec2:ReplaceRouteTableAssociation");
        public static readonly ActionIdentifier ReportInstanceStatus = new ActionIdentifier("ec2:ReportInstanceStatus");
        public static readonly ActionIdentifier RequestSpotInstances = new ActionIdentifier("ec2:RequestSpotInstances");
        public static readonly ActionIdentifier ResetImageAttribute = new ActionIdentifier("ec2:ResetImageAttribute");
        public static readonly ActionIdentifier ResetInstanceAttribute = new ActionIdentifier("ec2:ResetInstanceAttribute");
        public static readonly ActionIdentifier ResetNetworkInterfaceAttribute = new ActionIdentifier("ec2:ResetNetworkInterfaceAttribute");
        public static readonly ActionIdentifier ResetSnapshotAttribute = new ActionIdentifier("ec2:ResetSnapshotAttribute");
        public static readonly ActionIdentifier RevokeSecurityGroupEgress = new ActionIdentifier("ec2:RevokeSecurityGroupEgress");
        public static readonly ActionIdentifier RevokeSecurityGroupIngress = new ActionIdentifier("ec2:RevokeSecurityGroupIngress");
        public static readonly ActionIdentifier RunInstances = new ActionIdentifier("ec2:RunInstances");
        public static readonly ActionIdentifier StartInstances = new ActionIdentifier("ec2:StartInstances");
        public static readonly ActionIdentifier StopInstances = new ActionIdentifier("ec2:StopInstances");
        public static readonly ActionIdentifier TerminateInstances = new ActionIdentifier("ec2:TerminateInstances");
        public static readonly ActionIdentifier UnassignPrivateIpAddresses = new ActionIdentifier("ec2:UnassignPrivateIpAddresses");
        public static readonly ActionIdentifier UnmonitorInstances = new ActionIdentifier("ec2:UnmonitorInstances");
    }
}

