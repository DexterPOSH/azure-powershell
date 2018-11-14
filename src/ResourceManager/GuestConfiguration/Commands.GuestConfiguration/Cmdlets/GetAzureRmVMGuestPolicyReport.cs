﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.Azure.Commands.GuestConfiguration.Cmdlets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using Microsoft.Azure.Commands.GuestConfiguration.Common;
    using Microsoft.Azure.Commands.GuestConfiguration.Models;
    using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
    using Microsoft.Azure.Management.GuestConfiguration.Models;

    /// <summary>
    /// Gets Vm Guest Policy reports (GuestConfiguration policy reports)
    /// </summary>
    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "VMGuestPolicyReport", DefaultParameterSetName = ParameterSetNames.VmNameScope), 
        OutputType(typeof(IList<GuestConfigurationAssignment>), typeof(IList<GuestConfigurationAssignmentReport>))]
    public class GetAzureRmVMGuestPolicyReport : GuestConfigurationCmdletBase
    {
        [Parameter(ParameterSetName = ParameterSetNames.VmNameScope, Mandatory = true, Position = 0, HelpMessage = ParameterHelpMessages.ResourceGroupName)]
        [Parameter(ParameterSetName = ParameterSetNames.InitiativeIdScope, Mandatory = true, Position = 0, HelpMessage = ParameterHelpMessages.ResourceGroupName)]
        [Parameter(ParameterSetName = ParameterSetNames.InitiativeNameScope, Mandatory = true, Position = 0, HelpMessage = ParameterHelpMessages.ResourceGroupName)]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(ParameterSetName = ParameterSetNames.VmNameScope, Mandatory = true, Position = 1, HelpMessage = ParameterHelpMessages.VMName)]
        [Parameter(ParameterSetName = ParameterSetNames.InitiativeIdScope, Mandatory = true, Position = 1, HelpMessage = ParameterHelpMessages.VMName)]
        [Parameter(ParameterSetName = ParameterSetNames.InitiativeNameScope, Mandatory = true, Position = 1, HelpMessage = ParameterHelpMessages.VMName)]
        [ValidateNotNullOrEmpty]
        public string VMName { get; set; }

        [Parameter(ParameterSetName = ParameterSetNames.InitiativeNameScope, Mandatory = true, Position = 2, HelpMessage = ParameterHelpMessages.InitiativeName)]
        [ValidateNotNullOrEmpty]
        public string InitiativeName { get; set; }

        [Parameter(ParameterSetName = ParameterSetNames.InitiativeIdScope, Mandatory = true, Position = 2, HelpMessage = ParameterHelpMessages.InitiativeId)]
        [ValidateNotNullOrEmpty]
        public string InitiativeId { get; set; }

        [Parameter(ParameterSetName = ParameterSetNames.ReportIdScope, Mandatory = true, Position = 2, HelpMessage = ParameterHelpMessages.ReportId)]
        [ValidateNotNullOrEmpty]
        public string ReportId { get; set; }

        [Parameter(ParameterSetName = ParameterSetNames.InitiativeNameScope, Mandatory = false, HelpMessage = ParameterHelpMessages.DetailedSwitch)]
        [Parameter(ParameterSetName = ParameterSetNames.InitiativeIdScope, Mandatory = false, HelpMessage = ParameterHelpMessages.DetailedSwitch)]
        [Parameter(ParameterSetName = ParameterSetNames.VmNameScope, Mandatory = false, HelpMessage = ParameterHelpMessages.DetailedSwitch)]
        public SwitchParameter Detailed { get; set; }

        [Parameter(ParameterSetName = ParameterSetNames.InitiativeNameScope, Mandatory = false, HelpMessage = ParameterHelpMessages.LatestSwitch)]
        [Parameter(ParameterSetName = ParameterSetNames.InitiativeIdScope, Mandatory = false,  HelpMessage = ParameterHelpMessages.LatestSwitch)]
        [Parameter(ParameterSetName = ParameterSetNames.VmNameScope, Mandatory = false, Position = 3,  HelpMessage = ParameterHelpMessages.LatestSwitch)]
        public SwitchParameter Latest { get; set; }

        /// <summary>
        /// Executes the cmdlet
        /// </summary>
        public override void ExecuteCmdlet()
        {
            IEnumerable<GuestConfigurationPolicyAssignment> gcPolicyAssignments = null;
            IEnumerable<GuestConfigurationPolicyAssignmentReport> gcPolicyAssignmentReports = null;

            switch (ParameterSetName)
            {
                case ParameterSetNames.InitiativeNameScope:
                    if (Detailed.IsPresent)
                    {
                        gcPolicyAssignmentReports = GetAllGuestConfigurationAssignmentReportsByInitiativeName(ResourceGroupName, VMName, InitiativeName, Latest.IsPresent);
                        if(gcPolicyAssignmentReports == null || gcPolicyAssignmentReports.Count() > 0)
                        {
                            WriteObject(gcPolicyAssignmentReports, true);
                        }                        
                    }
                    else
                    {
                        gcPolicyAssignments = GetAllGuestConfigurationAssignmentsByInitiativeName(ResourceGroupName, VMName, InitiativeName);
                        if (gcPolicyAssignments == null || gcPolicyAssignments.Count() > 0)
                        {
                            WriteObject(gcPolicyAssignments, true);
                        }
                    }
                    break;

                case ParameterSetNames.InitiativeIdScope:
                    if (Detailed.IsPresent)
                    {
                        gcPolicyAssignmentReports = GetAllGuestConfigurationAssignmentReportsByInitiativeId(ResourceGroupName, VMName, InitiativeId, Latest.IsPresent);

                        if (gcPolicyAssignmentReports == null || gcPolicyAssignmentReports.Count() > 0)
                        {
                            WriteObject(gcPolicyAssignmentReports, true);
                        }                   
                    }
                    else
                    {
                        gcPolicyAssignments = GetAllGuestConfigurationAssignmentsByInitiativeId(ResourceGroupName, VMName, InitiativeId);

                        if (gcPolicyAssignments == null || gcPolicyAssignments.Count() > 0)
                        {
                            WriteObject(gcPolicyAssignments, true);
                        }                       
                    }
                    break;

                case ParameterSetNames.ReportIdScope:
                    var policyReport = GetGuestConfigurationAssignmentReportById(ReportId);
                    if (policyReport != null)
                    {
                        WriteObject(policyReport);
                    }
                    break;

                case ParameterSetNames.VmNameScope:
                    if (Detailed.IsPresent)
                    {
                        gcPolicyAssignmentReports = GetAllGuestConfigurationAssignmentReports(ResourceGroupName, VMName, Latest.IsPresent);
                        if (gcPolicyAssignmentReports == null || gcPolicyAssignmentReports.Count() > 0)
                        {
                            WriteObject(gcPolicyAssignmentReports, true);
                        }
                    }
                    else
                    {
                        gcPolicyAssignments = GetAllGuestConfigurationAssignments(ResourceGroupName, VMName);
                        if (gcPolicyAssignments == null || gcPolicyAssignments.Count() > 0)
                        {
                            WriteObject(gcPolicyAssignments, true);
                        }
                    }
                    break;
            }    
        }
    }
}