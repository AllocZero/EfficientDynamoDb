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

/*
 * Do not modify this file. This file is generated from the dynamodb-2012-08-10.normal.json service model.
 */

using System;

using Amazon.Runtime;

namespace Amazon.DynamoDBv2
{

    /// <summary>
    /// Constants used for properties of type AttributeAction.
    /// </summary>
    public class AttributeAction : ConstantClass
    {

        /// <summary>
        /// Constant ADD for AttributeAction
        /// </summary>
        public static readonly AttributeAction ADD = new AttributeAction("ADD");
        /// <summary>
        /// Constant DELETE for AttributeAction
        /// </summary>
        public static readonly AttributeAction DELETE = new AttributeAction("DELETE");
        /// <summary>
        /// Constant PUT for AttributeAction
        /// </summary>
        public static readonly AttributeAction PUT = new AttributeAction("PUT");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public AttributeAction(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static AttributeAction FindValue(string value)
        {
            return FindValue<AttributeAction>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator AttributeAction(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type BackupStatus.
    /// </summary>
    public class BackupStatus : ConstantClass
    {

        /// <summary>
        /// Constant AVAILABLE for BackupStatus
        /// </summary>
        public static readonly BackupStatus AVAILABLE = new BackupStatus("AVAILABLE");
        /// <summary>
        /// Constant CREATING for BackupStatus
        /// </summary>
        public static readonly BackupStatus CREATING = new BackupStatus("CREATING");
        /// <summary>
        /// Constant DELETED for BackupStatus
        /// </summary>
        public static readonly BackupStatus DELETED = new BackupStatus("DELETED");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public BackupStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static BackupStatus FindValue(string value)
        {
            return FindValue<BackupStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator BackupStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type BackupType.
    /// </summary>
    public class BackupType : ConstantClass
    {

        /// <summary>
        /// Constant AWS_BACKUP for BackupType
        /// </summary>
        public static readonly BackupType AWS_BACKUP = new BackupType("AWS_BACKUP");
        /// <summary>
        /// Constant SYSTEM for BackupType
        /// </summary>
        public static readonly BackupType SYSTEM = new BackupType("SYSTEM");
        /// <summary>
        /// Constant USER for BackupType
        /// </summary>
        public static readonly BackupType USER = new BackupType("USER");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public BackupType(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static BackupType FindValue(string value)
        {
            return FindValue<BackupType>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator BackupType(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type BackupTypeFilter.
    /// </summary>
    public class BackupTypeFilter : ConstantClass
    {

        /// <summary>
        /// Constant ALL for BackupTypeFilter
        /// </summary>
        public static readonly BackupTypeFilter ALL = new BackupTypeFilter("ALL");
        /// <summary>
        /// Constant AWS_BACKUP for BackupTypeFilter
        /// </summary>
        public static readonly BackupTypeFilter AWS_BACKUP = new BackupTypeFilter("AWS_BACKUP");
        /// <summary>
        /// Constant SYSTEM for BackupTypeFilter
        /// </summary>
        public static readonly BackupTypeFilter SYSTEM = new BackupTypeFilter("SYSTEM");
        /// <summary>
        /// Constant USER for BackupTypeFilter
        /// </summary>
        public static readonly BackupTypeFilter USER = new BackupTypeFilter("USER");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public BackupTypeFilter(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static BackupTypeFilter FindValue(string value)
        {
            return FindValue<BackupTypeFilter>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator BackupTypeFilter(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type BillingMode.
    /// </summary>
    public class BillingMode : ConstantClass
    {

        /// <summary>
        /// Constant PAY_PER_REQUEST for BillingMode
        /// </summary>
        public static readonly BillingMode PAY_PER_REQUEST = new BillingMode("PAY_PER_REQUEST");
        /// <summary>
        /// Constant PROVISIONED for BillingMode
        /// </summary>
        public static readonly BillingMode PROVISIONED = new BillingMode("PROVISIONED");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public BillingMode(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static BillingMode FindValue(string value)
        {
            return FindValue<BillingMode>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator BillingMode(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ComparisonOperator.
    /// </summary>
    public class ComparisonOperator : ConstantClass
    {

        /// <summary>
        /// Constant BEGINS_WITH for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator BEGINS_WITH = new ComparisonOperator("BEGINS_WITH");
        /// <summary>
        /// Constant BETWEEN for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator BETWEEN = new ComparisonOperator("BETWEEN");
        /// <summary>
        /// Constant CONTAINS for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator CONTAINS = new ComparisonOperator("CONTAINS");
        /// <summary>
        /// Constant EQ for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator EQ = new ComparisonOperator("EQ");
        /// <summary>
        /// Constant GE for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator GE = new ComparisonOperator("GE");
        /// <summary>
        /// Constant GT for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator GT = new ComparisonOperator("GT");
        /// <summary>
        /// Constant IN for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator IN = new ComparisonOperator("IN");
        /// <summary>
        /// Constant LE for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator LE = new ComparisonOperator("LE");
        /// <summary>
        /// Constant LT for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator LT = new ComparisonOperator("LT");
        /// <summary>
        /// Constant NE for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator NE = new ComparisonOperator("NE");
        /// <summary>
        /// Constant NOT_CONTAINS for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator NOT_CONTAINS = new ComparisonOperator("NOT_CONTAINS");
        /// <summary>
        /// Constant NOT_NULL for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator NOT_NULL = new ComparisonOperator("NOT_NULL");
        /// <summary>
        /// Constant NULL for ComparisonOperator
        /// </summary>
        public static readonly ComparisonOperator NULL = new ComparisonOperator("NULL");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ComparisonOperator(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ComparisonOperator FindValue(string value)
        {
            return FindValue<ComparisonOperator>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ComparisonOperator(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ConditionalOperator.
    /// </summary>
    public class ConditionalOperator : ConstantClass
    {

        /// <summary>
        /// Constant AND for ConditionalOperator
        /// </summary>
        public static readonly ConditionalOperator AND = new ConditionalOperator("AND");
        /// <summary>
        /// Constant OR for ConditionalOperator
        /// </summary>
        public static readonly ConditionalOperator OR = new ConditionalOperator("OR");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ConditionalOperator(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ConditionalOperator FindValue(string value)
        {
            return FindValue<ConditionalOperator>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ConditionalOperator(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ContinuousBackupsStatus.
    /// </summary>
    public class ContinuousBackupsStatus : ConstantClass
    {

        /// <summary>
        /// Constant DISABLED for ContinuousBackupsStatus
        /// </summary>
        public static readonly ContinuousBackupsStatus DISABLED = new ContinuousBackupsStatus("DISABLED");
        /// <summary>
        /// Constant ENABLED for ContinuousBackupsStatus
        /// </summary>
        public static readonly ContinuousBackupsStatus ENABLED = new ContinuousBackupsStatus("ENABLED");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ContinuousBackupsStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ContinuousBackupsStatus FindValue(string value)
        {
            return FindValue<ContinuousBackupsStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ContinuousBackupsStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ContributorInsightsAction.
    /// </summary>
    public class ContributorInsightsAction : ConstantClass
    {

        /// <summary>
        /// Constant DISABLE for ContributorInsightsAction
        /// </summary>
        public static readonly ContributorInsightsAction DISABLE = new ContributorInsightsAction("DISABLE");
        /// <summary>
        /// Constant ENABLE for ContributorInsightsAction
        /// </summary>
        public static readonly ContributorInsightsAction ENABLE = new ContributorInsightsAction("ENABLE");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ContributorInsightsAction(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ContributorInsightsAction FindValue(string value)
        {
            return FindValue<ContributorInsightsAction>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ContributorInsightsAction(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ContributorInsightsStatus.
    /// </summary>
    public class ContributorInsightsStatus : ConstantClass
    {

        /// <summary>
        /// Constant DISABLED for ContributorInsightsStatus
        /// </summary>
        public static readonly ContributorInsightsStatus DISABLED = new ContributorInsightsStatus("DISABLED");
        /// <summary>
        /// Constant DISABLING for ContributorInsightsStatus
        /// </summary>
        public static readonly ContributorInsightsStatus DISABLING = new ContributorInsightsStatus("DISABLING");
        /// <summary>
        /// Constant ENABLED for ContributorInsightsStatus
        /// </summary>
        public static readonly ContributorInsightsStatus ENABLED = new ContributorInsightsStatus("ENABLED");
        /// <summary>
        /// Constant ENABLING for ContributorInsightsStatus
        /// </summary>
        public static readonly ContributorInsightsStatus ENABLING = new ContributorInsightsStatus("ENABLING");
        /// <summary>
        /// Constant FAILED for ContributorInsightsStatus
        /// </summary>
        public static readonly ContributorInsightsStatus FAILED = new ContributorInsightsStatus("FAILED");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ContributorInsightsStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ContributorInsightsStatus FindValue(string value)
        {
            return FindValue<ContributorInsightsStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ContributorInsightsStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type GlobalTableStatus.
    /// </summary>
    public class GlobalTableStatus : ConstantClass
    {

        /// <summary>
        /// Constant ACTIVE for GlobalTableStatus
        /// </summary>
        public static readonly GlobalTableStatus ACTIVE = new GlobalTableStatus("ACTIVE");
        /// <summary>
        /// Constant CREATING for GlobalTableStatus
        /// </summary>
        public static readonly GlobalTableStatus CREATING = new GlobalTableStatus("CREATING");
        /// <summary>
        /// Constant DELETING for GlobalTableStatus
        /// </summary>
        public static readonly GlobalTableStatus DELETING = new GlobalTableStatus("DELETING");
        /// <summary>
        /// Constant UPDATING for GlobalTableStatus
        /// </summary>
        public static readonly GlobalTableStatus UPDATING = new GlobalTableStatus("UPDATING");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public GlobalTableStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static GlobalTableStatus FindValue(string value)
        {
            return FindValue<GlobalTableStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator GlobalTableStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type IndexStatus.
    /// </summary>
    public class IndexStatus : ConstantClass
    {

        /// <summary>
        /// Constant ACTIVE for IndexStatus
        /// </summary>
        public static readonly IndexStatus ACTIVE = new IndexStatus("ACTIVE");
        /// <summary>
        /// Constant CREATING for IndexStatus
        /// </summary>
        public static readonly IndexStatus CREATING = new IndexStatus("CREATING");
        /// <summary>
        /// Constant DELETING for IndexStatus
        /// </summary>
        public static readonly IndexStatus DELETING = new IndexStatus("DELETING");
        /// <summary>
        /// Constant UPDATING for IndexStatus
        /// </summary>
        public static readonly IndexStatus UPDATING = new IndexStatus("UPDATING");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public IndexStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static IndexStatus FindValue(string value)
        {
            return FindValue<IndexStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator IndexStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type KeyType.
    /// </summary>
    public class KeyType : ConstantClass
    {

        /// <summary>
        /// Constant HASH for KeyType
        /// </summary>
        public static readonly KeyType HASH = new KeyType("HASH");
        /// <summary>
        /// Constant RANGE for KeyType
        /// </summary>
        public static readonly KeyType RANGE = new KeyType("RANGE");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public KeyType(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static KeyType FindValue(string value)
        {
            return FindValue<KeyType>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator KeyType(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type PointInTimeRecoveryStatus.
    /// </summary>
    public class PointInTimeRecoveryStatus : ConstantClass
    {

        /// <summary>
        /// Constant DISABLED for PointInTimeRecoveryStatus
        /// </summary>
        public static readonly PointInTimeRecoveryStatus DISABLED = new PointInTimeRecoveryStatus("DISABLED");
        /// <summary>
        /// Constant ENABLED for PointInTimeRecoveryStatus
        /// </summary>
        public static readonly PointInTimeRecoveryStatus ENABLED = new PointInTimeRecoveryStatus("ENABLED");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public PointInTimeRecoveryStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static PointInTimeRecoveryStatus FindValue(string value)
        {
            return FindValue<PointInTimeRecoveryStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator PointInTimeRecoveryStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ProjectionType.
    /// </summary>
    public class ProjectionType : ConstantClass
    {

        /// <summary>
        /// Constant ALL for ProjectionType
        /// </summary>
        public static readonly ProjectionType ALL = new ProjectionType("ALL");
        /// <summary>
        /// Constant INCLUDE for ProjectionType
        /// </summary>
        public static readonly ProjectionType INCLUDE = new ProjectionType("INCLUDE");
        /// <summary>
        /// Constant KEYS_ONLY for ProjectionType
        /// </summary>
        public static readonly ProjectionType KEYS_ONLY = new ProjectionType("KEYS_ONLY");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ProjectionType(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ProjectionType FindValue(string value)
        {
            return FindValue<ProjectionType>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ProjectionType(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ReplicaStatus.
    /// </summary>
    public class ReplicaStatus : ConstantClass
    {

        /// <summary>
        /// Constant ACTIVE for ReplicaStatus
        /// </summary>
        public static readonly ReplicaStatus ACTIVE = new ReplicaStatus("ACTIVE");
        /// <summary>
        /// Constant CREATING for ReplicaStatus
        /// </summary>
        public static readonly ReplicaStatus CREATING = new ReplicaStatus("CREATING");
        /// <summary>
        /// Constant CREATION_FAILED for ReplicaStatus
        /// </summary>
        public static readonly ReplicaStatus CREATION_FAILED = new ReplicaStatus("CREATION_FAILED");
        /// <summary>
        /// Constant DELETING for ReplicaStatus
        /// </summary>
        public static readonly ReplicaStatus DELETING = new ReplicaStatus("DELETING");
        /// <summary>
        /// Constant INACCESSIBLE_ENCRYPTION_CREDENTIALS for ReplicaStatus
        /// </summary>
        public static readonly ReplicaStatus INACCESSIBLE_ENCRYPTION_CREDENTIALS = new ReplicaStatus("INACCESSIBLE_ENCRYPTION_CREDENTIALS");
        /// <summary>
        /// Constant REGION_DISABLED for ReplicaStatus
        /// </summary>
        public static readonly ReplicaStatus REGION_DISABLED = new ReplicaStatus("REGION_DISABLED");
        /// <summary>
        /// Constant UPDATING for ReplicaStatus
        /// </summary>
        public static readonly ReplicaStatus UPDATING = new ReplicaStatus("UPDATING");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ReplicaStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ReplicaStatus FindValue(string value)
        {
            return FindValue<ReplicaStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ReplicaStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ReturnConsumedCapacity.
    /// </summary>
    public class ReturnConsumedCapacity : ConstantClass
    {

        /// <summary>
        /// Constant INDEXES for ReturnConsumedCapacity
        /// </summary>
        public static readonly ReturnConsumedCapacity INDEXES = new ReturnConsumedCapacity("INDEXES");
        /// <summary>
        /// Constant NONE for ReturnConsumedCapacity
        /// </summary>
        public static readonly ReturnConsumedCapacity NONE = new ReturnConsumedCapacity("NONE");
        /// <summary>
        /// Constant TOTAL for ReturnConsumedCapacity
        /// </summary>
        public static readonly ReturnConsumedCapacity TOTAL = new ReturnConsumedCapacity("TOTAL");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ReturnConsumedCapacity(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ReturnConsumedCapacity FindValue(string value)
        {
            return FindValue<ReturnConsumedCapacity>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ReturnConsumedCapacity(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ReturnItemCollectionMetrics.
    /// </summary>
    public class ReturnItemCollectionMetrics : ConstantClass
    {

        /// <summary>
        /// Constant NONE for ReturnItemCollectionMetrics
        /// </summary>
        public static readonly ReturnItemCollectionMetrics NONE = new ReturnItemCollectionMetrics("NONE");
        /// <summary>
        /// Constant SIZE for ReturnItemCollectionMetrics
        /// </summary>
        public static readonly ReturnItemCollectionMetrics SIZE = new ReturnItemCollectionMetrics("SIZE");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ReturnItemCollectionMetrics(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ReturnItemCollectionMetrics FindValue(string value)
        {
            return FindValue<ReturnItemCollectionMetrics>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ReturnItemCollectionMetrics(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ReturnValue.
    /// </summary>
    public class ReturnValue : ConstantClass
    {

        /// <summary>
        /// Constant ALL_NEW for ReturnValue
        /// </summary>
        public static readonly ReturnValue ALL_NEW = new ReturnValue("ALL_NEW");
        /// <summary>
        /// Constant ALL_OLD for ReturnValue
        /// </summary>
        public static readonly ReturnValue ALL_OLD = new ReturnValue("ALL_OLD");
        /// <summary>
        /// Constant NONE for ReturnValue
        /// </summary>
        public static readonly ReturnValue NONE = new ReturnValue("NONE");
        /// <summary>
        /// Constant UPDATED_NEW for ReturnValue
        /// </summary>
        public static readonly ReturnValue UPDATED_NEW = new ReturnValue("UPDATED_NEW");
        /// <summary>
        /// Constant UPDATED_OLD for ReturnValue
        /// </summary>
        public static readonly ReturnValue UPDATED_OLD = new ReturnValue("UPDATED_OLD");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ReturnValue(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ReturnValue FindValue(string value)
        {
            return FindValue<ReturnValue>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ReturnValue(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ReturnValuesOnConditionCheckFailure.
    /// </summary>
    public class ReturnValuesOnConditionCheckFailure : ConstantClass
    {

        /// <summary>
        /// Constant ALL_OLD for ReturnValuesOnConditionCheckFailure
        /// </summary>
        public static readonly ReturnValuesOnConditionCheckFailure ALL_OLD = new ReturnValuesOnConditionCheckFailure("ALL_OLD");
        /// <summary>
        /// Constant NONE for ReturnValuesOnConditionCheckFailure
        /// </summary>
        public static readonly ReturnValuesOnConditionCheckFailure NONE = new ReturnValuesOnConditionCheckFailure("NONE");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ReturnValuesOnConditionCheckFailure(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ReturnValuesOnConditionCheckFailure FindValue(string value)
        {
            return FindValue<ReturnValuesOnConditionCheckFailure>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ReturnValuesOnConditionCheckFailure(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type ScalarAttributeType.
    /// </summary>
    public class ScalarAttributeType : ConstantClass
    {

        /// <summary>
        /// Constant B for ScalarAttributeType
        /// </summary>
        public static readonly ScalarAttributeType B = new ScalarAttributeType("B");
        /// <summary>
        /// Constant N for ScalarAttributeType
        /// </summary>
        public static readonly ScalarAttributeType N = new ScalarAttributeType("N");
        /// <summary>
        /// Constant S for ScalarAttributeType
        /// </summary>
        public static readonly ScalarAttributeType S = new ScalarAttributeType("S");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public ScalarAttributeType(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static ScalarAttributeType FindValue(string value)
        {
            return FindValue<ScalarAttributeType>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator ScalarAttributeType(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type Select.
    /// </summary>
    public class Select : ConstantClass
    {

        /// <summary>
        /// Constant ALL_ATTRIBUTES for Select
        /// </summary>
        public static readonly Select ALL_ATTRIBUTES = new Select("ALL_ATTRIBUTES");
        /// <summary>
        /// Constant ALL_PROJECTED_ATTRIBUTES for Select
        /// </summary>
        public static readonly Select ALL_PROJECTED_ATTRIBUTES = new Select("ALL_PROJECTED_ATTRIBUTES");
        /// <summary>
        /// Constant COUNT for Select
        /// </summary>
        public static readonly Select COUNT = new Select("COUNT");
        /// <summary>
        /// Constant SPECIFIC_ATTRIBUTES for Select
        /// </summary>
        public static readonly Select SPECIFIC_ATTRIBUTES = new Select("SPECIFIC_ATTRIBUTES");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public Select(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static Select FindValue(string value)
        {
            return FindValue<Select>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator Select(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type SSEStatus.
    /// </summary>
    public class SSEStatus : ConstantClass
    {

        /// <summary>
        /// Constant DISABLED for SSEStatus
        /// </summary>
        public static readonly SSEStatus DISABLED = new SSEStatus("DISABLED");
        /// <summary>
        /// Constant DISABLING for SSEStatus
        /// </summary>
        public static readonly SSEStatus DISABLING = new SSEStatus("DISABLING");
        /// <summary>
        /// Constant ENABLED for SSEStatus
        /// </summary>
        public static readonly SSEStatus ENABLED = new SSEStatus("ENABLED");
        /// <summary>
        /// Constant ENABLING for SSEStatus
        /// </summary>
        public static readonly SSEStatus ENABLING = new SSEStatus("ENABLING");
        /// <summary>
        /// Constant UPDATING for SSEStatus
        /// </summary>
        public static readonly SSEStatus UPDATING = new SSEStatus("UPDATING");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public SSEStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static SSEStatus FindValue(string value)
        {
            return FindValue<SSEStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator SSEStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type SSEType.
    /// </summary>
    public class SSEType : ConstantClass
    {

        /// <summary>
        /// Constant AES256 for SSEType
        /// </summary>
        public static readonly SSEType AES256 = new SSEType("AES256");
        /// <summary>
        /// Constant KMS for SSEType
        /// </summary>
        public static readonly SSEType KMS = new SSEType("KMS");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public SSEType(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static SSEType FindValue(string value)
        {
            return FindValue<SSEType>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator SSEType(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type StreamViewType.
    /// </summary>
    public class StreamViewType : ConstantClass
    {

        /// <summary>
        /// Constant KEYS_ONLY for StreamViewType
        /// </summary>
        public static readonly StreamViewType KEYS_ONLY = new StreamViewType("KEYS_ONLY");
        /// <summary>
        /// Constant NEW_AND_OLD_IMAGES for StreamViewType
        /// </summary>
        public static readonly StreamViewType NEW_AND_OLD_IMAGES = new StreamViewType("NEW_AND_OLD_IMAGES");
        /// <summary>
        /// Constant NEW_IMAGE for StreamViewType
        /// </summary>
        public static readonly StreamViewType NEW_IMAGE = new StreamViewType("NEW_IMAGE");
        /// <summary>
        /// Constant OLD_IMAGE for StreamViewType
        /// </summary>
        public static readonly StreamViewType OLD_IMAGE = new StreamViewType("OLD_IMAGE");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public StreamViewType(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static StreamViewType FindValue(string value)
        {
            return FindValue<StreamViewType>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator StreamViewType(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type TableStatus.
    /// </summary>
    public class TableStatus : ConstantClass
    {

        /// <summary>
        /// Constant ACTIVE for TableStatus
        /// </summary>
        public static readonly TableStatus ACTIVE = new TableStatus("ACTIVE");
        /// <summary>
        /// Constant ARCHIVED for TableStatus
        /// </summary>
        public static readonly TableStatus ARCHIVED = new TableStatus("ARCHIVED");
        /// <summary>
        /// Constant ARCHIVING for TableStatus
        /// </summary>
        public static readonly TableStatus ARCHIVING = new TableStatus("ARCHIVING");
        /// <summary>
        /// Constant CREATING for TableStatus
        /// </summary>
        public static readonly TableStatus CREATING = new TableStatus("CREATING");
        /// <summary>
        /// Constant DELETING for TableStatus
        /// </summary>
        public static readonly TableStatus DELETING = new TableStatus("DELETING");
        /// <summary>
        /// Constant INACCESSIBLE_ENCRYPTION_CREDENTIALS for TableStatus
        /// </summary>
        public static readonly TableStatus INACCESSIBLE_ENCRYPTION_CREDENTIALS = new TableStatus("INACCESSIBLE_ENCRYPTION_CREDENTIALS");
        /// <summary>
        /// Constant UPDATING for TableStatus
        /// </summary>
        public static readonly TableStatus UPDATING = new TableStatus("UPDATING");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public TableStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static TableStatus FindValue(string value)
        {
            return FindValue<TableStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator TableStatus(string value)
        {
            return FindValue(value);
        }
    }


    /// <summary>
    /// Constants used for properties of type TimeToLiveStatus.
    /// </summary>
    public class TimeToLiveStatus : ConstantClass
    {

        /// <summary>
        /// Constant DISABLED for TimeToLiveStatus
        /// </summary>
        public static readonly TimeToLiveStatus DISABLED = new TimeToLiveStatus("DISABLED");
        /// <summary>
        /// Constant DISABLING for TimeToLiveStatus
        /// </summary>
        public static readonly TimeToLiveStatus DISABLING = new TimeToLiveStatus("DISABLING");
        /// <summary>
        /// Constant ENABLED for TimeToLiveStatus
        /// </summary>
        public static readonly TimeToLiveStatus ENABLED = new TimeToLiveStatus("ENABLED");
        /// <summary>
        /// Constant ENABLING for TimeToLiveStatus
        /// </summary>
        public static readonly TimeToLiveStatus ENABLING = new TimeToLiveStatus("ENABLING");

        /// <summary>
        /// This constant constructor does not need to be called if the constant
        /// you are attempting to use is already defined as a static instance of 
        /// this class.
        /// This constructor should be used to construct constants that are not
        /// defined as statics, for instance if attempting to use a feature that is
        /// newer than the current version of the SDK.
        /// </summary>
        public TimeToLiveStatus(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Finds the constant for the unique value.
        /// </summary>
        /// <param name="value">The unique value for the constant</param>
        /// <returns>The constant for the unique value</returns>
        public static TimeToLiveStatus FindValue(string value)
        {
            return FindValue<TimeToLiveStatus>(value);
        }

        /// <summary>
        /// Utility method to convert strings to the constant class.
        /// </summary>
        /// <param name="value">The string value to convert to the constant class.</param>
        /// <returns></returns>
        public static implicit operator TimeToLiveStatus(string value)
        {
            return FindValue(value);
        }
    }

}