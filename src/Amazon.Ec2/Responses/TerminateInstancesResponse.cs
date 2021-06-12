#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class TerminateInstancesResponse : IEc2Response
    {
        [XmlArray("instancesSet")]
        [XmlArrayItem("item")]
        public InstanceStateChange[] Instances { get; init; }
    }
}

/*
<TerminateInstancesResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
  <requestId>59dbff89-35bd-4eac-99ed-be587EXAMPLE</requestId>
  <instancesSet>
    <item>
      <instanceId>i-1234567890abcdef0</instanceId>
      <currentState>
        <code>32</code>
        <name>shutting-down</name>
      </currentState>
      <previousState>
        <code>16</code>
        <name>running</name>
      </previousState>
    </item>
  </instancesSet>
</TerminateInstancesResponse>
*/