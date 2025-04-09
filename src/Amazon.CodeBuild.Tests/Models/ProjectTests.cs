using System.Text.Json;

namespace Amazon.CodeBuild.Tests;

public class ProjectTests
{
    [Fact]
    public void CanDeserialize()
    {
        var project = JsonSerializer.Deserialize<Project>(
            """
            {
               "arn": "arn",
               "artifacts": {
                  "artifactIdentifier": "string",
                  "bucketOwnerAccess": "string",
                  "encryptionDisabled": false,
                  "location": "string",
                  "name": "string",
                  "namespaceType": "NONE",
                  "overrideArtifactName": true,
                  "packaging": "ZIP",
                  "path": "string",
                  "type": "NO_ARTIFACTS"
               },
               "badge": {
                  "badgeEnabled": true,
                  "badgeRequestUrl": "string"
               },
               "cache": {
                  "location": "string",
                  "modes": [ "string" ],
                  "type": "string"
               },
               "concurrentBuildLimit": 1,
               "created": 0,
               "description": "string",
               "encryptionKey": "string",
               "environment": { 
                  "certificate": "string",
                  "computeType": "string",
                  "environmentVariables": [ 
                     { 
                        "name": "string",
                        "type": "string",
                        "value": "string"
                     }
                  ],
                  "image": "string",
                  "imagePullCredentialsType": "string",
                  "privilegedMode": true,
                  "registryCredential": { 
                     "credential": "string",
                     "credentialProvider": "string"
                  },
                  "type": "ARM_CONTAINER"
               },
               "lastModified": 1,
               "logsConfig": { 
                  "cloudWatchLogs": { 
                     "groupName": "string",
                     "status": "string",
                     "streamName": "string"
                  },
                  "s3Logs": { 
                     "bucketOwnerAccess": "string",
                     "encryptionDisabled": true,
                     "location": "string",
                     "status": "string"
                  }
               },
               "name": "carbon",
               "projectVisibility": "string",
               "publicProjectAlias": "string",
               "queuedTimeoutInMinutes": 1,
               "resourceAccessRole": "string",
               "secondaryArtifacts": [ 
                  { 
                     "artifactIdentifier": "string",
                     "bucketOwnerAccess": "string",
                     "encryptionDisabled": true,
                     "location": "string",
                     "name": "string",
                     "namespaceType": "string",
                     "overrideArtifactName": true,
                     "packaging": "string",
                     "path": "string",
                     "type": "string"
                  }
               ],
               "secondarySources": [ 
                  { 
                     "auth": { 
                        "resource": "string",
                        "type": "string"
                     },
                     "buildspec": "string",
                     "buildStatusConfig": { 
                        "context": "string",
                        "targetUrl": "string"
                     },
                     "gitCloneDepth": true,
                     "gitSubmodulesConfig": { 
                        "fetchSubmodules": true
                     },
                     "insecureSsl": true,
                     "location": "string",
                     "reportBuildStatus": true,
                     "sourceIdentifier": "string",
                     "type": "string"
                  }
               ],
               "secondarySourceVersions": [ 
                  { 
                     "sourceIdentifier": "string",
                     "sourceVersion": "string"
                  }
               ],
               "serviceRole": "string",
               "source": { 
                  "auth": { 
                     "resource": "string",
                     "type": "string"
                  },
                  "buildspec": "string",
                  "buildStatusConfig": { 
                     "context": "string",
                     "targetUrl": "string"
                  },
                  "gitCloneDepth": true,
                  "gitSubmodulesConfig": { 
                     "fetchSubmodules": true
                  },
                  "insecureSsl": true,
                  "location": "string",
                  "reportBuildStatus": true,
                  "sourceIdentifier": "string",
                  "type": "string"
               },
               "sourceVersion": "string",
               "tags": [ 
                  { 
                     "key": "string",
                     "value": "string"
                  }
               ],
               "timeoutInMinutes": 11,
               "vpcConfig": { 
                  "securityGroupIds": [ "string" ],
                  "subnets": [ "string" ],
                  "vpcId": "string"
               },
               "webhook": { 
                  "branchFilter": "string",
                  "buildType": "string",
                  "filterGroups": [ 
                     [ 
                        { 
                           "excludeMatchedPattern": true,
                           "pattern": "string",
                           "type": "string"
                        }
                     ]
                  ],
                  "lastModifiedSecret": 1,
                  "payloadUrl": "string",
                  "secret": "string",
                  "url": "string"
               }
            }
            """u8);

        Assert.NotNull(project);
        Assert.Equal("arn", project.Arn);
        Assert.Equal("carbon", project.Name);
        Assert.NotNull(project.Webhook);
    }
}